using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ASPJ_Project.Models;

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
        public void DatabaseInsert()
        {
            Database d = new Database();
            d.Initialize();

            bool t = d.Insert("INSERT INTO accounts (name) VALUES('hellfire153');");
            Assert.AreEqual(true, t);
        }
    }
}
