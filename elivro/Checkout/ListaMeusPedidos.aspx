<%@ Page Title="" Language="C#" MasterPageFile="~/store/Store.Master" AutoEventWireup="true" CodeBehind="ListaMeusPedidos.aspx.cs" Inherits="elivro.Checkout.ListaMeusPedidos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Header Area End -->

    <div class="cart-table-area section-padding-100">
        <div class="container-fluid">
            <div class="row">
                <div class="col-9">
                    <div class="cart-title mt-50">
                        <h2>Meus Pedidos: </h2>
                    </div>
                    <div class="card shadow mb-">
                        <div class="cart-table clearfix">
                            <div class="table-responsive">
                                <div id="divTable" class="table table-bordered" runat="server">
                                </div>
                                <asp:GridView runat="server" CssClass="display" ID="GridViewGeral" EnableModelValidation="True" Width="200px">
                                    <HeaderStyle Font-Bold="true" />
                                </asp:GridView>
                            </div>
                            <asp:Label ID="lblResultado" runat="server" Visible="false"></asp:Label>
                        </div>
                    </div>

                </div>

            </div>

        </div>

    </div>
</asp:Content>
