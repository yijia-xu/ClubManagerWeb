using System.ComponentModel.DataAnnotations;

namespace Lab5.Models
{
    public class News
    {
        [Key]
        public int NewsId { get; set; }
        [StringLength(100)]
        [Display(Name = "File Name")]
        public string FileName { get; set;}

        [Url]
        [Display(Name = "Url")]
        public string Url { get; set;}

        public string SportClubId { get; set; }
        public virtual SportClub SportClub { get; set; }
    }
}
