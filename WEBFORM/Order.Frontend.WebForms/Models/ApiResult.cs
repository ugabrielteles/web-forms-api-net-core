using System.Collections.Generic;

namespace Order.Frontend.WebForms.Models
{
    public class ApiResult
    {
        public System.Net.HttpStatusCode httpStatusCode { get; set; }

        public string responseBody { get; set; }

        public bool Success { get; set; }

        public List<ApiResultError> Errors {get;set;}
    }
}