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
        //Initialize db object
        DatabaseStuff db = new DatabaseStuff();
        EncodeDecode encInit = new EncodeDecode();

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
        public void Send(string name, string message)
        {
            //Insert into db
            db.ChatSendMessage(message);
            //Censor Word
            Censors censorMessage = new Censors();
            string storeCensored = censorMessage.CrapCensor(message);
            // Call the addNewMessageToPage method to update clients.
            Clients.All.addNewMessageToPage(name, storeCensored);
        }
    }
}