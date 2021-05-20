<%@ Page Title="" Language="C#" MasterPageFile="~/store/Store.Master" AutoEventWireup="true" CodeBehind="CheckoutReview.aspx.cs" Inherits="elivro.Checkout.CheckoutReview" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="cart-table-area section-padding-100">
        <div class="container-fluid">
            <div class="row">
                <div class="col-7">
                    <div class="cart-title mt-50">
                        <h2>Livros: </h2>
                    </div>
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



                    <div class="col-9">
                        <br />
                        <div class="text" id="lblDestinatario" runat="server"></div>
                        <div class="text" id="lblEndereco" runat="server"></div>
                        <br />
                        <div class="text" id="lblStatus" runat="server"></div>
                        
                        <br />
                        <br />
                        
                    <div class="form-group row mb-3">
                        <a id="TrocaPedido" runat="server" title='Trocar' class='btn amado-btn'>Trocar</a>
                    </div>
                    </div>
                </div>
                <div class="col-12 col-lg-4">
                    <div class="cart-summary" id="CartTotal" runat="server">
                        <h5>Total: </h5>
                        <ul class="summary-table">
                            <li>
                                <span>
                                    <asp:Label ID="LabelSubTotalText" runat="server" Text=""></asp:Label>
                                </span>
                                <span>
                                    <asp:Label ID="LabelFreteText" runat="server" Text=""></asp:Label>
                                </span>
                                <span>
                                    <asp:Label ID="LabelTotalText" runat="server" Text=""></asp:Label>
                                </span>
                            </li>
                            <%--<li><span>delivery:</span> <span>Free</span></li>
                            <li><span>total:</span> <span>$140.00</span></li>--%>
                        </ul>
                    </div>
                </div>

            </div>

        </div>

    </div>
</asp:Content>
