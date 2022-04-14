using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace W2022A6KI.EntityModels
{
    //[Table("Artists")]
    public class Artist
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(120)]
        public string Name { get; set; }

        [StringLength(120)]
        public string BirthName { get; set; }

        public DateTime? BirthOrStartDate { get; set; }

        [Required]
        [StringLength(120)]
        public string Genre { get; set; }

        [Required]
        [StringLength(254)]
        public string Executive { get; set; }

        [StringLength(512)]
        public string UrlArtist { get; set; }

        [StringLength(10000)]
        public string Portrayal { get; set; }

        public ICollection<ArtistMediaItem> ArtistMediaItems { get; set; }

        public ICollection<Album> Albums { get; set; }

        public Artist()
        {
            BirthName = "";
            Albums = new HashSet<Album>();
            ArtistMediaItems = new HashSet<ArtistMediaItem>();
            BirthOrStartDate = DateTime.Now.AddYears(-20);
        }
    }
}