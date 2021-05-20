using Core.Aplicacao;
using Dominio.Cliente;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace elivro.admin
{
    public partial class cadastro_cartao : ViewGenerico
    {
        ClienteCartao clienteCartao = new ClienteCartao();
        CartaoCredito cc = new CartaoCredito();
        private Resultado resultado = new Resultado();
        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dropIdBandeira.DataSource = BandeiraDatatable(commands["CONSULTAR"].execute(new Bandeira()).Entidades.Cast<Bandeira>().ToList());
                dropIdBandeira.DataValueField = "ID";
                dropIdBandeira.DataTextField = "Name";
                dropIdBandeira.DataBind();


                if (!string.IsNullOrEmpty(Request.QueryString["idClientePF"]))
                {
                    clienteCartao.ID = Convert.ToInt32(Request.QueryString["idClientePF"]);
                    txtIdClientePF.Text = clienteCartao.ID.ToString();
                    txtIdClientePF.Enabled = false;
                }                
                else if (!string.IsNullOrEmpty(Request.QueryString["delIdCC"]))
                {
                    cc.ID = Convert.ToInt32(Request.QueryString["delIdCC"]);
                    resultado = commands["EXCLUIR"].execute(cc);

                    // verifica se deu erro de validação
                    if (!string.IsNullOrEmpty(resultado.Msg))
                    {
                        lblResultado.Visible = true;
                        lblResultado.Text = resultado.Msg;
                    }
                    // caso tudo OK delera e redireciona o usuário para ListaCliente.aspx
                    else
                    {
                        Response.Redirect("./lista_clientes.aspx");
                    }
                }

            }
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

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("./lista_clientes.aspx");
        }        

        protected void btnCadastrar_Click(object sender, EventArgs e)
        {
            clienteCartao.ID = Convert.ToInt32(txtIdClientePF.Text);

            // -------------------------- CARTÃO COMEÇO --------------------------------------
            clienteCartao.CC.NomeImpresso = txtNomeImpresso.Text.Trim();
            clienteCartao.CC.NumeroCC = txtNumeroCC.Text.Trim();
            clienteCartao.CC.NumeroCC = clienteCartao.CC.NumeroCC.Replace("-", "");             // substitui o caracter "-" por ""
            clienteCartao.CC.NumeroCC = clienteCartao.CC.NumeroCC.Replace(".", "");             // substitui o caracter "." por ""
            clienteCartao.CC.NumeroCC = clienteCartao.CC.NumeroCC.Replace(" ", "");             // substitui o caracter " " por ""


            clienteCartao.CC.CodigoSeguranca = txtCodigoSeguranca.Text.Trim();
            clienteCartao.CC.Bandeira.ID = Convert.ToInt32(dropIdBandeira.SelectedValue);
            if (!txtDtVencimento.Text.Equals(""))
            {
                string[] data = txtDtVencimento.Text.Split('/');
                clienteCartao.CC.DataVencimento = new DateTime(Convert.ToInt32(data[1].ToString()), Convert.ToInt32(data[0].ToString()), Convert.ToInt32("01"));
            }
            // -------------------------- CARTÃO FIM --------------------------------------

            resultado = commands["SALVAR"].execute(clienteCartao);
            if (!string.IsNullOrEmpty(resultado.Msg))
            {
                lblResultado.Visible = true;
                lblResultado.Text = resultado.Msg;
            }
            else
            {
                Response.Redirect("./lista_clientes.aspx");
            }
        }
               
    }
}