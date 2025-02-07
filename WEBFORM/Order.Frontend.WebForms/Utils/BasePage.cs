using System;
using System.Web;

namespace Order.Frontend.WebForms.Utils
{

    public class BasePage : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e)
        {
            if (!AuthHelper.IsAuthenticated())
                Response.Redirect("~/Login.aspx?ReturnUrl=" + Server.UrlEncode(Request.Url.AbsolutePath));

            base.OnLoad(e);
        }
    }
}