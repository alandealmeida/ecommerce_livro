using Core.Aplicacao;
using Dominio;
using Dominio.Livro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace elivro.store
{
    public partial class ProductDetails : ViewGenerico
    {
        Livro livro = new Livro();
        Estoque estoque = new Estoque();
        ImagemLivro imagemLivro = new ImagemLivro();

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["idLivro"]))
                {
                    livro.ID = Convert.ToInt32(Request.QueryString["idLivro"]);
                    txtIdLivro.Text = livro.ID.ToString();
                    estoque.Livro.ID = livro.ID;
                    imagemLivro.IdLivro = livro.ID;

                    entidades = commands["CONSULTAR"].execute(livro).Entidades;
                    livro = (Livro)entidades.ElementAt(0);

                    entidades = commands["CONSULTAR"].execute(estoque).Entidades;

                    if(entidades.Count == 0)
                    {
                        txtSemEstoque.Visible = true;
                        txtSemEstoque.InnerText = "Produto Indisponível";
                        btnAddToCart.Visible = false;
                        txtPrecoLivro.Visible = false;
                    }
                    else
                    {
                        btnAddToCart.Visible = true;
                        btnAddToCart.InnerHtml = string.Format(
                        "<a class='btn amado-btn' href='AddToCart.aspx?idLivro={0}' title='Adicionar ao Carrinho'>" +
                        "Adicionar ao Carrinho</a>",
                        livro.ID);
                        txtPrecoLivro.Visible = true;

                        estoque = (Estoque)entidades.ElementAt(0);
                    }

                    foreach (EntidadeDominio imagem in commands["CONSULTAR"].execute(imagemLivro).Entidades)
                    {
                        imagemLivro = (ImagemLivro)imagem;
                        livro.ImagensLivro.Add(imagemLivro);
                    }

                    txtTituloLivro.InnerText = livro.Titulo;
                    txtPrecoLivro.InnerText = "R$ " + estoque.ValorVenda.ToString("N2");
                    txtDescricao.InnerText = livro.Sinopse;

                    //imgLivro.ImageUrl = @"data:charset=utf-8;base64, " + (Convert.ToBase64String(livro.ImagensLivro.ElementAt(0).Imagem));
                    imgLivro.ImageUrl = "img/bg-img/" + livro.ID + ".jpg";

                    
                }
            }
        }
    }
}