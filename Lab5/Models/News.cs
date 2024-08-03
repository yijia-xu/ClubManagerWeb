using System.ComponentModel.DataAnnotations;

namespace Lab5.Models
{
    public enum Question
    {
        Earth, Computer
    }

    public class News
    {
        public int NewsId { get; set; }
        [Required]
        [StringLength(100)]
        [Display(Name = "File Name")]
        public string FileName { get; set;}

        [Display(Name = "Question")]
        public Question Question { get; set; }

        [Required]
        [Url]
        [Display(Name = "Url")]
        public string Url { get; set;}

        public string SportClubId { get; set; }
        public virtual SportClub SportClub { get; set; }
    }
}
