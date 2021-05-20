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
    public class TelefoneDAO : AbstractDAO
    {
        // construtor padrão
        public TelefoneDAO() : base("telefone", "id_telefone")
        {

        }

        // construtor para DAOs que também utilizarão o DAO de endereço
        public TelefoneDAO(MySqlConnection connection, bool ctrlTransaction) : base(connection, ctrlTransaction, "telefone", "id_telefone")
        {

        }

        public override void Salvar(EntidadeDominio entidade)
        {
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            Telefone telefone = (Telefone)entidade;
            
            pst.CommandText = "INSERT INTO telefone (tipo_telefone_fk, ddd_telefone, numero_telefone) VALUES (?1, ?2, ?3)";
            parameters = new MySqlParameter[]
                {
                    new MySqlParameter("?1", telefone.TipoTelefone.ID),
                    new MySqlParameter("?2", telefone.DDD),
                    new MySqlParameter("?3", telefone.NumeroTelefone)
                };

            pst.Parameters.Clear();
            pst.Parameters.AddRange(parameters);
            pst.Connection = connection;
            pst.CommandType = CommandType.Text;

            pst.ExecuteNonQuery();
            telefone.ID = entidade.ID = (int)pst.LastInsertedId;

            if (ctrlTransaction == true)
            {
                pst.CommandText = "COMMIT WORK";
                connection.Close();
            }
            return;
        }

        public override void Alterar(EntidadeDominio entidade)
        {
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            Telefone telefone = (Telefone)entidade;

            pst.CommandText = "UPDATE telefone SET tipo_telefone_fk = ?1, ddd_telefone = ?2, numero_telefone = ?3 WHERE id_telefone = ?4";
            parameters = new MySqlParameter[]
                {
                    new MySqlParameter("1", telefone.TipoTelefone.ID),
                    new MySqlParameter("2", telefone.DDD),
                    new MySqlParameter("3", telefone.NumeroTelefone),
                    new MySqlParameter("4", telefone.ID)
                };

            pst.Parameters.Clear();
            pst.Parameters.AddRange(parameters);
            pst.Connection = connection;
            pst.CommandType = CommandType.Text;
            pst.ExecuteNonQuery();
            if (ctrlTransaction == true)
            {
                pst.CommandText = "COMMIT WORK";
                connection.Close();
            }
            return;
        }

        public override List<EntidadeDominio> Consultar(EntidadeDominio entidade)
        {
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            Telefone telefone = (Telefone)entidade;
            StringBuilder sql = new StringBuilder();

            sql.Append("SELECT * FROM telefone JOIN tipo_telefone ON (telefone.tipo_telefone_fk = tipo_telefone.id_tipo_tel) ");

            // WHERE sem efeito, usado apenas para poder diminuir o número de ifs da construção da query
            sql.Append("WHERE 1 = 1 ");

            if (telefone.ID != 0)
            {
                sql.Append("AND id_telefone = ?1 ");
            }

            if (telefone.TipoTelefone.ID != 0)
            {
                sql.Append("AND tipo_telefone_fk = ?2 ");
            }

            if (!String.IsNullOrEmpty(telefone.DDD))
            {
                sql.Append("AND ddd_telefone = ?3 ");
            }

            if (!String.IsNullOrEmpty(telefone.NumeroTelefone))
            {
                sql.Append("AND numero_telefone = ?4 ");
            }

            sql.Append("ORDER BY id_telefone ");

            pst.CommandText = sql.ToString();
            parameters = new MySqlParameter[]
                {
                    new MySqlParameter("1", telefone.ID),
                    new MySqlParameter("2", telefone.TipoTelefone.ID),
                    new MySqlParameter("3", telefone.DDD),
                    new MySqlParameter("4", telefone.NumeroTelefone)
                };

            pst.Parameters.Clear();
            pst.Parameters.AddRange(parameters);
            pst.Connection = connection;
            pst.CommandType = CommandType.Text;

            reader = pst.ExecuteReader();

            // Lista de retorno da consulta do banco de dados, que conterá os endereços encontrados
            List<EntidadeDominio> telefones = new List<EntidadeDominio>();
            while (reader.Read())
            {
                telefone = new Telefone();
                telefone.ID = Convert.ToInt32(reader["id_telefone"]);
                telefone.TipoTelefone.ID = Convert.ToInt32(reader["id_tipo_tel"]);
                telefone.TipoTelefone.Nome = reader["nome_tipo_tel"].ToString();
                telefone.DDD = reader["ddd_telefone"].ToString();
                telefone.NumeroTelefone = reader["numero_telefone"].ToString();

                telefones.Add(telefone);
            }
            connection.Close();
            return telefones;
        }
    }
}
