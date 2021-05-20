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
using Dominio.Livro;

namespace Core.DAO
{
    public class PedidoDAO : AbstractDAO
    {
        // construtor padrão
        public PedidoDAO() : base("pedido", "id_pedido")
        {

        }

        // construtor para DAOs que também utilizarão o DAO de Pedido
        public PedidoDAO(MySqlConnection connection, bool ctrlTransaction) : base(connection, ctrlTransaction, "pedido", "id_pedido")
        {

        }

        public override void Salvar(EntidadeDominio entidade)
        {
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            Pedido pedido = (Pedido)entidade;

            pst.CommandText = "INSERT INTO pedido(username, total_pedido, status_pedido_fk, end_entrega_fk, frete, dt_cadastro_pedido) " +
                "VALUES (?1, ?2, ?3, ?4, ?5, ?6)";

            parameters = new MySqlParameter[]
                {
                    new MySqlParameter("?1", pedido.Usuario),
                    new MySqlParameter("?2", pedido.Total),
                    new MySqlParameter("?3", pedido.Status.ID),
                    new MySqlParameter("?4", pedido.EnderecoEntrega.ID),
                    new MySqlParameter("?5", pedido.Frete),
                    new MySqlParameter("?6", pedido.DataCadastro)
                };

            pst.Parameters.Clear();
            pst.Parameters.AddRange(parameters);
            pst.Connection = connection;
            pst.CommandType = CommandType.Text;

            pst.ExecuteNonQuery();
            pedido.ID = entidade.ID = (int)pst.LastInsertedId;
            // variáveis que conterão os valores do pedido (Subtotal) 
            // e também os valores dos cupons para tirar a diferença dos mesmo, 
            // se necessário criar outro cupom de troca
            decimal subtotal = 0;
            decimal valorCupons = 0;

            // salvando os itens do pedido
            PedidoDetalheDAO pedidoDetalheDAO = new PedidoDetalheDAO(connection, false);
            foreach (PedidoDetalhe detalhe in pedido.Detalhes)
            {
                detalhe.IdPedido = pedido.ID;
                pedidoDetalheDAO.Salvar(detalhe);

                if(pedido.Status.ID != 6)
                {
                    // a cada iteração da lista pega a quantidade e valor do item que está sendo iterado
                    subtotal += (decimal)(detalhe.Quantidade * detalhe.ValorUnit);

                    // Dando baixa no estoque
                    Estoque estoque = ((Estoque)new EstoqueDAO().Consultar(new Estoque() { Livro = new Livro() { ID = detalhe.Livro.ID } }).ElementAt(0));
                    estoque.Qtde -= detalhe.Quantidade;
                    new EstoqueDAO(connection, false).Alterar(estoque);
                }
            }

            // salvando os cartões utilizados no pedido
            CartaoPedidoDAO ccPedidoDAO = new CartaoPedidoDAO(connection, false);
            foreach (CartaoCreditoPedido cc in pedido.CCs)
            {
                cc.IdPedido = pedido.ID;
                ccPedidoDAO.Salvar(cc);
            }

            // verifica se cupom promocional foi utilizada
            if(pedido.CupomPromocional.ID != 0)
            {
                Cliente cliente = new Cliente();
                // passa usuário que é dono do pedido para o e-mail do cliente
                cliente.Email = pedido.Usuario;

                // para trazer o ID do cliente que será utilizado 
                cliente = ((Cliente)new ClienteDAO().Consultar(cliente).ElementAt(0));

                // contem que o cliente já utilizou o cupom promocional
                PedidoCupom clienteXCupom = new PedidoCupom();
                clienteXCupom.ID = pedido.ID;
                clienteXCupom.Cupom.ID = pedido.CupomPromocional.ID;

                // preparando e salvando no BD
                PedidoCupomDAO pedidoXCupomDAO = new PedidoCupomDAO(connection, false);
                pedidoXCupomDAO.Salvar(clienteXCupom);

                // consulta em CupomDAO para pegar o valor do cupom e assim fazer a conta para valor de cupons
                Cupom cupom = new Cupom();
                cupom = ((Cupom)new CupomDAO().Consultar(pedido.CupomPromocional).ElementAt(0));
                
                // Operação é multiplicação devido ser porcentagem (%)
                valorCupons = subtotal * (decimal)cupom.ValorCupom;
            }

            // alterando status do cupom que o cliente utilizou
            foreach (Cupom cp in pedido.CuponsTroca)
            {
                Cupom cupom = new Cupom();
                cupom = ((Cupom)new CupomDAO().Consultar(cp).ElementAt(0));
                cupom.IdPedido = pedido.ID;
                cupom.Status = 'I';
                new CupomDAO(connection,false).Alterar(cupom);

                // adiciona o valor do cupom para variável
                valorCupons += (decimal)cupom.ValorCupom;
            }

            // verifica se o valor dos cupons usados superam o valor do subtotal
            // se sim, criará outro cupom de troca para o cliente poder estar usando
            if(valorCupons > subtotal)
            {
                Cliente cliente = new Cliente();
                // passa usuário que é dono do pedido para o e-mail do cliente
                cliente.Email = pedido.Usuario;

                // para trazer o ID do cliente que será utilizado 
                cliente = ((Cliente)new ClienteDAO().Consultar(cliente).ElementAt(0));

                Cupom cupom = new Cupom();
                cupom.IdCliente = cliente.ID;
                cupom.Tipo.ID = 1;      // cupom troca
                cupom.Status = 'A';
                cupom.ValorCupom = (float)(valorCupons - subtotal);
                cupom.CodigoCupom = "CUPOMTROCO" + cupom.IdCliente + DateTime.Now.ToString("yyyyMMddHHmmss") + "$" + cupom.ValorCupom;
                new CupomDAO(connection, false).Salvar(cupom);
            }

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
            Pedido pedido = (Pedido)entidade;

            pst.CommandText = "UPDATE pedido SET status_pedido_fk = ?1 WHERE id_pedido = ?2";
            parameters = new MySqlParameter[]
                {
                    new MySqlParameter("?1", pedido.Status.ID),
                    new MySqlParameter("?2", pedido.ID)
                };

            pst.Parameters.Clear();
            pst.Parameters.AddRange(parameters);
            pst.Connection = connection;
            pst.CommandType = CommandType.Text;
            pst.ExecuteNonQuery();

            // verifica se status é igual a REPROVADO, 
            // para assim, então, poder fazer a reentrada no estoque
            if(pedido.Status.ID == 3)
            {
                // passa ID de pedido e consulta os itens inseridos no pedido
                foreach (PedidoDetalhe detalhe in
                    new PedidoDetalheDAO().Consultar(new PedidoDetalhe() { IdPedido = pedido.ID }).Cast<PedidoDetalhe>())
                {
                    // Reentrada no estoque
                    Estoque estoque = ((Estoque)new EstoqueDAO().Consultar(new Estoque() { Livro = new Livro() { ID = detalhe.Livro.ID } } ).ElementAt(0));
                    estoque.Qtde += detalhe.Quantidade;
                    new EstoqueDAO(connection, false).Alterar(estoque);
                }
            }

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
            Pedido pedido = (Pedido)entidade;
            StringBuilder sql = new StringBuilder();

            sql.Append("SELECT * FROM pedido JOIN status_pedido ON (pedido.status_pedido_fk = status_pedido.id_status_pedido) ");

            // WHERE sem efeito, usado apenas para poder diminuir o número de ifs da construção da query
            sql.Append("WHERE 1 = 1 ");

            if (pedido.ID != 0)
            {
                sql.Append("AND id_pedido = ?1 ");
            }

            if (!String.IsNullOrEmpty(pedido.Usuario))
            {
                sql.Append("AND username = ?2 ");
            }

            if (pedido.Total != 0.0)
            {
                sql.Append("AND total_pedido = ?3 ");
            }

            if (pedido.Status.ID != 0)
            {
                sql.Append("AND status_pedido_fk = ?4 ");
            }

            if (pedido.EnderecoEntrega.ID != 0)
            {
                sql.Append("AND end_entrega_fk = ?5 ");
            }

            if (pedido.Frete != 0.0)
            {
                sql.Append("AND frete = ?6 ");
            }

            if (pedido.DataCadastro != null)
            {
                sql.Append("AND dt_cadastro_pedido = ?7 ");
            }

            sql.Append("ORDER BY pedido.id_pedido,pedido.status_pedido_fk ");

            pst.CommandText = sql.ToString();
            parameters = new MySqlParameter[]
                {
                    new MySqlParameter("?1", pedido.ID),
                    new MySqlParameter("?2", pedido.Usuario),
                    new MySqlParameter("?3", pedido.Total),
                    new MySqlParameter("?4", pedido.Status.ID),
                    new MySqlParameter("?5", pedido.EnderecoEntrega.ID),
                    new MySqlParameter("?6", pedido.Frete),
                    new MySqlParameter("?7", pedido.DataCadastro)
                };

            if (pedido.DataCadastro != null)
            {
                MySqlParameter[] parametersAux = new MySqlParameter[]
                   {
                    new MySqlParameter("?7", pedido.DataCadastro)
                   };

                parameters.Concat(parametersAux);
            }

            pst.Parameters.Clear();
            pst.Parameters.AddRange(parameters);
            pst.Connection = connection;
            pst.CommandType = CommandType.Text;

            reader = pst.ExecuteReader();

            // Lista de retorno da consulta do banco de dados, que conterá os cartões do cliente encontrados
            List<EntidadeDominio> pedidos = new List<EntidadeDominio>();
            while (reader.Read())
            {
                pedido = new Pedido();
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
