using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab5.Models
{
    public class Subscription
    {
        public int FanId { get; set; }

        public string SportClubId { get; set; }

        public Fan Fans { get; set;  }

        public SportClub SportClubs { get; set; }

    }
}