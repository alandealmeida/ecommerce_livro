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
    class ImagemLivroDAO : AbstractDAO
    {
        public ImagemLivroDAO() : base("livro_imagem", "id_livro")
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

            ImagemLivro imagem = (ImagemLivro)entidade;
            StringBuilder sql = new StringBuilder();

            sql.Append("SELECT * FROM livro_imagem ");

            if (imagem.IdLivro != 0)
            {
                sql.Append("WHERE id_livro = ?1 ");
            }

            sql.Append("ORDER BY id_livro");


            pst.CommandText = sql.ToString();
            parameters = new MySqlParameter[]
                {
                    new MySqlParameter("?1", imagem.IdLivro)
                };
            
            pst.Parameters.Clear();
            pst.Parameters.AddRange(parameters);
            pst.Connection = connection;
            pst.CommandType = CommandType.Text;

            reader = pst.ExecuteReader();

            // Lista de retorno da consulta do banco de dados, que conterá os produtos encontrados
            List<EntidadeDominio> imagens = new List<EntidadeDominio>();
            while (reader.Read())
            {
                imagem = new ImagemLivro();
                imagem.ID = Convert.ToInt32(reader["id_livro"]);
                imagem.IdLivro = Convert.ToInt32(reader["id_livro"]);
                if (!DBNull.Value.Equals(reader["imagem"]))
                    imagem.Imagem = (byte[])(reader["imagem"]);

                imagens.Add(imagem);
            }

            connection.Close();
            return imagens;
        }
    }
}
