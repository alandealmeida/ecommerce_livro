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
    public class ClienteEnderecoDAO : AbstractDAO
    {
        // construtor padrão
        public ClienteEnderecoDAO() : base("cliente_endereco", "id_cliente")
        {

        }

        // construtor padrão
        public ClienteEnderecoDAO(string id) : base("cliente_endereco", id)
        {

        }

        // construtor para DAOs que também utilizarão o DAO do relacionamento n x n de cliente e endereço
        public ClienteEnderecoDAO(MySqlConnection connection, bool ctrlTransaction) : base(connection, ctrlTransaction, "cliente_endereco", "id_cliente")
        {

        }

        public override void Salvar(EntidadeDominio entidade)
        {
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            ClienteEndereco clienteEndereco = (ClienteEndereco)entidade;

            // construtor já passando conexão de cliente para cartao
            EnderecoDAO enderecoDAO = new EnderecoDAO(connection, false);
            enderecoDAO.Salvar(clienteEndereco.Endereco);

            pst.CommandText = "INSERT INTO cliente_endereco(id_cliente, id_endereco) VALUES (?1, ?2); ";
            parameters = new MySqlParameter[]
                {
                    new MySqlParameter("?1", clienteEndereco.ID),
                    new MySqlParameter("?2", clienteEndereco.Endereco.ID)
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
            // e para que não tenha problema na hora de alteração de quantidade de endereços
            throw new NotImplementedException();
        }

        public override List<EntidadeDominio> Consultar(EntidadeDominio entidade)
        {
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            ClienteEndereco clienteEndereco = (ClienteEndereco)entidade;
            StringBuilder sql = new StringBuilder();

            sql.Append("SELECT * FROM cliente_endereco JOIN endereco ON (cliente_endereco.id_endereco = endereco.id_endereco) ");
            sql.Append("                                    JOIN cidades ON (endereco.cidade_fk = cidades.id_cidade) ");
            sql.Append("                                    JOIN estados ON (cidades.estado_id = estados.id_estado) ");
            sql.Append("                                    JOIN paises ON (estados.pais_id = paises.id_pais) ");
            sql.Append("                                    JOIN tipo_residencia ON (endereco.tipo_residencia_fk = tipo_residencia.id_tipo_res) ");
            sql.Append("                                    JOIN tipo_logradouro ON (endereco.tipo_logradouro_fk = tipo_logradouro.id_tipo_log) ");

            // WHERE sem efeito, usado apenas para poder diminuir o número de ifs da construção da query
            sql.Append("WHERE 1 = 1 ");

            if (clienteEndereco.ID != 0)
            {
                sql.Append("AND id_cliente = ?1 ");
            }

            if (clienteEndereco.Endereco != null)
            {
                if (clienteEndereco.Endereco.ID != 0)
                {
                    sql.Append(" AND id_endereco = ?2 ");
                }
            }

            sql.Append("ORDER BY cliente_endereco.id_cliente, cliente_endereco.id_endereco ");

            pst.CommandText = sql.ToString();
            parameters = new MySqlParameter[]
                {
                    new MySqlParameter("?1", clienteEndereco.ID),
                    new MySqlParameter("?2", clienteEndereco.Endereco.ID)
                };

            pst.Parameters.Clear();
            pst.Parameters.AddRange(parameters);
            pst.Connection = connection;
            pst.CommandType = CommandType.Text;

            reader = pst.ExecuteReader();

            // Lista de retorno da consulta do banco de dados, que conterá os endereços do cliente encontrados
            List<EntidadeDominio> clienteEnderecos = new List<EntidadeDominio>();
            while (reader.Read())
            {
                clienteEndereco = new ClienteEndereco();

                clienteEndereco.ID = Convert.ToInt32(reader["id_cliente"]);

                clienteEndereco.Endereco.ID = Convert.ToInt32(reader["id_endereco"]);
                clienteEndereco.Endereco.Nome = reader["nome_endereco"].ToString();
                clienteEndereco.Endereco.Destinatario = reader["destinatario_endereco"].ToString();
                clienteEndereco.Endereco.TipoResidencia.ID = Convert.ToInt32(reader["id_tipo_res"]);
                clienteEndereco.Endereco.TipoResidencia.Nome = reader["nome_tipo_res"].ToString();
                clienteEndereco.Endereco.TipoLogradouro.ID = Convert.ToInt32(reader["id_tipo_log"]);
                clienteEndereco.Endereco.TipoLogradouro.Nome = reader["nome_tipo_log"].ToString();
                clienteEndereco.Endereco.Logradouro = reader["log_endereco"].ToString();
                clienteEndereco.Endereco.Numero = reader["numero_endereco"].ToString();
                clienteEndereco.Endereco.Bairro = reader["bairro_endereco"].ToString();
                clienteEndereco.Endereco.Cidade.ID = Convert.ToInt32(reader["id_cidade"].ToString());
                clienteEndereco.Endereco.Cidade.Nome = reader["nome_cidade"].ToString();
                clienteEndereco.Endereco.Cidade.Estado.ID = Convert.ToInt32(reader["id_estado"].ToString());
                clienteEndereco.Endereco.Cidade.Estado.Nome = reader["nome_estado"].ToString();
                clienteEndereco.Endereco.Cidade.Estado.Sigla = reader["sigla_estado"].ToString();
                clienteEndereco.Endereco.Cidade.Estado.Pais.ID = Convert.ToInt32(reader["id_pais"].ToString());
                clienteEndereco.Endereco.Cidade.Estado.Pais.Nome = reader["nome_pais"].ToString();
                clienteEndereco.Endereco.Cidade.Estado.Pais.Sigla = reader["sigla_pais"].ToString();
                clienteEndereco.Endereco.CEP = reader["cep_endereco"].ToString();
                clienteEndereco.Endereco.Observacao = reader["observacao_endereco"].ToString();

                clienteEnderecos.Add(clienteEndereco);
            }
            connection.Close();
            return clienteEnderecos;
        }
    }
}
