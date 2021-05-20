<%@ Page Title="" Language="C#" MasterPageFile="~/admin/Adm.Master" AutoEventWireup="true" CodeBehind="lista_pedidos.aspx.cs" Inherits="elivro.admin.lista_pedidos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <title>Livros - Lista de Pedidos</title>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <!-- Begin Page Content -->
    <div class="container-fluid">

        <!-- Page Heading -->
        <h1 class="h3 mb-2 text-gray-800">Lista de Pedidos</h1>

        <!-- DataTales Example -->
        <div class="card shadow mb-4">
            <div class="card-header py-3">
                <h6 class="m-0 font-weight-bold text-primary">Lista de Pedidos
                </h6>
            </div>
            <div class="card-body">
                <div class="table-responsive">
                    
                    <asp:Label ID="lblResultadoAtualiza" CssClass="text-danger" runat="server" Visible="false"></asp:Label>
                    <asp:Panel runat="server" GroupingText="Filtro" >
                        Status do Pedido: <asp:DropDownList AutoPostBack="true" ID="dropIdStatus" DataTextField="Name" DataValueField="ID" CssClass="form-control" runat="server" OnSelectedIndexChanged="dropIdStatus_SelectedIndexChanged" ></asp:DropDownList>
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
