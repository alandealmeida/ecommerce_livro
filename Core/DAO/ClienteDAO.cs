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
    public class ClienteDAO : AbstractDAO
    {
        public ClienteDAO() : base("cliente_pf", "id_cli_pf")
        {

        }

        public override void Salvar(EntidadeDominio entidade)
        {
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            Cliente cliente = (Cliente)entidade;
            TelefoneDAO telefoneDAO = new TelefoneDAO(connection, false);
            telefoneDAO.Salvar(cliente.Telefone);

            pst.CommandText = "INSERT INTO cliente_pf (nome_cli_pf, telefone_cli_fk, email_cli_pf, cpf_cli_pf, genero_cli_pf, dt_nascimento_cli_pf, dt_cadastro_cli_pf) VALUES (?1, ?2, ?3, ?4, ?5, ?6, ?7); SELECT LAST_INSERT_ID(); ";
            parameters = new MySqlParameter[]
                {
                    new MySqlParameter("?1", cliente.Nome),
                    new MySqlParameter("?2", cliente.Telefone.ID),
                    new MySqlParameter("?3", cliente.Email),
                    new MySqlParameter("?4", cliente.CPF),
                    new MySqlParameter("?5", cliente.Genero),
                    new MySqlParameter("?6", cliente.DataNascimento),
                    new MySqlParameter("?7", cliente.DataCadastro)
                };

            pst.Parameters.Clear();
            pst.Parameters.AddRange(parameters);
            pst.Connection = connection;
            pst.CommandType = CommandType.Text;

            pst.ExecuteNonQuery();
            cliente.ID = (int)pst.LastInsertedId;

            // construtor já passando conexão de ClientePFDAO para EnderecoDAO
            EnderecoDAO enderecoDAO = new EnderecoDAO(connection, false);
            foreach (Endereco endereco in cliente.Enderecos)
            {
                enderecoDAO.Salvar(endereco);
            }

            // construtor para salvar o relacionamento n x n de cliente e endereço
            ClienteEnderecoDAO clienteXEnderecoDAO = new ClienteEnderecoDAO(connection, false);
            ClienteEndereco clienteEndereco = new ClienteEndereco();
            clienteEndereco.ID = cliente.ID;
            clienteEndereco.Endereco = cliente.Enderecos.First();
            clienteXEnderecoDAO.Salvar(clienteEndereco);
        
            pst.CommandText = "COMMIT WORK";
            connection.Close();
            return;
        }

        public override void Alterar(EntidadeDominio entidade)
        {
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            Cliente cliente = (Cliente)entidade;

            pst.CommandText = "UPDATE cliente_pf SET nome_cli_pf = ?1, telefone_cli_fk = ?2, email_cli_pf = ?3, cpf_cli_pf = ?4, genero_cli_pf = ?5, dt_nascimento_cli_pf = ?6 WHERE id_cli_pf = ?7 ";
            parameters = new MySqlParameter[]
                {
                    new MySqlParameter("1", cliente.Nome),
                    new MySqlParameter("2", cliente.Telefone.ID),
                    new MySqlParameter("3", cliente.Email),
                    new MySqlParameter("4", cliente.CPF),
                    new MySqlParameter("5", cliente.Genero),
                    new MySqlParameter("6", cliente.DataNascimento),
                    new MySqlParameter("7", cliente.ID)
                };

            pst.Parameters.Clear();
            pst.Parameters.AddRange(parameters);
            pst.Connection = connection;
            pst.CommandType = CommandType.Text;
            pst.ExecuteNonQuery();

            // construtor já passando conexão de cliente para telefone
            TelefoneDAO telefoneDAO = new TelefoneDAO(connection, false);
            telefoneDAO.Alterar(cliente.Telefone);

            pst.CommandText = "COMMIT WORK";
            connection.Close();
            return;
        }

        public override List<EntidadeDominio> Consultar(EntidadeDominio entidade)
        {
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            Cliente cliente = (Cliente)entidade;
            StringBuilder sql = new StringBuilder();

            sql.Append("SELECT * FROM cliente_pf JOIN telefone ON (cliente_pf.telefone_cli_fk = telefone.id_telefone) ");
            sql.Append("                            JOIN tipo_telefone ON (telefone.tipo_telefone_fk = tipo_telefone.id_tipo_tel) ");

            // WHERE sem efeito, usado apenas para poder diminuir o número de ifs da construção da query
            sql.Append("WHERE 1 = 1 ");

            if (cliente.ID != 0)
            {
                sql.Append("AND id_cli_pf = ?1 ");
            }

            if (!String.IsNullOrEmpty(cliente.Nome))
            {
                sql.Append("AND nome_cli_pf = ?2 ");
            }
            
            if (cliente.Telefone != null)
            {
                Telefone telefone = cliente.Telefone;
                if (telefone.ID != 0)
                {
                    sql.Append("AND id_telefone = ?3 ");
                }

                if (telefone.TipoTelefone.ID != 0)
                {
                    sql.Append("AND id_tipo_tel = ?4 ");
                }

                if (!String.IsNullOrEmpty(telefone.DDD))
                {
                    sql.Append("AND ddd_telefone = ?5 ");
                }

                if (!String.IsNullOrEmpty(telefone.NumeroTelefone))
                {
                    sql.Append("AND numero_telefone = ?6 ");
                }
            }

            if (!String.IsNullOrEmpty(cliente.Email))
            {
                sql.Append("AND email_cli_pf = ?7 ");
            }            

            if (!String.IsNullOrEmpty(cliente.CPF))
            {
                sql.Append("AND cpf_cli_pf = ?8 ");
            }

            if (!cliente.Genero.Equals("") && !cliente.Genero.Equals(null) && !cliente.Genero.Equals('\0') && !cliente.Genero.Equals('0'))
            {
                sql.Append("AND genero_cli_pf = ?9 ");
            }

            if (cliente.DataNascimento != null && !cliente.DataNascimento.Equals(null))
            {
                sql.Append("AND dt_nascimento_cli_pf = ?10 ");
            }

            if (cliente.DataCadastro != null)
            {
                sql.Append("AND dt_cadastro_cli_pf = ?11 ");
            }

            //sql.Append("ORDER BY id_cli_pf, endereco.id_endereco, cartao_credito.id_cc ");
            sql.Append("ORDER BY id_cli_pf ");

            pst.CommandText = sql.ToString();
            parameters = new MySqlParameter[]
                {
                    new MySqlParameter("?1", cliente.ID),
                    new MySqlParameter("?2", cliente.Nome),
                    new MySqlParameter("?3", cliente.Telefone.ID),
                    new MySqlParameter("?4", cliente.Telefone.TipoTelefone.ID),
                    new MySqlParameter("?5", cliente.Telefone.DDD),
                    new MySqlParameter("?6", cliente.Telefone.NumeroTelefone),
                    new MySqlParameter("?7", cliente.Email),
                    new MySqlParameter("?8", cliente.CPF),
                    new MySqlParameter("?9", cliente.Genero)
                };

            if (cliente.DataNascimento != null)
            {
                MySqlParameter[] parametersAux = new MySqlParameter[]
                   {
                    new MySqlParameter("?10", cliente.DataNascimento)
                   };

                parameters.Concat(parametersAux);
            }

            if (cliente.DataCadastro != null)
            {
                MySqlParameter[] parametersAux = new MySqlParameter[]
                   {
                    new MySqlParameter("?11", cliente.DataCadastro)
                   };

                parameters.Concat(parametersAux);
            }

            pst.Parameters.Clear();
            pst.Parameters.AddRange(parameters);
            pst.Connection = connection;
            pst.CommandType = CommandType.Text;

            reader = pst.ExecuteReader();

            // Lista de retorno da consulta do banco de dados, que conterá os clientes encontrados
            List<EntidadeDominio> clientes = new List<EntidadeDominio>();

            Cliente clienteAux = new Cliente();
            clienteAux.ID = 0;

            cliente = new Cliente();

            while (reader.Read())
            {
                // verifica se cliente que está trazendo do BD é igual ao anterior
                if (Convert.ToInt32(reader["id_cli_pf"]) != clienteAux.ID)
                {
                    cliente = new Cliente();
                    cliente.ID = Convert.ToInt32(reader["id_cli_pf"]);

                    // passando id do cliente que está vindo para o auxiliar
                    clienteAux.ID = cliente.ID;

                    cliente.Nome = reader["nome_cli_pf"].ToString();

                    // -------------------- TELEFONE - COMEÇO ----------------------------------
                    cliente.Telefone.ID = Convert.ToInt32(reader["id_telefone"]);
                    cliente.Telefone.TipoTelefone.ID = Convert.ToInt32(reader["id_tipo_tel"]);
                    cliente.Telefone.TipoTelefone.Nome = reader["nome_tipo_tel"].ToString();
                    cliente.Telefone.DDD = reader["ddd_telefone"].ToString();
                    cliente.Telefone.NumeroTelefone = reader["numero_telefone"].ToString();
                    // -------------------- TELEFONE - FIM ----------------------------------

                    cliente.Email = reader["email_cli_pf"].ToString();
                    cliente.CPF = reader["cpf_cli_pf"].ToString();
                    cliente.Genero = reader["genero_cli_pf"].ToString().First();
                    cliente.DataNascimento = Convert.ToDateTime(reader["dt_nascimento_cli_pf"].ToString());
                    cliente.DataCadastro = Convert.ToDateTime(reader["dt_cadastro_cli_pf"].ToString());

                }               
                clientes.Add(cliente);
            }
            connection.Close();
            return clientes;
        }

    }
}
