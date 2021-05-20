﻿using Core.Aplicacao;
using Core.DAO;
using Dominio;
using Dominio.Livro;
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
    public partial class lista_livros : ViewGenerico
    {
        private Livro livro = new Livro();

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dropAtivo.DataSource = AtivoDatatable();
                dropAtivo.DataValueField = "ID";
                dropAtivo.DataTextField = "Name";
                dropAtivo.DataBind();

                dropIdCategoriaMotivo.DataSource = CategoriaMotivoDatatable(commands["CONSULTAR"].execute(new CategoriaMotivo()).Entidades.Cast<CategoriaMotivo>().ToList());
                dropIdCategoriaMotivo.DataValueField = "ID";
                dropIdCategoriaMotivo.DataTextField = "Name";
                dropIdCategoriaMotivo.DataBind();

                ConstruirTabela();
            }
        }

        public static DataTable AtivoDatatable()
        {
            DataTable data = new DataTable();
            data.Columns.Add(new DataColumn("ID", typeof(char)));
            data.Columns.Add(new DataColumn("Name", typeof(string)));

            DataRow dr = data.NewRow();
            dr[0] = 'Z';
            dr[1] = "Selecione um Status";
            data.Rows.Add(dr);

            dr = data.NewRow();
            dr[0] = 'A';
            dr[1] = "A - Ativo";
            data.Rows.Add(dr);

            dr = data.NewRow();
            dr[0] = 'I';
            dr[1] = "I - Inativo";
            data.Rows.Add(dr);

            return data;
        }

        public static DataTable CategoriaMotivoDatatable(List<CategoriaMotivo> input)
        {
            DataTable data = new DataTable();
            data.Columns.Add(new DataColumn("ID", typeof(int)));
            data.Columns.Add(new DataColumn("Name", typeof(string)));

            DataRow dr = data.NewRow();
            dr[0] = 0;
            dr[1] = "Selecione uma Categoria de Motivo de Ativação/Inativação";
            data.Rows.Add(dr);

            int a = input.Count;
            for (int i = 0; i < a; i++)
            {
                CategoriaMotivo categoria = input.ElementAt(i);
                dr = data.NewRow();
                dr[0] = categoria.ID;
                dr[1] = categoria.Ativo + " - " + categoria.Nome;
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
                "<th>Título</th>" +
                "<th>Autor(es)</th>" +
                "<th>Categoria</th>" +
                "<th>Subcategorias</th>" +
                "<th>Ano</th>" +
                "<th>Editora/Cidade</th>" +
                "<th>Edição</th>" +
                "<th>ISBN</th>" +
                "<th>Número de Páginas</th>" +
                "<th>Sinopse</th>" +
                "<th>Dimensões (A x L x P - Peso)</th>" +
                "<th>Grupo de Precificação</th>" +
                "<th>Código de Barras</th>" +
                "<th>Status</th>" +
                "<th>Categoria Motivo</th>" +
                "<th>Motivo</th>" +
                "<th>Operações</th>" +
                "</tr></THEAD>";
            tituloColunas += "<TFOOT><tr>" +
                "<th>ID</th>" +
                "<th>Título</th>" +
                "<th>Autor(es)</th>" +
                "<th>Categoria</th>" +
                "<th>Subcategorias</th>" +
                "<th>Ano</th>" +
                "<th>Editora/Cidade</th>" +
                "<th>Edição</th>" +
                "<th>ISBN</th>" +
                "<th>Número de Páginas</th>" +
                "<th>Sinopse</th>" +
                "<th>Dimensões (A x L x P - Peso)</th>" +
                "<th>Grupo de Precificação</th>" +
                "<th>Código de Barras</th>" +
                "<th>Status</th>" +
                "<th>Categoria Motivo</th>" +
                "<th>Motivo</th>" +
                "<th>Operações</th>" +
                "</tr></TFOOT>";
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
                "<td>{10}</td>" +
                "<td>{11}</td>" +
                "<td>{12}</td>" +
                "<td>{13}</td>" +
                "<td>{14}</td>" +
                "<td>{15}</td>" +
                "<td>{16}</td>" +
                "<td style='text-align-last: center;'>" +
                    "<a class='btn btn-info' href='AtivacaoLivro.aspx?idLivro={0}' title='Ativação/Inativação'>" +
                        "<div class='fas fa-check'></div></a>" +
                "</td></tr>";

            if (Convert.ToInt32(dropIdCategoriaMotivo.SelectedValue) >= 0)
                livro.CategoriaMotivo.ID = Convert.ToInt32(dropIdCategoriaMotivo.SelectedValue);

            if (Convert.ToInt32(dropAtivo.SelectedIndex) > 0)
                livro.CategoriaMotivo.Ativo = dropAtivo.SelectedValue.First();

            entidades = commands["CONSULTAR"].execute(livro).Entidades;
            try
            {
                evade = entidades.Count;
            }
            catch
            {
                evade = 0;
            }

            StringBuilder conteudo = new StringBuilder();

            Livro livroAux = new Livro();
            livroAux.ID = 0;

            for (int i = 0; i < evade; i++)
            {
                livro = (Livro)entidades.ElementAt(i);
                if (livro.ID != livroAux.ID)
                {
                    conteudo.AppendFormat(linha,
                    livro.ID,
                    livro.Titulo,
                    AutoresToString(livro),
                    CategoriaToString(livro),
                    SubcategoriasToString(livro),
                    livro.Ano,
                    livro.Editora.Nome + "/" + livro.Editora.Cidade.Nome,
                    livro.Edicao,
                    livro.ISBN,
                    livro.NumeroPaginas,
                    livro.Sinopse,
                    livro.Dimensoes.Altura + "mm x" + livro.Dimensoes.Largura + "mm x" + livro.Dimensoes.Profundidade + "mm x" + livro.Dimensoes.Peso + "kg",
                    livro.GrupoPrecificacao.Nome,
                    livro.CodigoBarras,
                    livro.CategoriaMotivo.Ativo,
                    livro.CategoriaMotivo.Nome,
                    livro.Motivo
                    );

                    livroAux.ID = livro.ID;
                }
            }
            string tabelafinal = string.Format(GRID, tituloColunas, conteudo.ToString());
            divTable.InnerHtml = tabelafinal;
            livro.ID = 0;

            // Rodapé da tabela informativo de quando foi a última vez que foi atualizada a lista
            lblRodaPeTabela.InnerText = "Lista atualizada em " + DateTime.Now.ToString();
        }

        public string AutoresToString(Livro livro)
        {
            string retorno = "";
            
            foreach (Autor autor in livro.Autores)
            {
                retorno +=autor.Nome + " " + 
                    "<br />";
            }

            return retorno;
        }

        public string CategoriaToString(Livro livro)
        {
            string retorno = "";

            Categoria categoria = new Categoria();
            categoria.ID = livro.CategoriaPrincipal.ID;

            categoria = commands["CONSULTAR"].execute(categoria).Entidades.Cast<Categoria>().ElementAt(0);

            retorno += categoria.Nome + " " +
                "<br /> ";

            return retorno;
        }

        public string SubcategoriasToString(Livro livro)
        {
            string retorno = "";
            // ordena lista de categorias
            List<Categoria> categorias = livro.Categorias.OrderBy(c => c.ID).ToList();
            for (int i = 0; i < categorias.Count; i++)
            {
                if (i == 0 || categorias[i].ID != categorias[i - 1].ID)
                {
                    retorno +=  categorias[i].Nome + " " +
                       "<br /> ";
                }
            }

            return retorno;
        }

        protected void DropAtivo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dropAtivo.SelectedIndex != 0)
            {
                dropIdCategoriaMotivo.DataSource = CategoriaMotivoDatatable(commands["CONSULTAR"].execute(new CategoriaMotivo() { Ativo = dropAtivo.SelectedValue.First() }).Entidades.Cast<CategoriaMotivo>().ToList());
                dropIdCategoriaMotivo.DataValueField = "ID";
                dropIdCategoriaMotivo.DataTextField = "Name";
                dropIdCategoriaMotivo.DataBind();
                dropIdCategoriaMotivo.Enabled = true;
            }
            else
            {
                dropIdCategoriaMotivo.DataSource = CategoriaMotivoDatatable(commands["CONSULTAR"].execute(new CategoriaMotivo()).Entidades.Cast<CategoriaMotivo>().ToList());
                dropIdCategoriaMotivo.DataValueField = "ID";
                dropIdCategoriaMotivo.DataTextField = "Name";
                dropIdCategoriaMotivo.DataBind();
                dropIdCategoriaMotivo.Enabled = false;
            }

            ConstruirTabela();
        }

        protected void DropIdCategoriaMotivo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ConstruirTabela();
        }

    }
}