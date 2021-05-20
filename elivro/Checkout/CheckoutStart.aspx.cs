using Core.Aplicacao;
using Dominio.Cliente;
using Dominio.Livro;
using Dominio.Venda;
using elivro.Logic;
using elivro.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace elivro.Checkout
{
    public partial class CheckoutStart : ViewGenerico
    {
        ClienteEndereco clienteEndereco = new ClienteEndereco();
        ClienteCartao clienteCartao = new ClienteCartao();
        Endereco endereco = new Endereco();
        Cliente cliente = new Cliente();
        private Resultado resultado = new Resultado();
        decimal cartTotal;
        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["idCupomPromo"] = null;
                Session["dropIdCupomTroca1"] = null;
                Session["dropIdCupomTroca2"] = null;
                Session["dropIdCupomTroca3"] = null;
                Session["dropIdCupomTroca4"] = null;
                Session["dropIdCupomTroca5"] = null;
                Session["txtValorCCPagto1"] = null;
                Session["txtValorCCPagto2"] = null;
                Session["txtValorCCPagto3"] = null;
                Session["txtValorCCPagto4"] = null;
                Session["txtValorCCPagto5"] = null;

                Session["dropIdEstado"] = null;

                cliente = new Cliente();
                // pega o e-mail/usuário conectado e passa para cliente

                cliente.Email = Context.User.Identity.Name;

                // pesquisa no BD pelo cliente com e-mail
                entidades = commands["CONSULTAR"].execute(cliente).Entidades;

                cliente = (Cliente)entidades.ElementAt(0);

                // passa ID de cliente e consulta na tabela n-n
                foreach (ClienteEndereco clienteXEndereco in
                    commands["CONSULTAR"].execute(new ClienteEndereco { ID = cliente.ID }).Entidades)
                {
                    // Passa endereços para o cliente
                    cliente.Enderecos.Add(clienteXEndereco.Endereco);
                }

                dropIdEnderecoEntrega.DataSource = EnderecoEntregaDatatable(cliente.Enderecos);
                dropIdEnderecoEntrega.DataValueField = "ID";
                dropIdEnderecoEntrega.DataTextField = "Name";
                dropIdEnderecoEntrega.DataBind();

                // passa ID de cliente e consulta na tabela n-n
                foreach (ClienteCartao clienteXCC in
                    commands["CONSULTAR"].execute(new ClienteCartao { ID = cliente.ID }).Entidades)
                {
                    // Passa ccs para o cliente
                    cliente.CartoesCredito.Add(clienteXCC.CC);
                }

                dropIdCC1.DataSource = CartaoCreditoDatatable(cliente.CartoesCredito);
                dropIdCC1.DataValueField = "ID";
                dropIdCC1.DataTextField = "Name";
                dropIdCC1.DataBind();

                dropIdCC2.DataSource = CartaoCreditoDatatable(cliente.CartoesCredito);
                dropIdCC2.DataValueField = "ID";
                dropIdCC2.DataTextField = "Name";
                dropIdCC2.DataBind();

                dropIdCC3.DataSource = CartaoCreditoDatatable(cliente.CartoesCredito);
                dropIdCC3.DataValueField = "ID";
                dropIdCC3.DataTextField = "Name";
                dropIdCC3.DataBind();

                dropIdCC4.DataSource = CartaoCreditoDatatable(cliente.CartoesCredito);
                dropIdCC4.DataValueField = "ID";
                dropIdCC4.DataTextField = "Name";
                dropIdCC4.DataBind();

                dropIdCC5.DataSource = CartaoCreditoDatatable(cliente.CartoesCredito);
                dropIdCC5.DataValueField = "ID";
                dropIdCC5.DataTextField = "Name";
                dropIdCC5.DataBind();

                dropIdCupomTroca1.DataSource = CupomDatatable(commands["CONSULTAR"].execute(new Cupom() { IdCliente = cliente.ID, Status = 'A', Tipo = new TipoCupom() { ID = 1 } }).Entidades.Cast<Cupom>().ToList());
                dropIdCupomTroca1.DataValueField = "ID";
                dropIdCupomTroca1.DataTextField = "Name";
                dropIdCupomTroca1.DataBind();

                dropIdCupomTroca2.DataSource = CupomDatatable(commands["CONSULTAR"].execute(new Cupom() { IdCliente = cliente.ID, Status = 'A', Tipo = new TipoCupom() { ID = 1 } }).Entidades.Cast<Cupom>().ToList());
                dropIdCupomTroca2.DataValueField = "ID";
                dropIdCupomTroca2.DataTextField = "Name";
                dropIdCupomTroca2.DataBind();

                dropIdCupomTroca3.DataSource = CupomDatatable(commands["CONSULTAR"].execute(new Cupom() { IdCliente = cliente.ID, Status = 'A', Tipo = new TipoCupom() { ID = 1 } }).Entidades.Cast<Cupom>().ToList());
                dropIdCupomTroca3.DataValueField = "ID";
                dropIdCupomTroca3.DataTextField = "Name";
                dropIdCupomTroca3.DataBind();

                dropIdCupomTroca4.DataSource = CupomDatatable(commands["CONSULTAR"].execute(new Cupom() { IdCliente = cliente.ID, Status = 'A', Tipo = new TipoCupom() { ID = 1 } }).Entidades.Cast<Cupom>().ToList());
                dropIdCupomTroca4.DataValueField = "ID";
                dropIdCupomTroca4.DataTextField = "Name";
                dropIdCupomTroca4.DataBind();

                dropIdCupomTroca5.DataSource = CupomDatatable(commands["CONSULTAR"].execute(new Cupom() { IdCliente = cliente.ID, Status = 'A', Tipo = new TipoCupom() { ID = 1 } }).Entidades.Cast<Cupom>().ToList());
                dropIdCupomTroca5.DataValueField = "ID";
                dropIdCupomTroca5.DataTextField = "Name";
                dropIdCupomTroca5.DataBind();

                dropIdTipoResidencia.DataSource = TipoResidenciaDatatable(commands["CONSULTAR"].execute(new TipoResidencia()).Entidades.Cast<TipoResidencia>().ToList());
                dropIdTipoResidencia.DataValueField = "ID";
                dropIdTipoResidencia.DataTextField = "Name";
                dropIdTipoResidencia.DataBind();

                dropIdTipoLogradouro.DataSource = TipoLogradouroDatatable(commands["CONSULTAR"].execute(new TipoLogradouro()).Entidades.Cast<TipoLogradouro>().ToList());
                dropIdTipoLogradouro.DataValueField = "ID";
                dropIdTipoLogradouro.DataTextField = "Name";
                dropIdTipoLogradouro.DataBind();

                dropIdPais.DataSource = PaisDatatable(commands["CONSULTAR"].execute(new Pais()).Entidades.Cast<Pais>().ToList());
                dropIdPais.DataValueField = "ID";
                dropIdPais.DataTextField = "Name";
                dropIdPais.DataBind();

                dropIdEstado.DataSource = EstadoDatatable(commands["CONSULTAR"].execute(new Estado()).Entidades.Cast<Estado>().ToList());
                dropIdEstado.DataValueField = "ID";
                dropIdEstado.DataTextField = "Name";
                dropIdEstado.DataBind();

                dropIdCidade.DataSource = CidadeDatatable(commands["CONSULTAR"].execute(new Cidade()).Entidades.Cast<Cidade>().ToList());
                dropIdCidade.DataValueField = "ID";
                dropIdCidade.DataTextField = "Name";
                dropIdCidade.DataBind();

                dropIdBandeiraCC1.DataSource = BandeiraDatatable(commands["CONSULTAR"].execute(new Bandeira()).Entidades.Cast<Bandeira>().ToList());
                dropIdBandeiraCC1.DataValueField = "ID";
                dropIdBandeiraCC1.DataTextField = "Name";
                dropIdBandeiraCC1.DataBind();

                dropIdBandeiraCC2.DataSource = BandeiraDatatable(commands["CONSULTAR"].execute(new Bandeira()).Entidades.Cast<Bandeira>().ToList());
                dropIdBandeiraCC2.DataValueField = "ID";
                dropIdBandeiraCC2.DataTextField = "Name";
                dropIdBandeiraCC2.DataBind();

                dropIdBandeiraCC3.DataSource = BandeiraDatatable(commands["CONSULTAR"].execute(new Bandeira()).Entidades.Cast<Bandeira>().ToList());
                dropIdBandeiraCC3.DataValueField = "ID";
                dropIdBandeiraCC3.DataTextField = "Name";
                dropIdBandeiraCC3.DataBind();

                dropIdBandeiraCC4.DataSource = BandeiraDatatable(commands["CONSULTAR"].execute(new Bandeira()).Entidades.Cast<Bandeira>().ToList());
                dropIdBandeiraCC4.DataValueField = "ID";
                dropIdBandeiraCC4.DataTextField = "Name";
                dropIdBandeiraCC4.DataBind();

                dropIdBandeiraCC5.DataSource = BandeiraDatatable(commands["CONSULTAR"].execute(new Bandeira()).Entidades.Cast<Bandeira>().ToList());
                dropIdBandeiraCC5.DataValueField = "ID";
                dropIdBandeiraCC5.DataTextField = "Name";
                dropIdBandeiraCC5.DataBind();

                if (Session["payment_amt"] != null)
                {
                    string amt = Session["payment_amt"].ToString();
                }
                else
                {
                    Response.Redirect("~/Checkout/CheckoutError.aspx?ErrorCode=AmtMissing");
                }
            }

            using (ShoppingCartActions usersShoppingCart = new ShoppingCartActions())
            {
                cartTotal = 0;
                cartTotal = usersShoppingCart.GetTotal();
                if (cartTotal > 0)
                {
                    // Display Total.
                    lblSubtotal.Text = String.Format("{0:c}", cartTotal);
                    lblFrete.Text = "-";
                    lblTotal.Text = "-";

                    AplicaCupom();

                    CalculaFrete();

                    RecebeValorCC();
                }
                else
                {
                    lblSubtotal.Text = "-";
                    UpdateBtn.Visible = false;
                    CheckoutBtn.Visible = false;
                }
            }
        }

        public static DataTable EnderecoEntregaDatatable(List<Endereco> input)
        {
            DataTable data = new DataTable();
            data.Columns.Add(new DataColumn("ID", typeof(int)));
            data.Columns.Add(new DataColumn("Name", typeof(string)));

            DataRow dr = data.NewRow();
            dr[0] = 0;
            dr[1] = "Selecione um Endereço de Entrega";
            data.Rows.Add(dr);

            int a = input.Count;
            for (int i = 0; i < a; i++)
            {
                Endereco endereco = input.ElementAt(i);
                dr = data.NewRow();
                dr[0] = endereco.ID;
                dr[1] = endereco.Nome + ", " + endereco.Destinatario + ", " + endereco.TipoResidencia.Nome + ", " +
                    endereco.TipoLogradouro.Nome + " " + endereco.Logradouro + ", " + endereco.Numero + ", " + endereco.Bairro + ", " +
                    endereco.Cidade.Nome + "/" + endereco.Cidade.Estado.Sigla + ", CEP: " + endereco.CEP + ", " + endereco.Observacao;
                data.Rows.Add(dr);
            }

            dr = data.NewRow();
            dr[0] = -1;
            dr[1] = "Usar um Novo Endereço de Entrega";
            data.Rows.Add(dr);

            return data;
        }

        public static DataTable CartaoCreditoDatatable(List<CartaoCredito> input)
        {
            DataTable data = new DataTable();
            data.Columns.Add(new DataColumn("ID", typeof(int)));
            data.Columns.Add(new DataColumn("Name", typeof(string)));

            DataRow dr = data.NewRow();
            dr[0] = 0;
            dr[1] = "Selecione um Cartão de Crédito";
            data.Rows.Add(dr);

            int a = input.Count;
            for (int i = 0; i < a; i++)
            {
                CartaoCredito cc = input.ElementAt(i);
                dr = data.NewRow();
                dr[0] = cc.ID;
                dr[1] = cc.NomeImpresso + ", " + cc.NumeroCC + ", " + cc.Bandeira.Nome + ", " +
                    cc.CodigoSeguranca;
                data.Rows.Add(dr);
            }

            dr = data.NewRow();
            dr[0] = -1;
            dr[1] = "Usar um Novo Cartão de Crédito";
            data.Rows.Add(dr);

            return data;
        }

        public static DataTable CupomDatatable(List<Cupom> input)
        {
            DataTable data = new DataTable();
            data.Columns.Add(new DataColumn("ID", typeof(int)));
            data.Columns.Add(new DataColumn("Name", typeof(string)));

            DataRow dr = data.NewRow();
            dr[0] = 0;
            dr[1] = "Selecione um Cupom de Troca";
            data.Rows.Add(dr);

            int a = input.Count;
            for (int i = 0; i < a; i++)
            {
                Cupom cupom = input.ElementAt(i);
                dr = data.NewRow();
                dr[0] = cupom.ID;
                dr[1] = cupom.CodigoCupom + " - R$ " + cupom.ValorCupom.ToString("N2");
                data.Rows.Add(dr);
            }
            return data;
        }

        public static DataTable TipoResidenciaDatatable(List<TipoResidencia> input)
        {
            DataTable data = new DataTable();
            data.Columns.Add(new DataColumn("ID", typeof(int)));
            data.Columns.Add(new DataColumn("Name", typeof(string)));

            DataRow dr = data.NewRow();
            dr[0] = 0;
            dr[1] = "Selecione um Tipo de Residência";
            data.Rows.Add(dr);

            int a = input.Count;
            for (int i = 0; i < a; i++)
            {
                TipoResidencia tipoResidencia = input.ElementAt(i);
                dr = data.NewRow();
                dr[0] = tipoResidencia.ID;
                dr[1] = tipoResidencia.Nome;
                data.Rows.Add(dr);
            }
            return data;
        }

        public static DataTable TipoLogradouroDatatable(List<TipoLogradouro> input)
        {
            DataTable data = new DataTable();
            data.Columns.Add(new DataColumn("ID", typeof(int)));
            data.Columns.Add(new DataColumn("Name", typeof(string)));

            DataRow dr = data.NewRow();
            dr[0] = 0;
            dr[1] = "Selecione um Tipo de Logradouro";
            data.Rows.Add(dr);

            int a = input.Count;
            for (int i = 0; i < a; i++)
            {
                TipoLogradouro tipoLogradouro = input.ElementAt(i);
                dr = data.NewRow();
                dr[0] = tipoLogradouro.ID;
                dr[1] = tipoLogradouro.Nome;
                data.Rows.Add(dr);
            }
            return data;
        }

        public static DataTable PaisDatatable(List<Pais> input)
        {
            DataTable data = new DataTable();
            data.Columns.Add(new DataColumn("ID", typeof(int)));
            data.Columns.Add(new DataColumn("Name", typeof(string)));

            DataRow dr = data.NewRow();
            dr[0] = 0;
            dr[1] = "Selecione um Pais";
            data.Rows.Add(dr);

            int a = input.Count;
            for (int i = 0; i < a; i++)
            {
                Pais pais = input.ElementAt(i);
                dr = data.NewRow();
                dr[0] = pais.ID;
                dr[1] = pais.Nome;
                data.Rows.Add(dr);
            }
            return data;
        }

        public static DataTable EstadoDatatable(List<Estado> input)
        {
            DataTable data = new DataTable();
            data.Columns.Add(new DataColumn("ID", typeof(int)));
            data.Columns.Add(new DataColumn("Name", typeof(string)));

            DataRow dr = data.NewRow();
            dr[0] = 0;
            dr[1] = "Selecione um Estado";
            data.Rows.Add(dr);

            int a = input.Count;
            for (int i = 0; i < a; i++)
            {
                Estado estado = input.ElementAt(i);
                dr = data.NewRow();
                dr[0] = estado.ID;
                dr[1] = estado.Nome;
                data.Rows.Add(dr);
            }
            return data;
        }

        public static DataTable CidadeDatatable(List<Cidade> input)
        {
            DataTable data = new DataTable();
            data.Columns.Add(new DataColumn("ID", typeof(int)));
            data.Columns.Add(new DataColumn("Name", typeof(string)));

            DataRow dr = data.NewRow();
            dr[0] = 0;
            dr[1] = "Selecione uma Cidade";
            data.Rows.Add(dr);

            int a = input.Count;
            for (int i = 0; i < a; i++)
            {
                Cidade cidade = input.ElementAt(i);
                dr = data.NewRow();
                dr[0] = cidade.ID;
                dr[1] = cidade.Nome;
                data.Rows.Add(dr);
            }
            return data;
        }

        public static DataTable BandeiraDatatable(List<Bandeira> input)
        {
            DataTable data = new DataTable();
            data.Columns.Add(new DataColumn("ID", typeof(int)));
            data.Columns.Add(new DataColumn("Name", typeof(string)));

            DataRow dr = data.NewRow();
            dr[0] = 0;
            dr[1] = "Selecione uma Bandeira";
            data.Rows.Add(dr);

            int a = input.Count;
            for (int i = 0; i < a; i++)
            {
                Bandeira bandeira = input.ElementAt(i);
                dr = data.NewRow();
                dr[0] = bandeira.ID;
                dr[1] = bandeira.Nome;
                data.Rows.Add(dr);
            }
            return data;
        }

        protected void dropIdEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["estado_sel"] = dropIdEstado.SelectedIndex;

            if (!dropIdEstado.SelectedValue.Equals("0"))
            {
                Session["cidade_sel"] = dropIdCidade.SelectedIndex;
                dropIdCidade.DataSource = CidadeDatatable(commands["CONSULTAR"].execute(new Cidade() { Estado = new Estado() { ID = int.Parse(dropIdEstado.SelectedValue) } }).Entidades.Cast<Cidade>().ToList());
                dropIdCidade.DataBind();
                dropIdCidade.Enabled = true;
            }
            else
            {
                dropIdCidade.DataSource = CidadeDatatable(commands["CONSULTAR"].execute(new Cidade()).Entidades.Cast<Cidade>().ToList());
                dropIdCidade.DataValueField = "ID";
                dropIdCidade.DataTextField = "Name";
                dropIdCidade.DataBind();
                dropIdCidade.Enabled = false;
            }
        }

        protected void dropIdPais_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["pais_sel"] = dropIdPais.SelectedIndex;

            if (!dropIdPais.SelectedValue.Equals("0"))
            {
                Session["estado_sel"] = dropIdEstado.SelectedIndex;
                dropIdCidade.DataSource = CidadeDatatable(commands["CONSULTAR"].execute(new Cidade() { Estado = new Estado() { ID = int.Parse(dropIdEstado.SelectedValue), Pais = new Pais() { ID = int.Parse(dropIdPais.SelectedValue) } } }).Entidades.Cast<Cidade>().ToList());
                dropIdEstado.DataBind();
                dropIdEstado.Enabled = true;
            }
            else
            {
                dropIdEstado.DataSource = EstadoDatatable(commands["CONSULTAR"].execute(new Estado()).Entidades.Cast<Estado>().ToList());
                dropIdEstado.DataValueField = "ID";
                dropIdEstado.DataTextField = "Name";
                dropIdEstado.DataBind();
                dropIdEstado.Enabled = false;

                dropIdCidade.DataSource = CidadeDatatable(commands["CONSULTAR"].execute(new Cidade()).Entidades.Cast<Cidade>().ToList());
                dropIdCidade.DataValueField = "ID";
                dropIdCidade.DataTextField = "Name";
                dropIdCidade.DataBind();
                dropIdCidade.Enabled = false;
            }
        }

        public List<CartItem> GetShoppingCartItems()
        {
            ShoppingCartActions actions = new ShoppingCartActions();
            return actions.GetCartItems();
        }

        public List<CartItem> UpdateCartItems()
        {
            using (ShoppingCartActions usersShoppingCart = new ShoppingCartActions())
            {
                String cartId = usersShoppingCart.GetCartId();

                ShoppingCartActions.ShoppingCartUpdates[] cartUpdates = new ShoppingCartActions.ShoppingCartUpdates[CartList.Rows.Count];
                for (int i = 0; i < CartList.Rows.Count; i++)
                {
                    IOrderedDictionary rowValues = new OrderedDictionary();
                    rowValues = GetValues(CartList.Rows[i]);
                    cartUpdates[i].LivroId = Convert.ToInt32(rowValues["livro_id"]);

                    CheckBox cbRemove = new CheckBox();
                    cbRemove = (CheckBox)CartList.Rows[i].FindControl("Remover");
                    cartUpdates[i].RemoverItem = cbRemove.Checked;

                    int quantidadeAnterior = GetShoppingCartItems().Cast<CartItem>().ElementAt(i).quantidade;

                    Estoque estoque = commands["CONSULTAR"].execute(new Estoque() { Livro = new Dominio.Livro.Livro() { ID = cartUpdates[i].LivroId } }).Entidades.Cast<Estoque>().ElementAt(0);

                    TextBox quantityTextBox = new TextBox();
                    quantityTextBox = (TextBox)CartList.Rows[i].FindControl("PurchaseQuantity");

                    if (estoque.Qtde < Convert.ToInt32(quantityTextBox.Text))
                    {
                        cartUpdates[i].PurchaseQuantity = quantidadeAnterior;
                        lblResultadoCarrinho.Text = "Quantidade em estoque do livro " + estoque.Livro.Titulo + " é de " + estoque.Qtde + " unidade(s)";
                        lblResultadoCarrinho.Visible = true;
                    }
                    else
                    {
                        cartUpdates[i].PurchaseQuantity = Convert.ToInt16(quantityTextBox.Text.ToString());
                        lblResultadoCarrinho.Text = "";
                        lblResultadoCarrinho.Visible = false;
                    }
                }
                usersShoppingCart.UpdateShoppingCartDatabase(cartId, cartUpdates);
                CartList.DataBind();
                lblSubtotal.Text = String.Format("{0:c}", usersShoppingCart.GetTotal());
                return usersShoppingCart.GetCartItems();
            }
        }

        public static IOrderedDictionary GetValues(GridViewRow row)
        {
            IOrderedDictionary values = new OrderedDictionary();
            foreach (DataControlFieldCell cell in row.Cells)
            {
                if (cell.Visible)
                {
                    // extrai os valores da célula
                    cell.ContainingField.ExtractValuesFromCell(values, cell, row.RowState, true);
                }
            }
            return values;
        }

        protected void UpdateBtn_Click(object sender, EventArgs e)
        {
            UpdateCartItems();
            if (GetShoppingCartItems().Count == 0)
            {
                Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
            }
        }

        protected void CheckoutBtn_Click(object sender, EventArgs e)
        {
            bool flgErroEndereco = false;
            bool flgErroCC1 = false;
            bool flgErroCC2 = false;
            bool flgErroCC3 = false;
            bool flgErroCC4 = false;
            bool flgErroCC5 = false;
            ClienteCartao clienteCartao2 = new ClienteCartao();
            ClienteCartao clienteCartao3 = new ClienteCartao();
            ClienteCartao clienteCartao4 = new ClienteCartao();
            ClienteCartao clienteCartao5 = new ClienteCartao();

            cliente = new Cliente();

            // pega o e-mail/usuário conectado e passa para cliente
            cliente.Email = Context.User.Identity.Name;

            // pesquisa no BD pelo cliente com e-mail
            entidades = commands["CONSULTAR"].execute(cliente).Entidades;

            cliente = (Cliente)entidades.ElementAt(0);

            clienteEndereco.ID = cliente.ID;

            // -------------------------- ENDEREÇO COMEÇO --------------------------------------
            clienteEndereco.Endereco.Nome = txtNomeEndereco.Text.Trim();
            clienteEndereco.Endereco.Destinatario = txtNomeDestinatario.Text.Trim();
            clienteEndereco.Endereco.Logradouro = txtRua.Text.Trim();
            clienteEndereco.Endereco.Numero = txtNumero.Text.Trim();
            clienteEndereco.Endereco.Bairro = txtBairro.Text.Trim();
            clienteEndereco.Endereco.CEP = txtCEP.Text.Trim();
            clienteEndereco.Endereco.Observacao = txtObservacao.Text.Trim();

            clienteEndereco.Endereco.TipoResidencia.ID = Convert.ToInt32(dropIdTipoResidencia.SelectedValue);
            clienteEndereco.Endereco.TipoLogradouro.ID = Convert.ToInt32(dropIdTipoLogradouro.SelectedValue);
            clienteEndereco.Endereco.Cidade.ID = Convert.ToInt32(dropIdCidade.SelectedValue);
            clienteEndereco.Endereco.Cidade.Estado.ID = Convert.ToInt32(dropIdEstado.SelectedValue);
            clienteEndereco.Endereco.Cidade.Estado.Pais.ID = Convert.ToInt32(dropIdPais.SelectedValue);
            // -------------------------- ENDEREÇO FIM --------------------------------------

            clienteCartao.ID = cliente.ID;
            // -------------------------- CC1 COMEÇO --------------------------------------
            clienteCartao.CC.NomeImpresso = txtNomeImpressoCC1.Text.Trim();
            clienteCartao.CC.NumeroCC = txtNumeroCC1.Text.Trim();
            clienteCartao.CC.CodigoSeguranca = txtCodigoSegurancaCC1.Text.Trim();
            if (!txtDtVencimentoCC1.Text.Equals(""))
            {
                string[] data = txtDtVencimentoCC1.Text.Split('/');
                clienteCartao.CC.DataVencimento = new DateTime(Convert.ToInt32(data[1].ToString()), Convert.ToInt32(data[0].ToString()), Convert.ToInt32("01"));
            }
            clienteCartao.CC.Bandeira.ID = Convert.ToInt32(dropIdBandeiraCC1.SelectedValue);
            // -------------------------- CC1 FIM --------------------------------------


            if (chkIncluir.Checked == true)
            {
                resultado = commands["SALVAR"].execute(clienteEndereco);
                if (!string.IsNullOrEmpty(resultado.Msg))
                {
                    lblResultado.Visible = true;
                    lblResultado.Text = resultado.Msg;
                    flgErroEndereco = true;
                }
                else
                {
                    lblResultado.Visible = false;
                    flgErroEndereco = false;
                }
            }

            // CC1
            if (chkIncluirCC1.Checked == true)
            {
                resultado = commands["SALVAR"].execute(clienteCartao);
                if (!string.IsNullOrEmpty(resultado.Msg))
                {
                    lblResultado.Visible = true;
                    lblResultado.Text = resultado.Msg;
                    flgErroCC1 = true;
                }
                else
                {
                    lblResultado.Visible = false;
                    flgErroCC1 = false;
                }
            }

            clienteCartao2.ID = cliente.ID;
            // -------------------------- CC2 COMEÇO --------------------------------------
            clienteCartao2.CC.NomeImpresso = txtNomeImpressoCC2.Text.Trim();
            clienteCartao2.CC.NumeroCC = txtNumeroCC2.Text.Trim();
            clienteCartao2.CC.CodigoSeguranca = txtCodigoSegurancaCC2.Text.Trim();
            if (!txtDtVencimentoCC2.Text.Equals(""))
            {
                string[] data = txtDtVencimentoCC2.Text.Split('/');
                clienteCartao2.CC.DataVencimento = new DateTime(Convert.ToInt32(data[1].ToString()), Convert.ToInt32(data[0].ToString()), Convert.ToInt32("01"));
            }
            clienteCartao2.CC.Bandeira.ID = Convert.ToInt32(dropIdBandeiraCC2.SelectedValue);
            // -------------------------- CC2 FIM --------------------------------------

            // CC2
            if (chkIncluirCC2.Checked == true)
            {
                resultado = commands["SALVAR"].execute(clienteCartao2);
                if (!string.IsNullOrEmpty(resultado.Msg))
                {
                    lblResultado.Visible = true;
                    lblResultado.Text = resultado.Msg;
                    flgErroCC2 = true;
                }
                else
                {
                    lblResultado.Visible = false;
                    flgErroCC2 = false;
                }
            }

            clienteCartao3.ID = cliente.ID;
            // -------------------------- CC3 COMEÇO --------------------------------------
            clienteCartao3.CC.NomeImpresso = txtNomeImpressoCC3.Text.Trim();
            clienteCartao3.CC.NumeroCC = txtNumeroCC3.Text.Trim();
            clienteCartao3.CC.CodigoSeguranca = txtCodigoSegurancaCC3.Text.Trim();
            if (!txtDtVencimentoCC3.Text.Equals(""))
            {
                string[] data = txtDtVencimentoCC3.Text.Split('/');
                clienteCartao3.CC.DataVencimento = new DateTime(Convert.ToInt32(data[1].ToString()), Convert.ToInt32(data[0].ToString()), Convert.ToInt32("01"));
            }
            clienteCartao3.CC.Bandeira.ID = Convert.ToInt32(dropIdBandeiraCC3.SelectedValue);
            // -------------------------- CC3 FIM --------------------------------------

            // CC3
            if (chkIncluirCC3.Checked == true)
            {
                resultado = commands["SALVAR"].execute(clienteCartao3);
                if (!string.IsNullOrEmpty(resultado.Msg))
                {
                    lblResultado.Visible = true;
                    lblResultado.Text = resultado.Msg;
                    flgErroCC3 = true;
                }
                else
                {
                    lblResultado.Visible = false;
                    flgErroCC3 = false;
                }
            }

            clienteCartao4.ID = cliente.ID;
            // -------------------------- CC4 COMEÇO --------------------------------------
            clienteCartao4.CC.NomeImpresso = txtNomeImpressoCC4.Text.Trim();
            clienteCartao4.CC.NumeroCC = txtNumeroCC4.Text.Trim();
            clienteCartao4.CC.CodigoSeguranca = txtCodigoSegurancaCC4.Text.Trim();
            if (!txtDtVencimentoCC4.Text.Equals(""))
            {
                string[] data = txtDtVencimentoCC4.Text.Split('/');
                clienteCartao4.CC.DataVencimento = new DateTime(Convert.ToInt32(data[1].ToString()), Convert.ToInt32(data[0].ToString()), Convert.ToInt32("01"));
            }
            clienteCartao4.CC.Bandeira.ID = Convert.ToInt32(dropIdBandeiraCC4.SelectedValue);
            // -------------------------- CC4 FIM --------------------------------------

            // CC4
            if (chkIncluirCC4.Checked == true)
            {
                resultado = commands["SALVAR"].execute(clienteCartao4);
                if (!string.IsNullOrEmpty(resultado.Msg))
                {
                    lblResultado.Visible = true;
                    lblResultado.Text = resultado.Msg;
                    flgErroCC4 = true;
                }
                else
                {
                    lblResultado.Visible = false;
                    flgErroCC4 = false;
                }
            }

            clienteCartao5.ID = cliente.ID;
            // -------------------------- CC5 COMEÇO --------------------------------------
            clienteCartao5.CC.NomeImpresso = txtNomeImpressoCC5.Text.Trim();
            clienteCartao5.CC.NumeroCC = txtNumeroCC5.Text.Trim();
            clienteCartao5.CC.CodigoSeguranca = txtCodigoSegurancaCC5.Text.Trim();
            if (!txtDtVencimentoCC5.Text.Equals(""))
            {
                string[] data = txtDtVencimentoCC5.Text.Split('/');
                clienteCartao5.CC.DataVencimento = new DateTime(Convert.ToInt32(data[1].ToString()), Convert.ToInt32(data[0].ToString()), Convert.ToInt32("01"));
            }
            clienteCartao5.CC.Bandeira.ID = Convert.ToInt32(dropIdBandeiraCC5.SelectedValue);
            // -------------------------- CC5 FIM --------------------------------------

            // CC5
            if (chkIncluirCC5.Checked == true)
            {
                resultado = commands["SALVAR"].execute(clienteCartao5);
                if (!string.IsNullOrEmpty(resultado.Msg))
                {
                    lblResultado.Visible = true;
                    lblResultado.Text = resultado.Msg;
                    flgErroCC5 = true;
                }
                else
                {
                    lblResultado.Visible = false;
                    flgErroCC5 = false;
                }
            }

            Pedido pedido = new Pedido();

            // pegando e adicionando usuário ao pedido
            pedido.Usuario = Context.User.Identity.Name;

            if (!flgErroEndereco)
            {
                if (chkIncluir.Checked == true)
                {
                    // pega endereço (ID) que foi inserido no BD e adiciona ao pedido
                    pedido.EnderecoEntrega.ID = clienteEndereco.Endereco.ID;
                }
                else
                {
                    // pegando e adicionando endereço (ID) selecionado ao pedido
                    pedido.EnderecoEntrega.ID = Convert.ToInt32(dropIdEnderecoEntrega.SelectedValue);
                }
            }

            // pegando e adicionando frete ao pedido
            float frete = (float)0.0;
            if (!lblFrete.Text.Trim().Equals("-"))
            {
                if (float.TryParse(lblFrete.Text.Remove(0, 3), out frete))
                {
                    pedido.Frete = frete;
                }
            }

            // pegando e adicionando total ao pedido
            float total = (float)0.0;
            if (!lblTotal.Text.Trim().Equals("-"))
            {
                if (float.TryParse(lblTotal.Text.Remove(0, 3), out total))
                {
                    pedido.Total = total;
                }
            }

            // Pegando e adicionando os item do carrinho em pedido
            foreach (CartItem cartItem in GetShoppingCartItems())
            {
                PedidoDetalhe detalhe = new PedidoDetalhe();
                detalhe.Livro.ID = cartItem.livro_id;
                detalhe.Livro.Titulo = cartItem.titulo_livro;
                detalhe.Quantidade = cartItem.quantidade;
                detalhe.ValorUnit = cartItem.valor_venda;

                pedido.Detalhes.Add(detalhe);
            }

            CartaoCreditoPedido cartaoPedido = new CartaoCreditoPedido();
            if (Session["txtValorCCPagto1"] != null)
            {
                if (!flgErroCC1)
                {
                    if (chkIncluirCC1.Checked == true)
                    {
                        // pega CC (ID) que foi inserido no BD e adiciona ao pedido
                        cartaoPedido.CC.ID = clienteCartao.CC.ID;
                    }
                    else
                    {
                        // pegando e adicionando CC (ID) selecionado ao pedido
                        cartaoPedido.CC.ID = Convert.ToInt32(dropIdCC1.SelectedValue);
                    }
                }
                decimal valorCC = 0;
                if (decimal.TryParse(Session["txtValorCCPagto1"].ToString(), NumberStyles.Number, CultureInfo.CreateSpecificCulture("en-US"), out valorCC))
                {
                    cartaoPedido.ValorCCPagto = (float)valorCC;
                }
                pedido.CCs.Add(cartaoPedido);
            }

            cartaoPedido = new CartaoCreditoPedido();
            if (Session["txtValorCCPagto2"] != null)
            {
                if (!flgErroCC2)
                {
                    if (chkIncluirCC2.Checked == true)
                    {
                        // pega CC (ID) que foi inserido no BD e adiciona ao pedido
                        cartaoPedido.CC.ID = clienteCartao2.CC.ID;
                    }
                    else
                    {
                        // pegando e adicionando CC (ID) selecionado ao pedido
                        cartaoPedido.CC.ID = Convert.ToInt32(dropIdCC2.SelectedValue);
                    }
                }
                decimal valorCC = 0;
                if (decimal.TryParse(Session["txtValorCCPagto2"].ToString(), NumberStyles.Number, CultureInfo.CreateSpecificCulture("en-US"), out valorCC))
                {
                    cartaoPedido.ValorCCPagto = (float)valorCC;
                }
                pedido.CCs.Add(cartaoPedido);
            }

            cartaoPedido = new CartaoCreditoPedido();
            if (Session["txtValorCCPagto3"] != null)
            {
                if (!flgErroCC3)
                {
                    if (chkIncluirCC3.Checked == true)
                    {
                        // pega CC (ID) que foi inserido no BD e adiciona ao pedido
                        cartaoPedido.CC.ID = clienteCartao3.CC.ID;
                    }
                    else
                    {
                        // pegando e adicionando CC (ID) selecionado ao pedido
                        cartaoPedido.CC.ID = Convert.ToInt32(dropIdCC3.SelectedValue);
                    }
                }
                decimal valorCC = 0;
                if (decimal.TryParse(Session["txtValorCCPagto3"].ToString(), NumberStyles.Number, CultureInfo.CreateSpecificCulture("en-US"), out valorCC))
                {
                    cartaoPedido.ValorCCPagto = (float)valorCC;
                }
                pedido.CCs.Add(cartaoPedido);
            }

            cartaoPedido = new CartaoCreditoPedido();
            if (Session["txtValorCCPagto4"] != null)
            {
                if (!flgErroCC4)
                {
                    if (chkIncluirCC4.Checked == true)
                    {
                        // pega CC (ID) que foi inserido no BD e adiciona ao pedido
                        cartaoPedido.CC.ID = clienteCartao4.CC.ID;
                    }
                    else
                    {
                        // pegando e adicionando CC (ID) selecionado ao pedido
                        cartaoPedido.CC.ID = Convert.ToInt32(dropIdCC4.SelectedValue);
                    }
                }
                decimal valorCC = 0;
                if (decimal.TryParse(Session["txtValorCCPagto4"].ToString(), NumberStyles.Number, CultureInfo.CreateSpecificCulture("en-US"), out valorCC))
                {
                    cartaoPedido.ValorCCPagto = (float)valorCC;
                }
                pedido.CCs.Add(cartaoPedido);
            }

            cartaoPedido = new CartaoCreditoPedido();
            if (Session["txtValorCCPagto5"] != null)
            {
                if (!flgErroCC5)
                {
                    if (chkIncluirCC5.Checked == true)
                    {
                        // pega CC (ID) que foi inserido no BD e adiciona ao pedido
                        cartaoPedido.CC.ID = clienteCartao5.CC.ID;
                    }
                    else
                    {
                        // pegando e adicionando CC (ID) selecionado ao pedido
                        cartaoPedido.CC.ID = Convert.ToInt32(dropIdCC5.SelectedValue);
                    }
                }
                decimal valorCC = 0;
                if (decimal.TryParse(Session["txtValorCCPagto5"].ToString(), NumberStyles.Number, CultureInfo.CreateSpecificCulture("en-US"), out valorCC))
                {
                    cartaoPedido.ValorCCPagto = (float)valorCC;
                }
                pedido.CCs.Add(cartaoPedido);
            }

            if (Session["idCupomPromo"] != null)
            {
                pedido.CupomPromocional.ID = Convert.ToInt32(Session["idCupomPromo"]);
            }

            Cupom cupomTroca = new Cupom();
            if (Session["dropIdCupomTroca1"] != null)
            {
                cupomTroca.ID = Convert.ToInt32(Session["dropIdCupomTroca1"]);
                pedido.CuponsTroca.Add(cupomTroca);
            }

            cupomTroca = new Cupom();
            if (Session["dropIdCupomTroca2"] != null)
            {
                cupomTroca.ID = Convert.ToInt32(Session["dropIdCupomTroca2"]);
                pedido.CuponsTroca.Add(cupomTroca);
            }

            cupomTroca = new Cupom();
            if (Session["dropIdCupomTroca3"] != null)
            {
                cupomTroca.ID = Convert.ToInt32(Session["dropIdCupomTroca3"]);
                pedido.CuponsTroca.Add(cupomTroca);
            }

            cupomTroca = new Cupom();
            if (Session["dropIdCupomTroca4"] != null)
            {
                cupomTroca.ID = Convert.ToInt32(Session["dropIdCupomTroca4"]);
                pedido.CuponsTroca.Add(cupomTroca);
            }

            cupomTroca = new Cupom();
            if (Session["dropIdCupomTroca5"] != null)
            {
                cupomTroca.ID = Convert.ToInt32(Session["dropIdCupomTroca5"]);
                pedido.CuponsTroca.Add(cupomTroca);
            }

            // passando Status inicial para o pedido
            // Status.ID = 1 / EM PROCESSAMENTO
            pedido.Status.ID = 1;


            // verifica se caso inclusão de endereço novo e Cartões novos, houve erro
            // se houve erro retorna para checkout e exibe erro
            // se não continua execução
            if (!flgErroEndereco && !flgErroCC1 && !flgErroCC2 && !flgErroCC3 && !flgErroCC4 && !flgErroCC5)
            {
                resultado = commands["SALVAR"].execute(pedido);
                if (!string.IsNullOrEmpty(resultado.Msg))
                {
                    lblResultado.Visible = true;
                    lblResultado.Text = resultado.Msg;
                }
                else
                {
                    Response.Redirect("./CheckoutReview.aspx?idPedido=" + pedido.ID);
                }
            }
            else
            {
                lblResultado.Visible = true;
                string msg = "";

                if (flgErroEndereco)
                {
                    msg += "ERRO NA INCLUSÃO DO NOVO ENDEREÇO! </br> ";
                }

                if (flgErroCC1)
                {
                    msg += "ERRO NA INCLUSÃO DO NOVO CARTÃO DE CRÉDITO (1)! </br> ";
                }

                if (flgErroCC2)
                {
                    msg += "ERRO NA INCLUSÃO DO NOVO CARTÃO DE CRÉDITO (2)! </br> ";
                }

                if (flgErroCC3)
                {
                    msg += "ERRO NA INCLUSÃO DO NOVO CARTÃO DE CRÉDITO (3)! </br> ";
                }

                if (flgErroCC4)
                {
                    msg += "ERRO NA INCLUSÃO DO NOVO CARTÃO DE CRÉDITO (4)! </br> ";
                }

                if (flgErroCC5)
                {
                    msg += "ERRO NA INCLUSÃO DO NOVO CARTÃO DE CRÉDITO (5)! </br> ";
                }

                lblResultado.Text = msg;
            }


            //using (ShoppingCartActions usersShoppingCart = new ShoppingCartActions())
            //{
            //    Session["payment_amt"] = usersShoppingCart.GetTotal(); // falta frete
            //}
            //Response.Redirect("~/Checkout/CheckoutStart.aspx");
        }

        protected void dropIdEnderecoEntrega_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(dropIdEnderecoEntrega.SelectedValue) != -1)
            {
                txtNomeEndereco.Visible = false;
                txtNomeEndereco.Text = "";
                txtNomeDestinatario.Visible = false;
                txtNomeDestinatario.Text = "";
                dropIdTipoLogradouro.Visible = false;
                dropIdTipoLogradouro.SelectedIndex = 0;
                dropIdTipoResidencia.Visible = false;
                dropIdTipoResidencia.SelectedIndex = 0;
                txtRua.Visible = false;
                txtRua.Text = "";
                txtNumero.Visible = false;
                txtNumero.Text = "";
                txtBairro.Visible = false;
                txtBairro.Text = "";
                txtCEP.Visible = false;
                txtCEP.Text = "";
                txtObservacao.Visible = false;
                txtObservacao.Text = "";
                dropIdPais.Visible = false;
                dropIdPais.SelectedIndex = 0;
                dropIdEstado.Visible = false;
                dropIdEstado.SelectedIndex = 0;
                dropIdCidade.Visible = false;
                dropIdCidade.SelectedIndex = 0;
                chkIncluir.Visible = false;
                chkIncluir.Checked = false;
                btnAplicarEndereco.Visible = false;

                CalculaFrete();
            }
            else
            {
                txtNomeEndereco.Visible = true;
                txtNomeDestinatario.Visible = true;
                dropIdTipoLogradouro.Visible = true;
                dropIdTipoResidencia.Visible = true;
                txtRua.Visible = true;
                txtNumero.Visible = true;
                txtBairro.Visible = true;
                txtCEP.Visible = true;
                txtObservacao.Visible = true;
                dropIdPais.Visible = true;
                dropIdEstado.Visible = true;
                dropIdCidade.Visible = true;
                chkIncluir.Visible = true;
                btnAplicarEndereco.Visible = true;
            }

            lblIncluirEndereco.Visible = false;
            lblIncluirEndereco.Text = "";
        }

        public void CalculaFrete()
        {
            // Irá calcular frete conforme o endereço de entrega alterar

            double frete = 0;

            if (Convert.ToInt32(dropIdEnderecoEntrega.SelectedValue) > 0 || Session["dropIdEstado"] != null)
            {
                Endereco endereco = new Endereco();

                if (Session["dropIdEstado"] != null)
                {
                    endereco.Cidade.Estado.ID = Convert.ToInt32(Session["dropIdEstado"]);
                }
                else
                {
                    // passar o ID do endereço selecionado 
                    endereco.ID = Convert.ToInt32(dropIdEnderecoEntrega.SelectedValue);

                    // pesquisa no BD pelo cliente com e-mail
                    entidades = commands["CONSULTAR"].execute(endereco).Entidades;

                    endereco = (Endereco)entidades.ElementAt(0);
                }

                int quantidade = 0;

                using (ShoppingCartActions usersShoppingCart = new ShoppingCartActions())
                {
                    // pega quantidade de livros no carrinho
                    quantidade = usersShoppingCart.GetCount();
                }

                // if por ID de estado e arranjo de região
                // SUL
                if (endereco.Cidade.Estado.ID == 18 ||
                    endereco.Cidade.Estado.ID == 23 ||
                    endereco.Cidade.Estado.ID == 24)
                {
                    frete = (quantidade * 5.5);
                }
                // SUDESTE
                else if (endereco.Cidade.Estado.ID == 8 ||
                    endereco.Cidade.Estado.ID == 11 ||
                    endereco.Cidade.Estado.ID == 19 ||
                    endereco.Cidade.Estado.ID == 26)
                {
                    frete = (quantidade * 4.5);
                }
                // CENTRO-OESTE
                else if (endereco.Cidade.Estado.ID == 7 ||
                    endereco.Cidade.Estado.ID == 9 ||
                    endereco.Cidade.Estado.ID == 12 ||
                    endereco.Cidade.Estado.ID == 13)
                {
                    frete = (quantidade * 6.5);
                }
                // NORTE
                else if (endereco.Cidade.Estado.ID == 1 ||
                    endereco.Cidade.Estado.ID == 3 ||
                    endereco.Cidade.Estado.ID == 4 ||
                    endereco.Cidade.Estado.ID == 14 ||
                    endereco.Cidade.Estado.ID == 21 ||
                    endereco.Cidade.Estado.ID == 22 ||
                    endereco.Cidade.Estado.ID == 27)
                {
                    frete = (quantidade * 8.5);
                }
                // NORDESTE (2, 5, 6, 10, 15, 16, 17, 20 e 25)
                else
                {
                    frete = (quantidade * 7.5);
                }
            }

            if (frete != 0)
            {
                // passa para lblFrete o valor do frete
                lblFrete.Text = String.Format("{0:c}", frete);
                float subtotal;
                if (float.TryParse(lblSubtotal.Text.Remove(0, 3), out subtotal))
                {
                    lblTotal.Text = String.Format("{0:c}", frete + subtotal);
                }
            }
        }

        protected void btnAplicaCupomPromo_Click(object sender, EventArgs e)
        {
            Cupom cupomPromo = new Cupom();
            cupomPromo.CodigoCupom = txtCupomPromo.Text;

            entidades = commands["CONSULTAR"].execute(cupomPromo).Entidades;

            // verifica se veio APENAS 1 
            if (entidades.Count == 1)
            {
                cupomPromo = (Cupom)entidades.ElementAt(0);

                //se o código é IGUAL ao que foi digitado
                if (cupomPromo.CodigoCupom.Trim().Equals(txtCupomPromo.Text.Trim()))
                {
                    // verifica se é um cupom promocional
                    if (cupomPromo.Tipo.ID == 2)
                    {
                        // verifica se cupom está ativo
                        if (cupomPromo.Status == 'A')
                        {
                            Session["idCupomPromo"] = cupomPromo.ID;
                            lblResultadoAplicaCupomPromo.Visible = false;
                            lblResultadoAplicaCupomPromo.Text = "";
                            AplicaCupom();
                        }
                        else
                        {
                            lblResultadoAplicaCupomPromo.Visible = true;
                            lblResultadoAplicaCupomPromo.Text = "CUPOM INATIVO!";
                            Session["idCupomPromo"] = null;
                        }

                    }
                    else
                    {
                        lblResultadoAplicaCupomPromo.Visible = true;
                        lblResultadoAplicaCupomPromo.Text = "NÃO É UM CUPOM PROMOCIONAL!";
                        Session["idCupomPromo"] = null;
                    }
                }
                else
                {
                    lblResultadoAplicaCupomPromo.Visible = true;
                    lblResultadoAplicaCupomPromo.Text = "CUPOM INEXISTENTE!";
                    Session["idCupomPromo"] = null;
                }
            }
            else
            {
                lblResultadoAplicaCupomPromo.Visible = true;
                lblResultadoAplicaCupomPromo.Text = "CUPOM INEXISTENTE!";
                Session["idCupomPromo"] = null;
            }
        }

        protected void dropIdCupomTroca1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dropIdCupomTroca1.SelectedIndex != 0)
            {
                // passar o ID do cupom selecionado 
                Session["dropIdCupomTroca1"] = Convert.ToInt32(dropIdCupomTroca1.SelectedValue);
                AplicaCupom();
                dropIdCupomTroca2.Visible = true;
            }
            else
            {
                Session["dropIdCupomTroca1"] = null;
                Session["dropIdCupomTroca2"] = null;
                Session["dropIdCupomTroca3"] = null;
                Session["dropIdCupomTroca4"] = null;
                Session["dropIdCupomTroca5"] = null;
                dropIdCupomTroca2.Visible = false;
                dropIdCupomTroca3.Visible = false;
                dropIdCupomTroca4.Visible = false;
                dropIdCupomTroca5.Visible = false;
                dropIdCupomTroca2.SelectedIndex = 0;
                dropIdCupomTroca3.SelectedIndex = 0;
                dropIdCupomTroca4.SelectedIndex = 0;
                dropIdCupomTroca5.SelectedIndex = 0;
            }
        }

        protected void dropIdCupomTroca2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dropIdCupomTroca2.SelectedIndex != 0)
            {
                // passar o ID do cupom selecionado 
                Session["dropIdCupomTroca2"] = Convert.ToInt32(dropIdCupomTroca2.SelectedValue);
                AplicaCupom();
                dropIdCupomTroca3.Visible = true;

            }
            else
            {
                Session["dropIdCupomTroca2"] = null;
                Session["dropIdCupomTroca3"] = null;
                Session["dropIdCupomTroca4"] = null;
                Session["dropIdCupomTroca5"] = null;
                dropIdCupomTroca3.Visible = false;
                dropIdCupomTroca4.Visible = false;
                dropIdCupomTroca5.Visible = false;
                dropIdCupomTroca3.SelectedIndex = 0;
                dropIdCupomTroca4.SelectedIndex = 0;
                dropIdCupomTroca5.SelectedIndex = 0;
            }

        }

        protected void dropIdCupomTroca3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dropIdCupomTroca3.SelectedIndex != 0)
            {
                // passar o ID do cupom selecionado 
                Session["dropIdCupomTroca3"] = Convert.ToInt32(dropIdCupomTroca3.SelectedValue);
                AplicaCupom();
                dropIdCupomTroca4.Visible = true;

            }
            else
            {
                Session["dropIdCupomTroca3"] = null;
                Session["dropIdCupomTroca4"] = null;
                Session["dropIdCupomTroca5"] = null;
                dropIdCupomTroca4.Visible = false;
                dropIdCupomTroca5.Visible = false;
                dropIdCupomTroca4.SelectedIndex = 0;
                dropIdCupomTroca5.SelectedIndex = 0;
            }

        }

        protected void dropIdCupomTroca4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dropIdCupomTroca4.SelectedIndex != 0)
            {
                // passar o ID do cupom selecionado 
                Session["dropIdCupomTroca4"] = Convert.ToInt32(dropIdCupomTroca4.SelectedValue);
                AplicaCupom();
                dropIdCupomTroca5.Visible = true;

            }
            else
            {
                Session["dropIdCupomTroca4"] = null;
                Session["dropIdCupomTroca5"] = null;
                dropIdCupomTroca5.Visible = false;
                dropIdCupomTroca5.SelectedIndex = 0;
            }
        }

        protected void dropIdCupomTroca5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dropIdCupomTroca5.SelectedIndex != 0)
            {
                // passar o ID do cupom selecionado 
                Session["dropIdCupomTroca5"] = Convert.ToInt32(dropIdCupomTroca5.SelectedValue);
                AplicaCupom();

            }
            else
            {
                Session["dropIdCupomTroca5"] = null;
                dropIdCupomTroca5.SelectedIndex = 0;
            }
        }

        public void AplicaCupom()
        {
            Cupom cupom = new Cupom();
            double total = (double)cartTotal;

            if (Session["idCupomPromo"] != null)
            {
                cupom = new Cupom();
                // passar o ID do cupom selecionado 
                cupom.ID = Convert.ToInt32(Session["idCupomPromo"].ToString());

                cupom = (Cupom)commands["CONSULTAR"].execute(cupom).Entidades.ElementAt(0);
                total = total - (total * cupom.ValorCupom);
            }

            if (Session["dropIdCupomTroca1"] != null)
            {
                cupom = new Cupom();
                // passar o ID do cupom selecionado 
                cupom.ID = Convert.ToInt32(Session["dropIdCupomTroca1"].ToString());

                cupom = (Cupom)commands["CONSULTAR"].execute(cupom).Entidades.ElementAt(0);
                total = total - (cupom.ValorCupom);
            }

            if (Session["dropIdCupomTroca2"] != null)
            {
                cupom = new Cupom();
                // passar o ID do cupom selecionado 
                cupom.ID = Convert.ToInt32(Session["dropIdCupomTroca2"].ToString());

                cupom = (Cupom)commands["CONSULTAR"].execute(cupom).Entidades.ElementAt(0);
                total = total - (cupom.ValorCupom);
            }

            if (Session["dropIdCupomTroca3"] != null)
            {
                cupom = new Cupom();
                // passar o ID do cupom selecionado 
                cupom.ID = Convert.ToInt32(Session["dropIdCupomTroca3"].ToString());

                cupom = (Cupom)commands["CONSULTAR"].execute(cupom).Entidades.ElementAt(0);
                total = total - (cupom.ValorCupom);
            }

            if (Session["dropIdCupomTroca4"] != null)
            {
                cupom = new Cupom();
                // passar o ID do cupom selecionado 
                cupom.ID = Convert.ToInt32(Session["dropIdCupomTroca4"].ToString());

                cupom = (Cupom)commands["CONSULTAR"].execute(cupom).Entidades.ElementAt(0);
                total = total - (cupom.ValorCupom);
            }

            if (Session["dropIdCupomTroca5"] != null)
            {
                cupom = new Cupom();
                // passar o ID do cupom selecionado 
                cupom.ID = Convert.ToInt32(Session["dropIdCupomTroca5"].ToString());

                cupom = (Cupom)commands["CONSULTAR"].execute(cupom).Entidades.ElementAt(0);
                total = total - (cupom.ValorCupom);
            }

            if (total < 0)
            {
                total = 0.0;
            }

            lblSubtotal.Text = String.Format("{0:c}", total);
            if (!lblFrete.Text.Trim().Equals("-"))
            {
                float frete;
                if (float.TryParse(lblFrete.Text.Remove(0, 3), out frete))
                {
                    lblTotal.Text = String.Format("{0:c}", frete + total);
                    RecebeValorCC();
                }
            }
        }

        public void RecebeValorCC()
        {
            decimal aPagar = 0;
            decimal valorCC = 0;
            if (!lblTotal.Text.Trim().Equals("-"))
            {
                if (decimal.TryParse(lblTotal.Text.Remove(0, 3), out valorCC))
                {
                    aPagar = valorCC;
                }
            }
            else if (decimal.TryParse(lblSubtotal.Text.Remove(0, 3), out valorCC))
            {
                aPagar = valorCC;
            }

            if (Session["txtValorCCPagto1"] != null)
            {
                valorCC = 0;
                if (decimal.TryParse(Session["txtValorCCPagto1"].ToString(), NumberStyles.Number, CultureInfo.CreateSpecificCulture("en-US"), out valorCC))
                {
                    aPagar = aPagar - valorCC;
                }
            }

            if (Session["txtValorCCPagto2"] != null)
            {
                valorCC = 0;
                if (decimal.TryParse(Session["txtValorCCPagto2"].ToString(), NumberStyles.Number, CultureInfo.CreateSpecificCulture("en-US"), out valorCC))
                {
                    aPagar = aPagar - valorCC;
                }
            }

            if (Session["txtValorCCPagto3"] != null)
            {
                valorCC = 0;
                if (decimal.TryParse(Session["txtValorCCPagto3"].ToString(), NumberStyles.Number, CultureInfo.CreateSpecificCulture("en-US"), out valorCC))
                {
                    aPagar = aPagar - valorCC;
                }
            }

            if (Session["txtValorCCPagto4"] != null)
            {
                valorCC = 0;
                if (decimal.TryParse(Session["txtValorCCPagto4"].ToString(), NumberStyles.Number, CultureInfo.CreateSpecificCulture("en-US"), out valorCC))
                {
                    aPagar = aPagar - valorCC;
                }
            }

            if (Session["txtValorCCPagto5"] != null)
            {
                valorCC = 0;
                if (decimal.TryParse(Session["txtValorCCPagto5"].ToString(), NumberStyles.Number, CultureInfo.CreateSpecificCulture("en-US"), out valorCC))
                {
                    aPagar = aPagar - valorCC;
                }
            }

            if (aPagar < 0)
            {
                lblResultadoPagto.Visible = true;
                lblResultadoPagto.Text = "VALORES DOS CARTÕES DE CRÉDITO NÃO PODE SER MAIOR QUE O VALOR A PAGAR!";

                valorCC = 0;
                if (!lblTotal.Text.Trim().Equals("-"))
                {
                    if (decimal.TryParse(lblTotal.Text.Remove(0, 3), out valorCC))
                    {
                        aPagar = valorCC;
                    }
                }
            }
            else
            {
                lblResultadoPagto.Visible = false;
            }

            if (aPagar == 0)
            {
                CheckoutBtn.Enabled = true;
            }
            else
            {
                CheckoutBtn.Enabled = false;
            }

            lblAPagar.Text = String.Format("{0:c}", aPagar);
        }

        protected void txtValorCCPagto1_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtValorCCPagto1.Text))
            {
                // passar o ID do cupom selecionado 
                Session["txtValorCCPagto1"] = txtValorCCPagto1.Text;
                RecebeValorCC();
                dropIdCC2.Visible = true;
                txtValorCCPagto2.Visible = true;
            }
            else
            {
                Session["txtValorCCPagto1"] = null;
                Session["txtValorCCPagto2"] = null;
                Session["txtValorCCPagto3"] = null;
                Session["txtValorCCPagto4"] = null;
                Session["txtValorCCPagto5"] = null;
                dropIdCC2.Visible = false;
                dropIdCC2.SelectedIndex = 0;
                txtValorCCPagto2.Visible = false;
                txtValorCCPagto2.Text = "";
                dropIdCC3.Visible = false;
                dropIdCC3.SelectedIndex = 0;
                txtValorCCPagto3.Visible = false;
                txtValorCCPagto3.Text = "";
                dropIdCC4.Visible = false;
                dropIdCC4.SelectedIndex = 0;
                txtValorCCPagto4.Visible = false;
                txtValorCCPagto4.Text = "";
                dropIdCC5.Visible = false;
                dropIdCC5.SelectedIndex = 0;
                txtValorCCPagto5.Visible = false;
                txtValorCCPagto5.Text = "";
            }
        }

        protected void txtValorCCPagto2_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtValorCCPagto2.Text))
            {
                // passar o ID do cupom selecionado 
                Session["txtValorCCPagto2"] = txtValorCCPagto2.Text;
                RecebeValorCC();
                dropIdCC3.Visible = true;
                txtValorCCPagto3.Visible = true;
            }
            else
            {
                Session["txtValorCCPagto2"] = null;
                Session["txtValorCCPagto3"] = null;
                Session["txtValorCCPagto4"] = null;
                Session["txtValorCCPagto5"] = null;
                dropIdCC3.Visible = false;
                dropIdCC3.SelectedIndex = 0;
                txtValorCCPagto3.Visible = false;
                txtValorCCPagto3.Text = "";
                dropIdCC4.Visible = false;
                dropIdCC4.SelectedIndex = 0;
                txtValorCCPagto4.Visible = false;
                txtValorCCPagto4.Text = "";
                dropIdCC5.Visible = false;
                dropIdCC5.SelectedIndex = 0;
                txtValorCCPagto5.Visible = false;
                txtValorCCPagto5.Text = "";
            }
        }

        protected void txtValorCCPagto3_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtValorCCPagto3.Text))
            {
                // passar o ID do cupom selecionado 
                Session["txtValorCCPagto3"] = txtValorCCPagto3.Text;
                RecebeValorCC();
                dropIdCC4.Visible = true;
                txtValorCCPagto4.Visible = true;
            }
            else
            {
                Session["txtValorCCPagto3"] = null;
                Session["txtValorCCPagto4"] = null;
                Session["txtValorCCPagto5"] = null;
                dropIdCC4.Visible = false;
                dropIdCC4.SelectedIndex = 0;
                txtValorCCPagto4.Visible = false;
                txtValorCCPagto4.Text = "";
                dropIdCC5.Visible = false;
                dropIdCC5.SelectedIndex = 0;
                txtValorCCPagto5.Visible = false;
                txtValorCCPagto5.Text = "";
            }
        }

        protected void txtValorCCPagto4_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtValorCCPagto4.Text))
            {
                // passar o ID do cupom selecionado 
                Session["txtValorCCPagto4"] = txtValorCCPagto4.Text;
                RecebeValorCC();
                dropIdCC5.Visible = true;
                txtValorCCPagto5.Visible = true;
            }
            else
            {
                Session["txtValorCCPagto4"] = null;
                Session["txtValorCCPagto5"] = null;
                dropIdCC5.Visible = false;
                dropIdCC5.SelectedIndex = 0;
                txtValorCCPagto5.Visible = false;
                txtValorCCPagto5.Text = "";
            }
        }

        protected void txtValorCCPagto5_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtValorCCPagto5.Text))
            {
                // passar o ID do cupom selecionado 
                Session["txtValorCCPagto5"] = txtValorCCPagto5.Text;
                RecebeValorCC();
            }
            else
            {
                Session["txtValorCCPagto5"] = null;
            }
        }

        protected void btnAplicarEndereco_Click(object sender, EventArgs e)
        {

            StringBuilder sb = new StringBuilder();

            // verifica se nome está vazio ou nulo
            if (String.IsNullOrEmpty(txtNomeEndereco.Text.Trim()))
            {
                sb.Append("NOME DO ENDEREÇO É UM CAMPO OBRIGATÓRIO! <br />");
            }

            // verifica se destinatário está vazio ou nulo
            if (String.IsNullOrEmpty(txtNomeDestinatario.Text.Trim()))
            {
                sb.Append("NOME DO ENDEREÇO É UM CAMPO OBRIGATÓRIO! <br />");
            }

            // verifica se tipo da residência foi selecionado
            if (Convert.ToInt32(dropIdTipoResidencia.SelectedValue) == 0)
            {
                sb.Append("TIPO DA RESIDÊNCIA É UM CAMPO OBRIGATÓRIO! <br />");
            }

            // verifica se tipo do logradouro foi selecionado
            if (Convert.ToInt32(dropIdTipoLogradouro.SelectedValue) == 0)
            {
                sb.Append("TIPO DO LOGRADOURO É UM CAMPO OBRIGATÓRIO! <br />");
            }

            // verifica se rua está vazio ou nulo
            if (String.IsNullOrEmpty(txtRua.Text.Trim()))
            {
                sb.Append("LOGRADOURO É UM CAMPO OBRIGATÓRIO! <br />");
            }

            // verifica se número está vazio ou nulo
            if (String.IsNullOrEmpty(txtNumero.Text.Trim()))
            {
                sb.Append("NÚMERO DO ENDEREÇO É UM CAMPO OBRIGATÓRIO! <br />");
            }

            // verifica se bairro está vazio ou nulo
            if (String.IsNullOrEmpty(txtBairro.Text.Trim()))
            {
                sb.Append("BAIRRO É UM CAMPO OBRIGATÓRIO! <br />");
            }

            // verifica se uma cidade foi selecionada
            if (Convert.ToInt32(dropIdCidade.SelectedValue) == 0)
            {
                sb.Append("UMA CIDADE DEVE SER SELECIONADA, POIS É UM CAMPO OBRIGATÓRIO! <br />");
            }

            // verifica se um estado foi selecionado
            if (Convert.ToInt32(dropIdEstado.SelectedValue) == 0)
            {
                Session["dropIdEstado"] = null;
            }
            else
            {
                Session["dropIdEstado"] = dropIdEstado.SelectedValue;
            }

            // verifica se CEP está vazio ou nulo
            if (String.IsNullOrEmpty(txtCEP.Text.Trim()))
            {
                sb.Append("CEP É UM CAMPO OBRIGATÓRIO! <br />");
            }
            else
            {
                string cep = txtCEP.Text.Trim();

                // remove caracteres indesejados
                cep = cep.Replace(".", "");
                cep = cep.Replace("-", "");
                cep = cep.Replace(" ", "");

                // verifica se CEP está vazio ou nulo
                if (cep.Equals(null) || cep.Equals("") || cep.Equals(String.Empty))
                {
                    sb.Append("CEP É UM CAMPO OBRIGATÓRIO! <br />");
                }
                else
                {
                    // expressão regular para verificar se contém 8 dígitos numéricos
                    Regex Rgx = new Regex(@"^\d{8}$");

                    // Verifica tamanho do CEP 8 dígitos
                    if (!Rgx.IsMatch(cep))
                    {
                        sb.Append("CEP INCORRETO! OBRIGATÓRIO 8 DÍGITOS NUMÉRICOS! <br />");
                    }
                }
            }

            if (!string.IsNullOrEmpty(sb.ToString()))
            {
                lblIncluirEndereco.Visible = true;
                lblIncluirEndereco.Text = sb.ToString();
            }
            else
            {
                lblIncluirEndereco.Visible = false;
            }

            CalculaFrete();

        }

        protected void dropIdCC1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(dropIdCC1.SelectedValue) != -1)
            {
                txtNomeImpressoCC1.Visible = false;
                txtNumeroCC1.Visible = false;
                txtCodigoSegurancaCC1.Visible = false;
                dropIdBandeiraCC1.Visible = false;
                chkIncluirCC1.Visible = false;

                txtNomeImpressoCC1.Text = "";
                txtNumeroCC1.Text = "";
                txtCodigoSegurancaCC1.Text = "";
                txtDtVencimentoCC1.Text = "";
                dropIdBandeiraCC1.SelectedIndex = 0;
                chkIncluirCC1.Checked = false;
                //lblIncluirCC1.Visible = false;
                //btnIncluirCC1.Visible = false;
            }
            else
            {
                txtNomeImpressoCC1.Visible = true;
                txtNumeroCC1.Visible = true;
                txtCodigoSegurancaCC1.Visible = true;
                txtDtVencimentoCC1.Visible = true;
                dropIdBandeiraCC1.Visible = true;
                chkIncluirCC1.Visible = true;
                //lblIncluirCC1.Visible = true;
                //btnIncluirCC1.Visible = true;
            }
        }

        protected void dropIdCC2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(dropIdCC2.SelectedValue) != -1)
            {
                txtNomeImpressoCC2.Visible = false;
                txtNumeroCC2.Visible = false;
                txtCodigoSegurancaCC2.Visible = false;
                dropIdBandeiraCC2.Visible = false;
                chkIncluirCC2.Visible = false;

                txtNomeImpressoCC2.Text = "";
                txtNumeroCC2.Text = "";
                txtCodigoSegurancaCC2.Text = "";
                txtDtVencimentoCC2.Text = "";
                dropIdBandeiraCC2.SelectedIndex = 0;
                chkIncluirCC2.Checked = false;
                //lblIncluirCC2.Visible = false;
                //btnIncluirCC2.Visible = false;
            }
            else
            {
                txtNomeImpressoCC2.Visible = true;
                txtNumeroCC2.Visible = true;
                txtCodigoSegurancaCC2.Visible = true;
                txtDtVencimentoCC2.Visible = true;
                dropIdBandeiraCC2.Visible = true;
                chkIncluirCC2.Visible = true;
                //lblIncluirCC2.Visible = true;
                //btnIncluirCC2.Visible = true;
            }
        }

        protected void dropIdCC3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(dropIdCC3.SelectedValue) != -1)
            {
                txtNomeImpressoCC3.Visible = false;
                txtNumeroCC3.Visible = false;
                txtCodigoSegurancaCC3.Visible = false;
                dropIdBandeiraCC3.Visible = false;
                chkIncluirCC3.Visible = false;

                txtNomeImpressoCC3.Text = "";
                txtNumeroCC3.Text = "";
                txtCodigoSegurancaCC3.Text = "";
                txtDtVencimentoCC3.Text = "";
                dropIdBandeiraCC3.SelectedIndex = 0;
                chkIncluirCC3.Checked = false;
                //lblIncluirCC3.Visible = false;
                //btnIncluirCC3.Visible = false;
            }
            else
            {
                txtNomeImpressoCC3.Visible = true;
                txtNumeroCC3.Visible = true;
                txtCodigoSegurancaCC3.Visible = true;
                txtDtVencimentoCC3.Visible = true;
                dropIdBandeiraCC3.Visible = true;
                chkIncluirCC3.Visible = true;
                //lblIncluirCC3.Visible = true;
                //btnIncluirCC3.Visible = true;
            }
        }

        protected void dropIdCC4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(dropIdCC4.SelectedValue) != -1)
            {
                txtNomeImpressoCC4.Visible = false;
                txtNumeroCC4.Visible = false;
                txtCodigoSegurancaCC4.Visible = false;
                dropIdBandeiraCC4.Visible = false;
                chkIncluirCC4.Visible = false;

                txtNomeImpressoCC4.Text = "";
                txtNumeroCC4.Text = "";
                txtCodigoSegurancaCC4.Text = "";
                txtDtVencimentoCC4.Text = "";
                dropIdBandeiraCC4.SelectedIndex = 0;
                chkIncluirCC4.Checked = false;
                //lblIncluirCC4.Visible = false;
                //btnIncluirCC4.Visible = false;
            }
            else
            {
                txtNomeImpressoCC4.Visible = true;
                txtNumeroCC4.Visible = true;
                txtCodigoSegurancaCC4.Visible = true;
                txtDtVencimentoCC4.Visible = true;
                dropIdBandeiraCC4.Visible = true;
                chkIncluirCC4.Visible = true;
                //lblIncluirCC4.Visible = true;
                //btnIncluirCC4.Visible = true;
            }
        }

        protected void dropIdCC5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(dropIdCC5.SelectedValue) != -1)
            {
                txtNomeImpressoCC5.Visible = false;
                txtNumeroCC5.Visible = false;
                txtCodigoSegurancaCC5.Visible = false;
                dropIdBandeiraCC5.Visible = false;
                chkIncluirCC5.Visible = false;

                txtNomeImpressoCC5.Text = "";
                txtNumeroCC5.Text = "";
                txtCodigoSegurancaCC5.Text = "";
                txtDtVencimentoCC5.Text = "";
                dropIdBandeiraCC5.SelectedIndex = 0;
                chkIncluirCC5.Checked = false;
                //lblIncluirCC5.Visible = false;
                //btnIncluirCC5.Visible = false;
            }
            else
            {
                txtNomeImpressoCC5.Visible = true;
                txtNumeroCC5.Visible = true;
                txtCodigoSegurancaCC5.Visible = true;
                txtDtVencimentoCC5.Visible = true;
                dropIdBandeiraCC5.Visible = true;
                chkIncluirCC5.Visible = true;
                //lblIncluirCC5.Visible = true;
                //btnIncluirCC5.Visible = true;
            }
        }
    }
}
