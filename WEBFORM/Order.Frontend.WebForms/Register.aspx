<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Order.Frontend.WebForms.Register" Async="true" %>

<!DOCTYPE html>
<html lang="pt">
<head>
    <title>Registrar </title>
    <link rel="stylesheet" href="Content/bootstrap.min.css" />
</head>
<body>
    <form id="form1" runat="server" class="container">
        <asp:ScriptManager runat="server"></asp:ScriptManager>

        <asp:UpdatePanel ID="updPanel" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="container body-content">
                    <div class="row justify-content-center mt-5">
                        <div class="col-md-5 col-lg-4 col-sm-12">
                            <h2 class="text-center">Novo Usuário</h2>
                            <div class="form-group">
                                <label for="txtUsername">Nome</label>
                                <asp:TextBox ID="txtName" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="txtUsername">E-mail</label>
                                <asp:TextBox ID="txtUsername" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label for="txtPassword">Senha</label>
                                <asp:TextBox ID="txtPassword" CssClass="form-control" runat="server" TextMode="Password"></asp:TextBox>
                            </div>

                            <div role="alert" id="lblMessage" runat="server" visible="false">
                            </div>

                            
                            <div class="mt-1">
                                <asp:Button ID="btnRegister" CssClass="btn btn-primary btn-block col-12" runat="server" Text="Entrar" OnClick="btnRegister_Click" />
                            </div>
                            <div class="mt-1">
                                <a class="btn btn-light btn-block col-12" runat="server" href="~/Login">Voltar</a>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnRegister" />
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

    </form>
</body>
</html>
