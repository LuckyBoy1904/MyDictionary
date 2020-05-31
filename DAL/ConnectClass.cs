using BAL;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class ConnectClass : IDisposable
    {
        private readonly IConfiguration configuration;
        private SqlConnection _sqlConnection;
        private SqlCommand _sqlCommand;
        public ConnectClass()
        {
            _sqlConnection = new SqlConnection(this.GetConnectionString());
            _sqlCommand = _sqlConnection.CreateCommand();
            _sqlCommand.CommandType = CommandType.StoredProcedure;
            if (_sqlConnection.State != ConnectionState.Open)
            {
                try
                {
                    _sqlConnection.Open();
                }
                catch (SqlException ex)
                {
                    throw (ex);
                }
            }
        }

        public void Dispose()
        {
            _sqlConnection.Close();
        }

        public string GetConnectionString()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            string connectString = builder.Build().GetSection("Data").GetSection("ConnectionString").Value;
            return connectString;
        }

        public List<Vocabulary> GetListWord(string storeName)
        {
            List<Vocabulary> lstVocab = new List<Vocabulary>();
            _sqlCommand.CommandText = storeName;
            //_sqlCommand.Parameters.AddWithValue("word", word);
            SqlDataReader sqlDataReader = _sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                var theWord = new Vocabulary();
                for (int i = 0; i < sqlDataReader.FieldCount; i++)
                {
                    var fieldName = sqlDataReader.GetName(i);
                    var fieldValue = sqlDataReader.GetValue(i);
                    var property = theWord.GetType().GetProperty(fieldName);
                    if (property != null && fieldValue != DBNull.Value)
                    {
                        property.SetValue(theWord, fieldValue);
                    }
                }
                lstVocab.Add(theWord);
            }
            return lstVocab;
        }

        public Vocabulary Search(string storeName, string word)
        {
            Vocabulary vocab = new Vocabulary();
            _sqlCommand.CommandText = storeName;
            _sqlCommand.Parameters.AddWithValue("@tu", word);
            SqlDataReader sqlDataReader = _sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                for (int i = 0; i < sqlDataReader.FieldCount; i++)
                {
                    var fieldName = sqlDataReader.GetName(i);
                    var fieldValue = sqlDataReader.GetValue(i);
                    var property = vocab.GetType().GetProperty(fieldName);
                    if (property != null && fieldValue != DBNull.Value)
                    {
                        property.SetValue(vocab, fieldValue);
                    }
                }
            }
            return vocab;
        }
    }
}
