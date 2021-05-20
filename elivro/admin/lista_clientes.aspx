<%@ Page Title="" Language="C#" MasterPageFile="~/admin/Adm.Master" AutoEventWireup="true" CodeBehind="lista_clientes.aspx.cs" Inherits="elivro.admin.lista_clientes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <title>Livros - Lista de Clientes</title>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- Begin Page Content -->
    <div class="container-fluid">
        <!-- Page Heading -->
        <h1 class="h3 mb-2 text-gray-800">Lista de Clientes</h1>

        <!-- DataTales Example -->
        <div class="card shadow mb-4">
            <div class="card-header py-3">
                <h6 class="m-0 font-weight-bold text-primary">Lista de Clientes
                
                    <!-- Botão para adição de cliente -->
                    <a class="btn btn-primary fa-pull-right" href="cadastro_cliente.aspx" title="Novo Cliente">
                        <div class="fas fa-fw fa-plus"></div>
                    </a>

                </h6>
            </div>
            <div class="card-body">
                <div class="table-responsive">
                    <div id="divTable" class="table table-bordered" runat="server">
                    </div>
                    <asp:GridView runat="server" CssClass="display" ID="GridViewGeral" EnableModelValidation="True" Width="204px">
                        <HeaderStyle Font-Bold="true" />
                    </asp:GridView>
                </div>
                <asp:Label ID="lblResultado" runat="server" Visible="false"></asp:Label>
            </div>

            <div class="card-footer small text-muted" id="lblRodaPeTabela" runat="server"></div>
        </div>

    </div>
    <!-- /.container-fluid -->

</asp:Content>
