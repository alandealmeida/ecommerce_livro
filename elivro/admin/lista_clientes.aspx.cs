using Core.Aplicacao;
using Core.DAO;
using Dominio;
using Dominio.Cliente;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace elivro.admin
{
    public partial class lista_clientes : ViewGenerico
    {
        private Dominio.Cliente.Cliente clientePF = new Dominio.Cliente.Cliente();

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
                "<th>ID</th>" +
                "<th>Nome</th>" +
                "<th>CPF</th>" +
                "<th>Data de Nascimento</th>" +
                "<th>Telefone</th>" +
                "<th>E-mail</th>" +
                "<th>Enderecos</th>" +
                "<th>Cartões de Credito</th>" +
                "<th>Data de Cadastro</th>" +
                "<th>Operações</th>" +
                "</tr></THEAD>";
           
            string linha = "<tr>" +
            //linha += "{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td style='text-align-last: center;'><a class='btn fas fa-edit' style='background-color: #ddd; border-color: #999; color: #111' href='Produtor.aspx?idProdutor={0}' title='Editar'></a><a class='btn fas fa-trash-alt' style='background-color: #ddd; border-color: #999; color: #111' href='Produtor.aspx?delIdProdutor={0}' title='Apagar'></a></td></tr>";
                "<td>{0}</td>" +
                "<td>{1}</td>" +
                "<td>{2}</td>" +
                "<td>{3}</td>" +
                "<td>{4}</td>" +
                "<td>{5}</td>" +
                "<td>{6}</td>" +
                "<td>{7}</td>" +
                "<td>{8}</td>" +
                "<td style='text-align-last: center;'>" +
                    "<a class='btn btn-warning' href='cadastro_cliente.aspx?idClientePF={0}' title='Editar'>" +
                        "<div class='fas fa-edit'></div></a>" +
                    "<a class='btn btn-danger' href='cadastro_cliente.aspx?delIdClientePF={0}' title='Apagar'>" +
                        "<div class='fas fa-trash-alt'></div></a>" +
                    "<a class='nav-link' href='lista_pedidos.aspx?idClientePF={0}'>Listar Pedidos</a>" +
                "</td></tr>";

            entidades = commands["CONSULTAR"].execute(clientePF).Entidades;
            try
            {
                evade = entidades.Count;
            }
            catch
            {
                evade = 0;
            }

            StringBuilder conteudo = new StringBuilder();

            // lista para conter todos clientes retornados do BD
            List<Cliente> clientes = new List<Cliente>();
            foreach (Cliente cliente in entidades)
            {
                clientes.Add(cliente);
            }

            foreach (var cliente in clientes)
            {
                // para pesquisar os endereços que o cliente tem

                // passa ID de cliente e consulta na tabela n-n
                foreach (ClienteEndereco clienteEndereco in
                    commands["CONSULTAR"].execute(new ClienteEndereco { ID = cliente.ID }).Entidades)
                {
                    // Passa endereços para o cliente
                    cliente.Enderecos.Add(clienteEndereco.Endereco);
                }

                // passa ID de cliente e consulta na tabela n-n
                foreach (ClienteCartao clienteCartao in
                    commands["CONSULTAR"].execute(new ClienteCartao { ID = cliente.ID }).Entidades)
                {
                    // Passa ccs para o cliente
                    cliente.CartoesCredito.Add(clienteCartao.CC);
                }

                conteudo.AppendFormat(linha,
                cliente.ID,
                cliente.Nome,
                cliente.CPF,
                cliente.DataNascimento.ToString().Substring(0, 2) + "/" +
                cliente.DataNascimento.ToString().Substring(3, 2) + "/" +
                cliente.DataNascimento.ToString().Substring(6, 4) + "<br /> " ,
                "(" + cliente.Telefone.DDD + ")" + cliente.Telefone.NumeroTelefone,
                cliente.Email,
                EnderecosToString(cliente),
                CartoesToString(cliente),
                cliente.DataCadastro
                );
            }
            string tabelafinal = string.Format(GRID, tituloColunas, conteudo.ToString());
            divTable.InnerHtml = tabelafinal;
            clientePF.ID = 0;

            // Rodapé da tabela informativo de quando foi a última vez que foi atualizada a lista
            lblRodaPeTabela.InnerText = "Lista atualizada em " + DateTime.Now.ToString();
        }

        public string EnderecosToString(Cliente cliente)
        {
            string retorno = "";
            foreach (Endereco endereco in cliente.Enderecos)
            {
                retorno += endereco.Nome+ ", " +
                    endereco.TipoLogradouro.Nome + " " +
                    endereco.Logradouro + ", " +
                    endereco.Numero + ", " +
                    endereco.Bairro + ", " +
                    endereco.Cidade.Nome + " - " +
                    endereco.Cidade.Estado.Sigla + ", " +
                    "CEP: " + endereco.CEP + "<br />" +
                    "<a class='btn btn-warning' href='cadastro_endereco.aspx?idEndereco=" + endereco.ID +
                        "' title='Alterar Endereço'>" +
                        "<div class='fas fa-edit'></div></a>" +
                    "<a class='btn btn-danger' href='cadastro_endereco.aspx?delIdEndereco=" + endereco.ID +
                        "' title='Apagar Endereço'>" +
                        "<div class='fas fa-trash-alt'></div></a><br />";
            }

            retorno += "<a class='btn btn-success' href='cadastro_endereco.aspx?idClientePF=" + cliente.ID +
                            "' title='Novo Endereço'>" +
                            "<div class='fas fa-fw fa-plus'></div></a>";


            return retorno;
        }

        public string CartoesToString(Cliente cliente)
        {
            string retorno = "";
            foreach (CartaoCredito cc in cliente.CartoesCredito)
            {
                if (cc.ID != 0)
                    retorno += cc.Bandeira.Nome + ", " +
                        "Final: " + cc.NumeroCC.ToString().Substring(12, 4) + ", " +

                        "Vencimento: " +
                        //pegando somente mes e ano da data salva
                        cc.DataVencimento.ToString().Substring(3, 2) + "/" +
                        cc.DataVencimento.ToString().Substring(6, 4) + "<br /> " +

                    "<a class='btn btn-danger' href='cadastro_cartao.aspx?delIdCC=" + cc.ID +
                        "' title='Apagar Cartão de Crédito'>" +
                        "<div class='fas fa-trash-alt'></div></a><br />";
            }

            retorno += "<a class='btn btn-success' href='cadastro_cartao.aspx?idClientePF=" + cliente.ID +
                            "' title='Novo Cartão de Crédito'>" +
                            "<div class='fas fa-fw fa-plus'></div></a>";

            return retorno;
        }
    }
}