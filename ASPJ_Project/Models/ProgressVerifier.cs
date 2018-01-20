using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPJ_Project.Models
{
    public class ProgressVerifier
    {
        public static Boolean VerifyProgress(
            SaveFile save, ProgressData progress, long currentUtcTime)
        {
            //calculate time passed in seconds
            long timePassed = (currentUtcTime - save.Time) / 1000;

            //apply upgrades
            return true;
        }
    }

    //class to hold item tps data during calculation
    class ItemTps
    {
        private object items = new
        {
            I1 = 0.1,
            I2 = 10
        };
        private int Click = 1;

    }
}