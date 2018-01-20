using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace ASPJ_Project.Models
{
    public class Database
    {
        private MySqlConnection conn;

        public void Initialize()
        {
            string connectionString = "SERVER=localhost;DATABASE=dububase;UID=root;PASSWORD='';";
            conn = new MySqlConnection(connectionString);
        }

        public bool OpenConnection()
        {
            try
            {
                conn.Open();
                return true;
            } catch(MySqlException e)
            {
                Debug.WriteLine("MySql Connection Error!");
                Debug.WriteLine(e.StackTrace);
                return false;
            }
        }

        public bool CloseConnection()
        {
            try
            {
                conn.Close();
                return true;
            } catch(MySqlException e)
            {
                Debug.WriteLine("Could not close connection!");
                Debug.WriteLine(e.StackTrace);
                return false;
            }
        }

        public bool Insert(string query)
        {
            if(this.OpenConnection())
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
                return true;
            }
            return false;
        }

        public bool Delete(string query)
        {
            if(this.OpenConnection())
            {
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
                return true;
            }
            return false;
        }
    }
}