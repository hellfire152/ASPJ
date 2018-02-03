using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASPJ_Project.Models
{
    public class SearchModel
    {
        public string Search { get; set; }
        public string SearchType { get; set; }
    }
    
    public class BanUserModel
    {
        public string Username { get; set; }
        public string BanReason { get; set; }
        public int BanPeriod { get; set; }
    }


    public enum BannedReason
    {
        Cheating,
        Spamming,
        SuspiciousTransaction,
        BadForumBehaviour
    }

    public enum Period
    {
       OneWeek = 7,
        TwoWeeks = 14,
         Month = 30,
         ThreeMonth = 90,
         OneYear = 365


    }

    public class BanSearchModel
    {
        public string Username { get; set; }
    }

    public class ChangeRoleModel
    {
        public string Username { get; set; }
        public string NewRole { get; set; }
    }
}