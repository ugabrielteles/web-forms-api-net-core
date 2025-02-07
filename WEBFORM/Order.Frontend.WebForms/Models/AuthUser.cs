namespace Order.Frontend.WebForms.Models
{
    public class AuthUser
    {
        public string access_token { get; set; }

        public long expires_in { get; set; }

        public string token_type { get; set; }
    }
}