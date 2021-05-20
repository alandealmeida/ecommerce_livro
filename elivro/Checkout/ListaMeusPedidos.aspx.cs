using Dominio.Venda;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace elivro.Checkout
{
    public partial class ListaMeusPedidos : ViewGenerico
    {
        private Dominio.Venda.Pedido pedido = new Dominio.Venda.Pedido();
        protected override void Page_Load(object sender, EventArgs e)
        {          
            Session["tipo_sel"] = null;
            if (!IsPostBack)
            {
                ConstruirTabela();
            }
        }
        private void ConstruirTabela()
        {
            int evade = 0;

            string GRID = "<TABLE class='table table-bordered' id='GridViewGeral' width='100%' cellspacing='0'>{0}<TBODY>{1}</TBODY></TABLE>";
            string tituloColunas = "<THEAD><tr>" +
                "<th>Número</th>" +
                "<th>Valor</th>" +
                "<th>Data</th>" +
                "<th>Status</th>" +
                "</tr></THEAD>";

            string linha = "<tr>" +

                "<td style='text-align-last: center;'>" +
                    "<a class='nav-link' href='CheckoutReview.aspx?idPedido={0}'>{0}</a>" +
                "</td> " +
                "<td>R${1}</td>" +
                "<td>{2}</td>" +
                "<td>{3}</td>" +
                "</tr>";

            pedido.Usuario = Context.User.Identity.GetUserName();


            entidades = commands["CONSULTAR"].execute(pedido).Entidades;
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
            List<Pedido> pedidos = new List<Pedido>();
            foreach (Pedido pedido in entidades)
            {
                pedidos.Add(pedido);
            }

            foreach (var pedido in pedidos)
            {
                conteudo.AppendFormat(linha,
                pedido.ID,
                Convert.ToDecimal(pedido.Total),
                pedido.DataCadastro,
                pedido.Status.Nome
                );
            }
            string tabelafinal = string.Format(GRID, tituloColunas, conteudo.ToString());
            divTable.InnerHtml = tabelafinal;
            pedido.ID = 0;
        }
    }
}