<%@ Page Title="Lista de Orders" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ListOrders.aspx.cs" Inherits="Order.Frontend.WebForms.Order.ListOrders" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <asp:UpdatePanel ID="updPanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

            <div role="alert" id="lblMessage" runat="server" visible="false">
            </div>
            <div class="row">
                <span>
                       <h2 class="mt-3">Lista de Ordens</h2>
                    <hr />
                </span>
                <span>
                    <a class="btn btn-primary btn-block" runat="server" href="~/Order/NewOrder">Cadastrar nova ordem</a>
                </span>
                
            </div>

            <hr />
            <asp:GridView ID="gvOrders" runat="server" AutoGenerateColumns="False"
                CssClass="table table-striped mt-2" OnRowEditing="gvOrders_RowEditing"
                OnRowCancelingEdit="gvOrders_RowCancelingEdit"
                OnRowUpdating="gvOrders_RowUpdating" OnRowDeleting="gvOrders_RowDeleting">
                <Columns>
                    <asp:BoundField DataField="orderId" HeaderText="ID" SortExpression="OrderId" />
                    <asp:BoundField DataField="description" HeaderText="Descrição" SortExpression="Description" />
                    <asp:BoundField DataField="value" HeaderText="Valor" SortExpression="Value" DataFormatString="{0:C}" />
                    <asp:BoundField DataField="createAt" HeaderText="Data Criação" SortExpression="CreateAt"
                        DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="street" HeaderText="Rua" SortExpression="Street" />
                    <asp:BoundField DataField="zipCode" HeaderText="Cep" SortExpression="ZipCode" />
                    <asp:BoundField DataField="number" HeaderText="Número" SortExpression="Number" />
                    <asp:BoundField DataField="neighborhood" HeaderText="Bairro" SortExpression="Neighborhood" />
                    <asp:BoundField DataField="city" HeaderText="Cidade" SortExpression="City" />
                    <asp:BoundField DataField="state" HeaderText="Estado" SortExpression="State" />
                    <asp:BoundField DataField="orderStatus.name" HeaderText="Status" SortExpression="OrderStatus" />

                    <asp:CommandField ShowEditButton="False" ShowDeleteButton="False" />
                </Columns>
            </asp:GridView>


        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="updPanel">
        <ProgressTemplate>
            <div id="exampleModalLive" class="modal fade show" tabindex="-1" role="dialog" aria-labelledby="exampleModalLiveLabel" style="padding-right: 19px; display: block;">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-body">
                            <p>Processando...</p>
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal-backdrop fade show"></div>
        </ProgressTemplate>
    </asp:UpdateProgress>


</asp:Content>
