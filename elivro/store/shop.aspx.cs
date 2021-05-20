﻿using Dominio;
using Dominio.Livro;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace elivro.store
    {
    public partial class shop : ViewGenerico
    {
        private Livro livro = new Livro();
        ImagemLivro imagemLivro = new ImagemLivro();

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                checkBoxIdCategoria.DataSource = CategoriaLivroDatatable(commands["CONSULTAR"].execute(new Categoria()).Entidades.Cast<Categoria>().ToList());
                checkBoxIdCategoria.DataValueField = "ID";
                checkBoxIdCategoria.DataTextField = "Name";
                checkBoxIdCategoria.DataBind();

                dropIdEditora.DataSource = EditoraDatatable(commands["CONSULTAR"].execute(new Editora()).Entidades.Cast<Editora>().ToList());
                dropIdEditora.DataValueField = "ID";
                dropIdEditora.DataTextField = "Name";
                dropIdEditora.DataBind();

                entidades = commands["CONSULTAR"].execute(new Editora()).Entidades;

                ConstruirLista();
            }
        }

        public static DataTable CategoriaLivroDatatable(List<Categoria> input)
        {
            DataTable data = new DataTable();
            data.Columns.Add(new DataColumn("ID", typeof(int)));
            data.Columns.Add(new DataColumn("Name", typeof(string)));

            int a = input.Count;
            for (int i = 0; i < a; i++)
            {
                Categoria categoria = input.ElementAt(i);
                DataRow dr = data.NewRow();
                dr[0] = categoria.ID;
                dr[1] = categoria.Nome;
                data.Rows.Add(dr);
            }
            return data;
        }

        public static DataTable EditoraDatatable(List<Editora> input)
        {
            DataTable data = new DataTable();
            data.Columns.Add(new DataColumn("ID", typeof(int)));
            data.Columns.Add(new DataColumn("Name", typeof(string)));

            DataRow dr = data.NewRow();
            dr[0] = 0;
            dr[1] = "Selecione uma Editora";
            data.Rows.Add(dr);

            int a = input.Count;
            for (int i = 0; i < a; i++)
            {
                Editora editora = input.ElementAt(i);
                dr = data.NewRow();
                dr[0] = editora.ID;
                dr[1] = editora.Nome;
                data.Rows.Add(dr);
            }
            return data;
        }

        private void ConstruirLista()
        {
            int evade = 0;

            string linha = "<div class='col-12 col-sm-6 col-md-12 col-xl-6'>" +
                            "<div class='single-product-wrapper'>" +
                                "<!-- Product Image -->" +
                                "<div class='product-img'>" +
                                    // falta salvar imagem no BD e tratar na classe de Livro
                                    "<img src = 'img/bg-img/{1}.jpg' alt = ''>" +
                                    //"<img src = '../img/SEM-IMAGEM.jpg' alt=''>" +
                                    "<!-- Hover Thumb -->" +
                                    // falta salvar imagem no BD e tratar na classe de Livro
                                    "<img class='hover-img' src = '{1}' alt = ''>" +
                                //"<img class='hover-img' src='../img/SEM-IMAGEM.jpg' alt=''>" +
                                "</div>" +

                                "<!-- Product Description -->" +
                                "<div class='product-description d-flex align-items-center justify-content-between'>" +
                                    "<!-- Product Meta Data -->" +
                                    "<div class='product-meta-data'>" +
                                        "<div class='line'></div>" +
                                        "<p class='product-price'>{2}</p>" +
                                        "<a href = 'ProductDetails.aspx?idLivro={0}' >" +
                                            "<h6>{3}</h6>" +
                                        "</a>" +
                                    "</div>" +
                                    "<!-- Ratings & Cart -->" +
                                    "<div class='ratings-cart text-right'>" +
                                        "<div class='cart'>" +
                                            "<a href = 'AddToCart.aspx?idLivro={0}' data-toggle='tooltip' data-placement='left' title='Adicionar ao Carrinho'>" +
                                                "<img src = '../img/core-img/cart.png' alt=''></a>" +
                                        "</div>" +
                                    "</div>" +
                                "</div>" +
                            "</div>" +
                        "</div>";

            if (Convert.ToInt32(dropIdEditora.SelectedValue) >= 0)
            {
                livro.Editora.ID = Convert.ToInt32(dropIdEditora.SelectedValue);
            }

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

            int contador = 0;

            for (int i = 0; i < entidades.Count; i++)
            {
                livro = (Livro)entidades.ElementAt(i);
                if (livro.ID != livroAux.ID)
                {
                    Estoque estoque = new Estoque();
                    estoque.Livro.ID = livro.ID;
                    entidadesAux = commands["CONSULTAR"].execute(estoque).Entidades;

                    imagemLivro.IdLivro = livro.ID;

                    foreach (EntidadeDominio imagem in commands["CONSULTAR"].execute(imagemLivro).Entidades)
                    {
                        imagemLivro = (ImagemLivro)imagem;
                        livro.ImagensLivro.Add(imagemLivro);
                    }

                    if (entidadesAux.Count > 0)
                    {
                        conteudo.AppendFormat(linha,
                            livro.ID,
                            livro.ID,
                            // @"data:charset=utf-8;base64, " + (Convert.ToBase64String(livro.ImagensLivro.ElementAt(0).Imagem)),
                            @"R$ " + ((Estoque)entidadesAux.ElementAt(0)).ValorVenda.ToString("N2"),
                            livro.Titulo
                        );
                    }
                    else
                    {
                        conteudo.AppendFormat(linha,
                            livro.ID,
                            livro.ID,
                            //@"data:charset=utf-8;base64, " + (Convert.ToBase64String(livro.ImagensLivro.ElementAt(0).Imagem)),
                            "Produto Indisponível",
                            livro.Titulo
                        );
                    }

                    livroAux.ID = livro.ID;

                    contador++;
                }

                if (contador > 8)
                {
                    break;
                }
            }

            divConteudo.InnerHtml = conteudo.ToString();
            livro.ID = 0;
        }

        protected void CheckBoxIdCategoriaCheckedChanged(object sender, EventArgs e)
        {
            ConstruirLista();
        }

        protected void DropIdEditoraSelectedIndexChanged(object sender, EventArgs e)
        {
            ConstruirLista();
        }
    }
}