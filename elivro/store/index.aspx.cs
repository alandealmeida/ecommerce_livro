using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using Core.Aplicacao;
using Core.DAO;
using Dominio;
using Dominio.Livro;
using System.Text;

namespace elivro.store
{
    public partial class index : ViewGenerico
    {
        private Livro livro = new Livro();
        ImagemLivro imagemLivro = new ImagemLivro();

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ConstruirLista();
            }
        }

        private void ConstruirLista()
        {
            int evade = 0;

            string linha = "<!-- Single Catagory -->" +
                          "<div class='single-products-catagory clearfix'>" +
                            "<a href = 'ProductDetails.aspx?idLivro={0}'>" +
                                // falta salvar imagem no BD e tratar na classe de Livro
                                //"<img src = '{1}' alt = ''>" +
                                "<img src = 'img/bg-img/{1}.jpg' alt = ''>" +
                                "<!-- Hover Content -->" +
                                "<div class='hover-content'>" +
                                    "<div class='line'></div>" +
                                    "<p>{2}</p>" +
                                    "<h4>{3}</h4>" +
                                "</div>" +
                            "</a>" +
                          "</div>";

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

            // Ordenando lista em ordem descendente para pegar os últimos 9
            var list = entidades.OrderByDescending(x => x.DataCadastro).ToList();

            for (int i = 0; i < list.Count; i++)
            {
                livro = (Livro)list.ElementAt(i);
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
                        conteudo.AppendFormat(
                            linha,
                            livro.ID,
                            livro.ID,
                            @"R$ " + ((Estoque)entidadesAux.ElementAt(0)).ValorVenda.ToString("N2"),
                            livro.Titulo
                        );
                    }
                    else
                    {
                        conteudo.AppendFormat(
                            linha,
                            livro.ID,
                            livro.ID,
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
    }
}