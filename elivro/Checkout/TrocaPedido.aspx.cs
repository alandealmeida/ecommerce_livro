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


namespace elivro.Checkout
{
    public partial class TrocaPedido : ViewGenerico
    {
        private Dominio.Venda.Pedido pedido = new Dominio.Venda.Pedido();
        private Dominio.Venda.PedidoDetalhe pedidoDetalhe = new Dominio.Venda.PedidoDetalhe();
        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var pedido = new Pedido();
                if (!string.IsNullOrEmpty(Request.QueryString["idPedido"]))
                {
                    pedido.ID = Convert.ToInt32(Request.QueryString["idPedido"]);
                }

                TrocaPedidoTodo.HRef = "GerarPedidoTroca.aspx?idPedido=" + pedido.ID;

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
                "<th>Trocar</th>" +
                "</tr></THEAD>";

            string linha = "<tr>" +
                "<td>{0}</td>" +
                "<td>R${1}</td>" +
                "<td>{2}</td>" +
                "<td> <div class='cart-btn mt-5'> "+
                         "<a href = 'GerarPedidoTroca.aspx?idPedido={3}&idLivro={4}' class='btn amado-btn'>Trocar</a>" +
                      "</div>" +
                "</td>" +
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
                pedidoDetalhe.Quantidade,
                pedidoDetalhe.ID,
                pedidoDetalhe.Livro.ID
                );
            }
            string tabelafinal = string.Format(GRID, tituloColunas, conteudo.ToString());
            divTable.InnerHtml = tabelafinal;
            pedido.ID = 0;
        }
        protected void CheckoutConfirm_Click(object sender, EventArgs e)
        {
            Session["userCheckoutCompleted"] = "true";
            Response.Redirect("./CheckoutComplete.aspx");
        }
    }
}