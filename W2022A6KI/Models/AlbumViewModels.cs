using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace W2022A6KI.Models
{
    public class AlbumAddViewModel
    {
        [Required]
        [StringLength(120)]
        [Display(Name = "Album Name")]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Release Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ReleaseDate { get; set; }

        [StringLength(500)]
        [Display(Name = "Album Image (cover art)")]
        public string UrlAlbum { get; set; }

        [Display(Name = "Album's Primary Genre")]
        public string Genre { get; set; }

        [StringLength(10000)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Album Background")]
        public string Background { get; set; }

        [StringLength(254)]
        [Display(Name = "Album's Coordinator")]
        public string Coordinator { get; set; }

        [ScaffoldColumn(false)]
        public IEnumerable<int> ArtistIds { get; set; }

        [ScaffoldColumn(false)]
        public IEnumerable<int> TrackIds { get; set; }

        public AlbumAddViewModel()
        {
            ArtistIds = new List<int>();
            TrackIds = new List<int>();
        }
    }

    public class AlbumBaseViewModel : AlbumAddViewModel
    {
        [Key]
        public int Id { get; set; }
    }

    public class AlbumWithDetailViewModel : AlbumBaseViewModel
    {
        [Display(Name = "Number of Tracks on this album")]
        public int TracksCount { get; set; }

        [Display(Name = "Tracks")]
        public IEnumerable<TrackBaseViewModel> Tracks { get; set; }

        [Display(Name = "Number of Artists on this album")]
        public int ArtistsCount { get; set; }

        [Display(Name = "Artists")]
        public IEnumerable<ArtistBaseViewModel> Artists { get; set; }

        public AlbumWithDetailViewModel()
        {
            Tracks = new List<TrackBaseViewModel>();
            Artists = new List<ArtistBaseViewModel>();
        }
    }

    public class AlbumAddFormViewModel
    {
        [Required]
        [StringLength(120)]
        [Display(Name = "Album Name")]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Release Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ReleaseDate { get; set; }

        [StringLength(500)]
        [Display(Name = "URL to album image (cover art)")]
        public string UrlAlbum { get; set; }

        [Required]
        [Display(Name = "Album's Primary Genre")]
        public SelectList GenreList { get; set; }

        [StringLength(10000)]
        [Display(Name = "Album Background")]
        [DataType(DataType.MultilineText)]
        public string Background { get; set; }

        [Required]
        [Display(Name = "All Artists")]
        public MultiSelectList ArtistList { get; set; }

        [Display(Name = "All Tracks")]
        public MultiSelectList TrackList { get; set; }

        [StringLength(120)]
        [ScaffoldColumn(false)]
        public string ArtistName { get; set; }

        public AlbumAddFormViewModel()
        {
            ReleaseDate = DateTime.Today;
        }
    }
}