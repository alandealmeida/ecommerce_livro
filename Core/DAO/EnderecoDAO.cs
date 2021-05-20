﻿using System;
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
    public class EnderecoDAO : AbstractDAO
    {
        // construtor padrão
        public EnderecoDAO() : base("endereco", "id_endereco")
        {

        }

        // construtor para DAOs que também utilizarão o DAO de endereço
        public EnderecoDAO(MySqlConnection connection, bool ctrlTransaction) : base(connection, ctrlTransaction, "endereco", "id_endereco")
        {

        }

        public override void Salvar(EntidadeDominio entidade)
        {
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            Endereco endereco = (Endereco)entidade;
            
            pst.CommandText = "INSERT INTO endereco (nome_endereco, destinatario_endereco, tipo_residencia_fk, tipo_logradouro_fk, log_endereco, numero_endereco, bairro_endereco, cidade_fk, cep_endereco, observacao_endereco) VALUES (?1, ?2, ?3, ?4, ?5, ?6, ?7, ?8, ?9, ?10)";
            parameters = new MySqlParameter[]
                {
                    new MySqlParameter("?1", endereco.Nome),
                    new MySqlParameter("?2", endereco.Destinatario),
                    new MySqlParameter("?3", endereco.TipoResidencia.ID),
                    new MySqlParameter("?4", endereco.TipoLogradouro.ID),
                    new MySqlParameter("?5", endereco.Logradouro),
                    new MySqlParameter("?6", endereco.Numero),
                    new MySqlParameter("?7", endereco.Bairro),
                    new MySqlParameter("?8", endereco.Cidade.ID),
                    new MySqlParameter("?9", endereco.CEP),
                    new MySqlParameter("?10", endereco.Observacao)
                };
                        pst.Parameters.Clear();
            pst.Parameters.AddRange(parameters);
            pst.Connection = connection;
            pst.CommandType = CommandType.Text;

            pst.ExecuteNonQuery();
            endereco.ID = entidade.ID = (int)pst.LastInsertedId;

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
            Endereco endereco = (Endereco)entidade;

            pst.CommandText = "UPDATE endereco SET nome_endereco = ?1, destinatario_endereco = ?2, tipo_residencia_fk = ?3, tipo_logradouro_fk = ?4, log_endereco = ?5, numero_endereco = ?6, bairro_endereco = ?7, cidade_fk = ?8, cep_endereco = ?9, observacao_endereco = ?10 WHERE id_endereco = ?11";
            parameters = new MySqlParameter[]
                {
                    new MySqlParameter("?1", endereco.Nome),
                    new MySqlParameter("?2", endereco.Destinatario),
                    new MySqlParameter("?3", endereco.TipoResidencia.ID),
                    new MySqlParameter("?4", endereco.TipoLogradouro.ID),
                    new MySqlParameter("?5", endereco.Logradouro),
                    new MySqlParameter("?6", endereco.Numero),
                    new MySqlParameter("?7", endereco.Bairro),
                    new MySqlParameter("?8", endereco.Cidade.ID),
                    new MySqlParameter("?9", endereco.CEP),
                    new MySqlParameter("?10", endereco.Observacao),
                    new MySqlParameter("?11", endereco.ID)
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
            Endereco endereco = (Endereco)entidade;
            StringBuilder sql = new StringBuilder();

            sql.Append("SELECT * FROM endereco JOIN cidades ON (endereco.cidade_fk = cidades.id_cidade) ");
            sql.Append("                          JOIN estados ON (cidades.estado_id = estados.id_estado) ");
            sql.Append("                          JOIN paises ON (estados.pais_id = paises.id_pais) ");
            sql.Append("                          JOIN tipo_residencia ON (endereco.tipo_residencia_fk = tipo_residencia.id_tipo_res) ");
            sql.Append("                          JOIN tipo_logradouro ON (endereco.tipo_logradouro_fk = tipo_logradouro.id_tipo_log) ");

            // WHERE sem efeito, usado apenas para poder diminuir o número de ifs da construção da query
            sql.Append("WHERE 1 = 1 ");

            if (endereco.ID != 0)
            {
                sql.Append("AND id_endereco = ?1 ");
            }

            if (!String.IsNullOrEmpty(endereco.Nome))
            {
                sql.Append("AND nome_endereco = ?2 ");
            }

            if (endereco.TipoResidencia.ID != 0)
            {
                sql.Append("AND tipo_residencia_fk = ?3 ");
            }

            if (endereco.TipoLogradouro.ID != 0)
            {
                sql.Append("AND tipo_logradouro_fk = ?4 ");
            }

            if (!String.IsNullOrEmpty(endereco.Logradouro))
            {
                sql.Append("AND log_endereco = ?5 ");
            }

            if (!String.IsNullOrEmpty(endereco.Numero))
            {
                sql.Append("AND numero_endereco = ?6 ");
            }

            if (!String.IsNullOrEmpty(endereco.Bairro))
            {
                sql.Append("AND bairro_endereco = ?7 ");
            }

            if (endereco.Cidade.ID != 0)
            {
                sql.Append("AND id_cidade = ?8 ");
            }

            if (endereco.Cidade.Estado.ID != 0)
            {
                sql.Append("AND id_estado = ?9 ");
            }

            if (endereco.Cidade.Estado.Pais.ID != 0)
            {
                sql.Append("AND id_pais = ?10 ");
            }

            if (!String.IsNullOrEmpty(endereco.CEP))
            {
                sql.Append("AND cep_endereco = ?11 ");
            }

            if (!String.IsNullOrEmpty(endereco.Observacao))
            {
                sql.Append("AND observacao_endereco = ?12 ");
            }

            sql.Append("ORDER BY id_endereco ");

            pst.CommandText = sql.ToString();
            parameters = new MySqlParameter[]
                {
                    new MySqlParameter("?1", endereco.ID),
                    new MySqlParameter("?2", endereco.Nome),
                    new MySqlParameter("?3", endereco.TipoResidencia.ID),
                    new MySqlParameter("?4", endereco.TipoLogradouro.ID),
                    new MySqlParameter("?5", endereco.Logradouro),
                    new MySqlParameter("?6", endereco.Numero),
                    new MySqlParameter("?7", endereco.Bairro),
                    new MySqlParameter("?8", endereco.Cidade.ID),
                    new MySqlParameter("?9", endereco.Cidade.Estado.ID),
                    new MySqlParameter("?10", endereco.Cidade.Estado.Pais.ID),
                    new MySqlParameter("?11", endereco.CEP),
                    new MySqlParameter("?12", endereco.Observacao)
                };

            pst.Parameters.Clear();
            pst.Parameters.AddRange(parameters);
            pst.Connection = connection;
            pst.CommandType = CommandType.Text;

            reader = pst.ExecuteReader();

            // Lista de retorno da consulta do banco de dados, que conterá os endereços encontrados
            List<EntidadeDominio> enderecos = new List<EntidadeDominio>();
            while (reader.Read())
            {
                endereco = new Endereco();
                endereco.ID = Convert.ToInt32(reader["id_endereco"]);
                endereco.Nome = reader["nome_endereco"].ToString();
                endereco.Destinatario = reader["destinatario_endereco"].ToString();
                endereco.TipoResidencia.ID = Convert.ToInt32(reader["id_tipo_res"]);
                endereco.TipoResidencia.Nome = reader["nome_tipo_res"].ToString();
                endereco.TipoLogradouro.ID = Convert.ToInt32(reader["id_tipo_log"]);
                endereco.TipoLogradouro.Nome = reader["nome_tipo_log"].ToString();
                endereco.Logradouro = reader["log_endereco"].ToString();
                endereco.Numero = reader["numero_endereco"].ToString();
                endereco.Bairro = reader["bairro_endereco"].ToString();
                endereco.Cidade.ID = Convert.ToInt32(reader["id_cidade"].ToString());
                endereco.Cidade.Nome = reader["nome_cidade"].ToString();
                endereco.Cidade.Estado.ID = Convert.ToInt32(reader["id_estado"].ToString());
                endereco.Cidade.Estado.Nome = reader["nome_estado"].ToString();
                endereco.Cidade.Estado.Sigla = reader["sigla_estado"].ToString();
                endereco.Cidade.Estado.Pais.ID = Convert.ToInt32(reader["id_pais"].ToString());
                endereco.Cidade.Estado.Pais.Nome = reader["nome_pais"].ToString();
                endereco.Cidade.Estado.Pais.Sigla = reader["sigla_pais"].ToString();
                endereco.CEP = reader["cep_endereco"].ToString();
                endereco.Observacao = reader["observacao_endereco"].ToString();

                enderecos.Add(endereco);
            }
            connection.Close();
            return enderecos;
        }
    }
}
