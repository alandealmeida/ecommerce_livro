﻿using Core.Aplicacao;
using Dominio;
using elivro.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace elivro
{
    public class ViewGenerico : System.Web.UI.Page
    {
        protected Resultado res { get; set; } = new Resultado();
        protected Dictionary<string, ICommand> commands { get; set; } = new Dictionary<string, ICommand>();
        protected List<EntidadeDominio> entidades = new List<EntidadeDominio>();
        protected List<EntidadeDominio> entidadesAux = new List<EntidadeDominio>();
        public ViewGenerico()
        {
            commands.Add("SALVAR", new SalvarCommand());
            commands.Add("ALTERAR", new AlterarCommand());
            commands.Add("EXCLUIR", new ExcluirCommand());
            commands.Add("CONSULTAR", new ConsultarCommand());
        }

        protected virtual void Page_Load(object sender, EventArgs e)
        {
            //if (Request.QueryString["vazar"] != null)
            //    if (Request.QueryString["vazar"] == "0")
            //        Session["login"] = null;

            //if (Session["login"] == null)
            //{
            //    if ("Log in" != Page.Title && "Register" != Page.Title)
            //        Response.Redirect("~/Account/Login");
            //}
        }
    }
}