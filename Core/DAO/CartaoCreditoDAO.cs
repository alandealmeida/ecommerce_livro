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
    public class CartaoCreditoDAO : AbstractDAO
    {
        public CartaoCreditoDAO() : base("cartao_credito", "id_cc")
        {

        }

        // construtor para DAOs que também utilizarão o DAO de Cartão
        public CartaoCreditoDAO(MySqlConnection connection, bool ctrlTransaction) : base(connection, ctrlTransaction, "cartao_credito", "id_cc")
        {

        }

        public override void Salvar(EntidadeDominio entidade)
        {
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            CartaoCredito cc = (CartaoCredito)entidade;

            pst.CommandText = "INSERT INTO cartao_credito (nome_impresso_cc, numero_cc, bandeira_cc_fk, codigo_seguranca_cc, dt_vencimento_cc) VALUES (?1, ?2, ?3, ?4, ?5)";
            parameters = new MySqlParameter[]
                {
                    new MySqlParameter("?1", cc.NomeImpresso),
                    new MySqlParameter("?2", cc.NumeroCC),
                    new MySqlParameter("?3", cc.Bandeira.ID),
                    new MySqlParameter("?4", cc.CodigoSeguranca),
                    new MySqlParameter("?5", cc.DataVencimento)
                };

            pst.Parameters.Clear();
            pst.Parameters.AddRange(parameters);
            pst.Connection = connection;
            pst.CommandType = CommandType.Text;

            pst.ExecuteNonQuery();
            cc.ID = entidade.ID = (int)pst.LastInsertedId;

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
            CartaoCredito cc = (CartaoCredito)entidade;
            StringBuilder sql = new StringBuilder();

            sql.Append("SELECT * FROM cartao_credito JOIN bandeira ON (cartao_credito.bandeira_cc_fk = bandeira.id_bandeira) ");
            
            // WHERE sem efeito, usado apenas para poder diminuir o número de ifs da construção da query
            sql.Append("WHERE 1 = 1 ");

            if (cc.ID != 0)
            {
                sql.Append("AND id_cc = ?1 ");
            }

            if (!String.IsNullOrEmpty(cc.NomeImpresso))
            {
                sql.Append("AND nome_impresso_cc = ?2 ");
            }

            if (!String.IsNullOrEmpty(cc.NumeroCC))
            {
                sql.Append("AND numero_cc = ?3 ");
            }

            if (cc.Bandeira.ID != 0)
            {
                sql.Append("AND bandeira_cc_fk = ?4 ");
            }

            if (!String.IsNullOrEmpty(cc.CodigoSeguranca))
            {
                sql.Append("AND codigo_seguranca_cc = ?5 ");
            }

            if (cc.DataVencimento != null)
            {
                sql.Append("AND dt_vencimento_cc = ?6 ");
            }

            sql.Append("ORDER BY id_cc ");

            pst.CommandText = sql.ToString();
            parameters = new MySqlParameter[]
                {
                    new MySqlParameter("?1", cc.ID),
                    new MySqlParameter("?2", cc.NomeImpresso),
                    new MySqlParameter("?3", cc.NumeroCC),
                    new MySqlParameter("?4", cc.Bandeira.ID),
                    new MySqlParameter("?5", cc.CodigoSeguranca),
                    new MySqlParameter("?6", cc.DataVencimento)
                };

            pst.Parameters.Clear();
            pst.Parameters.AddRange(parameters);
            pst.Connection = connection;
            pst.CommandType = CommandType.Text;

            reader = pst.ExecuteReader();

            // Lista de retorno da consulta do banco de dados, que conterá os endereços encontrados
            List<EntidadeDominio> ccs = new List<EntidadeDominio>();
            while (reader.Read())
            {
                cc = new CartaoCredito();
                cc.ID = Convert.ToInt32(reader["id_cc"]);
                cc.NomeImpresso = reader["nome_impresso_cc"].ToString();
                cc.NumeroCC = reader["numero_cc"].ToString();
                cc.Bandeira.ID = Convert.ToInt32(reader["id_bandeira"]);
                cc.Bandeira.Nome = reader["nome_bandeira"].ToString();
                cc.CodigoSeguranca = reader["codigo_seguranca_cc"].ToString();
                cc.DataVencimento = Convert.ToDateTime(reader["dt_vencimento_cc"].ToString());

                ccs.Add(cc);
            }
            connection.Close();
            return ccs;
        }
    }
}
