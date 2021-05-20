using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;
using System.Data;
using MySql.Data.MySqlClient;
using Dominio.Cliente;

namespace Core.DAO
{
    class CidadeDAO : AbstractDAO
    {
        public CidadeDAO() : base("cidades", "id_cidade")
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

            Cidade cidade = (Cidade)entidade;
            StringBuilder sql = new StringBuilder();

            sql.Append("SELECT * FROM cidades JOIN estados ON (cidades.estado_id = estados.id_estado) WHERE estado_id = ?id_estado order by nome_cidade ");

            pst.CommandText = sql.ToString();
            parameters = new MySqlParameter[]
                {
                    new MySqlParameter("?id_estado", cidade.Estado.ID)
                };

            pst.Parameters.Clear();
            pst.Parameters.AddRange(parameters);
            pst.Connection = connection;
            pst.CommandType = CommandType.Text;

            reader = pst.ExecuteReader();

            // Lista de retorno da consulta do banco de dados, que conterá os produtores encontrados
            List<EntidadeDominio> cidades = new List<EntidadeDominio>();
            while (reader.Read())
            {
                cidade = new Cidade();
                cidade.ID = Convert.ToInt32(reader["id_cidade"]);
                cidade.Nome = reader["nome_cidade"].ToString();
                cidade.Estado.ID = Convert.ToInt32(reader["estado_id"].ToString());

                cidades.Add(cidade);
            }
            connection.Close();
            return cidades;
        }
    }
}
