<%@ Page Title="" Language="C#" MasterPageFile="~/admin/Adm.Master" AutoEventWireup="true" CodeBehind="cadastro_cliente.aspx.cs" Inherits="elivro.admin.cadastro_cliente" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Livros</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Begin Page Content -->
    <div class="cart-table-area section-padding-100">
        <div class="container-fluid">
            <div class="row">
                <div class="col-12 col-lg-8">
                    <div class="checkout_details_area mt-50 clearfix">

                        <div class="text-center">
                            <h1 id="idTitle" runat="server" class="h4 text-gray-900 mb-4">Cadastro de Endereço</h1>
                        </div>

                        <div class="cart-subtitle">
                            <h5>Dados Pessoais</h5>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-6 mb-3 mb-sm-0">
                                <asp:Label ID="idLinhaCodigo" runat="server" Visible="false">ID do Cliente: </asp:Label>
                            </div>
                            <div class="col-sm-6">
                                <asp:TextBox ID="txtIdClientePF" runat="server" CssClass="form-control" Visible="false"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group row">
                            <div class="col-md-6 mb-3">
                                <asp:TextBox ID="txtNome" runat="server" CssClass="form-control form-control-user" Placeholder="Nome"></asp:TextBox>
                            </div>
                            <div class="col-md-6 mb-3">
                                <asp:TextBox ID="txtSobrenome" runat="server" CssClass="form-control form-control-user" Placeholder="Sobrenome"></asp:TextBox>
                            </div>
                            <div class="col-md-6 mb-3">
                                <asp:TextBox ID="txtCPF" runat="server" CssClass="form-control form-control-user" Placeholder="CPF"></asp:TextBox>
                            </div>

                            <div class="col-md-6 mb-3">
                                <asp:TextBox ID="txtDtNascimento" type="date" runat="server" CssClass="form-control form-control-user" Placeholder="Data de Nascimento"></asp:TextBox>
                            </div>

                            <div class="col-12 mb-3">
                                <asp:DropDownList ID="dropIdGenero" CssClass="form-control form-control-user" runat="server" Enabled="true"></asp:DropDownList>
                            </div>
                        </div>
                        <asp:TextBox ID="txtIdTelefone" type="hidden" runat="server" CssClass="form-control form-control-user" Placeholder="ID Telefone"></asp:TextBox>
                        <div class="form-group row">
                            <div class="col-sm-4 mb-3 mb-sm-0">
                                <asp:DropDownList ID="dropIdTipoTelefone" CssClass="form-control form-control-user" runat="server" Enabled="true"></asp:DropDownList>
                            </div>
                            <div class="col-sm-4 mb-3 mb-sm-0">
                                <div class="input-group-append">
                                    <asp:TextBox ID="txtDDD" runat="server" CssClass="form-control form-control-user" Placeholder="DDD"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-4">
                                <div class="input-group-append">
                                    <asp:TextBox ID="txtTelefone" runat="server" CssClass="form-control form-control-user" Placeholder="Nº do Telefone"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-12 mb-3">
                            <asp:TextBox ID="txtEmail" type="email" runat="server" CssClass="form-control form-control-user" Placeholder="E-mail"></asp:TextBox>
                        </div>
                    </div>
                    <div id="idEnderecoCabecalho">
                        <hr class="sidebar-divider" />
                        <div class="sidebar-heading mb-3">
                            Dados de Endereço
                        </div>

                        <asp:TextBox ID="txtIdEndereco" type="hidden" runat="server" CssClass="form-control form-control-user" Placeholder="ID Endereco"></asp:TextBox>
                        <div class="form-group">
                            <asp:TextBox ID="txtNomeEndereco" runat="server" CssClass="form-control form-control-user" Placeholder="Atribua um nome para o endereço"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:TextBox ID="txtNomeDestinatario" runat="server" CssClass="form-control form-control-user" Placeholder="Nome do destinatário"></asp:TextBox>
                        </div>
                    </div>

                    <div class="form-group">
                        <asp:DropDownList ID="dropIdTipoResidencia" CssClass="form-control form-control-user" runat="server" Enabled="true"></asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <asp:DropDownList ID="dropIdTipoLogradouro" CssClass="form-control form-control-user" runat="server" Enabled="true"></asp:DropDownList>
                    </div>

                    <div id="idEnderecoDados">
                        <div class="form-group row">
                            <div class="col-sm-6 mb-3 mb-sm-0">
                                <div class="input-group-append">
                                    <asp:TextBox ID="txtRua" runat="server" CssClass="form-control form-control-user" Placeholder="Rua"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-6 mb-3 mb-sm-0">
                                <div class="input-group-append">
                                    <asp:TextBox ID="txtNumero" runat="server" CssClass="form-control form-control-user" Placeholder="Número"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-6 mb-3 mb-sm-0">
                                <div class="input-group-append">
                                    <asp:TextBox ID="txtBairro" runat="server" CssClass="form-control form-control-user" Placeholder="Bairro"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <div class="input-group-append">
                                    <asp:TextBox ID="txtCEP" runat="server" CssClass="form-control form-control-user" Placeholder="CEP"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:TextBox ID="txtObservacao" runat="server" CssClass="form-control form-control-user" Placeholder="Observação"></asp:TextBox>
                        </div>
                    </div>
                    <asp:UpdatePanel ID="upDados" runat="server">
                        <ContentTemplate>
                            <div class="form-group row">
                                <div class="col-sm-4 mb-3 mb-sm-0">
                                    <asp:DropDownList AutoPostBack="true" ID="dropIdPais" CssClass="form-control form-control-user" runat="server" Enabled="true" OnSelectedIndexChanged="dropIdPais_SelectedIndexChanged"></asp:DropDownList>
                                </div>
                                <div class="col-sm-4 mb-3 mb-sm-0">
                                    <asp:DropDownList AutoPostBack="true" ID="dropIdEstado" CssClass="form-control form-control-user" runat="server" Enabled="false" OnSelectedIndexChanged="dropIdEstado_SelectedIndexChanged"></asp:DropDownList>
                                </div>
                                <div class="col-sm-4 mb-3 mb-sm-0">
                                    <asp:DropDownList AutoPostBack="true" ID="dropIdCidade" CssClass="form-control form-control-user" runat="server" Enabled="false"></asp:DropDownList>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="form-group row">
                        <div class="col-sm-4 mb-3 mb-sm-0">
                            <asp:Button runat="server" Text="Cancelar" Visible="true" ID="btnCancelar" CssClass="btn btn-danger btn-user btn-block" OnClick="btnCancelar_Click" />
                        </div>
                        <div class="col-sm-4">
                            <asp:Button runat="server" Text="Cadastrar" Visible="true" ID="btnCadastrar" CssClass="btn btn-success btn-user btn-block" OnClick="btnCadastrar_Click" />

                            <asp:Button runat="server" Text="Alterar" Visible="false" ID="btnAlterar" CssClass="btn btn-success btn-user btn-block" OnClick="btnAlterar_Click" />
                        </div>
                    </div>
                    <asp:Label ID="lblResultado" CssClass="text-danger" runat="server" Visible="false"></asp:Label>
                </div>
            </div>

        </div>

    </div>
</asp:Content>
