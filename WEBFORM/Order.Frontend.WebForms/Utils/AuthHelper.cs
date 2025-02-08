using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Order.Frontend.WebForms.Utils
{

    public class AuthHelper
    {
        public static bool IsAuthenticated()
        {
            string token = GetToken();
            if (string.IsNullOrEmpty(token))
                return false;

            // Verifica se o token ainda é válido
            return IsTokenValid(token);
        }

        public static string GetToken()
        {
            // Verifica se o token está na sessão ou nos cookies
            string token = HttpContext.Current.Session["JWToken"] as string;
            if (string.IsNullOrEmpty(token))
            {
                var cookie = HttpContext.Current.Request.Cookies["JWToken"];
                if (cookie != null)
                {
                    token = cookie.Value;
                }
            }

            return token;
        }

        public static string GetNameUser()
        {
            // Verifica se o token está na sessão ou nos cookies
            string token = HttpContext.Current.Session["JWTokenUser"] as string;
            if (string.IsNullOrEmpty(token))
            {
                var cookie = HttpContext.Current.Request.Cookies["JWTokenUser"];
                if (cookie != null)
                {
                    token = cookie.Value;
                }
            }

            return token;
        }

        public static bool IsTokenValid(string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                    return false;

                // Decodifica o token localmente e verifica o tempo de expiração
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var expClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "exp")?.Value;

                if (expClaim != null && long.TryParse(expClaim, out long expUnixTime))
                {
                    var expirationDate = DateTimeOffset.FromUnixTimeSeconds(expUnixTime).UtcDateTime;
                    if (expirationDate <= DateTime.Now)
                        return false; // Token expirado
                }

                // Validação extra na API para garantir que o token não foi revogado
                return true;
            }
            catch
            {
                return false;
            }
        }


        public static void Logout()
        {
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();

            if (HttpContext.Current.Request.Cookies["JWToken"] != null)
            {
                HttpCookie cookie = new HttpCookie("JWToken", "")
                {
                    Expires = DateTime.Now.AddDays(-1),
                    HttpOnly = true
                };
                HttpContext.Current.Response.Cookies.Add(cookie);
            }

            if (HttpContext.Current.Request.Cookies["JWTokenUser"] != null)
            {
                HttpCookie cookie = new HttpCookie("JWTokenUser", "")
                {
                    Expires = DateTime.Now.AddDays(-1),
                    HttpOnly = true
                };
                HttpContext.Current.Response.Cookies.Add(cookie);
            }

            HttpContext.Current.Request.Headers.Remove("Authorization");
        }
    }
}