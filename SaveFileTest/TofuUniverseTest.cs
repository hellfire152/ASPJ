﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ASPJ_Project.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Collections.Generic;
using System.Reflection;
using MySql.Data.MySqlClient;

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
        [TestMethod]
        public void UpgradeInitializesProperly()
        {
            string u = System.IO.File.ReadAllText(@"C:\Users\Kuan\Desktop\ASPJ\ASPJ_Project\App_Data\tofu-universe-upgrades.js");
            Upgrade.Initialize(JsonConvert.DeserializeObject<Dictionary<int, dynamic>>(u));
            Upgrade u2 = Upgrade.upgradeData[100];
            Upgrade u3 = Upgrade.upgradeData[1000];

            Assert.AreEqual("0", u2.Effects[0].Benefactor);
            Assert.AreEqual("tps", u2.Effects[0].BenefactorProperty);
            Assert.AreEqual("+", u2.Effects[0].Operator);
            Assert.AreEqual((double)1, u2.Effects[0].Operand);
        }

        [TestMethod]
        public void ItemDataPurchaseCalculationIsAccurate()
        {
            string itemRaw = System.IO.File.ReadAllText(@"C:\Users\Kuan\Desktop\ASPJ\ASPJ_Project\App_Data\tofu-universe-items.js");
            Item.Initialize(JsonConvert.DeserializeObject<Dictionary<int, dynamic>>(itemRaw));
            ItemData i = new ItemData();

            double costCalculated = i.Purchase(1, 17, 20);
            Debug.WriteLine(costCalculated);
            Assert.AreEqual((double)374, costCalculated);
        }

        /*  [TestMethod] 
        public void DatabaseInsert()
        {
            Database d = new Database();
            d.Initialize();

            bool t = d.Insert("INSERT INTO accounts (name) VALUES('hellfire153');");
            Assert.AreEqual(true, t);
        }*/

        [TestMethod]
        public void DatabaseRetrieve()
        {
            //you DO NOT need this line if using in the mainproject,
            //Database is initialized in Global.asax 
            Database.Initialize(@"server=localhost;UID=admin;PWD=adminASPJ;database=dububase;");

            //ONE database object is created for the WebServer,
            //as a static object Database.CurrentInstance
            //you may still use new Database(connString) if you like, but there's not much point
            Database d = Database.CurrentInstance;

            //list to store all results
            List<string> allAccounts = new List<string>();
            try
            {
                if (d.OpenConnection())
                {
                    //table I'm reading from
                    string query = "SELECT * FROM accounts";
                    MySqlCommand c = new MySqlCommand(query, d.conn);

                    //using the 'using' syntax auto-closes the reader
                    using (MySqlDataReader r = c.ExecuteReader())
                    {
                        while (r.Read()) //while the reader has data...
                        {
                            //add the result in the 'name' column to the list
                            allAccounts.Add(r["name"].ToString());
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                Debug.WriteLine("MySQL Error!");
            }
            finally
            {
                d.CloseConnection();
            }


            //to test if the list has all the correct data...
            Assert.AreEqual("hellfire153", allAccounts[0]);
            Assert.AreEqual("hellfire154", allAccounts[1]);
            Assert.AreEqual("hellfire155", allAccounts[2]);
        }
    }
}