using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace W2022A6KI.EntityModels
{
    //[Table("Artists")]
    public class Album
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(160)]
        public string Name { get; set; }

        public DateTime ReleaseDate { get; set; }

        [Required]
        [StringLength(120)]
        public string Genre { get; set; }

        [Required]
        [StringLength(254)]
        public string Coordinator { get; set; }

        [StringLength(512)]
        public string UrlAlbum { get; set; }

        [StringLength(10000)]
        public string Background { get; set; }

        public ICollection<Artist> Artists { get; set; }

        public ICollection<Track> Tracks { get; set; }

        public Album()
        {
            Artists = new HashSet<Artist>();
            Tracks = new HashSet<Track>();
            ReleaseDate = DateTime.Today;
        }
    }
}