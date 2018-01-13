﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Numerics;
using System.Collections;

namespace ASPJ_Project.Models
{
    public class Player
    {
        public BigInteger tofu;
        public BigInteger tps;
        public Dictionary<int, int> items;
        public Dictionary<string, List<Upgrade>> upgrades;

        //Creates a new player object, from scratch
        public Player()
        {

        }

        //for creating a player object from a save file
        public Player(BigInteger tofu, BigInteger beans,
            BigInteger tps, int[] items, List<Upgrade> upgrades)
        {

        }
    }
}