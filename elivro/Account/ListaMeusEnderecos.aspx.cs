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
    public partial class ListaMeusEnderecos : ViewGenerico
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
                "<th>Nome</th>" +
                "<th>Destinatário</th>" +
                "<th>Endereço</th>" +
                "<th>CEP</th>" +
                "</tr></THEAD>";

            string linha = "<tr>" +
                "<td>{0}</td>" +
                "<td>{1}</td>" +
                "<td>{2}</td>" +
                "<td>{3}</td>" +
                "</tr>";

            Cliente cliente = commands["CONSULTAR"].execute(new Cliente() { Email = Context.User.Identity.GetUserName() }).Entidades.Cast<Cliente>().ElementAt(0);

            entidades = commands["CONSULTAR"].execute(new ClienteEndereco() { ID = cliente.ID }).Entidades;
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
            List<ClienteEndereco> enderecos = new List<ClienteEndereco>();
            foreach (ClienteEndereco endereco in entidades)
            {
                enderecos.Add(endereco);
            }

            foreach (var endereco in enderecos)
            {
                conteudo.AppendFormat(linha,
                    endereco.Endereco.Nome,
                    endereco.Endereco.Destinatario,
                    endereco.Endereco.TipoResidencia.Nome + " - " +
                    endereco.Endereco.TipoLogradouro.Nome + " " +
                    endereco.Endereco.Logradouro + ", " +
                    endereco.Endereco.Numero + " - " +
                    endereco.Endereco.Observacao + " - " +
                    endereco.Endereco.Bairro + ", " +
                    endereco.Endereco.Cidade.Nome + " - " +
                    endereco.Endereco.Cidade.Estado.Sigla,
                    endereco.Endereco.CEP
                );
            }
            string tabelafinal = string.Format(GRID, tituloColunas, conteudo.ToString());
            divTable.InnerHtml = tabelafinal;
        }
    }
}