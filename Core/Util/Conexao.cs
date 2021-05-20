using System;
using MySql.Data.MySqlClient;

namespace Core.Util
{
    public static class Conexao
    {
        private static MySqlConnection _Connection;

        public static MySqlConnection Connection
        {
            get
            {
                if (_Connection == null)
                    _Connection = new MySqlConnection("Server=localhost;Database=elivro;Uid=root;Pwd=09121731;");
                return _Connection;
            }
            set { _Connection = value; }
        }

    }
}
