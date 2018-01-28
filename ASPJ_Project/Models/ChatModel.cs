using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ASPJ_Project.Models
{
    public class ChatModel
    {

        public struct ChatMsg
        {
            public int ChatId;
            public string ChatMessage;
            public object ViewBag { get; }
        }
        

        //public ChatModel(string chatMessage)
        //{
        //    this.ChatMessage = chatMessage;
        //}
    }
}