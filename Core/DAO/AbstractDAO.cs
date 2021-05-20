using Core.Core;
using Core.Util;
using Dominio;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO
{
    public abstract class AbstractDAO : IDAO
    {
        protected MySqlDataReader reader;
        protected MySqlConnection connection = Conexao.Connection;
        protected string table;
        protected string id_table;
        protected bool ctrlTransaction = true;
        protected MySqlCommand pst = new MySqlCommand();
        protected MySqlParameter[] parameters;

        public AbstractDAO(MySqlConnection connection, string table, string id_table)
        {
            this.table = table;
            this.id_table = id_table;
            this.connection = connection;
        }

        public AbstractDAO(MySqlConnection connection, bool ctrlTransaction, string table, string id_table)
        {
            this.table = table;
            this.id_table = id_table;
            this.connection = connection;
            this.ctrlTransaction = ctrlTransaction;
        }

        protected AbstractDAO(string table, string id_table)
        {
            this.table = table;
            this.id_table = id_table;
        }

        public abstract void Salvar(EntidadeDominio entidade);

        public abstract void Alterar(EntidadeDominio entidade);

        public abstract List<EntidadeDominio> Consultar(EntidadeDominio entidade);

        /*
         * Método de exclusão genérico (exclusão do banco de dados)
         */
        public virtual void Excluir(EntidadeDominio entidade)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                pst.CommandText = "DELETE FROM " + table + " WHERE " + id_table + " = ?1";
                pst.Parameters.Clear();
                pst.Parameters.Add(new MySqlParameter("?1", entidade.ID));
                pst.Connection = connection;
                pst.CommandType = CommandType.Text;
                pst.ExecuteNonQuery();
                pst.CommandText = "COMMIT WORK";
                pst.ExecuteNonQuery();
                if (ctrlTransaction)
                    connection.Close();
            }
            catch (Exception e)
            {
                throw e;
            }

        }
    }
}
