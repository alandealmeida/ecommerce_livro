﻿<%@ Page Title="" Language="C#" MasterPageFile="~/admin/Adm.Master" AutoEventWireup="true" CodeBehind="lista_estoque.aspx.cs" Inherits="elivro.admin.lista_estoque" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <title>Livros - Lista de Estoques</title>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- Begin Page Content -->
    <div class="container-fluid">

        <!-- Page Heading -->
        <h1 class="h3 mb-2 text-gray-800">Lista de Estoques</h1>

        <!-- DataTales Example -->
        <div class="card shadow mb-4">
            <div class="card-header py-3">
                <h6 class="m-0 font-weight-bold text-primary">Lista de Estoques
                </h6>
            </div>
            <div class="card-body">
                <div class="table-responsive">
                    <asp:Panel runat="server" GroupingText="Filtro" >
                        Ativo/Inativo: <asp:DropDownList AutoPostBack="true" ID="dropAtivo" DataTextField="Name" DataValueField="ID" CssClass="form-control" runat="server" OnSelectedIndexChanged="DropAtivo_SelectedIndexChanged"></asp:DropDownList>
                        Categoria Ativação/Inativação: <asp:DropDownList AutoPostBack="true" ID="dropIdCategoriaMotivo" DataTextField="Name" DataValueField="ID" CssClass="form-control" runat="server" OnSelectedIndexChanged="DropIdCategoriaMotivo_SelectedIndexChanged" Enabled="false"></asp:DropDownList>
                    </asp:Panel>
                    
                    <br />
                    <div id="divTable" class="table table-bordered" runat="server" >
                        
                    </div>
                    <asp:GridView runat="server" CssClass="display" ID="GridViewGeral" EnableModelValidation="True" Width="204px" >
                        <HeaderStyle Font-Bold="true" />
                    </asp:GridView >

                </div>
                <asp:Label id="lblResultado" runat="server" Visible="false"></asp:Label>
            </div>
            
            <div class="card-footer small text-muted" id="lblRodaPeTabela" runat="server"></div>
        </div>

    </div>

</asp:Content>
