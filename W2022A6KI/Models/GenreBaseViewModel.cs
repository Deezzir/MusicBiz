using System.ComponentModel.DataAnnotations;

namespace W2022A6KI.Models
{
    public class GenreBaseViewModel
    {
        [Key]
        public int Id { get; set; }

        [StringLength(120)]
        [Display(Name = "Genre Name")]
        public string Name { get; set; }

        public string GenreName
        {
            get { return string.Format("{0}", this.Name); }
        }
    }
}