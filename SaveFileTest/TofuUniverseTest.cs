using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ASPJ_Project.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Collections.Generic;
using System.Reflection;

namespace SaveFileTest
{
    [TestClass]
    public class TofuUniverseTest
    {
        [TestMethod]
        public void SaveFileParserWorks()
        {
            string saveString = "1516351188332\n{ \"tCount\":3481800.6594187,\"items\":{ \"1\":17,\"2\":3,\"3\":0},\"upgrades\":[100,1000]}";
            SaveFile s = SaveFile.Parse(saveString);
            s.Items.TryGetValue("1", out int t);
            s.Items.TryGetValue("2", out int y);
            Assert.AreEqual(t, 17);
            Assert.AreEqual(y, 3);
            Assert.AreEqual(s.TCount, 3481800.6594187);
            Assert.AreEqual(s.Time, 1516351188332);
            Assert.AreEqual(s.Upgrades[1], 1000);
        }

        [TestMethod]
        public void EffectParserWorks()
        {
            string eff = @"1.tps+0.1";
            Effect e = Effect.Parse(eff);

            Assert.AreEqual("1", e.Benefactor);
            Assert.AreEqual("tps", e.BenefactorProperty);
            Assert.AreEqual("+", e.Operator);
            Assert.AreEqual(0.1, e.Operand);
        }

        [TestMethod]
        public void EffectParserWorks2()
        {
            string eff = @"0.tps+1";
            Effect e = Effect.Parse(eff);

            Assert.AreEqual("0", e.Benefactor);
            Assert.AreEqual("tps", e.BenefactorProperty);
            Assert.AreEqual("+", e.Operator);
            Assert.AreEqual(1, e.Operand);
        }
        /*[TestMethod]
        public void UpgradeInitializesProperly()
        {
            string u = System.IO.File.ReadAllText(@"C:\Users\Kuan\Desktop\ASPJ\ASPJ_Project\App_Data\tofu-universe-upgrades.js");
            Initialize(JsonConvert.DeserializeObject<Dictionary<int, dynamic>>(u));

            Debug.Write(Upgrade.upgradeData);

            Upgrade u2 = Upgrade.upgradeData[100];
            Upgrade u3 = Upgrade.upgradeData[1000];
            Effect e = Effect.Parse("0+1");
            Assert.AreEqual(e.Benefactor, "0");
            Assert.AreEqual(e.BenefactorProperty, "tps");
            Assert.AreEqual(e.Operator, "+");
            Assert.AreEqual(e.Operand, (double)1);
            //Assert.AreEqual(Effect.Parse("1.tps+0.1"), u2.Effects[1]);
            //Assert.AreEqual(Effect.Parse("0+1000"), u3.Effects[0]);
        }*/

        //initializes the upgrade list (contains all upgrade definitions)
        [TestMethod]
        public void Initialize()
        {
            //init dictionary
            string upgradeRaw = System.IO.File.ReadAllText(@"C:\Users\Kuan\Desktop\ASPJ\ASPJ_Project\App_Data\tofu-universe-upgrades.js");
            Dictionary<int, dynamic> upgradeJson = JsonConvert.DeserializeObject<Dictionary<int, dynamic>>(upgradeRaw);
            Dictionary<int, Upgrade> upgradeData = new Dictionary<int, Upgrade>();

            foreach (int upgradeId in upgradeJson.Keys)
            {
                //get upgrade
                dynamic u = upgradeJson[upgradeId];

                string[] effStrArr = ((string)u.effect).Split(",".ToCharArray());
                Effect[] effArr = new Effect[effStrArr.Length];
                for (int i = 0; i < effStrArr.Length; i++)
                {
                    Debug.WriteLine(effStrArr[i]);
                    effArr[i] = Effect.Parse(effStrArr[i].Trim());
                }
                Debug.Write(effArr);
                Upgrade upgrade = new Upgrade(upgradeId, effArr);

                upgradeData.Add(upgradeId, upgrade);
            }
        }

        /*  [TestMethod] 
        public void DatabaseInsert()
        {
            Database d = new Database();
            d.Initialize();

            bool t = d.Insert("INSERT INTO accounts (name) VALUES('hellfire153');");
            Assert.AreEqual(true, t);
        }*/
    }
}
