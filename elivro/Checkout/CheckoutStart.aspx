﻿<%@ Page Title="" Language="C#" MasterPageFile="~/store/Store.Master" AutoEventWireup="true" CodeBehind="CheckoutStart.aspx.cs" Inherits="elivro.Checkout.CheckoutStart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="cart-table-area section-padding-100">
        <div class="container-fluid">
            <div class="row">
                <div class="col-12 col-lg-8">
                    <div class="checkout_details_area mt-50 clearfix">

                        <div class="cart-title">
                            <h2>Checkout</h2>
                        </div>


                        <div class="cart-table clearfix">
                            <asp:GridView ID="CartList" runat="server" AutoGenerateColumns="False" GridLines="Vertical" CellPadding="4"
                                ItemType="elivro.Models.CartItem" SelectMethod="GetShoppingCartItems"
                                CssClass="table table-striped table-bordered table-responsive">
                                <Columns>
                                    <asp:BoundField DataField="livro_id" HeaderText="ID" SortExpression="livro_id" HeaderStyle-Width="15%" />
                                    <asp:BoundField DataField="titulo_livro" HeaderText="Título" HeaderStyle-Width="15%" />
                                    <asp:BoundField DataField="valor_venda" HeaderText="Valor Unit." DataFormatString="{0:c}" HeaderStyle-Width="15%" />
                                    <asp:TemplateField HeaderText="Quantidade" HeaderStyle-Width="15%">
                                        <ItemTemplate>
                                            <div class="qty">
                                                <div class="qty-btn d-flex">
                                                    <p>Qtde</p>
                                                    <div class="quantity">
                                                        <span class="qty-minus" onclick="var effect = document.getElementById('PurchaseQuantity'); var qty = effect.value; if( !isNaN( qty ) &amp;&amp; qty &gt; 1 ) effect.value--;return false;"><i class="fa fa-minus" aria-hidden="true"></i></span>
                                                        <%--<input id="PurchaseQuantity" type="number" class="qty-text" step="1" min="1" max="300" name="quantity" value="<%#: Item.quantidade %>" runat="server">--%>
                                                        <asp:TextBox ID="PurchaseQuantity" Width="40" runat="server" Text="<%#: Item.quantidade %>"></asp:TextBox>
                                                        <span class="qty-plus" onclick="var effect = document.getElementById('PurchaseQuantity'); var qty = effect.value; if( !isNaN( qty )) effect.value++;return false;"><i class="fa fa-plus" aria-hidden="true"></i></span>
                                                    </div>
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total do Item" HeaderStyle-Width="20%">
                                        <ItemTemplate>
                                            <span>
                                                <%#: String.Format("{0:c}", ((Convert.ToDouble(Item.quantidade)) *  Convert.ToDouble(Item.valor_venda)))%>
                                            </span>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Remover Item" HeaderStyle-Width="20%">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="Remover" runat="server"></asp:CheckBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <asp:Label ID="lblResultadoCarrinho" CssClass="text-danger" runat="server" Visible="false"></asp:Label>

                        <asp:Button ID="UpdateBtn" CssClass="btn amado-btn w-100 mb-4" runat="server" Text="Atualizar Carrinho" OnClick="UpdateBtn_Click" />


                        <hr class="sidebar-divider" />
                        <div class="sidebar-heading mb-3">
                            <h3>Endereço de Entrega</h3>
                        </div>
                        <div class="row">
                            <div class="col-md-12 mb-3">
                                <asp:DropDownList ID="dropIdEnderecoEntrega" CssClass="nice-select w-100" runat="server" Enabled="true" AutoPostBack="True" OnSelectedIndexChanged="dropIdEnderecoEntrega_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                        </div>



                        <div class="form-group">
                            <asp:TextBox ID="txtNomeEndereco" runat="server" CssClass="form-control form-control-user" Placeholder="Atribua um nome para o endereço" Visible="false"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:TextBox ID="txtNomeDestinatario" runat="server" CssClass="form-control form-control-user" Placeholder="Nome do destinatário" Visible="false"></asp:TextBox>
                        </div>

                        <div class="form-group">
                            <asp:DropDownList ID="dropIdTipoResidencia" CssClass="form-control form-control-user" runat="server" Enabled="true" Visible="false"></asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <asp:DropDownList ID="dropIdTipoLogradouro" CssClass="form-control form-control-user" runat="server" Enabled="true" Visible="false"></asp:DropDownList>
                        </div>

                        <div class="form-group row">
                            <div class="col-sm-6 mb-3 mb-sm-0">
                                <div class="input-group-append">
                                    <asp:TextBox ID="txtRua" runat="server" CssClass="form-control form-control-user" Placeholder="Rua" Visible="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-6 mb-3 mb-sm-0">
                                <div class="input-group-append">
                                    <asp:TextBox ID="txtNumero" runat="server" CssClass="form-control form-control-user" Placeholder="Número" Visible="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-6 mb-3 mb-sm-0">
                                <div class="input-group-append">
                                    <asp:TextBox ID="txtBairro" runat="server" CssClass="form-control form-control-user" Placeholder="Bairro" Visible="false"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <div class="input-group-append">
                                    <asp:TextBox ID="txtCEP" runat="server" CssClass="form-control form-control-user" Placeholder="CEP" Visible="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:TextBox ID="txtObservacao" runat="server" CssClass="form-control form-control-user" Placeholder="Observação" Visible="false"></asp:TextBox>
                        </div>

                        <asp:UpdatePanel ID="upDados" runat="server">
                            <ContentTemplate>
                                <div class="form-group row">
                                    <div class="col-sm-4 mb-3 mb-sm-0">
                                        <asp:DropDownList AutoPostBack="true" ID="dropIdPais" CssClass="form-control form-control-user" runat="server" Enabled="true" Visible="false" OnSelectedIndexChanged="dropIdPais_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                    <div class="col-sm-4 mb-3 mb-sm-0">
                                        <asp:DropDownList AutoPostBack="true" ID="dropIdEstado" CssClass="form-control form-control-user" runat="server" Enabled="false" Visible="false" OnSelectedIndexChanged="dropIdEstado_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                    <div class="col-sm-4 mb-3 mb-sm-0">
                                        <asp:DropDownList AutoPostBack="true" ID="dropIdCidade" CssClass="form-control form-control-user" runat="server" Enabled="false" Visible="false"></asp:DropDownList>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                        <div class="form-group">
                            <asp:CheckBox ID="chkIncluir" runat="server" Text="Incluir na lista de endereços?" Visible="false"></asp:CheckBox>
                        </div>

                        <asp:Label ID="lblIncluirEndereco" CssClass="text-danger" runat="server" Visible="false"></asp:Label>

                        <div class="form-group">
                            <asp:Button ID="btnAplicarEndereco" CssClass="btn amado-btn w-100" runat="server" Text="Aplicar Endereço" Visible="false" OnClick="btnAplicarEndereco_Click" />
                        </div>
                        <hr class="sidebar-divider" />
                        <div class="sidebar-heading mb-3">
                            <h3>Cupom Promocional</h3>
                        </div>
                        <div class="row">
                            <div class="col-md-8 mb-3">
                                <asp:TextBox ID="txtCupomPromo" runat="server" CssClass="form-control" Visible="true" Placeholder="Digite o código do cupom"></asp:TextBox>
                            </div>
                            <div class="col-md-4 mb-3">
                                <asp:Button ID="btnAplicaCupomPromo" CssClass="btn amado-btn w-100" runat="server" Text="Aplicar Cupom" OnClick="btnAplicaCupomPromo_Click" />
                            </div>
                        </div>

                        <asp:Label ID="lblResultadoAplicaCupomPromo" CssClass="text-danger" runat="server" Visible="false"></asp:Label>


                        <hr class="sidebar-divider" />
                        <div class="sidebar-heading mb-3">
                            <h3>Cupom de Troca</h3>
                        </div>
                        <div class="row">
                            <div class="col-md-12 mb-3">
                                <asp:DropDownList ID="dropIdCupomTroca1" CssClass="nice-select w-100" runat="server" Enabled="true" AutoPostBack="True" OnSelectedIndexChanged="dropIdCupomTroca1_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                            <div class="col-md-12 mb-3">
                                <asp:DropDownList ID="dropIdCupomTroca2" CssClass="nice-select w-100" runat="server" Enabled="true" AutoPostBack="True" OnSelectedIndexChanged="dropIdCupomTroca2_SelectedIndexChanged" Visible="false"></asp:DropDownList>
                            </div>
                            <div class="col-md-12 mb-3">
                                <asp:DropDownList ID="dropIdCupomTroca3" CssClass="nice-select w-100" runat="server" Enabled="true" AutoPostBack="True" OnSelectedIndexChanged="dropIdCupomTroca3_SelectedIndexChanged" Visible="false"></asp:DropDownList>
                            </div>
                            <div class="col-md-12 mb-3">
                                <asp:DropDownList ID="dropIdCupomTroca4" CssClass="nice-select w-100" runat="server" Enabled="true" AutoPostBack="True" OnSelectedIndexChanged="dropIdCupomTroca4_SelectedIndexChanged" Visible="false"></asp:DropDownList>
                            </div>
                            <div class="col-md-12 mb-3">
                                <asp:DropDownList ID="dropIdCupomTroca5" CssClass="nice-select w-100" runat="server" Enabled="true" AutoPostBack="True" OnSelectedIndexChanged="dropIdCupomTroca5_SelectedIndexChanged" Visible="false"></asp:DropDownList>
                            </div>
                        </div>


                        <hr class="sidebar-divider" />
                        <div class="sidebar-heading mb-3">
                            <h3>Forma de Pagamento</h3>
                        </div>

                        <asp:Label ID="lblResultadoPagto" CssClass="text-danger" runat="server" Visible="false"></asp:Label>

                        <div class="row">
                            <div class="col-md-8 mb-3">
                                <asp:DropDownList ID="dropIdCC1" CssClass="nice-select w-100" runat="server" Enabled="true" AutoPostBack="True" OnSelectedIndexChanged="dropIdCC1_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                            <div class="col-md-4 mb-3">
                                <asp:TextBox ID="txtValorCCPagto1" type="number" step="0.01" min="0" runat="server" CssClass="form-control" Visible="true" Placeholder="Valor" OnTextChanged="txtValorCCPagto1_TextChanged"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="input-group-append">
                                <asp:DropDownList ID="dropIdBandeiraCC1" type="text" CssClass="form-control form-control-user" runat="server" Visible="false"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="form-group">
                            <asp:TextBox ID="txtNomeImpressoCC1" runat="server" CssClass="form-control form-control-user" Placeholder="Nome Impresso no Cartão" Visible="false"></asp:TextBox>
                        </div>

                        <div class="form-group">
                            <asp:TextBox ID="txtNumeroCC1" runat="server" CssClass="form-control form-control-user" Placeholder="Número do Cartão" Visible="false"></asp:TextBox>
                        </div>

                        <div class="form-group">
                            <asp:TextBox ID="txtDtVencimentoCC1" type="text" runat="server" CssClass="form-control form-control-user" Placeholder="Data de Vencimento: MM/AAAA" Visible="false"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <div class="input-group-append">
                                <asp:TextBox ID="txtCodigoSegurancaCC1" runat="server" CssClass="form-control form-control-user" Placeholder="Código de Segurança" Visible="false"></asp:TextBox>
                            </div>
                        </div>


                        <div class="form-group">
                            <asp:CheckBox ID="chkIncluirCC1" runat="server" Text="Incluir na lista de cartões?" Visible="false"></asp:CheckBox>
                        </div>

                        <div class="form-group">
                            <asp:Label ID="lblIncluirCC1" CssClass="text-danger" runat="server" Visible="false"></asp:Label>
                        </div>
                        <%--<div class="form-group">
                            <asp:Button ID="btnIncluirCC1" CssClass="btn amado-btn w-100" runat="server" Text="Aplicar Cartão" Visible="false" OnClick="btnAplicarEndereco_Click" />
                        </div>--%>

                        <div class="row">
                            <div class="col-md-8 mb-3">
                                <asp:DropDownList ID="dropIdCC2" CssClass="nice-select w-100" runat="server" Enabled="true" AutoPostBack="True" Visible="false" OnSelectedIndexChanged="dropIdCC2_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                            <div class="col-md-4 mb-3">
                                <asp:TextBox ID="txtValorCCPagto2" type="number" step="0.01" min="0" runat="server" CssClass="form-control" Visible="false" Placeholder="Valor" OnTextChanged="txtValorCCPagto2_TextChanged"></asp:TextBox>
                            </div>
                        </div>

                        
                        <div class="form-group">
                            <div class="input-group-append">
                                <asp:DropDownList ID="dropIdBandeiraCC2" type="text" CssClass="form-control form-control-user" runat="server" Visible="false"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:TextBox ID="txtNomeImpressoCC2" runat="server" CssClass="form-control form-control-user" Placeholder="Nome Impresso no Cartão" Visible="false"></asp:TextBox>
                        </div>

                        <div class="form-group">
                            <asp:TextBox ID="txtNumeroCC2" runat="server" CssClass="form-control form-control-user" Placeholder="Número do Cartão" Visible="false"></asp:TextBox>
                        </div>
                        
                        <div class="form-group">
                            <asp:TextBox ID="txtDtVencimentoCC2" type="text" runat="server" CssClass="form-control form-control-user" Placeholder="Data de Vencimento: MM/AAAA" Visible="false"></asp:TextBox>
                        </div>

                        <div class="form-group">
                            <div class="input-group-append">
                                <asp:TextBox ID="txtCodigoSegurancaCC2" runat="server" CssClass="form-control form-control-user" Placeholder="Código de Segurança" Visible="false"></asp:TextBox>
                            </div>
                        </div>


                        <div class="form-group">
                            <asp:CheckBox ID="chkIncluirCC2" runat="server" Text="Incluir na lista de cartões?" Visible="false"></asp:CheckBox>
                        </div>

                        <div class="form-group">
                            <asp:Label ID="lblIncluirCC2" CssClass="text-danger" runat="server" Visible="false"></asp:Label>
                        </div>
                        <%--<div class="form-group">
                            <asp:Button ID="btnIncluirCC2" CssClass="btn amado-btn w-100" runat="server" Text="Aplicar Cartão" Visible="false" OnClick="btnAplicarEndereco_Click" />
                        </div>--%>

                        <div class="row">
                            <div class="col-md-8 mb-3">
                                <asp:DropDownList ID="dropIdCC3" CssClass="nice-select w-100" runat="server" Enabled="true" AutoPostBack="True" Visible="false" OnSelectedIndexChanged="dropIdCC3_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                            <div class="col-md-4 mb-3">
                                <asp:TextBox ID="txtValorCCPagto3" type="number" step="0.01" min="0" runat="server" CssClass="form-control" Visible="false" Placeholder="Valor" OnTextChanged="txtValorCCPagto3_TextChanged"></asp:TextBox>
                            </div>
                        </div>

                        
                        <div class="form-group">
                            <div class="input-group-append">
                                <asp:DropDownList ID="dropIdBandeiraCC3" type="text" CssClass="form-control form-control-user" runat="server" Visible="false"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:TextBox ID="txtNomeImpressoCC3" runat="server" CssClass="form-control form-control-user" Placeholder="Nome Impresso no Cartão" Visible="false"></asp:TextBox>
                        </div>

                        <div class="form-group">
                            <asp:TextBox ID="txtNumeroCC3" runat="server" CssClass="form-control form-control-user" Placeholder="Número do Cartão" Visible="false"></asp:TextBox>
                        </div>
                        
                        <div class="form-group">
                            <asp:TextBox ID="txtDtVencimentoCC3" type="text" runat="server" CssClass="form-control form-control-user" Placeholder="Data de Vencimento: MM/AAAA" Visible="false"></asp:TextBox>
                        </div>

                        <div class="form-group">
                            <div class="input-group-append">
                                <asp:TextBox ID="txtCodigoSegurancaCC3" runat="server" CssClass="form-control form-control-user" Placeholder="Código de Segurança" Visible="false"></asp:TextBox>
                            </div>
                        </div>


                        <div class="form-group">
                            <asp:CheckBox ID="chkIncluirCC3" runat="server" Text="Incluir na lista de cartões?" Visible="false"></asp:CheckBox>
                        </div>

                        <div class="form-group">
                            <asp:Label ID="lblIncluirCC3" CssClass="text-danger" runat="server" Visible="false"></asp:Label>
                        </div>
                        <%--<div class="form-group">
                            <asp:Button ID="btnIncluirCC3" CssClass="btn amado-btn w-100" runat="server" Text="Aplicar Cartão" Visible="false" OnClick="btnAplicarEndereco_Click" />
                        </div>--%>

                        <div class="row">
                            <div class="col-md-8 mb-3">
                                <asp:DropDownList ID="dropIdCC4" CssClass="nice-select w-100" runat="server" Enabled="true" AutoPostBack="True" Visible="false" OnSelectedIndexChanged="dropIdCC4_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                            <div class="col-md-4 mb-3">
                                <asp:TextBox ID="txtValorCCPagto4" type="number" step="0.01" min="0" runat="server" CssClass="form-control" Visible="false" Placeholder="Valor" OnTextChanged="txtValorCCPagto4_TextChanged"></asp:TextBox>
                            </div>
                        </div>

                        
                        <div class="form-group">
                            <div class="input-group-append">
                                <asp:DropDownList ID="dropIdBandeiraCC4" type="text" CssClass="form-control form-control-user" runat="server" Visible="false"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:TextBox ID="txtNomeImpressoCC4" runat="server" CssClass="form-control form-control-user" Placeholder="Nome Impresso no Cartão" Visible="false"></asp:TextBox>
                        </div>

                        <div class="form-group">
                            <asp:TextBox ID="txtNumeroCC4" runat="server" CssClass="form-control form-control-user" Placeholder="Número do Cartão" Visible="false"></asp:TextBox>
                        </div>
                        
                        <div class="form-group">
                            <asp:TextBox ID="txtDtVencimentoCC4" type="text" runat="server" CssClass="form-control form-control-user" Placeholder="Data de Vencimento: MM/AAAA" Visible="false"></asp:TextBox>
                        </div>

                        <div class="form-group">
                            <div class="input-group-append">
                                <asp:TextBox ID="txtCodigoSegurancaCC4" runat="server" CssClass="form-control form-control-user" Placeholder="Código de Segurança" Visible="false"></asp:TextBox>
                            </div>
                        </div>


                        <div class="form-group">
                            <asp:CheckBox ID="chkIncluirCC4" runat="server" Text="Incluir na lista de cartões?" Visible="false"></asp:CheckBox>
                        </div>

                        <div class="form-group">
                            <asp:Label ID="lblIncluirCC4" CssClass="text-danger" runat="server" Visible="false"></asp:Label>
                        </div>
                        <%--<div class="form-group">
                            <asp:Button ID="btnIncluirCC4" CssClass="btn amado-btn w-100" runat="server" Text="Aplicar Cartão" Visible="false" OnClick="btnAplicarEndereco_Click" />
                        </div>--%>

                        <div class="row">
                            <div class="col-md-8 mb-3">
                                <asp:DropDownList ID="dropIdCC5" CssClass="nice-select w-100" runat="server" Enabled="true" AutoPostBack="True" Visible="false" OnSelectedIndexChanged="dropIdCC5_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                            <div class="col-md-4 mb-3">
                                <asp:TextBox ID="txtValorCCPagto5" type="number" step="0.01" min="0" runat="server" CssClass="form-control" Visible="false" Placeholder="Valor" OnTextChanged="txtValorCCPagto5_TextChanged"></asp:TextBox>
                            </div>
                        </div>

                        
                        <div class="form-group">
                            <div class="input-group-append">
                                <asp:TextBox ID="txtCodigoSegurancaCC5" runat="server" CssClass="form-control form-control-user" Placeholder="Código de Segurança" Visible="false"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <asp:TextBox ID="txtNomeImpressoCC5" runat="server" CssClass="form-control form-control-user" Placeholder="Nome Impresso no Cartão" Visible="false"></asp:TextBox>
                        </div>

                        <div class="form-group">
                            <asp:TextBox ID="txtNumeroCC5" runat="server" CssClass="form-control form-control-user" Placeholder="Número do Cartão" Visible="false"></asp:TextBox>
                        </div>

                        
                        <div class="form-group">
                            <asp:TextBox ID="txtDtVencimentoCC5" type="text" runat="server" CssClass="form-control form-control-user" Placeholder="Data de Vencimento: MM/AAAA" Visible="false"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <div class="input-group-append">
                                <asp:DropDownList ID="dropIdBandeiraCC5" type="text" CssClass="form-control form-control-user" runat="server" Visible="false"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="form-group">
                            <asp:CheckBox ID="chkIncluirCC5" runat="server" Text="Incluir na lista de cartões?" Visible="false"></asp:CheckBox>
                        </div>

                        <div class="form-group">
                            <asp:Label ID="lblIncluirCC5" CssClass="text-danger" runat="server" Visible="false"></asp:Label>
                        </div>
                        <%--<div class="form-group">
                            <asp:Button ID="btnIncluirCC5" CssClass="btn amado-btn w-100" runat="server" Text="Aplicar Cartão" Visible="false" OnClick="btnAplicarEndereco_Click" />
                        </div>--%>
                    </div>
                </div>


                <div class="col-12 col-lg-4">
                    <div class="cart-summary">
                        <h5>Cart Total</h5>
                        <ul class="summary-table">
                            <li>
                                <span>
                                    <asp:Label ID="LabelSubtotalText" runat="server" Text="Subtotal do Pedido: "></asp:Label>
                                </span>
                                <span>
                                    <asp:Label ID="lblSubtotal" runat="server" EnableViewState="false"></asp:Label>
                                </span>
                            </li>
                            <li>
                                <span>
                                    <asp:Label ID="LabelFreteText" runat="server" Text="Frete: "></asp:Label>
                                </span>
                                <span>
                                    <asp:Label ID="lblFrete" runat="server" EnableViewState="false"></asp:Label>
                                </span>
                            </li>
                            <li>
                                <span>
                                    <asp:Label ID="LabelTotalText" runat="server" Text="Total do Pedido: "></asp:Label>
                                </span>
                                <span>
                                    <asp:Label ID="lblTotal" runat="server" EnableViewState="false"></asp:Label>
                                </span>
                            </li>
                            <li>
                                <span>
                                    <asp:Label ID="LabelAPagartxt" runat="server" Text="A Pagar: "></asp:Label>
                                </span>
                                <span>
                                    <asp:Label ID="lblAPagar" runat="server" EnableViewState="false"></asp:Label>
                                </span>
                            </li>
                        </ul>
                        <div class="cart-btn mt-100">
                            <asp:Button ID="CheckoutBtn" CssClass="btn amado-btn w-100" runat="server" OnClick="CheckoutBtn_Click" Text="Checkout" Enabled="false" />
                        </div>
                    </div>

                    <asp:Label ID="lblResultado" CssClass="text-danger" runat="server" Visible="false"></asp:Label>

                </div>


            </div>
        </div>
    </div>

</asp:Content>
