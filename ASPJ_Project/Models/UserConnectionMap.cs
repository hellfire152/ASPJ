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
        private Dictionary<string, string> _revConnMap;
        
        public UserConnectionMap()
        {
            _connMap = new Dictionary<string, string>();
            _revConnMap = new Dictionary<string, string>();
        }

        public Boolean Add(string username, string connectionId)
        {
            if(_connMap.ContainsKey(username) || _revConnMap.ContainsKey(connectionId))
            {
                return false;
            }
            else
            {
                _connMap[username] = connectionId;
                _revConnMap[connectionId] = username;
                return true;
            }
        }

        public Boolean Remove(string id)
        {
            if(_connMap.ContainsKey(id))
            {
                _revConnMap.Remove(_connMap[id]);
                _connMap.Remove(id);
                return true;
            }
            else if (_revConnMap.ContainsKey(id))
            {
                _connMap.Remove(_revConnMap[id]);
                _revConnMap.Remove(id);
                return true;
            }
            return false;
        }

        public string ConnectionOf(string username)
        {
            _connMap.TryGetValue(username, out string connId);
            return connId;
        }

        public string UsernameOf(string connectionId)
        {
            _revConnMap.TryGetValue(connectionId, out string username);
            return username;
        }
        
        public string GetAllConnections()
        {
            string output = "";
            foreach(KeyValuePair<string, string> c in _connMap) {
                output += c.Key + " is " + c.Value + '\n';
            }
            return output;
        }
    }
}