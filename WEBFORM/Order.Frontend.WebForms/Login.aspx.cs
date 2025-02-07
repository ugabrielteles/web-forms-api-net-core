using Order.Frontend.WebForms.Models;
using Order.Frontend.WebForms.Utils;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace Order.Frontend.WebForms
{
    /// <summary>
    /// Pagina de login 
    /// 
    /// Responsável por gerenciar credenciais do usuário
    /// </summary>
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /// <summary>
            /// Zera sessão do usuário 
            /// </summary>
            // AuthHelper.Logout();
            if (!IsPostBack)
            {
                AuthHelper.Logout();
            }
        }

        /// <summary>
        /// Evento clique com ação do usuário para autenticar 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected async void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            ///<summary>
            ///Valida se usuário preencheu as informações de login e senha 
            ///</summary>            
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                lblMessage.InnerHtml = "Usuário e senha são obrigatórios.";
                lblMessage.Visible = true;
                return;
            }

            try
            {
                // string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoidGVsZXMuZ2FicmllbEBvdXRsb29rLmNvbS5iciIsImp0aSI6IjBhMmY5OTliLWJjMzktNDY1NS1iMTVhLTczOTIwNDZkMzU5MiIsImV4cCI6MTczODkzMzE1NSwiaXNzIjoiQVBJIC0gT3JkZXJzIiwiYXVkIjoiT1JERVIgLSBXRUJGT1JNUyJ9.ruv45wmmH0ncG-AlXB3A64J62MNQTWfvvJFOtXaIock";

                var authToken= await AuthenticateUser(username, password);

                ///<summary>
                ///Verifica se houve retorno pisitivo da api e 
                ///</summary>
                if (authToken != null && !string.IsNullOrEmpty(authToken.access_token))
                {
                    // Define o token na sessão
                    Session["JWToken"] = authToken.access_token;                    

                    // Define cookie do JWT
                    HttpCookie jwtCookie = new HttpCookie("JWToken", authToken.access_token)
                    {
                        HttpOnly = true,
                        Secure = Request.IsSecureConnection
                    };

                    // Define cookie do JWT
                    HttpCookie jwtCookieUser = new HttpCookie("JWTokenUser", username)
                    {
                        HttpOnly = true,
                        Secure = Request.IsSecureConnection
                    };

                    // Obtém o tempo de expiração do JWT
                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadJwtToken(authToken.access_token);
                    var expClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "exp")?.Value;


                    if (expClaim != null && long.TryParse(expClaim, out long expUnixTime))
                    {
                        var expirationDate = DateTimeOffset.FromUnixTimeSeconds(expUnixTime).UtcDateTime;
                        jwtCookie.Expires = expirationDate;
                        jwtCookieUser.Expires = expirationDate;

                        if (expirationDate < DateTime.Now)
                        {
                            var ex = new Exception("Token gerado pela api já expirado.");
                            LogUtil.LogError(ex);
                        }

                        Session.Timeout = (int)(expirationDate - DateTime.Now).TotalMinutes; // Ajusta tempo de sessão
                    }

                    Response.Cookies.Add(jwtCookie);
                    Response.Cookies.Add(jwtCookieUser);

                    string returnUrl = Request.QueryString["ReturnUrl"] as string ?? "~/Default.aspx";
                    Response.Redirect(returnUrl, false);
                }
                else
                {
                    lblMessage.InnerHtml = "Usuário ou senha inválidos.";
                    lblMessage.Visible = true;
                }
            }
            catch (Exception ex)
            {
                string erro = "Não foi possível autenticar, entre em contato com administrador do sistema.";
                lblMessage.InnerHtml = erro;
                lblMessage.Visible = true;
                LogUtil.LogError(new Exception(erro + "Detalhe: " + ex.Message));
            }
        }

        /// <summary>
        /// Autentica na api com padrão JWT
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private async Task<AuthUser> AuthenticateUser(string username, string password)
        {
            try
            {
                var requestData = new { email = username, password = password };
                var jsonRequest = new JavaScriptSerializer().Serialize(requestData);

                var result = await new ApiClient().PostNotHeadersAsync("auth/login", jsonRequest);

                if (result == null || string.IsNullOrEmpty(result.responseBody))
                    return null;

                return new JavaScriptSerializer().Deserialize<AuthUser>(result.responseBody);
            }
            catch (Exception ex)
            {
                LogUtil.LogError(ex);
                throw;
            }
        }

        
    }


}