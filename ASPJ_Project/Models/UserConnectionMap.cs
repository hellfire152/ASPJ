using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPJ_Project.Models
{
    public class UserConnectionMap
    {
        public static UserConnectionMap CurrentInstance { get; set; }

        private Dictionary<string, string> _connMap;
        
        public UserConnectionMap()
        {
            _connMap = new Dictionary<string, string>;
        }

        public Boolean Add(string username, string connectionId)
        {
            if(_connMap.ContainsKey(username))
            {
                return false;
            }
            else
            {
                _connMap[username] = connectionId;
                return true;
            }
        }

        public Boolean Remove(string username)
        {
            if(_connMap.ContainsKey(username))
            {
                _connMap.Remove(username);
                return true;
            }
            return false;
        }

        public string ConnectionOf(string username)
        {
            string connId;
            _connMap.TryGetValue(username, out connId);
            return connId;
        }

        public string UsernameOf(string connectionId)
        {
            return _connMap.FirstOrDefault(x => x.Value == connectionId).Key;
        }
    }
}