using Core.DAO;
using Dominio;
using Dominio.Cliente;
using Dominio.Venda;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Newtonsoft.Json.Linq;
using elivro.Models;
using Dominio.Livro;

namespace elivro.Checkout
{
    public partial class CheckoutReview : ViewGenerico
    {

        private Dominio.Venda.Pedido pedido = new Dominio.Venda.Pedido();
        private Dominio.Venda.PedidoDetalhe pedidoDetalhe = new Dominio.Venda.PedidoDetalhe();
        protected override void Page_Load(object sender, EventArgs e)
        {
            Session["tipo_sel"] = null;
            if (!IsPostBack)
            {
                // limpa carrinho
                using (elivro.Logic.ShoppingCartActions usersShoppingCart =
                    new elivro.Logic.ShoppingCartActions())
                {
                    usersShoppingCart.EmptyCart();
                }

                // Limpa sessão
                Session["currentOrderId"] = string.Empty;

                var pedido = new Pedido();
                if (!string.IsNullOrEmpty(Request.QueryString["idPedido"]))
                {
                    pedido.ID = Convert.ToInt32(Request.QueryString["idPedido"]);
                }


                pedido = commands["CONSULTAR"].execute(pedido).Entidades.Cast<Pedido>().ElementAt(0);

                pedido.Detalhes = commands["CONSULTAR"].execute(new PedidoDetalhe() { IdPedido = pedido.ID }).Entidades.Cast<PedidoDetalhe>().ToList();

                pedido.CCs = commands["CONSULTAR"].execute(new CartaoCreditoPedido() { IdPedido = pedido.ID }).Entidades.Cast<CartaoCreditoPedido>().ToList();

                entidades = new List<EntidadeDominio>();
                entidades = commands["CONSULTAR"].execute(new PedidoCupom() { ID = pedido.ID }).Entidades;
                if (entidades.Count > 0)
                {
                    pedido.CupomPromocional = commands["CONSULTAR"].execute(new PedidoCupom() { ID = pedido.ID }).Entidades.Cast<PedidoCupom>().ElementAt(0).Cupom;
                }


                entidades = new List<EntidadeDominio>();
                entidades = commands["CONSULTAR"].execute(new Cupom() { IdPedido = pedido.ID }).Entidades;
                if (entidades.Count > 0)
                {
                    pedido.CuponsTroca = commands["CONSULTAR"].execute(new Cupom() { IdPedido = pedido.ID }).Entidades.Cast<Cupom>().ToList();
                }

                // Set OrderId.
                Session["currentOrderId"] = pedido.ID;

                pedido.EnderecoEntrega = commands["CONSULTAR"].execute(new Endereco() { ID = pedido.EnderecoEntrega.ID }).Entidades.Cast<Endereco>().ElementAt(0);

                lblDestinatario.InnerText = "Destinatário: " +
                                        pedido.EnderecoEntrega.Destinatario;

                lblEndereco.InnerText = "Endereço de entrega: " + 
                                        pedido.EnderecoEntrega.Nome + " - " + 
                                        pedido.EnderecoEntrega.TipoLogradouro.Nome + " " + 
                                        pedido.EnderecoEntrega.Logradouro + ", " + 
                                        pedido.EnderecoEntrega.Numero + " - " +
                                        pedido.EnderecoEntrega.Bairro + ", " +
                                        pedido.EnderecoEntrega.Cidade.Nome + " - " +
                                        pedido.EnderecoEntrega.Cidade.Estado.Sigla + ", " +
                                        "CEP: " + pedido.EnderecoEntrega.CEP + ", " +
                                        pedido.EnderecoEntrega.Observacao;

                lblStatus.InnerText = "Status do Pedido: " + pedido.Status.Nome;

                LabelSubTotalText.Text = "Subtotal do Pedido: R$" + Convert.ToString(pedido.Total - pedido.Frete);

                LabelFreteText.Text = "Frete: R$" + Convert.ToString(pedido.Frete);

                LabelTotalText.Text = "Total do Pedido: R$" + Convert.ToString(pedido.Total);

                if (pedido.Status.ID == 6 || pedido.Status.ID == 7 || pedido.Status.ID == 8)
                {
                    lblEndereco.Visible = false;
                } else
                {
                    lblEndereco.Visible = true;
                }

                TrocaPedido.HRef = "TrocaPedido.aspx?idPedido=" + pedido.ID;

                if (pedido.Status.ID == 5)
                {
                    TrocaPedido.Visible = true;
                }
                else
                {
                    TrocaPedido.Visible = false;
                }

                ConstruirTabela(pedido);
            }
            else
            {
                //Response.Redirect("./CheckoutError.aspx?Desc=ID%20de%20pedido%20incompatível.");
            }

        }
        private void ConstruirTabela(Pedido pedido)
            {
                int evade = 0;

                string GRID = "<TABLE class='table table-bordered' id='GridViewGeral' width='100%' cellspacing='0'>{0}<TBODY>{1}</TBODY></TABLE>";
                string tituloColunas = "<THEAD><tr>" +
                    "<th>Título</th>" +
                    "<th>Valor Unitário</th>" +
                    "<th>Quantidade</th>" +
                    "</tr></THEAD>";

                string linha = "<tr>" +
                    "<td>{0}</td>" +
                    "<td>R${1}</td>" +
                    "<td>{2}</td>" +
                    "</tr>";

            pedidoDetalhe.IdPedido = pedido.ID;

            entidades = commands["CONSULTAR"].execute(pedidoDetalhe).Entidades;

                try
                {
                    evade = entidades.Count;
                }
                catch
                {
                    evade = 0;
                }

                StringBuilder conteudo = new StringBuilder();

                // lista para conter todos pedidos retornados do BD
                List<PedidoDetalhe> pedidoDetalhes = new List<PedidoDetalhe>();
                foreach (PedidoDetalhe pedidoDetalhe in entidades)
                {
                    pedidoDetalhes.Add(pedidoDetalhe);
                }

                foreach (var pedidoDetalhe in pedidoDetalhes)
                {
                    conteudo.AppendFormat(linha,
                    pedidoDetalhe.Livro.Titulo,
                    pedidoDetalhe.ValorUnit,
                    pedidoDetalhe.Quantidade
                    );
                }
                string tabelafinal = string.Format(GRID, tituloColunas, conteudo.ToString());
                divTable.InnerHtml = tabelafinal;
                pedido.ID = 0;
            }
    }
}