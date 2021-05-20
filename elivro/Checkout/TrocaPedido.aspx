<%@ Page Title="" Language="C#" MasterPageFile="~/store/Store.Master" AutoEventWireup="true" CodeBehind="TrocaPedido.aspx.cs" Inherits="elivro.Checkout.TrocaPedido" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="cart-table-area section-padding-100">
        <div class="container-fluid">
            <div class="row">
                <div class="col-12 col-lg-8">
                    <div class="cart-title mt-50">
                        <h1>Informações do Pedido</h1>
                    </div>
                    <h3 style="padding-left: 33px">Livros:</h3>
                    <div class="card shadow mb-4">
                        <div class="cart-table clearfix">
                            <div class="table-responsive">
                                <div id="divTable" class="table table-bordered" runat="server">
                                </div>
                                <asp:GridView runat="server" CssClass="display" ID="GridViewGeral" EnableModelValidation="True" Width="200px">
                                    <HeaderStyle Font-Bold="true" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                    <br />
                    <div class="form-group row mb-3">
                        <h3>Trocar todo o pedido? &nbsp;</h3>
                        <a id="TrocaPedidoTodo" runat="server" title='Trocar' class='btn amado-btn'>Trocar</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
