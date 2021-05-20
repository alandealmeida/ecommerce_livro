using Dominio.Cliente;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace elivro.Account
{
    public partial class ListaMeusCartoes : ViewGenerico
    {
        protected override void Page_Load(object sender, EventArgs e)
        {
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
                "<th>Bandeira</th>" +
                "<th>Final</th>" +
                "<th>Vencimento</th>" +
                "</tr></THEAD>";

            string linha = "<tr>" +
                "<td>{0}</td>" +
                "<td>{1}</td>" +
                "<td>{2}</td>" +
                "</tr>";

            Cliente cliente = commands["CONSULTAR"].execute(new Cliente() { Email = Context.User.Identity.GetUserName() }).Entidades.Cast<Cliente>().ElementAt(0);

            entidades = commands["CONSULTAR"].execute(new ClienteCartao() { ID = cliente.ID }).Entidades;
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
            List<ClienteCartao> cartoes = new List<ClienteCartao>();
            foreach (ClienteCartao cartao in entidades)
            {
                cartoes.Add(cartao);
            }

            foreach (var cartao in cartoes)
            {
                conteudo.AppendFormat(linha,
                    cartao.CC.Bandeira.Nome,
                    cartao.CC.NumeroCC.ToString().Substring(12, 4),
                    cartao.CC.DataVencimento.ToString().Substring(3, 2) + "/" +
                    cartao.CC.DataVencimento.ToString().Substring(6, 4)
                );
            }
            string tabelafinal = string.Format(GRID, tituloColunas, conteudo.ToString());
            divTable.InnerHtml = tabelafinal;
        }
    }
}