using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace UseJiebaSegmenter
{
    public class Persister
    {
        public static string DefaultDataSource { get; private set; }
        static Persister()
        {
            DefaultDataSource = ConfigurationManager.ConnectionStrings["DefaultDataSource"].ConnectionString;
        }
        public static int ExecuteNonQuery(SqlCommand command)
        {
            return ExecuteNonQuery(command, DefaultDataSource);
        }

        public static int ExecuteNonQuery(SqlCommand command, string connectionString)
        {
            int affect = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                command.Connection = connection;

                connection.Open();
                affect = command.ExecuteNonQuery();
            }
            return affect;
        }

        public static DataTable Execute(SqlCommand command)
        {
            return Execute(command, DefaultDataSource);
        }

        public static DataTable Execute(SqlCommand command, string connectionString)
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                command.Connection = connection;

                connection.Open();
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    dt.BeginLoadData();
                    adapter.Fill(dt);
                    dt.EndLoadData();
                }
            }
            return dt;
        }

        public static int ExecuteScalar(SqlCommand command)
        {
            return ExecuteScalar(command, DefaultDataSource);
        }

        public static int ExecuteScalar(SqlCommand command, string connectionString)
        {
            int num = new int();
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                command.Connection = sqlConnection;
                sqlConnection.Open();
                num = command.ExecuteNonQuery();
            }
            return num;
        }
    }
}