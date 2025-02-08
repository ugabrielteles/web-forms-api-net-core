<%@ Page Title="Nova Ordem" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NewOrder.aspx.cs" Inherits="Order.Frontend.WebForms.Order.NewOrder" Async="true"%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <asp:UpdatePanel ID="updPanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="row">
                <h2>Nova Ordem</h2>
                <hr />


                <div class="form-group col-md-6 col-sm-12">
                    <label for="txtDescription">Descrição</label>
                    <asp:TextBox ID="txtDescription" CssClass="form-control" runat="server"></asp:TextBox>
                </div>


                <div class="form-group col-md-6 col-sm-12">
                    <label for="txtValue">Valor</label>
                    <asp:TextBox ID="txtValue" CssClass="form-control" runat="server"></asp:TextBox>
                </div>

                
                <div class="form-group col-md-3 col-sm-12">
                    <label for="txtZipCode">CEP</label>
                    <asp:TextBox ID="txtZipCode" CssClass="form-control" runat="server" AutoPostBack="True" OnTextChanged="txtZipCode_TextChanged"></asp:TextBox>
                </div>


                <div class="form-group col-md-7 col-sm-12">
                    <label for="txtStreet">Rua</label>
                    <asp:TextBox ID="txtStreet" CssClass="form-control" runat="server"></asp:TextBox>
                </div>

                
                <div class="form-group col-md-2 col-sm-12">
                    <label for="txtNumber">Número</label>
                    <asp:TextBox ID="txtNumber" CssClass="form-control" runat="server"></asp:TextBox>
                </div>

                <div class="form-group col-md-4 col-sm-12">
                    <label for="txtNeighborhood">Bairro</label>
                    <asp:TextBox ID="txtNeighborhood" CssClass="form-control" runat="server"></asp:TextBox>
                </div>

                <div class="form-group  col-md-4 col-sm-12">
                    <label for="txtCity">Cidade</label>
                    <asp:TextBox ID="txtCity" CssClass="form-control" runat="server"></asp:TextBox>
                </div>

                <div class="form-group  col-md-4 col-sm-12">
                    <label for="txtState">Estado</label>
                    <asp:TextBox ID="txtState" CssClass="form-control" runat="server"></asp:TextBox>
                </div>

                <div role="alert" id="lblMessage" runat="server" visible="false">
                </div>

                <div class="form-group mt-2 text-rigth">
                    <asp:Button ID="btnSaveOrder" CssClass="btn btn-primary btn-block" runat="server" Text="Salvar Ordem" OnClick="btnSaveOrder_Click" />

                    <a class="btn btn-light btn-block" href="~/Order/ListOrders" runat="server">Voltar</a>
                </div>


            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSaveOrder" />
            <asp:AsyncPostBackTrigger ControlID="txtZipCode" EventName="TextChanged"/>
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
