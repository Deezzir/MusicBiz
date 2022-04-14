using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace W2022A6KI.Controllers
{
    public class ArtistMediaItemsController : Controller
    {
        Manager mn = new Manager();

        // GET: ArtistMediaItems
        public ActionResult Index()
        {
            return View("Index", "Home");
        }

        // GET: media/5
        [Route("media/{stringId}")]
        public ActionResult Details(string stringId = "")
        {
            var file = mn.ArtistMediaItemGetById(stringId);
            if (file != null)
            {
                return File(file.Content, file.ContentType);
            }
            return View();
        }

        // GET: media/5/Download
        [Route("media/{stringId}/download")]
        public ActionResult Download(string stringId = "")
        {
            var file = mn.ArtistMediaItemGetById(stringId);
            if(file != null)
            {
                string extension;
                RegistryKey key;
                object value;

                key = Registry.ClassesRoot.OpenSubKey(@"MIME\Database\Content Type\" + file.ContentType, false);
                value = (key == null) ? null : key.GetValue("Extension", null);
                extension = (value == null) ? string.Empty : value.ToString();

                if(string.IsNullOrEmpty(extension))
                {
                    extension = new FileInfo(file.FileName).Extension;
                }

                var cd = new System.Net.Mime.ContentDisposition
                {
                    // Assemble the file name + extension
                    FileName = $"file-{stringId}{extension}",
                    // Force the media item to be saved (not viewed)
                    Inline = false
                };
                // Add the header to the response
                Response.AppendHeader("Content-Disposition", cd.ToString());

                return File(file.Content, file.ContentType);
            }
            else
            {
                return HttpNotFound();
            }
        }
    }
}
