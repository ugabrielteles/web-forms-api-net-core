using Order.Frontend.WebForms.Utils;
using System;
using System.Linq;
using System.Threading;
using System.Web.Script.Serialization;
using System.Web.UI;

namespace Order.Frontend.WebForms
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected async void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                string username = txtUsername.Text.Trim();
                string password = txtPassword.Text.Trim();
                string name = txtName.Text.Trim();

                ///<summary>
                ///Valida se usuário preencheu as informações de login e senha 
                ///</summary>            
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(name))
                {
                    lblMessage.InnerHtml = "Nome, Usuário e senha são obrigatórios.";
                    lblMessage.Attributes["class"] = "alert alert-danger mt-1";
                    lblMessage.Visible = true;
                    return;
                }

                var requestData = new { name = name, email = username, password = password };
                var jsonRequest = new JavaScriptSerializer().Serialize(requestData);

                var result = await new ApiClient().PostNotHeadersAsync("auth/register", jsonRequest);

                if (result != null)
                {
                    if (result.Success)
                    {
                        lblMessage.Attributes["class"] = "alert alert-success mt-1";
                        lblMessage.InnerText = "Cadastro realizado com sucesso! Redirecionando...";
                        lblMessage.Visible = true;

                        // Atualiza o UpdatePanel para mostrar a mensagem antes de redirecionar
                        ScriptManager.RegisterStartupScript(this, GetType(), "Redirect", "setTimeout(function(){ window.location = '/Login.aspx'; }, 2000);", true);
                    }
                    else
                    {
                        lblMessage.Attributes["class"] = "alert alert-danger mt-1";
                        lblMessage.InnerText = string.Join(",", result.Errors.Select(x=> x.errorMessage).ToArray());
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
    }
}