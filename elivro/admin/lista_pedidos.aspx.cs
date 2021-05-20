﻿using Dominio.Cliente;
using Dominio.Venda;
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
    public partial class lista_pedidos : ViewGenerico
    {
        Pedido pedido = new Pedido();
        Cliente cliente = new Cliente();
        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dropIdStatus.DataSource = CategoriaMotivoDatatable(commands["CONSULTAR"].execute(new StatusPedido()).Entidades.Cast<StatusPedido>().ToList());
                dropIdStatus.DataValueField = "ID";
                dropIdStatus.DataTextField = "Name";
                dropIdStatus.DataBind();

                if (!string.IsNullOrEmpty(Request.QueryString["idClientePF"]))
                {
                    cliente.ID = Convert.ToInt32(Request.QueryString["idClientePF"]);
                    entidades = commands["CONSULTAR"].execute(cliente).Entidades;
                    cliente = (Cliente)entidades.ElementAt(0);
                    pedido.Usuario = cliente.Email;

                }

                ConstruirTabela();

                if (!string.IsNullOrEmpty(Request.QueryString["resultadoAtualiza"]))
                {
                    lblResultadoAtualiza.Visible = true;
                    lblResultadoAtualiza.Text = Request.QueryString["resultadoAtualiza"];
                }
                else
                {
                    lblResultadoAtualiza.Visible = false;
                    lblResultadoAtualiza.Text = "";
                }
            }
        }

        public static DataTable CategoriaMotivoDatatable(List<StatusPedido> input)
        {
            DataTable data = new DataTable();
            data.Columns.Add(new DataColumn("ID", typeof(int)));
            data.Columns.Add(new DataColumn("Name", typeof(string)));

            DataRow dr = data.NewRow();
            dr[0] = 0;
            dr[1] = "Selecione um Status de Pedido";
            data.Rows.Add(dr);

            int a = input.Count;
            for (int i = 0; i < a; i++)
            {
                StatusPedido status = input.ElementAt(i);
                dr = data.NewRow();
                dr[0] = status.ID;
                dr[1] = status.Nome;
                data.Rows.Add(dr);
            }
            return data;
        }

        private void ConstruirTabela()
        {
            int evade = 0;

            string GRID = "<TABLE class='table table-bordered' id='GridViewGeral' width='100%' cellspacing='0'>{0}<TBODY>{1}</TBODY></TABLE>";
            string tituloColunas = "<THEAD><tr>" +
                "<th>ID</th>" +
                "<th>Usuário</th>" +
                "<th>Entrega em:</th>" +
                "<th>Item(ns)</th>" +
                "<th>Cartão(ões)</th>" +
                "<th>Cupom Promo</th>" +
                "<th>Cupom(ns) Troca</th>" +
                "<th>Status</th>" +
                "<th>Frete</th>" +
                "<th>Total</th>" +
                "<th>Data Ent/Atual</th>" +
                "<th>Operações</th>" +
                "</tr></THEAD>";
            tituloColunas += "<TFOOT><tr>" +
                "<th>ID</th>" +
                "<th>Usuário</th>" +
                "<th>Entrega em:</th>" +
                "<th>Item(ns)</th>" +
                "<th>Cartão(ões)</th>" +
                "<th>Cupom Promo</th>" +
                "<th>Cupom(ns) Troca</th>" +
                "<th>Status</th>" +
                "<th>Frete</th>" +
                "<th>Total</th>" +
                "<th>Data Ent/Atual</th>" +
                "<th>Operações</th>" +
                "</tr></TFOOT>";

            if (Convert.ToInt32(dropIdStatus.SelectedValue) >= 0)
                pedido.Status.ID = Convert.ToInt32(dropIdStatus.SelectedValue);

            pedido.Usuario = cliente.Email;
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
                // para pesquisar os itens que o pedido tem

                pedido.EnderecoEntrega = commands["CONSULTAR"].execute(new Endereco() { ID = pedido.EnderecoEntrega.ID }).Entidades.Cast<Endereco>().ElementAt(0);

                // passa ID de pedido e consulta
                foreach (PedidoDetalhe detalhe in
                    commands["CONSULTAR"].execute(new PedidoDetalhe { IdPedido = pedido.ID }).Entidades)
                {
                    // Passa itens para o pedido
                    pedido.Detalhes.Add(detalhe);
                }

                foreach (CartaoCreditoPedido cc in
                    commands["CONSULTAR"].execute(new CartaoCreditoPedido { IdPedido = pedido.ID }).Entidades)
                {
                    // Passa ccs para o pedido
                    pedido.CCs.Add(cc);
                }

                // passa ID pedido e consulta 
                foreach (PedidoCupom pedidoXCupom in
                    commands["CONSULTAR"].execute(new PedidoCupom { ID = pedido.ID }).Entidades)
                {
                    // Passa cupom promo para o pedido
                    pedido.CupomPromocional = pedidoXCupom.Cupom;
                }

                // passa ID pedido e consulta 
                foreach (Cupom cupom in
                    commands["CONSULTAR"].execute(new Cupom { IdPedido = pedido.ID }).Entidades)
                {
                    // Passa cupom troca para o pedido
                    pedido.CuponsTroca.Add(cupom);
                }

                string linha = "<tr>" +
               "<td>{0}</td>" +
               "<td>{1}</td>" +
               "<td>{2}</td>" +
               "<td>{3}</td>" +
               "<td>{4}</td>" +
               "<td>{5}</td>" +
               "<td>{6}</td>" +
               "<td>{7}</td>" +
               "<td>{8}</td>" +
               "<td>{9}</td>" +
               "<td>{10}</td>";

                if (pedido.Status.ID == 3 || pedido.Status.ID == 5 || pedido.Status.ID == 8)
                {
                    linha += "<td></td></tr>";
                }
                else
                {
                    linha += "<td style='text-align-last: center;'>" +
                                "<a class='btn btn-success' href='AtualizaPedido.aspx?idPedido={0}' title='Avançar'>" +
                                    "<div class='fas fa-chevron-right'></div></a>" +
                            "</td></tr>";
                }

                conteudo.AppendFormat(linha,
                pedido.ID,
                pedido.Usuario,
                EnderecoToString(pedido.EnderecoEntrega),
                DetalhesToString(pedido.Detalhes),
                CartoesToString(pedido.CCs),
                CupomPromoToString(pedido.CupomPromocional),
                CupomTrocaToString(pedido.CuponsTroca),
                "ID Status: " + pedido.Status.ID + " - " + pedido.Status.Nome,
                "R$" + pedido.Frete.ToString("N2"),
                "R$" + pedido.Total.ToString("N2"),
                pedido.DataCadastro.ToString()
                );
            }
            string tabelafinal = string.Format(GRID, tituloColunas, conteudo.ToString());
            divTable.InnerHtml = tabelafinal;
            pedido.ID = 0;

            // Rodapé da tabela informativo de quando foi a última vez que foi atualizada a lista
            lblRodaPeTabela.InnerText = "Lista atualizada em " + DateTime.Now.ToString();
        }

        public string EnderecoToString(Endereco endereco)
        {
            string retorno = "";
            retorno += "ID: " + endereco.ID + ", " +
                endereco.TipoLogradouro.Nome + " " +
                endereco.Logradouro + ", " +
                endereco.Numero + ", " +
                endereco.Bairro + ", " +
                endereco.Cidade.Nome + " - " +
                endereco.Cidade.Estado.Sigla + ", " +
                "CEP: " + endereco.CEP + "<br />";

            return retorno;
        }

        public string DetalhesToString(List<PedidoDetalhe> detalhes)
        {
            string retorno = "";
            foreach (PedidoDetalhe detalhe in detalhes)
            {
                if (detalhe.ID != 0)
                    retorno += "ID Livro: " + detalhe.Livro.ID + " - " +
                        detalhe.Livro.Titulo + ", " +
                        "Qtde: " + detalhe.Quantidade + ", " +
                        "Vlr Unit: R$" + detalhe.ValorUnit + "<br /> ";
            }

            return retorno;
        }


        public string CartoesToString(List<CartaoCreditoPedido> ccs)
        {
            string retorno = "";
            foreach (CartaoCreditoPedido cc in ccs)
            {
                if (cc.ID != 0)
                    retorno +=  retorno += cc.CC.Bandeira.Nome + ", " +
                        "Final: " + cc.CC.NumeroCC.ToString().Substring(12, 4) + ", " +

                        "Vencimento: " +
                        //pegando somente mes e ano da data salva
                        cc.CC.DataVencimento.ToString().Substring(3, 2) + "/" +
                        cc.CC.DataVencimento.ToString().Substring(6, 4) + "<br /> " +
                        "Vlr Pgto: " + cc.ValorCCPagto + "<br /> ";
            }

            return retorno;
        }

        public string CupomPromoToString(Cupom cupom)
        {
            string retorno = "";

            if(cupom.ValorCupom != 0)
            {
                retorno += cupom.CodigoCupom + " - " +
                           cupom.ValorCupom * 100 + "%, " +
                           cupom.Tipo.Nome;
            }

            return retorno;
        }

        public string CupomTrocaToString(List<Cupom> cupons)
        {
            string retorno = "";
            foreach (Cupom cupom in cupons)
                retorno += cupom.CodigoCupom +
                " Valor: R$" + cupom.ValorCupom.ToString("N2") + ", " +
                cupom.Tipo.Nome + "</br> ";

            return retorno;
        }

        protected void dropIdStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            ConstruirTabela();
        }
    }
}