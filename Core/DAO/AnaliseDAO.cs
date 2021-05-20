using Dominio;
using Dominio.Analise;
using Dominio.Venda;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO
{
    public class AnaliseDAO : AbstractDAO
    {
        public AnaliseDAO() : base("pedido", "id_pedido")
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
            Analise analise = (Analise)entidade;
            StringBuilder sql = new StringBuilder();

            sql.Append("SELECT * FROM pedido JOIN status_pedido ON (pedido.status_pedido_fk = status_pedido.id_status_pedido) ");

            // WHERE sem efeito, usado apenas para poder diminuir o número de ifs da construção da query
            sql.Append("WHERE 1 = 1 ");

            if (analise.DataCadastro != null)
            {
                sql.Append("AND dt_cadastro_pedido >= '");
                sql.Append(analise.DataCadastro.Value.Year + "-"
                                            + analise.DataCadastro.Value.Month + "-"
                                            + analise.DataCadastro.Value.Day + " "
                                            + analise.DataCadastro.Value.Hour + ":"
                                            + analise.DataCadastro.Value.Minute + ":"
                                            + analise.DataCadastro.Value.Second);
                sql.Append("' ");
            }

            if (analise.DataFim != null)
            {
                sql.Append("AND dt_cadastro_pedido <= '");
                sql.Append(analise.DataFim.Value.Year + "-"
                                            + analise.DataFim.Value.Month + "-"
                                            + analise.DataFim.Value.Day + " "
                                            + analise.DataFim.Value.Hour + ":"
                                            + analise.DataFim.Value.Minute + ":"
                                            + analise.DataFim.Value.Second);
                sql.Append("' ");
            }

            sql.Append("ORDER BY pedido.dt_cadastro_pedido ");

            pst.CommandText = sql.ToString();
            parameters = new MySqlParameter[]
                {
                    new MySqlParameter("0", analise.ID)
                };

            if (analise.DataCadastro != null)
            {
                MySqlParameter[] parametersAux = new MySqlParameter[]
                   {
                    new MySqlParameter("1", analise.DataCadastro.Value.Year + "-"
                                            + analise.DataCadastro.Value.Month + "-"
                                            + analise.DataCadastro.Value.Day + " "
                                            + analise.DataCadastro.Value.Hour + ":"
                                            + analise.DataCadastro.Value.Minute + ":"
                                            + analise.DataCadastro.Value.Second)
                   };
                parameters.Concat(parametersAux);
            }
            if (analise.DataFim != null)
            {
                MySqlParameter[] parametersAux = new MySqlParameter[]
                      {
                      new MySqlParameter("2", analise.DataFim.Value.Year + "-"
                                            + analise.DataFim.Value.Month + "-"
                                            + analise.DataFim.Value.Day + " "
                                            + analise.DataFim.Value.Hour + ":"
                                            + analise.DataFim.Value.Minute + ":"
                                            + analise.DataFim.Value.Second)
                   };
                parameters.Concat(parametersAux);
            }

            pst.Parameters.Clear();
            if (parameters != null)
            {
                pst.Parameters.AddRange(parameters);
            }
            pst.Connection = connection;
            pst.CommandType = CommandType.Text;

            reader = pst.ExecuteReader();

            // Lista de retorno da consulta do banco de dados, que conterá os pedidos encontrados
            List<EntidadeDominio> pedidos = new List<EntidadeDominio>();
            while (reader.Read())
            {
                Pedido pedido = new Pedido();
                pedido.ID = Convert.ToInt32(reader["id_pedido"]);
                pedido.Usuario = reader["username"].ToString();
                pedido.Total = Convert.ToSingle(reader["total_pedido"]);

                pedido.Status.ID = Convert.ToInt32(reader["id_status_pedido"]);
                pedido.Status.Nome = reader["nome_status_pedido"].ToString();

                pedido.EnderecoEntrega.ID = Convert.ToInt32(reader["end_entrega_fk"]);

                pedido.Frete = Convert.ToSingle(reader["frete"]);

                pedido.DataCadastro = Convert.ToDateTime(reader["dt_cadastro_pedido"].ToString());

                pedidos.Add(pedido);
            }
            connection.Close();
            return pedidos;
        }
    }
}
