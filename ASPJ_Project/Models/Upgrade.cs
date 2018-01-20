using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ASPJ_Project.Models
{
    //holds all upgrade data
    public class Upgrade
    {
        //holds all upgrade data
        public static Dictionary<int, Upgrade> upgradeData;

        int Id;
        Effect[] Effects;
        public Upgrade(int id, params Effect[] effects)
        {
            this.Id = id;
            this.Effects = effects;
        }

        //initializes the upgrade list (contains all upgrade definitions)
        public static void Initialize(dynamic upgradeJson)
        {
            //init dictionary
            upgradeData = new Dictionary<int, Upgrade>();

            //iterate over all upgrade ids
            PropertyInfo[] properties = typeof(Upgrade).GetProperties();
            foreach(PropertyInfo upgradeId in properties)
            {
                //get upgrade
                dynamic u = upgradeJson[upgradeId.Name];

                string[] effStrArr = u.effect.Split(",");
                Effect[] effArr = new Effect[effStrArr.Length];
                for(int i = 0; i < effStrArr.Length; i++)
                {
                    effArr[i] = Effect.Parse(effStrArr[i]);
                }
                int.TryParse(upgradeId.Name, out int id);
                Upgrade upgrade = new Upgrade(id, effArr);

                upgradeData.Add(id, upgrade);
            }
        }
    }
}