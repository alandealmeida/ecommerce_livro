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
    class LivroCategoriaDAO : AbstractDAO
    {
        public LivroCategoriaDAO() : base("livro_cat", "id_cat_livro")
        {

        }

        public override void Salvar(EntidadeDominio entidade)
        {
            throw new NotImplementedException();
        }

        public override void Alterar(EntidadeDominio entidade)
        {
            throw new NotImplementedException();
        }

        public override List<EntidadeDominio> Consultar(EntidadeDominio entidade)
        {
            if (connection.State == ConnectionState.Closed)
                connection.Open();

            LivroCategoria categoria = (LivroCategoria)entidade;
            StringBuilder sql = new StringBuilder();

            sql.Append("SELECT * FROM livro_cat JOIN cat_livro ON cat_livro.id_cat_livro = livro_cat.id_cat_livro  ");
            sql.Append(" WHERE 1 = 1 ");

            if (categoria.ID != 0)
            {
                sql.Append("AND livro_cat.id_livro = ?1 ");
            }

            sql.Append("ORDER BY cat_livro.nome_cat_livro");


            pst.CommandText = sql.ToString();
            parameters = new MySqlParameter[]
                {
                    new MySqlParameter("?1", categoria.ID)
                };
            
            pst.Parameters.Clear();
            pst.Parameters.AddRange(parameters);
            pst.Connection = connection;
            pst.CommandType = CommandType.Text;

            reader = pst.ExecuteReader();

            // Lista de retorno da consulta do banco de dados, que conterá os produtores encontrados
            List<EntidadeDominio> categorias = new List<EntidadeDominio>();
            while (reader.Read())
            {
                categoria = new LivroCategoria();
                categoria.ID = Convert.ToInt32(reader["id_livro"]);
                categoria.Categoria.ID = Convert.ToInt32(reader["id_cat_livro"]);
                categoria.Categoria.Nome = reader["nome_cat_livro"].ToString();
                categoria.Categoria.Descricao = reader["descricao_cat_livro"].ToString();

                categorias.Add(categoria);
            }
            connection.Close();
            return categorias;
        }
    }
}
