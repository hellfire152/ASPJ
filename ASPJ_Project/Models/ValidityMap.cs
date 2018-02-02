using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPJ_Project.Models
{
    public class ValidityMap
    {
        public static ValidityMap CurrentInstance;

        public static void Initialize()
        {
            CurrentInstance = new ValidityMap();
        }

        private Dictionary<string, bool> data;
        public ValidityMap()
        {
            data = new Dictionary<string, bool>();
        }

        public bool Add(string index, bool validity)
        {
            if(data.ContainsKey(index))
            {
                data.Remove(index);
            }
            data.Add(index, validity);
            return true;
        }

        public bool Remove(string index)
        {
            if(data.ContainsKey(index))
            {
                data.Remove(index);
                return true;
            }
            return false;
        }

        public bool Contains(string index)
        {
            return data.ContainsKey(index);
        }

        public bool this[string index]
        {
            get
            {
                if(data.ContainsKey(index))
                {
                    return data[index];
                } else
                {
                    return false;
                }
            }
            set
            {
                if(data.ContainsKey(index))
                {
                    data[index] = value;
                } else
                {
                    data.Add(index, value);
                }
            }
        }
    }
}