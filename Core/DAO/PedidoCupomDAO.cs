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
    public class PedidoCupomDAO : AbstractDAO
    {
        // construtor padrão
        public PedidoCupomDAO() : base("cupom_pedido", "id_pedido")
        {

        }

        // construtor para DAOs que também utilizarão o DAO do relacionamento n x n de pedido e cupom
        public PedidoCupomDAO(MySqlConnection connection, bool ctrlTransaction) : base(connection, ctrlTransaction, "cupom_pedido", "id_pedido")
        {

        }

        public override void Salvar(EntidadeDominio entidade)
        {
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            PedidoCupom pedidoXCupom = (PedidoCupom)entidade;

            pst.CommandText = "INSERT INTO cupom_pedido(id_pedido, id_cupom) VALUES (?1, ?2); ";
            parameters = new MySqlParameter[]
                {
                    new MySqlParameter("?1", pedidoXCupom.ID),
                    new MySqlParameter("?2", pedidoXCupom.Cupom.ID)
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
            // não implementado devido que será excluído TODOS os registros para inserção do novo
            // e para que não tenha problema na hora de alteração de quantidade de cartões
            throw new NotImplementedException();
        }

        public override List<EntidadeDominio> Consultar(EntidadeDominio entidade)
        {
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            PedidoCupom pedidoXCupom = (PedidoCupom)entidade;
            StringBuilder sql = new StringBuilder();

            sql.Append("SELECT * FROM cupom_pedido JOIN cupom ON (cupom.id_cupom = cupom_pedido.id_cupom) ");
            sql.Append("                               JOIN tipo_cupom ON (tipo_cupom.id_tipo_cupom = cupom.tipo_cupom_fk) ");

            // WHERE sem efeito, usado apenas para poder diminuir o número de ifs da construção da query
            sql.Append("WHERE 1 = 1 ");

            if (pedidoXCupom.ID != 0)
            {
                sql.Append("AND id_pedido = ?1 ");
            }

            if (pedidoXCupom.Cupom != null)
            {
                if (pedidoXCupom.Cupom.ID != 0)
                {
                    sql.Append(" AND id_cupom = ?2 ");
                }
            }

            sql.Append("ORDER BY cupom_pedido.id_pedido,cupom_pedido.id_cupom ");

            pst.CommandText = sql.ToString();
            parameters = new MySqlParameter[]
                {
                    new MySqlParameter("?1", pedidoXCupom.ID),
                    new MySqlParameter("?2", pedidoXCupom.Cupom.ID)
                };

            pst.Parameters.Clear();
            pst.Parameters.AddRange(parameters);
            pst.Connection = connection;
            pst.CommandType = CommandType.Text;

            reader = pst.ExecuteReader();

            // Lista de retorno da consulta do banco de dados, que conterá os cartões do pedido encontrados
            List<EntidadeDominio> pedidoXCupoms = new List<EntidadeDominio>();
            while (reader.Read())
            {
                pedidoXCupom = new PedidoCupom();
                pedidoXCupom.ID = Convert.ToInt32(reader["id_pedido"]);
                pedidoXCupom.Cupom.ID = Convert.ToInt32(reader["id_cupom"]);

                if (!DBNull.Value.Equals(reader["cupom_pedido_fk"]))
                    pedidoXCupom.Cupom.IdPedido = Convert.ToInt32(reader["cupom_pedido_fk"]);

                if (!DBNull.Value.Equals(reader["cupom_pedido_fk"]))
                    pedidoXCupom.Cupom.IdPedido = Convert.ToInt32(reader["cupom_pedido_fk"]);

                pedidoXCupom.Cupom.CodigoCupom = reader["codigo_cupom"].ToString();
                pedidoXCupom.Cupom.Tipo.ID = Convert.ToInt32(reader["id_tipo_cupom"]);
                pedidoXCupom.Cupom.Tipo.Nome = reader["nome_tipo_cupom"].ToString();
                pedidoXCupom.Cupom.Status = reader["status_cupom"].ToString().First();
                pedidoXCupom.Cupom.ValorCupom = Convert.ToSingle(reader["valor_cupom"]);

                pedidoXCupoms.Add(pedidoXCupom);
            }
            connection.Close();
            return pedidoXCupoms;
        }
    }
}
