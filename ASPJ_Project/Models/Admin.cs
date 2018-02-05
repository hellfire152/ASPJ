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
        //public string SearchType { get; set; }
    }
    public class BanHistorySearchModel
    {
        public string Search { get; set; }
    }
    
    public class BanUserModel
    {
        public string Username { get; set; }
        public string BanReason { get; set; }
        public string BanPeriod { get; set; }

        public class BannedPeriod
        {
            public int Id { get; set; }
            public String BanPeriod { get; set; }
            public int IntPeriod { get; set; }
        }
        
        public IEnumerable<BannedPeriod> BanPeriodOptions =
             new List<BannedPeriod>
             {
                new BannedPeriod {Id = 0, BanPeriod = "1 Week", IntPeriod = 7 },
                new BannedPeriod {Id = 1, BanPeriod = "2 Weeks", IntPeriod = 14},
                new BannedPeriod {Id = 2, BanPeriod = "1 Month", IntPeriod = 30},
                new BannedPeriod {Id = 3, BanPeriod = "3 Months", IntPeriod = 90},
                new BannedPeriod {Id = 4, BanPeriod = "1 Year", IntPeriod = 365}

             };
        public class BannedReason
        {
            public int Id { get; set; }
            public string Reason { get; set; }
        }
        public IEnumerable<BannedReason> BanReasonOptions =
             new List<BannedReason>
             {
                 new BannedReason{ Id = 0, Reason ="Cheating" },
                 new BannedReason{ Id = 1, Reason ="Suspicious Transaction" },
                 new BannedReason{ Id = 2, Reason ="Bad Forum Behaviour" },
                 new BannedReason{ Id = 3, Reason ="Spamming" },
                 new BannedReason{ Id = 4, Reason ="For lulz" },
             };
    }

    public class BanSearchModel
    {
        public string Username { get; set; }
    }

    public class ChangeRoleModel
    {
        public string Username { get; set; }
        public string NewRole { get; set; }

        public class Roles
        {
            public int Id { get; set; }
            public string Role { get; set; }
        }

        public IEnumerable<Roles> RoleOption =
             new List<Roles>
             {
                 new Roles{ Id = 0, Role ="Admin" },
                 new Roles{ Id = 1, Role ="Moderator" },
                 new Roles{ Id = 2, Role ="Player" }
             };

    }
}