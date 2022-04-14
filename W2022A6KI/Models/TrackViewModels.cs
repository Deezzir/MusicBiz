using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace W2022A6KI.Models
{
    public class TrackAddViewModel
    {
        [Required]
        [StringLength(200)]
        [Display(Name = "Track Name")]
        public string Name { get; set; }

        [Required]
        [StringLength(220)]
        [Display(Name = "Composer Name(s)")]
        public string Composers { get; set; }

        [Required]
        [Display(Name = "Track Genre")]
        public string Genre { get; set; }

        [Range(1, Int32.MaxValue)]
        [ScaffoldColumn(false)]
        [Display(Name = "Album")]
        public int AlbumId { get; set; }

        [StringLength(254)]
        [Display(Name = "Artist's Clerk")]
        public string Clerk { get; set; }

        [Display(Name = "Sample")]
        public HttpPostedFileBase SampleUpload { get; set; }
    }

    public class TrackBaseViewModel : TrackAddViewModel
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Albums")]
        public IEnumerable<AlbumBaseViewModel> Albums { get; set; }

        public TrackBaseViewModel()
        {
            Albums = new List<AlbumBaseViewModel>();
        }

        public string TrackName
        {
            get { return string.Format("{0}", this.Name); }
        }
    }

    public class TrackWithDetailViewModel : TrackBaseViewModel
    {
        [Display(Name = "Number of Albums")]
        public int AlbumsCount { get; set; }

        [Display(Name = "Number of Artists")]
        public int ArtistsCount { get; set; }

        [Display(Name = "Artists")]
        public IEnumerable<ArtistBaseViewModel> Artists { get; set; }

        public TrackWithDetailViewModel()
        {
            Artists = new List<ArtistBaseViewModel>();
            ArtistsCount = 0;
        }
    }

    public class TrackAddFormViewModel
    {
        [Required]
        [StringLength(200)]
        [Display(Name = "Track Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Track Genre")]
        public SelectList GenreList { get; set; }

        [Required]
        [StringLength(220)]
        [Display(Name = "Composer Names(comma-separated)")]
        public string Composers { get; set; }

        [Range(1, Int32.MaxValue)]
        [ScaffoldColumn(false)]
        public int AlbumId { get; set; }

        [StringLength(160)]
        [ScaffoldColumn(false)]
        public string AlbumName { get; set; }

        [Display(Name = "Sample Clip")]
        [DataType(DataType.Upload)]
        public string SampleUpload { get; set; }
    }

    public class TrackEditViewModel
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Sample")]
        public HttpPostedFileBase SampleUpload { get; set; }
    }

    public class TrackEditFormViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Track Name")]
        public string Name { get; set; }

        [Display(Name = "Sample Clip")]
        [DataType(DataType.Upload)]
        public string SampleUpload { get; set; }
    }

    public class TrackSampleBaseViewModel
    {
        public int Id { get; set; }
        public string SampleType { get; set; }
        public byte[] Sample { get; set; }
    }
}