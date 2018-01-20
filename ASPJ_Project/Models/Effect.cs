using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ASPJ_Project.Models
{
    public class Effect
    {
        public string Benefactor { get; }
        public string Operator { get; }
        public double Operand { get; }
        public string BenefactorProperty { get; } //currently don't see a use for this

        Effect(string benefactor, string benefactorProperty, string op, double operand)
        {
            this.BenefactorProperty = benefactorProperty;
            this.Benefactor = benefactor;
            this.Operator = op;
            this.Operand = operand;
        }

        //takes in a *single* effect of an upgrade, and returns the representing effect
        public static Effect Parse(string effect)
        {
            //this regex is the reason I can't sleep at night
            Regex iLoveRegexSoMuch = new Regex(@"(\d+)(?:(?:([\+\-\*\/=])((?:\d+(?:\.\d+)?)|\w+))|(?:\.([a-zA-Z]+)(?:([\+\-\*\/=])((?:\d+(?:\.\d+)?)|\w+))))");
            Match betterThanTinder = iLoveRegexSoMuch.Match(effect);

            int i = 0;  string bid = null, bp = null, op = null; double opd = 0;
            GroupCollection m = betterThanTinder.Groups;
            for(int j = 1; j < m.Count; j++)
            {
                if(m[j].Value != null && m[j].Value != "")
                {
                    switch (i)
                    {
                        case 0:
                            bid = m[j].Value;
                            break;
                        case 1:
                            bp = m[j].Value;
                            break;
                        case 2:
                            op = m[j].Value;
                            break;
                        case 3:
                            Double.TryParse(m[j].Value, out opd);
                            break;
                    }
                    i++;
                }
            }
            return new Effect(bid, bp, op, opd);
        }
    }

}