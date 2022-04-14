using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace W2022A6KI.Models
{
    public class ArtistAddViewModel
    {
        [Required]
        [StringLength(120)]
        [Display(Name = "Artist or Stage Name")]
        public string Name { get; set; }

        [StringLength(120)]
        [Display(Name = "Artist's Birth Name")]
        public string BirthName { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Birth or Start Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? BirthOrStartDate { get; set; }

        [StringLength(500)]
        [Display(Name = "Artist's Photo")]
        public string UrlArtist { get; set; }

        [StringLength(10000)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Artist's Portrayal")]
        public string Portrayal { get; set; }

        [Required]
        [Display(Name = "Artist's Primary Genre")]
        public string Genre { get; set; }

        [StringLength(254)]
        [Display(Name = "Artist's Executive")]
        public string Executive { get; set; }
    }

    public class ArtistBaseViewModel : ArtistAddViewModel
    {
        [Key]
        public int Id { get; set; }

        public string ArtistName
        {
            get { return string.Format("{0}", this.Name); }
        }
    }

    public class ArtistWithDetailViewModel : ArtistBaseViewModel
    {
        [Display(Name = "Number of Albums")]
        public int AlbumsCount { get; set; }

        [Display(Name = "Albums")]
        public IEnumerable<AlbumBaseViewModel> Albums { get; set; }

        public IEnumerable<ArtistMediaItemBaseViewModel> ArtistMediaItems { get; set; }

        public ArtistWithDetailViewModel()
        {
            ArtistMediaItems = new List<ArtistMediaItemBaseViewModel>();
            Albums = new List<AlbumBaseViewModel>();
        }
    }

    public class ArtistAddFormViewModel
    {
        [Required]
        [StringLength(120)]
        [Display(Name = "Artist/Stage Name")]
        public string Name { get; set; }

        [StringLength(120)]
        [Display(Name = "Artist's Birth Name, if applicable")]
        public string BirthName { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Birth/Start Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? BirthOrStartDate { get; set; }

        [StringLength(500)]
        [Display(Name = "URL to artist's photo")]
        public string UrlArtist { get; set; }

        [Required]
        [Display(Name = "Artist's Primary Genre")]
        public SelectList GenreList { get; set; }

        [StringLength(10000)]
        [Display(Name = "Artist Portrayal")]
        [DataType(DataType.MultilineText)]
        public string Portrayal { get; set; }

        public ArtistAddFormViewModel()
        {
            BirthOrStartDate = DateTime.Today;
        }
    }
}