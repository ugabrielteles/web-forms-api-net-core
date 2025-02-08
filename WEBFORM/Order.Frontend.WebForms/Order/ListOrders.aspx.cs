using Order.Frontend.WebForms.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Order.Frontend.WebForms.Order
{
    public partial class ListOrders : BasePage
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                await LoadList();

        }

        private async Task LoadList()
        {
            try
            {
                var result = await new ApiClient().GetAsync("orders");

                List<Models.Order> list = new List<Models.Order>();

                if (result != null)
                {
                    if (result.Success)
                    {
                        list = new JavaScriptSerializer().Deserialize<List<Models.Order>>(result.responseBody);
                        gvOrders.DataSource = list;
                        gvOrders.DataBind();
                    }
                    else
                    {
                        if(result.httpStatusCode == System.Net.HttpStatusCode.NotFound)
                        {
                            lblMessage.Attributes["class"] = "alert alert-info mt-1";
                        }
                        else
                        {
                            lblMessage.Attributes["class"] = "alert alert-danger mt-1";
                        }
                        
                        lblMessage.InnerText = string.Join(",", result.Errors.Select(x => x.errorMessage).ToArray());
                        lblMessage.Visible = true;
                    }
                }
                else
                {
                    string erro = "Dados não encontrados.";
                    lblMessage.InnerHtml = erro;
                    lblMessage.Attributes["class"] = "alert alert-info mt-1";
                    lblMessage.Visible = true;
                    return;
                }
            }
            catch (Exception ex)
            {
                string erro = "Não foi possível cadastrar, entre em contato com administrador do sistema.";
                lblMessage.InnerHtml = erro;
                lblMessage.Attributes["class"] = "alert alert-danger mt-1";
                lblMessage.Visible = true;
                LogUtil.LogError(new Exception(erro + "Detalhe: " + ex.Message));
                return;
            }
        }

        protected void gvOrders_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
        {

        }

        protected void gvOrders_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {

        }

        protected void gvOrders_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {

        }

        protected void gvOrders_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {

        }
    }
}