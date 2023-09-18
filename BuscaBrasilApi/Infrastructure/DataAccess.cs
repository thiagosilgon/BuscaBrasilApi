using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

namespace BuscaBrasilApi.Infrastructure
{
    public class DataAccess : IDisposable
    {
        private readonly string _connectionString;
        private SqlConnection _connection;

        public DataAccess(string conn)
        {
            _connectionString = conn;
        }


        private bool OpenConnection()
        {
            bool resp = true;
            try
            {
                if (_connection == null) _connection = new SqlConnection(_connectionString);
                if (_connection.State != ConnectionState.Open) _connection.Open();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return resp;
        }

        public void CloseConnection()
        {
            if (_connection.State == ConnectionState.Open) _connection.Close();
        }


        public void Dispose()
        {
            if (_connection != null)
            {
                if (_connection.State == ConnectionState.Open) _connection.Close();
                _connection = null;
            }
        }

        public int ExecuteNonQuery(string commandText)
        {
            OpenConnection();
            int cte;
            try
            {
                if (_connection.State != ConnectionState.Open)
                    OpenConnection();
                SqlCommand command = new SqlCommand(commandText);
                command.Connection = _connection;
                cte = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return cte;
        }
    }
}
