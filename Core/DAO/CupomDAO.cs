using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;
using MySql.Data.MySqlClient;
using System.Data;
using Dominio.Cliente;

namespace Core.DAO
{
    public class CupomDAO : AbstractDAO
    {
        // construtor padrão
        public CupomDAO() : base("cupom", "id_cupom")
        {

        }

        // construtor para DAOs que também utilizarão o DAO de Cupom
        public CupomDAO(MySqlConnection connection, bool ctrlTransaction) : base(connection, ctrlTransaction, "cupom", "id_cupom")
        {

        }

        public override void Salvar(EntidadeDominio entidade)
        {
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            Cupom cupom = (Cupom)entidade;

            pst.CommandText = "INSERT INTO cupom(";
            
            if(cupom.IdPedido != 0)
                pst.CommandText += "cupom_pedido_fk, ";
            if (cupom.IdCliente != 0)
                pst.CommandText += "cupom_cliente_fk, ";

            pst.CommandText += "codigo_cupom, tipo_cupom_fk, status_cupom, valor_cupom) VALUES (";

            if (cupom.IdPedido != 0)
                pst.CommandText += "?1, ";
            if (cupom.IdCliente != 0)
                pst.CommandText += "?2, ";

            pst.CommandText += "?3, ?4, ?5, ?6)";

            parameters = new MySqlParameter[]
                {
                    new MySqlParameter("?1", cupom.IdPedido),
                    new MySqlParameter("?2", cupom.IdCliente),
                    new MySqlParameter("?3", cupom.CodigoCupom),
                    new MySqlParameter("?4", cupom.Tipo.ID),
                    new MySqlParameter("?5", cupom.Status),
                    new MySqlParameter("?6", cupom.ValorCupom)
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

        public override void Alterar(EntidadeDominio entidade)
        {
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            Cupom cupom = (Cupom)entidade;

            pst.CommandText = "UPDATE cupom SET ";

            if (cupom.IdPedido != 0)
                pst.CommandText += "cupom_pedido_fk = ?1, ";
            if (cupom.IdCliente != 0)
                pst.CommandText += "cupom_cliente_fk =?2, ";

            pst.CommandText += "codigo_cupom = ?3, tipo_cupom_fk = ?4, status_cupom = ?5, valor_cupom = ?6 WHERE id_cupom = ?7";

            parameters = new MySqlParameter[]
                {
                    new MySqlParameter("?1", cupom.IdPedido),
                    new MySqlParameter("?2", cupom.IdCliente),
                    new MySqlParameter("?3", cupom.CodigoCupom),
                    new MySqlParameter("?4", cupom.Tipo.ID),
                    new MySqlParameter("?5", cupom.Status),
                    new MySqlParameter("?6", cupom.ValorCupom),
                    new MySqlParameter("?7", cupom.ID)
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
            Cupom cupom = (Cupom)entidade;
            StringBuilder sql = new StringBuilder();

            sql.Append("SELECT * FROM cupom JOIN tipo_cupom ON (tipo_cupom.id_tipo_cupom = cupom.tipo_cupom_fk) ");

            // WHERE sem efeito, usado apenas para poder diminuir o número de ifs da construção da query
            sql.Append("WHERE 1 = 1 ");

            if (cupom.ID != 0)
            {
                sql.Append("AND id_cupom = ?1 ");
            }

            if (cupom.IdPedido != 0)
            {
                sql.Append("AND cupom_pedido_fk = ?2 ");
            }

            if (cupom.IdCliente != 0)
            {
                sql.Append("AND cupom_cliente_fk = ?3 ");
            }

            if (!String.IsNullOrEmpty(cupom.CodigoCupom))
            {
                sql.Append("AND codigo_cupom = ?4 ");
            }

            if (cupom.Tipo != null)
            {
                if (cupom.Tipo.ID != 0)
                {
                    sql.Append(" AND tipo_cupom_fk = ?5 ");
                }
            }

            if (cupom.Status != 'Z')
            {
                sql.Append("AND status_cupom = ?6 ");
            }

            if (cupom.ValorCupom != 0.0)
            {
                sql.Append("AND valor_cupom = ?7 ");
            }

            sql.Append("ORDER BY cupom.cupom_cliente_fk,cupom.id_cupom ");

            pst.CommandText = sql.ToString();
            parameters = new MySqlParameter[]
                {
                    new MySqlParameter("?1", cupom.ID),
                    new MySqlParameter("?2", cupom.IdPedido),
                    new MySqlParameter("?3", cupom.IdCliente),
                    new MySqlParameter("?4", cupom.CodigoCupom),
                    new MySqlParameter("?5", cupom.Tipo.ID),
                    new MySqlParameter("?6", cupom.Status),
                    new MySqlParameter("?7", cupom.ValorCupom)
                };

            pst.Parameters.Clear();
            pst.Parameters.AddRange(parameters);
            pst.Connection = connection;
            pst.CommandType = CommandType.Text;

            reader = pst.ExecuteReader();

            // Lista de retorno da consulta do banco de dados, que conterá os cartões do cliente encontrados
            List<EntidadeDominio> cupoms = new List<EntidadeDominio>();
            while (reader.Read())
            {
                cupom = new Cupom();
                cupom.ID = Convert.ToInt32(reader["id_cupom"]);

                if (!DBNull.Value.Equals(reader["cupom_pedido_fk"]))
                    cupom.IdPedido = Convert.ToInt32(reader["cupom_pedido_fk"]);

                if (!DBNull.Value.Equals(reader["cupom_cliente_fk"]))
                    cupom.IdCliente = Convert.ToInt32(reader["cupom_cliente_fk"]);

                cupom.CodigoCupom = reader["codigo_cupom"].ToString();
                cupom.Tipo.ID = Convert.ToInt32(reader["id_tipo_cupom"]);
                cupom.Tipo.Nome = reader["nome_tipo_cupom"].ToString();
                cupom.Status = reader["status_cupom"].ToString().First();
                cupom.ValorCupom = Convert.ToSingle(reader["valor_cupom"]);

                cupoms.Add(cupom);
            }
            connection.Close();
            return cupoms;
        }
    }
}
