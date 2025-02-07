<%@ Page Title="Home" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Order.Frontend.WebForms._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <section class="row" aria-labelledby="aspnetTitle">
            <h1 id="aspnetTitle">Order</h1>
            <p class="lead">Bem vindo ao projeto Order</p>
        </section>

        <div class="row">
            <section class="col-md-6" aria-labelledby="gettingStartedTitle">
                <h2 id="lista">Visualizar lista de ordens</h2>
                <p>
                    <a class="btn btn-default" runat="server" href="~/Order/ListOrders">Acesar &raquo;</a>
                </p>
            </section>
            <section class="col-md-6" aria-labelledby="librariesTitle">
                <h2 id="create">Cadastrar nova ordem</h2>
                <p>
                    <a class="btn btn-default" runat="server" href="~/Order/NewOrder">Acessar &raquo;</a>
                </p>
            </section>

            <section class="col-md-6" aria-labelledby="librariesTitle">
                <h2 id="kanbanOrders">Painel de Ordens</h2>
                <p>
                    <a class="btn btn-default" runat="server" href="~/Order/KanbanOrders">Acessar &raquo;</a>
                </p>
            </section>
        </div>
    </main>

</asp:Content>
