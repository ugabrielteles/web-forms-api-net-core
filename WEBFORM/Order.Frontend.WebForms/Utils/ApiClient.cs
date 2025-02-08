using Order.Frontend.WebForms.Utils;
using System.Configuration;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System;
using Order.Frontend.WebForms.Models;
using System.Web.Script.Serialization;
using System.Collections.Generic;

public class ApiClient : System.Web.UI.Page
{
    public string _baseUrl;
    private readonly string _token;

    public ApiClient()
    {
        _baseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"];
        if (string.IsNullOrEmpty(_baseUrl))
        {
            throw new ArgumentNullException("ApiBaseUrl", "A URL base da API não foi configurada no web.config.");
        }

        _token = AuthHelper.GetToken();        
    }

    public ApiClient(string baseUrl)
    {
        _baseUrl = baseUrl;
        _token = AuthHelper.GetToken();
    }

    public async Task<ApiResult> GetAsync(string endpoint)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

                HttpResponseMessage response = await client.GetAsync(endpoint);
                return await HandleResponse(response);
            }
        }
        catch (HttpRequestException ex)
        {
            LogUtil.LogError(ex);
            throw new Exception($"Erro na requisição GET para {endpoint}: {ex.Message}", ex);
        }
    }

    public async Task<ApiResult> PostAsync(string endpoint, string jsonContent)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(endpoint, content);
                return await HandleResponse(response);
            }
        }
        catch (HttpRequestException ex)
        {
            LogUtil.LogError(ex);
            throw new Exception($"Erro na requisição POST para {endpoint}: {ex.Message}", ex);
        }
    }

    public async Task<ApiResult> PatchAsync(string endpoint, string jsonContent)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var request = new HttpRequestMessage(new HttpMethod("PATCH"), endpoint) { Content = content };

                HttpResponseMessage response = await client.SendAsync(request);
                return await HandleResponse(response);
            }
        }
        catch (HttpRequestException ex)
        {
            LogUtil.LogError(ex);
            throw new Exception($"Erro na requisição PATCH para {endpoint}: {ex.Message}", ex);
        }
    }

    public async Task<ApiResult> PostNotHeadersAsync(string endpoint, string jsonContent)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(endpoint, content);
                return await HandleResponse(response);
            }
        }
        catch (HttpRequestException ex)
        {
            LogUtil.LogError(ex);
            throw new Exception($"Erro na requisição POST (sem headers) para {endpoint}: {ex.Message}", ex);
        }
    }

    public async Task<ApiResult> PutAsync(string endpoint, string jsonContent)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(endpoint, content);
                return await HandleResponse(response);
            }
        }
        catch (HttpRequestException ex)
        {
            LogUtil.LogError(ex);
            throw new Exception($"Erro na requisição PUT para {endpoint}: {ex.Message}", ex);
        }
    }

    public async Task<ApiResult> DeleteAsync(string endpoint)
    {
        try
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);

                HttpResponseMessage response = await client.DeleteAsync(endpoint);
                return await HandleResponse(response);
            }
        }
        catch (HttpRequestException ex)
        {
            LogUtil.LogError(ex);
            throw new Exception($"Erro na requisição DELETE para {endpoint}: {ex.Message}", ex);
        }
    }

    private async Task<ApiResult> HandleResponse(HttpResponseMessage response)
    {
       
        string responseBody = await response.Content.ReadAsStringAsync();

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            Response.Redirect("~/Login.aspx?ReturnUrl=" + Server.UrlEncode(Request.Url.AbsolutePath));
        }

        if (response.IsSuccessStatusCode)
        {
            return new ApiResult { Success = true, httpStatusCode = response.StatusCode, responseBody = responseBody };
        }
        else if(response.StatusCode == HttpStatusCode.BadRequest)
        {
            var listErros = new JavaScriptSerializer().Deserialize<List<ApiResultError>>(responseBody);
            return new ApiResult { Success = false, httpStatusCode = response.StatusCode, responseBody = responseBody, Errors = listErros };
        }
        else if (response.StatusCode == HttpStatusCode.NotFound)
        {
            var erro = new ApiResultError { errorMessage = "Dados não encontrados." };
            return new ApiResult { Success = false, httpStatusCode = response.StatusCode, responseBody = responseBody, Errors = new List<ApiResultError> { erro } };
        }
        else
        {
            var ex = new HttpRequestException($"Erro HTTP {response.StatusCode}: {response.ReasonPhrase}\n{responseBody}");

            LogUtil.LogError(ex);

            throw ex;
        }
    }
}
