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
    public partial class ListaMeusCupons : ViewGenerico
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
                "<th>Código do Cupom</th>" +
                "<th>Valor do Cupom</th>" +
                "</tr></THEAD>";

            string linha = "<tr>" +
                "<td>{0}</td>" +
                "<td>R${1}</td>" +
                "</tr>";

            Cliente cliente = commands["CONSULTAR"].execute(new Cliente() { Email = Context.User.Identity.GetUserName() }).Entidades.Cast<Cliente>().ElementAt(0);

            entidades = commands["CONSULTAR"].execute(new Cupom() { IdCliente = cliente.ID, Status = 'A', Tipo = new TipoCupom() { ID = 1 } }).Entidades;
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
            List<Cupom> cupons = new List<Cupom>();
            foreach (Cupom cupom in entidades)
            {
                cupons.Add(cupom);
            }

            foreach (var cupom in cupons)
            {
                conteudo.AppendFormat(linha,
                cupom.CodigoCupom,
                cupom.ValorCupom
                );
            }
            string tabelafinal = string.Format(GRID, tituloColunas, conteudo.ToString());
            divTable.InnerHtml = tabelafinal;
        }
    }
}