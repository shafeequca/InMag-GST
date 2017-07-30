using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
namespace InMag_V._16
{
    public sealed class Connections
    {
        private static Connections instance = null;
        string ConnectionString = @"Data Source=" + System.Configuration.ConfigurationSettings.AppSettings["Server"] + ";Initial Catalog=Inventory;Integrated Security=True;";

        //string ConnectionString = @"Data Source=USER-PC;Initial Catalog=Inventory;Integrated Security=True;";
        public SqlConnection con;

        public static Connections Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Connections();
                }
                return instance;
            }
        }
        public void OpenConection()
        {
            con = new SqlConnection(ConnectionString);
            con.Open();
        }


        public void CloseConnection()
        {
            con.Close();
        }


        public void ExecuteQueries(string Query_)
        {
            SqlCommand cmd = new SqlCommand(Query_, con);
            cmd.ExecuteNonQuery();
        }


        public SqlDataReader DataReader(string Query_)
        {
            SqlCommand cmd = new SqlCommand(Query_, con);
            SqlDataReader dr = cmd.ExecuteReader();
            return dr;
        }


        public object ShowDataInGridView(string Query_)
        {
            SqlDataAdapter dr = new SqlDataAdapter(Query_, ConnectionString);
            System.Data.DataSet ds = new System.Data.DataSet();
            dr.Fill(ds);
            object dataum = ds.Tables[0];
            return dataum;
        }
    }
}
