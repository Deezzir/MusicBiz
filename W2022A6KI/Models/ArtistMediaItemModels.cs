using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace W2022A6KI.Models
{
    public class ArtistMediaItemAddViewModel
    {
        [Range(1, Int32.MaxValue)]
        [ScaffoldColumn(false)]
        public int ArtistId { get; set; }

        [Required, StringLength(100)]
        [Display(Name = "Description")]
        public string Caption { get; set; }

        [Required]
        [Display(Name = "Media Item")]
        public HttpPostedFileBase ItemUpload { get; set; }
    }

    public class ArtistMediaItemBaseViewModel : ArtistMediaItemAddViewModel
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Added on date/time")]
        public DateTime Timestamp { get; set; }

        [Required, StringLength(100)]
        [Display(Name = "Unique identifier")]
        public string StringId { get; set; }

        [StringLength(200)]
        [ScaffoldColumn(false)]
        public string ContentType { get; set; }
    }

    public class ArtistMediaItemAddFormViewModel
    {
        [Range(1, Int32.MaxValue)]
        [ScaffoldColumn(false)]
        public int ArtistId { get; set; }

        public string ArtistName { get; set; }

        [Required, StringLength(100)]
        [Display(Name = "Description")]
        public string Caption { get; set; }

        [Required]
        [Display(Name = "Media Item")]
        [DataType(DataType.Upload)]
        public string ItemUpload { get; set; }
    }

    public class ArtistMediaItemContentViewModel
    {
        public int Id { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
        public string FileName { get; set; }
    }
}