﻿<%@ Page Title="Registre-se" Language="C#" MasterPageFile="~/store/Store.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="elivro.Account.Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="cart-table-area section-padding-100">
        <div class="container-fluid">
            <div class="row">
                <div class="col-12 col-lg-8">

                    <div class="cart-title mt-50">
                        <h2><%: Title %>.</h2>
                    </div>

                    <p class="text-danger">
                        <asp:Literal runat="server" ID="ErrorMessage" />
                    </p>

                    <asp:Label ID="lblResultado" CssClass="text-danger" runat="server" Visible="false"></asp:Label>

                    <div class="form-horizontal">
                        <h4>Criar uma nova conta</h4>

                        <hr class="sidebar-divider" />
                        <div class="sidebar-heading mb-3">Dados Pessoais</div>

                        <div class="form-group row">
                            <div class="col-sm-6 mb-3 mb-sm-0">
                                <asp:TextBox ID="txtNome" runat="server" CssClass="form-control form-control-user" Placeholder="Nome"></asp:TextBox>
                            </div>
                            <div class="col-sm-6">
                                <asp:TextBox ID="txtSobrenome" runat="server" CssClass="form-control form-control-user" Placeholder="Sobrenome"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-6 mb-3 mb-sm-0">
                                <asp:TextBox ID="txtCPF" runat="server" CssClass="form-control form-control-user" Placeholder="CPF"></asp:TextBox>
                            </div>
                            <div class="col-sm-6">
                                <div class="input-group-append">
                                    <asp:TextBox ID="txtDtNascimento" type="date" runat="server" CssClass="form-control form-control-user" Placeholder="Data de Nascimento"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-6 mb-3 mb-sm-0">
                                <asp:DropDownList ID="dropIdGenero" CssClass="form-control form-control-user" runat="server" Enabled="true"></asp:DropDownList>
                            </div>
                        </div>



                        <hr class="sidebar-divider" />
                        <div class="sidebar-heading mb-3">Dados de Contato</div>

                        <asp:TextBox ID="txtIdTelefone" type="hidden" runat="server" CssClass="form-control form-control-user" Placeholder="ID Telefone"></asp:TextBox>
                        <div class="form-group row">
                            <div class="col-sm-4 mb-3 mb-sm-0">
                                <asp:DropDownList ID="dropIdTipoTelefone" CssClass="form-control form-control-user" runat="server" Enabled="true"></asp:DropDownList>
                            </div>
                            <div class="col-sm-4 mb-3 mb-sm-0">
                                <div class="input-group-append">
                                    <asp:TextBox ID="txtDDD" runat="server" min ="0" max="99" CssClass="form-control form-control-user" Placeholder="DDD"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-4">
                                <div class="input-group-append">
                                    <asp:TextBox ID="txtTelefone" runat="server" CssClass="form-control form-control-user" Placeholder="Nº do Telefone"></asp:TextBox>
                                </div>
                            </div>
                        </div>


                        <hr class="sidebar-divider" />
                        <hr class="sidebar-divider" />
                        <div class="sidebar-heading mb-3">
                            Dados de Endereço
                        </div>

                        <asp:TextBox ID="txtIdEndereco" type="hidden" runat="server" CssClass="form-control form-control-user" Placeholder="ID Endereco"></asp:TextBox>

                        <div class="form-group row">
                            <div class="col-sm-6 mb-3 mb-sm-0">
                                <asp:TextBox ID="txtNomeEndereco" runat="server" CssClass="form-control form-control-user" Placeholder="Atribua um nome para o endereço"></asp:TextBox>
                            </div>
                            <div class="col-sm-6 mb-3 mb-sm-0">
                                <asp:TextBox ID="txtNomeDestinatario" runat="server" CssClass="form-control form-control-user" Placeholder="Nome do destinatário"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group row">
                            <div class="col-sm-6 mb-3 mb-sm-0">
                                <asp:DropDownList ID="dropIdTipoResidencia" CssClass="form-control form-control-user" runat="server" Enabled="true"></asp:DropDownList>
                            </div>
                            <div class="col-sm-6 mb-3 mb-sm-0">
                                <asp:DropDownList ID="dropIdTipoLogradouro" CssClass="form-control form-control-user" runat="server" Enabled="true"></asp:DropDownList>
                            </div>
                        </div>

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
                            <div class="col-sm-6 mb-3 mb-sm-0">
                                <asp:TextBox ID="txtObservacao" runat="server" CssClass="form-control form-control-user" Placeholder="Observação"></asp:TextBox>
                            </div>
                        </div>

                        <hr class="sidebar-divider" />
                        <div class="sidebar-heading mb-3">Dados de Acesso</div>

                        <asp:ValidationSummary runat="server" CssClass="text-danger" />
                        <div class="form-group">
                            <asp:Label runat="server" AssociatedControlID="Email" CssClass="col-md-2 control-label">E-mail</asp:Label>
                            <div class="col-md-10">
                                <asp:TextBox runat="server" ID="Email" CssClass="form-control" TextMode="Email" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="Email"
                                    CssClass="text-danger" ErrorMessage="O campo e-mail é exigido." />
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" AssociatedControlID="Password" CssClass="col-md-2 control-label">Senha</asp:Label>
                            <div class="col-md-10">
                                <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="form-control" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="Password"
                                    CssClass="text-danger" ErrorMessage="O campo de senha é obrigatório." />
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" AssociatedControlID="ConfirmPassword" CssClass="col-md-2 control-label">Confirmar senha</asp:Label>
                            <div class="col-md-10">
                                <asp:TextBox runat="server" ID="ConfirmPassword" TextMode="Password" CssClass="form-control" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="ConfirmPassword"
                                    CssClass="text-danger" Display="Dynamic" ErrorMessage="O campo para confirmar senha é obrigatório." />
                                <asp:CompareValidator runat="server" ControlToCompare="Password" ControlToValidate="ConfirmPassword"
                                    CssClass="text-danger" Display="Dynamic" ErrorMessage="A senha e a senha de confirmação não coincidem." />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-offset-2 col-md-10">
                                <asp:Button runat="server" OnClick="CreateUser_Click" Text="Registre-se" CssClass="btn amado-btn w-100" />
                            </div>
                        </div>

                    </div>

                </div>
            </div>
        </div>
    </div>

</asp:Content>
