﻿<%@ Page Title="" Language="C#" MasterPageFile="~/store/Store.Master" AutoEventWireup="true" CodeBehind="ListaMeusCupons.aspx.cs" Inherits="elivro.Account.ListaMeusCupons" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="cart-table-area section-padding-100">
        <div class="container-fluid">
            <div class="row">
                <div class="col-12 col-lg-8">
                    <div class="cart-title mt-50">
                        <h1>Meus Cupons</h1>
                    </div>
                    <div class="card shadow md-auto">
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
                </div>
            </div>
        </div>
    </div>
</asp:Content>
