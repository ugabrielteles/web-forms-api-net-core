<%@ Page Title="Kanban" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="KanbanOrders.aspx.cs" Inherits="Order.Frontend.WebForms.Order.KanbanOrders" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="updPanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div role="alert" id="lblMessage" runat="server" visible="false">
            </div>
            <div class="container">
                <h2 class="mt-3">Painel de Ordens</h2>
                <hr />
                <div class="kanban-board row">
                    <asp:HiddenField ID="hdnOrderId" runat="server" />
                    <asp:HiddenField ID="hdnNewStatus" runat="server" />

                    <asp:Button ID="btnAtualizarStatus" runat="server" OnClick="btnAtualizarStatus_Click" Style="display: none;" />

                    <asp:Repeater ID="rptKanban" runat="server">
                        <ItemTemplate>
                            <div class="kanban-column ml-1" data-status='<%# Eval("OrderStatusId") %>'>
                                <h4><%# Eval("Name") %></h4>
                                <div class="kanban-items">
                                    <asp:Repeater ID="rptOrders" runat="server" DataSource='<%# Eval("Orders") %>'>
                                        <ItemTemplate>
                                            <div class="kanban-item" data-order-status-id='<%# Eval("OrderStatusId") %>' draggable="true" data-order-id='<%# Eval("OrderId") %>'>
                                                Pedido #<%# Eval("OrderId") %> - R$ <%# Eval("Value") %>

                                                <%# Eval("deliveryOrder") != null && (int)Eval("OrderStatusId") == 4 ? "<hr/> Data da Entrega: <br>" + Eval("deliveryOrder.deliveryDate") :""%>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnAtualizarStatus" />
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

    <script src="https://cdnjs.cloudflare.com/ajax/libs/microsoft-signalr/3.1.18/signalr.min.js"></script>

    <script>
        $(document).ready(function () {
            let draggedItem = null;

            $(document).on("dragstart", ".kanban-item", function (event) {
                draggedItem = this;
                console.log($(draggedItem).data("order-status-id"))
                setTimeout(() => $(this).hide(), 0);
            });

            $(document).on("dragend", ".kanban-item", function () {
                setTimeout(() => $(this).show(), 0);
            });

            $(".kanban-items").on("dragover", function (event) {
                event.preventDefault();
                $(this).addClass("drag-over");
            });

            $(".kanban-items").on("dragleave", function () {
                $(this).removeClass("drag-over");
            });

            $(".kanban-items").on("drop", function (event) {
                event.preventDefault();
                $(this).removeClass("drag-over");
                $(this).append(draggedItem);

                let orderId = $(draggedItem).data("order-id");
                let newStatus = $(this).closest(".kanban-column").data("status");

                atualizarStatus(orderId, newStatus);
            });


            const connection = new signalR.HubConnectionBuilder()
                .withUrl("https://localhost:7139/hubs/order")
                .build();

            connection.on("ReceiveMessage", (message) => {

                const container = $("div").find('[data-status="1"]')
                var order = JSON.parse(message);

                var orderEle = '<div class="kanban-item" data-order-status-id="1" draggable="true" data-order-id="'+ order.OrderId +'"> Pedido #'+ order.OrderId +' - R$ '+order.Value.toFixed(2) +' </div>'

                $(container).find('.kanban-items').append(orderEle)
            });

            connection.start()
                .then(() => {
                    console.log("SignalR connection started.");
                })
                .catch(err => {
                    console.error("Error connecting to SignalR: ", err);
                });

            window.addEventListener("beforeunload", () => {
                connection.stop().then(() => {
                    console.log("SignalR connection stopped.");
                }).catch(err => {
                    console.error("Error stopping SignalR connection: ", err);
                });
            });



        });

        function atualizarStatus(orderId, newStatus) {
            document.getElementById('<%= hdnOrderId.ClientID %>').value = orderId;
            document.getElementById('<%= hdnNewStatus.ClientID %>').value = newStatus;
            document.getElementById('<%= btnAtualizarStatus.ClientID %>').click();
        }
    </script>

</asp:Content>
