using Order.Frontend.WebForms.Models;
using Order.Frontend.WebForms.Utils;
using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Web.UI;

namespace Order.Frontend.WebForms.Order
{
    public partial class NewOrder : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected async void txtZipCode_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtZipCode.Text))
                await GetCep(txtZipCode.Text);
        }

        protected async void btnSaveOrder_Click(object sender, EventArgs e)
        {
            try
            {                
                var order = new Models.Order
                {
                    description = txtDescription.Text,
                    value = decimal.TryParse(txtValue.Text, out decimal value) ? value : (decimal?)null,
                    street = txtStreet.Text,
                    zipCode = txtZipCode.Text,
                    number = txtNumber.Text,
                    neighborhood = txtNeighborhood.Text,
                    city = txtCity.Text,
                    state = txtState.Text
                };
                
                var jsonRequest = new JavaScriptSerializer().Serialize(order);

                var result = await new ApiClient().PostAsync("orders", jsonRequest);

                if (result != null)
                {
                    if (result.Success)
                    {
                        lblMessage.Attributes["class"] = "alert alert-success mt-1";
                        lblMessage.InnerText = "Cadastro realizado com sucesso! Redirecionando...";
                        lblMessage.Visible = true;

                        // Atualiza o UpdatePanel para mostrar a mensagem antes de redirecionar
                        ScriptManager.RegisterStartupScript(this, GetType(), "Redirect", "setTimeout(function(){ window.location = '/Order/ListOrders.aspx'; }, 2000);", true);
                    }
                    else
                    {
                        lblMessage.Attributes["class"] = "alert alert-danger mt-1";
                        lblMessage.InnerText = string.Join(",", result.Errors.Select(x => x.errorMessage).ToArray());
                        lblMessage.Visible = true;
                    }
                }
                else
                {
                    string erro = "Não foi possível cadastrar, entre em contato com administrador do sistema.";
                    lblMessage.InnerHtml = erro;
                    lblMessage.Attributes["class"] = "alert alert-danger mt-1";
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

        private async Task GetCep(string cep)
        {
            var baseUrl = ConfigurationManager.AppSettings["ApiBaseUrlCep"];
            var api = new ApiClient(baseUrl);

            baseUrl = baseUrl.Replace("{0}", cep);

            var result = await api.GetAsync(baseUrl);

            if (result.Success)
            {
                 var address = new JavaScriptSerializer().Deserialize<IntegrationCep>(result.responseBody);

                if (address != null)
                {
                    txtStreet.Text = address.logradouro;
                    txtNeighborhood.Text = address.bairro;
                    txtCity.Text = address.localidade;
                    txtState.Text = address.uf;
                    txtNumber.Focus();

                }
                
            }
        }

    }
}