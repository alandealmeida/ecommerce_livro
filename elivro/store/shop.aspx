<%@ Page Title="" Language="C#" MasterPageFile="~/store/Store.Master" AutoEventWireup="true" CodeBehind="shop.aspx.cs" Inherits="elivro.store.shop" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="shop_sidebar_area">

        <!-- CATEGORIA -->
        <!-- ##### Single Widget ##### -->
        <div class="widget brands mb-50">
            <!-- Widget Title -->
            <h6 class="widget-title mb-30">Categorias</h6>

            <div class="widget-desc">
                <!-- Single Form Check -->
                <asp:CheckBoxList AutoPostBack="true" ID="checkBoxIdCategoria" CssClass="form-check" DataTextField="Name" DataValueField="ID" runat="server" OnSelectedIndexChanged="CheckBoxIdCategoriaCheckedChanged"></asp:CheckBoxList>

            </div>
        </div>

        <!-- ##### Single Widget ##### -->
        <div class="widget catagory mb-50">
            <!-- Widget Title -->
            <h6 class="widget-title mb-30">Editora</h6>

            <!--  Catagories  -->
            <asp:DropDownList AutoPostBack="true" ID="dropIdEditora" DataTextField="Name" DataValueField="ID" CssClass="form-control" runat="server" OnSelectedIndexChanged="DropIdEditoraSelectedIndexChanged"></asp:DropDownList>
        </div>     
    </div>

    <div class="amado_product_area section-padding-100">
        <div class="container-fluid">

            <div class="row">
                <div class="col-12">
                    <div class="product-topbar d-xl-flex align-items-end justify-content-between">
                        <!-- Total Products -->
                        <div class="total-products">
                            <p>Showing 1-8 0f 25</p>
                            <div class="view d-flex">
                                <a href="#"><i class="fa fa-th-large" aria-hidden="true"></i></a>
                                <a href="#"><i class="fa fa-bars" aria-hidden="true"></i></a>
                            </div>
                        </div>
                        <!-- Sorting -->
                        <div class="product-sorting d-flex">
                            <div class="sort-by-date d-flex align-items-center mr-15">
                                <p>Sort by</p>
                                <form action="#" method="get">
                                    <select name="select" id="sortBydate">
                                        <option value="value">Date</option>
                                        <option value="value">Newest</option>
                                        <option value="value">Popular</option>
                                    </select>
                                </form>
                            </div>
                            <div class="view-product d-flex align-items-center">
                                <p>View</p>
                                <form action="#" method="get">
                                    <select name="select" id="viewProduct">
                                        <option value="value">12</option>
                                        <option value="value">24</option>
                                        <option value="value">48</option>
                                        <option value="value">96</option>
                                    </select>
                                </form>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div id="divConteudo" runat="server" class="row"></div>

            <div class="row">
                <div class="col-12">
                    <!-- Pagination -->
                    <nav aria-label="navigation">
                        <ul class="pagination justify-content-end mt-50">
                            <li class="page-item active"><a class="page-link" href="#">01.</a></li>
                            <li class="page-item"><a class="page-link" href="#">02.</a></li>
                            <li class="page-item"><a class="page-link" href="#">03.</a></li>
                            <li class="page-item"><a class="page-link" href="#">04.</a></li>
                        </ul>
                    </nav>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
