﻿<%@ Page Title="" Language="C#" MasterPageFile="~/admin/Adm.Master" AutoEventWireup="true" CodeBehind="AtivacaoLivro.aspx.cs" Inherits="elivro.admin.AtivacaoLivro" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <title>Livros - Cadastro de Cartão de Crédito</title>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- Begin Page Content -->
    <div class="container-fluid">

        <div class="text-center">
            <h1 id="idTitle" runat="server" class="h4 text-gray-900 mb-4"></h1>
        </div>

        <hr class="sidebar-divider" />
        <div class="sidebar-heading mb-3" id="idSubTitle" runat="server">
        </div>
        <div class="form-group row">
            <div class="col-sm-6 mb-3 mb-sm-0">
                <asp:Label ID="idLivro" runat="server" Visible="true">ID do Livro: </asp:Label>
            </div>
            <div class="col-sm-6">
                <asp:TextBox ID="txtIdLivro" runat="server" CssClass="form-control" Visible="true"></asp:TextBox>
            </div>
        </div>

        <div class="form-group">
            <asp:DropDownList ID="dropIdCategoriaMotivo" type="text" CssClass="form-control form-control-user" runat="server" Enabled="true"></asp:DropDownList>
        </div>
        
        <div class="form-group">
            <asp:TextBox ID="txtMotivo" runat="server" CssClass="form-control form-control-user" ></asp:TextBox>
        </div>

        <div class="form-group row">
            <div class="col-sm-4 mb-3 mb-sm-0">
                <asp:Button runat="server" Text="Cancelar" Visible="true" ID="btnCancelar" CssClass="btn btn-danger btn-user btn-block" OnClick="btnCancelar_Click" />
            </div>
            <div class="col-sm-4">
                <asp:Button runat="server" Visible="true" ID="btnAtivarInativar" CssClass="btn btn-success btn-user btn-block" OnClick="btnAtivarInativar_Click" />
            </div>
        </div>
        <asp:Label ID="lblResultado" CssClass="text-danger" runat="server" Visible="false"></asp:Label>
    </div>
</asp:Content>
