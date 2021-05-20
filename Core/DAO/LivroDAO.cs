using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;
using System.Data;
using MySql.Data.MySqlClient;
using Dominio.Cliente;
using Dominio.Livro;

namespace Core.DAO
{
    public class LivroDAO : AbstractDAO
    {
        public LivroDAO() : base("livro", "id_livro")
        {

        }

        public override void Salvar(EntidadeDominio entidade)
        {
            throw new NotImplementedException();
        }

        public override void Alterar(EntidadeDominio entidade)
        {
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            Livro livro = (Livro)entidade;

            pst.CommandText = "UPDATE livro SET categoria_motivo_fk = ?1, motivo = ?2 WHERE id_livro = ?3 ";
            parameters = new MySqlParameter[]
                {
                    new MySqlParameter("?1", livro.CategoriaMotivo.ID),
                    new MySqlParameter("?2", livro.Motivo),
                    new MySqlParameter("?3", livro.ID)
                };

            pst.Parameters.Clear();
            pst.Parameters.AddRange(parameters);
            pst.Connection = connection;
            pst.CommandType = CommandType.Text;
            pst.ExecuteNonQuery();
            pst.CommandText = "COMMIT WORK";
            connection.Close();
            return;
        }

        public override List<EntidadeDominio> Consultar(EntidadeDominio entidade)
        {
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            Livro livro = (Livro)entidade;
            StringBuilder sql = new StringBuilder();

            sql.Append("SELECT * FROM livro JOIN livro_autor ON (livro.id_livro = livro_autor.id_livro) ");
            sql.Append("                            JOIN autor ON (livro_autor.id_autor = autor.id_autor) ");
            sql.Append("                            LEFT JOIN livro_cat ON (livro.id_livro = livro_cat.id_livro) ");
            sql.Append("                            LEFT JOIN cat_livro ON (livro_cat.id_cat_livro = cat_livro.id_cat_livro) ");
            sql.Append("                            JOIN cat_livro as categoria_principal ON (livro.categoria_principal_fk = categoria_principal.id_cat_livro) ");
            sql.Append("                            JOIN editora ON (livro.editora_fk = editora.id_editora) ");
            sql.Append("                            JOIN cidades ON (editora.cidade_fk = cidades.id_cidade) ");
            sql.Append("                            JOIN estados ON (cidades.estado_id = estados.id_estado) ");
            sql.Append("                            JOIN paises ON (estados.pais_id = paises.id_pais) ");
            sql.Append("                            JOIN dimensoes ON (livro.dimensoes_fk = dimensoes.id_dimensoes) ");
            sql.Append("                            JOIN grupo_preco ON (grupo_preco.id_grupo_preco = livro.grupo_preco_fk) ");
            sql.Append("                            JOIN cat_motivo ON (cat_motivo.id_cat_motivo = livro.categoria_motivo_fk) ");

            // WHERE sem efeito, usado apenas para poder diminuir o número de ifs da construção da query
            sql.Append("WHERE 1 = 1 ");

            if (livro.ID != 0)
            {
                sql.Append("AND livro.id_livro = ?1 ");
            }

            if (livro.Autores.Count > 0)
            {
                if (livro.Autores.ElementAt(0).ID != 0)
                {
                    sql.Append("AND autor.id_autor = ?2 ");
                }

                if (!String.IsNullOrEmpty(livro.Autores.ElementAt(0).Nome))
                {
                    sql.Append("AND nome_autor = ?3 ");
                }
            }

            // categoria principal
            if(livro.CategoriaPrincipal != null)
            {
                if(livro.CategoriaPrincipal.ID != 0)
                {
                    sql.Append("AND livro.categoria_principal_fk = ?4 ");
                }
            }

            if (livro.Categorias.Count > 0)
            {
                if (livro.Categorias.ElementAt(0).ID != 0)
                {
                    sql.Append("AND cat_livro.id_cat_livro = ?4 ");
                }

                if (!String.IsNullOrEmpty(livro.Categorias.ElementAt(0).Nome))
                {
                    sql.Append("AND nome_cat_livro = ?5 ");
                }

                if (!String.IsNullOrEmpty(livro.Categorias.ElementAt(0).Descricao))
                {
                    sql.Append("AND descricao_cat_livro = ?6 ");
                }
            }

            if (!String.IsNullOrEmpty(livro.Ano))
            {
                sql.Append("AND ano_livro = ?7 ");
            }

            if (!String.IsNullOrEmpty(livro.Titulo))
            {
                sql.Append("AND titulo_livro = ?8 ");
            }

            if (livro.Editora != null)
            {
                if (livro.Editora.ID != 0)
                {
                    sql.Append("AND id_editora = ?9 ");
                }

                if (!String.IsNullOrEmpty(livro.Editora.Nome))
                {
                    sql.Append("AND nome_editora = ?10 ");
                }
            }

            if (!String.IsNullOrEmpty(livro.Edicao))
            {
                sql.Append("AND edicao_livro = ?11 ");
            }

            if (!String.IsNullOrEmpty(livro.ISBN))
            {
                sql.Append("AND isbn = ?12 ");
            }

            if (!String.IsNullOrEmpty(livro.NumeroPaginas))
            {
                sql.Append("AND numero_paginas = ?13 ");
            }

            if (!String.IsNullOrEmpty(livro.Sinopse))
            {
                sql.Append("AND sinopse = ?14 ");
            }

            if (livro.Dimensoes != null)
            {
                if (livro.Dimensoes.ID != 0)
                {
                    sql.Append("AND id_dimensoes = ?15 ");
                }

                if (livro.Dimensoes.Altura != 0)
                {
                    sql.Append("AND altura = ?16 ");
                }

                if (livro.Dimensoes.Largura != 0)
                {
                    sql.Append("AND largura = ?17 ");
                }

                if (livro.Dimensoes.Profundidade != 0)
                {
                    sql.Append("AND profundidade = ?18 ");
                }

                if (livro.Dimensoes.Peso != 0.0)
                {
                    sql.Append("AND peso = ?19 ");
                }
            }

            if(livro.GrupoPrecificacao != null)
            {
                if(livro.GrupoPrecificacao.ID != 0)
                {
                    sql.Append("AND id_grupo_preco = ?20 ");
                }

                if (!String.IsNullOrEmpty(livro.GrupoPrecificacao.Nome))
                {
                    sql.Append("AND nome_grupo_preco = ?21 ");
                }

                if (livro.GrupoPrecificacao.MargemLucro != 0.0)
                {
                    sql.Append("AND margem_lucro = ?22 ");
                }
            }

            if (!String.IsNullOrEmpty(livro.CodigoBarras))
            {
                sql.Append("AND codigo_barras_livro = ?23 ");
            }

            if (livro.CategoriaMotivo != null)
            {
                if (livro.CategoriaMotivo.ID != 0)
                {
                    sql.Append("AND id_cat_motivo = ?24 ");
                }

                if (livro.CategoriaMotivo.Ativo != 'Z')
                {
                    sql.Append("AND ativo = ?25 ");
                }
            }

            if (!String.IsNullOrEmpty(livro.Motivo))
            {
                sql.Append("AND motivo = ?26 ");
            }

            if (livro.DataCadastro != null)
            {
                sql.Append("AND dt_cadastro_livro = ?27 ");
            }

            // categoria principal
            if (livro.CategoriaPrincipal != null)
            {
                if (livro.CategoriaPrincipal.ID != 0)
                {
                    sql.Append("AND livro.categoria_principal_fk = ?28 ");
                }
            }

            sql.Append("ORDER BY livro.id_livro, autor.id_autor, cat_livro.id_cat_livro ");

            pst.CommandText = sql.ToString();
            parameters = new MySqlParameter[]
                {
                    new MySqlParameter("?1", livro.ID),
                    new MySqlParameter("?7", livro.Ano),
                    new MySqlParameter("?8", livro.Titulo),
                    new MySqlParameter("?9", livro.Editora.ID),
                    new MySqlParameter("?10", livro.Editora.Nome),
                    new MySqlParameter("?11", livro.Edicao),
                    new MySqlParameter("?12", livro.ISBN),
                    new MySqlParameter("?13", livro.NumeroPaginas),
                    new MySqlParameter("?14", livro.Sinopse),
                    new MySqlParameter("?15", livro.Dimensoes.ID),
                    new MySqlParameter("?16", livro.Dimensoes.Altura),
                    new MySqlParameter("?17", livro.Dimensoes.Largura),
                    new MySqlParameter("?18", livro.Dimensoes.Profundidade),
                    new MySqlParameter("?19", livro.Dimensoes.Peso),
                    new MySqlParameter("?20", livro.GrupoPrecificacao.ID),
                    new MySqlParameter("?21", livro.GrupoPrecificacao.Nome),
                    new MySqlParameter("?22", livro.GrupoPrecificacao.MargemLucro),
                    new MySqlParameter("?23", livro.CodigoBarras),
                    new MySqlParameter("?24", livro.CategoriaMotivo.ID),
                    new MySqlParameter("?25", livro.CategoriaMotivo.Ativo),
                    new MySqlParameter("?26", livro.Motivo),
                    new MySqlParameter("?28", livro.CategoriaPrincipal.ID)
                };

            if (livro.Autores.Count > 0)
            {
                MySqlParameter[] parametersAux = new MySqlParameter[]
                   {
                    new MySqlParameter("?2", livro.Autores.ElementAt(0).ID),
                    new MySqlParameter("?3", livro.Autores.ElementAt(0).Nome)
                   };

                parameters.Concat(parametersAux);
            }

            if (livro.Categorias.Count > 0)
            {
                MySqlParameter[] parametersAux = new MySqlParameter[]
                   {
                    new MySqlParameter("?4", livro.Categorias.ElementAt(0).ID),
                    new MySqlParameter("?5", livro.Categorias.ElementAt(0).Nome),
                    new MySqlParameter("?6", livro.Categorias.ElementAt(0).Descricao)
                   };

                parameters.Concat(parametersAux);
            }

            if (livro.DataCadastro != null)
            {
                MySqlParameter[] parametersAux = new MySqlParameter[]
                   {
                    new MySqlParameter("?27", livro.DataCadastro)
                   };

                parameters.Concat(parametersAux);
            }
            pst.Parameters.Clear();
            pst.Parameters.AddRange(parameters);
            pst.Connection = connection;
            pst.CommandType = CommandType.Text;

            reader = pst.ExecuteReader();

            // Lista de retorno da consulta do banco de dados, que conterá os livros encontrados
            List<EntidadeDominio> livros = new List<EntidadeDominio>();

            Livro livroAux = new Livro();
            livroAux.ID = 0;

            Autor autor = new Autor();
            Categoria categoria = new Categoria();

            Autor autorAux = new Autor();
            Categoria categoriaAux = new Categoria();

            livro = new Livro();

            while (reader.Read())
            {
                // verifica se livro que está trazendo do BD é igual ao anterior
                if (Convert.ToInt32(reader["id_livro"]) != livroAux.ID)
                {
                    livro = new Livro();
                    livro.ID = Convert.ToInt32(reader["id_livro"]);

                    // passando id do livro que está vindo para o auxiliar
                    livroAux.ID = livro.ID;

                    //// -------------------- AUTOR - COMEÇO ----------------------------------
                    autor = new Autor();

                    autor.ID = Convert.ToInt32(reader["id_autor"]);
                    autor.Nome = reader["nome_autor"].ToString();

                    livro.Autores.Add(autor);
                    autorAux.ID = autor.ID;
                    //// -------------------- AUTOR - FIM ----------------------------------

                    // -------------------- CATEGORIA LIVRO - COMEÇO ----------------------------------
                    categoria = new Categoria();

                    categoria.ID = Convert.ToInt32(reader["id_cat_livro"]);
                    categoria.Nome = reader["nome_cat_livro"].ToString();
                    categoria.Descricao = reader["descricao_cat_livro"].ToString();

                    livro.Categorias.Add(categoria);
                    categoriaAux.ID = categoria.ID;
                    // -------------------- CATEGORIA LIVRO - FIM ----------------------------------

                    livro.Ano = reader["ano_livro"].ToString();
                    livro.Titulo = reader["titulo_livro"].ToString();

                    // -------------------- EDITORA - COMEÇO ----------------------------------
                    livro.Editora.ID = Convert.ToInt32(reader["id_editora"]);
                    livro.Editora.Nome = reader["nome_editora"].ToString();
                    livro.Editora.Cidade.ID = Convert.ToInt32(reader["id_cidade"]);
                    livro.Editora.Cidade.Nome = reader["nome_cidade"].ToString();
                    // -------------------- EDITORA - FIM ----------------------------------

                    livro.Edicao = reader["edicao_livro"].ToString();
                    livro.ISBN = reader["isbn"].ToString();
                    livro.NumeroPaginas = reader["numero_paginas"].ToString();
                    livro.Sinopse = reader["sinopse"].ToString();

                    // -------------------- DIMENSÕES - COMEÇO ----------------------------------
                    livro.Dimensoes.ID = Convert.ToInt32(reader["id_dimensoes"]);
                    livro.Dimensoes.Altura = Convert.ToInt32(reader["altura"]);
                    livro.Dimensoes.Largura = Convert.ToInt32(reader["largura"]);
                    livro.Dimensoes.Profundidade = Convert.ToInt32(reader["profundidade"]);
                    livro.Dimensoes.Peso = Convert.ToSingle(reader["peso"]);
                    // -------------------- DIMENSÕES - FIM ----------------------------------

                    // -------------------- GRUPO PRECIFICAÇÃO - COMEÇO ----------------------------------
                    livro.GrupoPrecificacao.ID = Convert.ToInt32(reader["id_grupo_preco"]);
                    livro.GrupoPrecificacao.Nome = reader["nome_grupo_preco"].ToString();
                    livro.GrupoPrecificacao.MargemLucro = Convert.ToSingle(reader["margem_lucro"]);
                    // -------------------- GRUPO PRECIFICAÇÃO - FIM ----------------------------------

                    livro.CodigoBarras = reader["codigo_barras_livro"].ToString();

                    // -------------------- CATEGORIA MOTIVO - COMEÇO ----------------------------------
                    livro.CategoriaMotivo.ID = Convert.ToInt32(reader["id_cat_motivo"]);
                    livro.CategoriaMotivo.Ativo = reader["ativo"].ToString().First();
                    livro.CategoriaMotivo.Nome = reader["nome_cat_motivo"].ToString();
                    livro.CategoriaMotivo.Descricao = reader["descricao_cat_motivo"].ToString();
                    // -------------------- CATEGORIA MOTIVO - FIM ----------------------------------

                    livro.Motivo = reader["motivo"].ToString();
                    livro.DataCadastro = Convert.ToDateTime(reader["dt_cadastro_livro"].ToString());


                    // -------------------- CATEGORIA PRINCIPAL - COMEÇO ----------------------------------
                    livro.CategoriaPrincipal.ID = Convert.ToInt32(reader["categoria_principal_fk"]);
                    // -------------------- CATEGORIA PRINCIPAL - COMEÇO ----------------------------------

                }
                else
                {
                    //// -------------------- AUTOR - COMEÇO ----------------------------------
                    autor = new Autor();

                    autor.ID = Convert.ToInt32(reader["id_autor"]);
                    if (autorAux.ID != autor.ID)
                    {
                        autor.Nome = reader["nome_autor"].ToString();

                        livro.Autores.Add(autor);

                        autorAux.ID = autor.ID;
                    }
                    //// -------------------- AUTOR - FIM ----------------------------------

                    // -------------------- CATEGORIA LIVRO - COMEÇO ----------------------------------
                    categoria = new Categoria();

                    categoria.ID = Convert.ToInt32(reader["id_cat_livro"]);
                    if (categoriaAux.ID != categoria.ID)
                    {
                        categoria.Nome = reader["nome_cat_livro"].ToString();
                        categoria.Descricao = reader["descricao_cat_livro"].ToString();

                        livro.Categorias.Add(categoria);

                        categoriaAux.ID = categoria.ID;
                    }
                    // -------------------- CATEGORIA LIVRO - FIM ----------------------------------
                }

                livros.Add(livro);
            }
            connection.Close();
            return livros;
        }

    }
}
