using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ASPJ_Project.Models
{
    //holds all upgrade data
    public class Upgrade
    {
        //holds all upgrade data
        public static Dictionary<int, Upgrade> upgradeData = new Dictionary<int, Upgrade>();

        public int Id;
        public Effect[] Effects;
        public decimal cost;
        public Upgrade(int id, decimal cost, params Effect[] effects)
        {
            this.Id = id;
            this.Effects = effects;
        }

        //initializes the upgrade list (contains all upgrade definitions)
        public static void Initialize(Dictionary<int, dynamic> upgradeJson)
        {
            foreach(int upgradeId in upgradeJson.Keys)
            {
                //get upgrade
                dynamic u = upgradeJson[upgradeId];

                string[] effStrArr = ((string)u.effect).Split(",".ToCharArray());
                Effect[] effArr = new Effect[effStrArr.Length];
                for(int i = 0; i < effStrArr.Length; i++)
                {
                    effArr[i] = Effect.Parse(effStrArr[i].Trim());
                }
                
                Upgrade upgrade = new Upgrade(upgradeId, (decimal)u.cost, effArr);

                upgradeData.Add(upgradeId, upgrade);
            }
        }
    }
}