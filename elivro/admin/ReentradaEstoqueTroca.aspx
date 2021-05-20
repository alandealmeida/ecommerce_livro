<%@ Page Title="" Language="C#" MasterPageFile="~/admin/Adm.Master" AutoEventWireup="true" CodeBehind="ReentradaEstoqueTroca.aspx.cs" Inherits="elivro.admin.ReentradaEstoqueTroca" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <title>Livros - Buttons</title>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- Begin Page Content -->
    <div class="container-fluid">

        <!-- Page Heading -->
        <h1 class="h3 mb-4 text-gray-800">Reentrada no Estoque</h1>

        <div class="row">
            <div class="card shadow mb-4">
                <div class="card-header py-3">
                    <h6 class="m-0 font-weight-bold text-primary">Reentrada no Estoque</h6>
                </div>
                <div class="card-body">
                    <p>Retornar livro(s) trocado(s) para estoque?</p>
                    <div class="row">
                        <div class="my-2"></div>
                        <asp:LinkButton ID="btnSim" runat="server" CssClass="btn btn-success btn-icon-split btn-lg " OnClick="btnSim_Click">
                            <span class="icon text-white-50">
                                <i class="fas fa-check"></i>
                            </span>
                            <span class="text">Sim</span>                        
                        </asp:LinkButton>
                        <div class="my-2"></div>
                        <asp:LinkButton ID="btnNao" runat="server" CssClass="btn btn-danger btn-icon-split btn-lg " OnClick="btnNao_Click">
                            <span class="icon text-white-50">
                                <i class="fas fa-trash"></i>
                            </span>
                            <span class="text">Não</span>                        
                        </asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
