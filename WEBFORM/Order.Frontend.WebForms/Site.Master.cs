using Order.Frontend.WebForms.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Order.Frontend.WebForms
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        private string name;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                name = AuthHelper.GetNameUser();

                userName.InnerText = name;
            }
        }
    }
}