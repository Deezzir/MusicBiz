using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace W2022A6KI.EntityModels
{
    //[Table("ArtistMediaItems")]
    public class ArtistMediaItem
    {
        [Key]
        public int Id { get; set; }

        public DateTime Timestamp { get; set; }

        [Required, StringLength(100)]
        public string StringId { get; set; }

        public byte[] Content { get; set; }

        [StringLength(200)]
        public string ContentType { get; set; }

        [StringLength(255)]
        public string FileName { get; set; }

        [Required, StringLength(100)]
        public string Caption { get; set; }

        [Required]
        public Artist Artist { get; set; }

        public ArtistMediaItem()
        {
            Timestamp = DateTime.Now;

            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            StringId = string.Format("{0:x}", i - DateTime.Now.Ticks);
        }
    }
}