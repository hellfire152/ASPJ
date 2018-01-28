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
        public static Database CurrentInstance;
        
        public MySqlConnection conn;

        public static void Initialize(string connString)
        {
            CurrentInstance = new Database(connString);
        }

        public Database(string connString)
        {
            conn = new MySql.Data.MySqlClient.MySqlConnection(connString);
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
    }
}