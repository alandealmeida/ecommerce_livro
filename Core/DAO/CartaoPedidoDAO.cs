﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;
using MySql.Data.MySqlClient;
using System.Data;
using Dominio.Cliente;
using Dominio.Venda;

namespace Core.DAO
{
    public class CartaoPedidoDAO : AbstractDAO
    {
        // construtor padrão
        public CartaoPedidoDAO() : base("cartao_pedido", "id_cc_pedido")
        {

        }

        // construtor para DAOs que também utilizarão o DAO de CCPedido
        public CartaoPedidoDAO(MySqlConnection connection, bool ctrlTransaction) : base(connection, ctrlTransaction, "cartao_pedido", "id_cc_pedido")
        {

        }

        public override void Salvar(EntidadeDominio entidade)
        {
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            CartaoCreditoPedido ccPedido = (CartaoCreditoPedido)entidade;

            pst.CommandText = "INSERT INTO cartao_pedido(pedido_fk, cc_fk, valor_pagto) " +
                "VALUES (?1, ?2, ?3)";

            if(ccPedido.CC.ID < 0)
            {
                ccPedido.CC.ID = 0;
            }

            parameters = new MySqlParameter[]
                {
                    new MySqlParameter("?1", ccPedido.IdPedido),
                    new MySqlParameter("?2", ccPedido.CC.ID),
                    new MySqlParameter("?3", ccPedido.ValorCCPagto)
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
            throw new NotImplementedException();
        }

        public override List<EntidadeDominio> Consultar(EntidadeDominio entidade)
        {
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            CartaoCreditoPedido ccPedido = (CartaoCreditoPedido)entidade;
            StringBuilder sql = new StringBuilder();

            sql.Append("SELECT * FROM cartao_pedido JOIN cartao_credito ON cartao_credito.id_cc = cartao_pedido.cc_fk ");
            sql.Append("JOIN bandeira ON bandeira.id_bandeira = cartao_credito.bandeira_cc_fk ");

            // WHERE sem efeito, usado apenas para poder diminuir o número de ifs da construção da query
            sql.Append("WHERE 1 = 1 ");

            if (ccPedido.ID != 0)
            {
                sql.Append("AND id_cc_pedido = ?1 ");
            }

            if (ccPedido.IdPedido != 0)
            {
                sql.Append("AND pedido_fk = ?2 ");
            }

            if (ccPedido.CC.ID != 0)
            {
                sql.Append("AND cc_fk = ?3 ");
            }

            if (ccPedido.ValorCCPagto != 0.0)
            {
                sql.Append("AND valor_pagto = ?4 ");
            }

            sql.Append("ORDER BY cartao_pedido.id_cc_pedido,cartao_pedido.pedido_fk,cartao_pedido.cc_fk ");

            pst.CommandText = sql.ToString();
            parameters = new MySqlParameter[]
                {
                    new MySqlParameter("?1", ccPedido.ID),
                    new MySqlParameter("?2", ccPedido.IdPedido),
                    new MySqlParameter("?3", ccPedido.CC.ID),
                    new MySqlParameter("?4", ccPedido.ValorCCPagto)
                };

            pst.Parameters.Clear();
            pst.Parameters.AddRange(parameters);
            pst.Connection = connection;
            pst.CommandType = CommandType.Text;

            reader = pst.ExecuteReader();

            // Lista de retorno da consulta do banco de dados, que conterá os cartões do cliente encontrados
            List<EntidadeDominio> ccPedidos = new List<EntidadeDominio>();
            while (reader.Read())
            {
                ccPedido = new CartaoCreditoPedido();
                ccPedido.ID = Convert.ToInt32(reader["id_cc_pedido"]);
                ccPedido.IdPedido = Convert.ToInt32(reader["pedido_fk"]);

                ccPedido.CC.ID = Convert.ToInt32(reader["id_cc"]);
                ccPedido.CC.NomeImpresso = reader["nome_impresso_cc"].ToString();
                ccPedido.CC.NumeroCC = reader["numero_cc"].ToString();
                ccPedido.CC.Bandeira.ID = Convert.ToInt32(reader["id_bandeira"]);
                ccPedido.CC.Bandeira.Nome = reader["nome_bandeira"].ToString();
                ccPedido.CC.CodigoSeguranca = reader["codigo_seguranca_cc"].ToString();
                ccPedido.CC.DataVencimento = Convert.ToDateTime(reader["dt_vencimento_cc"].ToString());

                ccPedido.ValorCCPagto = Convert.ToSingle(reader["valor_pagto"]);

                ccPedidos.Add(ccPedido);
            }
            connection.Close();
            return ccPedidos;
        }
    }
}
