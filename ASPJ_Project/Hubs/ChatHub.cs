using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using ASPJ_Project.Controllers;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using ASPJ_Project.Models;

namespace ASPJ_Project.Hubs
{
    public class ChatHub : Hub
    {
        //Initialize db object
        DatabaseStuff db = new DatabaseStuff();

        public void JoinAGroup(string group)
        {
            Groups.Add(Context.ConnectionId, group);
            //Join group message
            //Clients.Group(roomName).addChatMessage(Context.User.Identity.Name + " joined.");
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
        public void Send(string getCookie, string message, string storeTime)
        {
            //Get value from cookie
            getCookie = AESCryptoStuff.CurrentInstance.AesDecrypt(HttpUtility.UrlDecode(Context.RequestCookies["UserID"].Value));
            //Debug.WriteLine(storeCookie);
            //Insert into db
            db.ChatSendMessage(message);
            foreach(var g in db.ChatGetTime())
            {
                storeTime = g;
            }
            //Censor Word
            Censors censorMessage = new Censors();
            string storeCensored = censorMessage.CrapCensor(message);
            // Call the addNewMessageToPage method to update clients.
            Clients.All.addNewMessageToPage(getCookie, storeCensored, storeTime);
        }
    }
}