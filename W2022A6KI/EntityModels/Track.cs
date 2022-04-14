using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace W2022A6KI.EntityModels
{
    //[Table("Tracks")]
    public class Track
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        [StringLength(120)]
        public string Genre { get; set; }

        [Required]
        [StringLength(220)]
        public string Composers { get; set; }

        [Required]
        [StringLength(254)]
        public string Clerk { get; set; }

        [StringLength(200)]
        public string SampleType { get; set; }

        public byte[] Sample { get; set; }

        public ICollection<Album> Albums { get; set; }

        public Track()
        {
            Albums = new HashSet<Album>();
        }
    }
}