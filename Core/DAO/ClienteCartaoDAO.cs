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
    public class ClienteCartaoDAO : AbstractDAO
    {
        // construtor padrão
        public ClienteCartaoDAO() : base("cliente_cartao", "id_cliente")
        {

        }

        // construtor padrão
        public ClienteCartaoDAO(string id) : base("cliente_cartao", id)
        {

        }

        // construtor para DAOs que também utilizarão o DAO do relacionamento n x n de cliente e cartão
        public ClienteCartaoDAO(MySqlConnection connection, bool ctrlTransaction) : base(connection, ctrlTransaction, "cliente_cartao", "id_cliente")
        {

        }

        public override void Salvar(EntidadeDominio entidade)
        {
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            ClienteCartao clienteCartao = (ClienteCartao)entidade;

            // construtor já passando conexão de cliente para cartao
            CartaoCreditoDAO ccDAO = new CartaoCreditoDAO(connection, false);
            ccDAO.Salvar(clienteCartao.CC);

            pst.CommandText = "INSERT INTO cliente_cartao(id_cliente, id_cartao) VALUES (?1, ?2); ";
            parameters = new MySqlParameter[]
                {
                    new MySqlParameter("?1", clienteCartao.ID),
                    new MySqlParameter("?2", clienteCartao.CC.ID)
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
            ClienteCartao clienteCartao = (ClienteCartao)entidade;
            StringBuilder sql = new StringBuilder();

            sql.Append("SELECT * FROM cliente_cartao JOIN cartao_credito ON (cliente_cartao.id_cartao = cartao_credito.id_cc) ");
            sql.Append("                                JOIN bandeira ON (cartao_credito.bandeira_cc_fk = bandeira.id_bandeira) ");

            // WHERE sem efeito, usado apenas para poder diminuir o número de ifs da construção da query
            sql.Append("WHERE 1 = 1 ");

            if (clienteCartao.ID != 0)
            {
                sql.Append("AND id_cliente = ?1 ");
            }

            if (clienteCartao.CC != null)
            {
                if (clienteCartao.CC.ID != 0)
                {
                    sql.Append(" AND id_cartao = ?2 ");
                }
            }

            sql.Append("ORDER BY cliente_cartao.id_cliente,cliente_cartao.id_cartao ");

            pst.CommandText = sql.ToString();
            parameters = new MySqlParameter[]
                {
                    new MySqlParameter("?1", clienteCartao.ID),
                    new MySqlParameter("?2", clienteCartao.CC.ID)
                };

            pst.Parameters.Clear();
            pst.Parameters.AddRange(parameters);
            pst.Connection = connection;
            pst.CommandType = CommandType.Text;

            reader = pst.ExecuteReader();

            // Lista de retorno da consulta do banco de dados, que conterá os cartões do cliente encontrados
            List<EntidadeDominio> clienteCartaos = new List<EntidadeDominio>();
            while (reader.Read())
            {
                clienteCartao = new ClienteCartao();
                clienteCartao.ID = Convert.ToInt32(reader["id_cliente"]);
                clienteCartao.CC.ID = Convert.ToInt32(reader["id_cc"]);
                clienteCartao.CC.NomeImpresso = reader["nome_impresso_cc"].ToString();
                clienteCartao.CC.NumeroCC = reader["numero_cc"].ToString();
                clienteCartao.CC.Bandeira.ID = Convert.ToInt32(reader["id_bandeira"]);
                clienteCartao.CC.Bandeira.Nome = reader["nome_bandeira"].ToString();
                clienteCartao.CC.CodigoSeguranca = reader["codigo_seguranca_cc"].ToString();
                clienteCartao.CC.DataVencimento = Convert.ToDateTime(reader["dt_vencimento_cc"].ToString());

                clienteCartaos.Add(clienteCartao);
            }
            connection.Close();
            return clienteCartaos;
        }
    }
}
