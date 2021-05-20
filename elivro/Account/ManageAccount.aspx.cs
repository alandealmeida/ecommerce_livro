using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using elivro.Models;
using Dominio.Cliente;
using Core.Aplicacao;
using System.Data;
using System.Collections.Generic;


namespace elivro.Account
{
    public partial class ManageAccount : ViewGenerico
    {
        Cliente cliente = new Cliente();
        private Resultado resultado = new Resultado();

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dropIdGenero.DataSource = GeneroDatatable();
                dropIdGenero.DataValueField = "ID";
                dropIdGenero.DataTextField = "Name";
                dropIdGenero.DataBind();

                dropIdTipoTelefone.DataSource = TipoTelefoneDatatable(commands["CONSULTAR"].execute(new TipoTelefone()).Entidades.Cast<TipoTelefone>().ToList());
                dropIdTipoTelefone.DataValueField = "ID";
                dropIdTipoTelefone.DataTextField = "Name";
                dropIdTipoTelefone.DataBind();

                cliente.Email = Context.User.Identity.GetUserName();

                cliente = commands["CONSULTAR"].execute(cliente).Entidades.Cast<Cliente>().ElementAt(0);

                txtIdClientePF.Text = cliente.ID.ToString();

                //int i = cliente.Nome.LastIndexOf(" ");
                // ------------------------ Dados Pessoais - COMEÇO ------------------------------
                txtNome.Text = cliente.Nome.Substring(0, cliente.Nome.LastIndexOf(" "));
                txtSobrenome.Text = cliente.Nome.Substring(cliente.Nome.LastIndexOf(" ") + 1);
                txtCPF.Text = cliente.CPF;
                txtDtNascimento.Text = cliente.DataNascimento.ToString().Substring(6, 4) + "-" +
                                        cliente.DataNascimento.ToString().Substring(3, 2) + "-" +
                                        cliente.DataNascimento.ToString().Substring(0, 2);

                dropIdGenero.SelectedValue = cliente.Genero.ToString();
                // ------------------------ Dados Pessoais - FIM ------------------------------

                // ------------------------ Dados Contato - COMEÇO ------------------------------
                txtEmail.Text = cliente.Email;

                txtIdTelefone.Text = cliente.Telefone.ID.ToString();
                dropIdTipoTelefone.SelectedValue = cliente.Telefone.TipoTelefone.ID.ToString();
                txtDDD.Text = cliente.Telefone.DDD;
                txtTelefone.Text = cliente.Telefone.NumeroTelefone;
                // ------------------------ Dados Contato - FIM ------------------------------

            }
        }



        public static DataTable GeneroDatatable()
        {
            DataTable data = new DataTable();
            data.Columns.Add(new DataColumn("ID", typeof(char)));
            data.Columns.Add(new DataColumn("Name", typeof(string)));

            DataRow dr = data.NewRow();
            dr[0] = '0';
            dr[1] = "Selecione um Gênero";
            data.Rows.Add(dr);

            dr = data.NewRow();
            dr[0] = 'F';
            dr[1] = "Feminino".ToString();
            data.Rows.Add(dr);

            dr = data.NewRow();
            dr[0] = 'M';
            dr[1] = "Masculino".ToString();
            data.Rows.Add(dr);

            dr = data.NewRow();
            dr[0] = 'O';
            dr[1] = "Outro".ToString();
            data.Rows.Add(dr);

            return data;
        }

        public static DataTable TipoTelefoneDatatable(List<TipoTelefone> input)
        {
            DataTable data = new DataTable();
            data.Columns.Add(new DataColumn("ID", typeof(int)));
            data.Columns.Add(new DataColumn("Name", typeof(string)));

            DataRow dr = data.NewRow();
            dr[0] = 0;
            dr[1] = "Selecione um Tipo de Telefone";
            data.Rows.Add(dr);

            int a = input.Count;
            for (int i = 0; i < a; i++)
            {
                TipoTelefone tipoTelefone = input.ElementAt(i);
                dr = data.NewRow();
                dr[0] = tipoTelefone.ID;
                dr[1] = tipoTelefone.Nome;
                data.Rows.Add(dr);
            }
            return data;
        }       

        protected void AlterUser_Click(object sender, EventArgs e)
        {
            cliente.ID = Convert.ToInt32(txtIdClientePF.Text);

            // -------------------------- DADOS PESSOAIS COMEÇO --------------------------------------
            cliente.Nome = txtNome.Text + " " + txtSobrenome.Text;
            cliente.CPF = txtCPF.Text;

            if (!txtDtNascimento.Text.Equals(""))
            {
                string[] data = txtDtNascimento.Text.Split('-');
                cliente.DataNascimento = new DateTime(Convert.ToInt32(data[0].ToString()), Convert.ToInt32(data[1].ToString()), Convert.ToInt32(data[2].ToString()));
            }

            cliente.Genero = (dropIdGenero.SelectedValue.First());
            // -------------------------- DADOS PESSOAIS FIM --------------------------------------

            // -------------------------- DADOS CONTATO COMEÇO --------------------------------------
            cliente.Email = txtEmail.Text;

            cliente.Telefone.ID = Convert.ToInt32(txtIdTelefone.Text);
            cliente.Telefone.TipoTelefone.ID = Convert.ToInt32(dropIdTipoTelefone.SelectedValue);
            cliente.Telefone.DDD = txtDDD.Text;
            cliente.Telefone.NumeroTelefone = txtTelefone.Text;
            // -------------------------- DADOS CONTATO FIM --------------------------------------
            
            resultado = commands["ALTERAR"].execute(cliente);
            if (!string.IsNullOrEmpty(resultado.Msg))
            {
                lblResultado.Visible = true;
                lblResultado.Text = resultado.Msg;
            }
            else
            {
                //var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                //var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
                //var user = new ApplicationUser() { UserName = Email.Text, Email = Email.Text };
                //IdentityResult result = manager.Create(user, Password.Text);
                //if (result.Succeeded)
                //{
                //    // Para obter mais informações sobre como habilitar a confirmação da conta e redefinição de senha, visite https://go.microsoft.com/fwlink/?LinkID=320771
                //    //string code = manager.GenerateEmailConfirmationToken(user.Id);
                //    //string callbackUrl = IdentityHelper.GetUserConfirmationRedirectUrl(code, user.Id, Request);
                //    //manager.SendEmail(user.Id, "Confirme sua conta", "Confirme sua conta clicando <a href=\"" + callbackUrl + "\">aqui</a>.");

                //    signInManager.SignIn(user, isPersistent: false, rememberBrowser: false);

                //    using (elivro.Logic.ShoppingCartActions usersShoppingCart = new elivro.Logic.ShoppingCartActions())
                //    {
                //        String cartId = usersShoppingCart.GetCartId();
                //        usersShoppingCart.MigrateCart(cartId, user.Id);
                //    }

                //    IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                //}
                //else
                //{
                //    ErrorMessage.Text = result.Errors.FirstOrDefault();
                //}

                Response.Redirect("../store/index.aspx");
            }

        }
    }
}