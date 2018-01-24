using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using ASPJ_Project.Controllers;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace ASPJ_Project.Hubs
{
    public class ChatHub : Hub
    {
        MySql.Data.MySqlClient.MySqlConnection conn;
        MySql.Data.MySqlClient.MySqlCommand cmd;
        MySql.Data.MySqlClient.MySqlDataReader reader;       
        AESCryptoStuff aes_obj = new AESCryptoStuff();
        Censored censorWord = new Censored();
        string wordCensored;
        string queryString;
        string name;

        //Insert message into the database
        private void ChatSendMessage(string chatMessageInsert)
        {
            //MySql.Data.MySqlClient.MySqlConnection conn;
            //MySql.Data.MySqlClient.MySqlCommand cmd;            
            try
            {
                String connString = System.Configuration.ConfigurationManager.ConnectionStrings["mvccruddbEntities"].ConnectionString;
                conn = new MySql.Data.MySqlClient.MySqlConnection(connString);
                conn.Open();

                /*
                 command.Parameters.AddWithValue("@chatId", chatid);
                 if(userid == chatinfo.firstUserID.ToString()){
                    command.Parameters.AddWithValue("@senderUserID", chatinfo.firstuserID);
                    }
                    else{                                           
                    }
                  */
                aes_obj.AesInitialize();
                aes_obj.AesEncrypt(chatMessageInsert);
                queryString = "";
                queryString = "INSERT INTO dububase.chat(chatMessage)" + "VALUES(chatMessageInsert)";                
                cmd = new MySql.Data.MySqlClient.MySqlCommand(queryString, conn);
                cmd.ExecuteNonQuery();
            }
            catch(System.Data.SqlClient.SqlException ex)
            {
                string errorMsg = "Error";
                errorMsg += ex.Message;
                throw new Exception(errorMsg);
            }
            finally
            {
                conn.Close();
            }
        }

        public void ChatGetMessage()
        {
            String connString = System.Configuration.ConfigurationManager.ConnectionStrings["mvccruddbEntites"].ConnectionString;
            conn = new MySql.Data.MySqlClient.MySqlConnection(connString);
            conn.Open();
            AESCryptoStuff aes_obj = new AESCryptoStuff();
            aes_obj.AesInitialize();
            queryString = "";
            queryString = "SELECT chatMessage FROM dububase.chat";
            foreach(var i in queryString)
            {
               queryString = aes_obj.AesDecrypt(i.ToString());
            }
            cmd = new MySql.Data.MySqlClient.MySqlCommand(queryString, conn);
            reader = cmd.ExecuteReader();
            conn.Close();
        }

        public void JoinAGroup(string group)
        {
            Groups.Add(Context.ConnectionId, group);
        }

        public void RemoveFromAGroup(string group)
        {
            Groups.Remove(Context.ConnectionId, group);
        }

        public void BroadcastToGroup(string message, string group)
        {
            Clients.Group(group).newMessageReceived(message);
        }

        //On click send button Signalr connection open insert into database
        public void Send(string name, string message)
        {
            wordCensored = censorWord.CensorStuff(message);
            //ChatSendMessage(message);
            // Call the addNewMessageToPage method to update clients.
            Clients.All.addNewMessageToPage(name, wordCensored);
        }
    }
}