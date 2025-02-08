using Microsoft.AspNet.SignalR.Client;
using Order.Frontend.WebForms.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web.UI;

namespace Order.Frontend.WebForms.Order
{
    public partial class KanbanOrders : BasePage
    {
        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                await LoadKanbanBoard();
            }
        }        

        private async Task LoadKanbanBoard()
        {
            try
            {
                var result = await new ApiClient().GetAsync("orders/order-by-status");

                if (result != null)
                {
                    if (result.Success)
                    {
                        var statusList = JsonSerializer.Deserialize<List<Models.OrderStatus>>(result.responseBody);

                        rptKanban.DataSource = statusList;
                        rptKanban.DataBind();
                    }
                    else
                    {
                        if (result.httpStatusCode == System.Net.HttpStatusCode.NotFound)
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

        protected async void btnAtualizarStatus_Click(object sender, EventArgs e)
        {
            int orderId = int.Parse(hdnOrderId.Value);
            int newStatus = int.Parse(hdnNewStatus.Value);
                       
            string route = string.Empty;

            switch (newStatus)
            {
                case 1:
                    route = "send-order-to-create";
                    break;
                case 2:
                    route = "send-order-to-preparing";
                    break;

                case 3:
                    route = "send-order-to-out-for-delivery";
                    break;

                case 4:
                    route = "send-order-to-delivered";
                    break;
            }           

            try
            {
                var result = await new ApiClient().PatchAsync($"orders/{orderId}/{route}", "{}");

                if (result != null)
                {
                    if (result.Success)
                    {
                        lblMessage.Attributes["class"] = "alert alert-success mt-1";
                        lblMessage.InnerText = "Cadastro realizado com sucesso! Redirecionando...";
                        lblMessage.Visible = true;

                        // Atualiza o UpdatePanel para mostrar a mensagem antes de redirecionar
                        ScriptManager.RegisterStartupScript(this, GetType(), "Redirect", "setTimeout(function(){ window.location = '/Order/KanbanOrders.aspx'; }, 2000);", true);                        
                    }
                    else
                    {
                        if (result.httpStatusCode == System.Net.HttpStatusCode.NotFound)
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
                    await LoadKanbanBoard();
                }
            }
            catch (Exception ex)
            {
                string erro = "Não foi possível cadastrar, entre em contato com administrador do sistema.";
                lblMessage.InnerHtml = erro;
                lblMessage.Attributes["class"] = "alert alert-danger mt-1";
                lblMessage.Visible = true;
                LogUtil.LogError(new Exception(erro + "Detalhe: " + ex.Message));
                await LoadKanbanBoard();
            }

        }
    }
}