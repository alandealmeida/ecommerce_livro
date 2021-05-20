<%@ Page Title="" Language="C#" MasterPageFile="~/store/Store.Master" AutoEventWireup="true" CodeBehind="ProductDetails.aspx.cs" Inherits="elivro.store.ProductDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
        <!-- Product Details Area Start -->

        <div class="single-product-area section-padding-100 clearfix">
            <div class="container-fluid">
                <!-- Para conter o ID do Livro -->
                <asp:TextBox ID="txtIdLivro" runat="server" CssClass="form-control" Visible="false"></asp:TextBox>

                <div class="row">
                    <div class="col-12 col-lg-7">
                        <div class="single_product_thumb">
                            <div id="product_details_slider" class="carousel slide" data-ride="carousel">
                                <div class="carousel-inner">
                                    <div class="carousel-item active">
                                        <a class="gallery_img" <%--href="../img/SEM-IMAGEM.jpg"--%>>
                                            <%--<img class="d-block w-100" src="../img/SEM-IMAGEM.jpg" alt="First slide">--%>
                                            <asp:Image ID="imgLivro" runat="server" />
                                        </a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-12 col-lg-5">
                        <div class="single_product_desc">
                            <!-- Product Meta Data -->
                            <div class="product-meta-data">
                                <div class="line"></div>
                                <p class="product-price" id="txtPrecoLivro" runat="server"></p>
                                <p class="product-price" id="txtSemEstoque" runat="server"></p>
                                <a>
                                    <h6 id="txtTituloLivro" runat="server"></h6>
                                </a>
                            </div>

                            <div class="short_overview my-5">
                                <p id="txtDescricao" runat="server"></p>
                            </div>

                            <!-- Add to Cart Form -->
                            <form class="cart clearfix" method="post">
                                <div class="cart-btn d-flex mb-50">
                                    <p>Qtde</p>
                                    <div class="quantity">
                                        <span class="qty-minus" onclick="var effect = document.getElementById('qty'); var qty = effect.value; if( !isNaN( qty ) &amp;&amp; qty &gt; 1 ) effect.value--;return false;"><i class="fa fa-caret-down" aria-hidden="true"></i></span>
                                        <input type="number" class="qty-text" id="qty" step="1" min="1" max="300" name="quantity" value="1"/>
                                        <span class="qty-plus" onclick="var effect = document.getElementById('qty'); var qty = effect.value; if( !isNaN( qty )) effect.value++;return false;"><i class="fa fa-caret-up" aria-hidden="true"></i></span>
                                    </div>
                                </div>
                                <div id="btnAddToCart" runat="server"></div>
                            </form>

                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- Product Details Area End -->

</asp:Content>
