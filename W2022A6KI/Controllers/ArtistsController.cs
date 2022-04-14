
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using W2022A6KI.Models;

namespace W2022A6KI.Controllers
{
    public class ArtistsController : Controller
    {
        private Manager mn = new Manager();

        // GET: Artists
        [Authorize(Roles = "Staff")]
        public ActionResult Index()
        {
            return View(mn.ArtistGetAll());
        }

        // GET: Artists/Details/5
        [Authorize(Roles = "Staff")]
        public ActionResult Details(int? id)
        {
            var artist = mn.ArtistGetByIdWithDetail(id.GetValueOrDefault());
            if (artist != null)
            {
                return View(artist);
            }
            else
            {
                return HttpNotFound();
            }
        }

        // GET: Artists/Create
        [Authorize(Roles = "Executive")]
        public ActionResult Create()
        {
            return View(GetArtistForm());
        }

        // POST: Artists/Create
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "Executive")]
        public ActionResult Create(ArtistAddViewModel obj)
        {
            var form = GetArtistForm();
            if (!ModelState.IsValid)
            {
                return View(form);
            }

            try
            {
                var artist = mn.ArtistAdd(obj);
                if (artist == null)
                {
                    return View(form);
                }
                else
                {
                    return RedirectToAction("Details", new { id = artist.Id });
                }
            }
            catch
            {
                return View(form);
            }
        }

        // GET: Artists/5/AddAlbum
        [Authorize(Roles = "Coordinator")]
        [Route("Artists/{id}/AddAlbum", Name = "AddAlbum")]
        public ActionResult AddAlbum(int? id)
        {
            var form = GetAlbumForm(id, new List<int>());
            if (form != null)
            {
                return View("~/Views/Albums/Create.cshtml", form);
                //return View(album);
            }
            else
            {
                return HttpNotFound();
            }
        }

        // POST: Artists/5/AddAlbum
        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "Coordinator")]
        [Route("Artists/{id}/AddAlbum")]
        public ActionResult AddAlbum(int? id, AlbumAddViewModel obj)
        {
            var form = GetAlbumForm(id, (List<int>)obj.TrackIds);
            if (form == null) return HttpNotFound();

            if (!ModelState.IsValid || obj.ArtistIds.Count() < 1)
            {
                ModelState.AddModelError(nameof(form.ArtistList), "At least one Artist must be selected");
                return View("~/Views/Albums/Create.cshtml", form);
                //return View(obj);
            }

            try
            {
                var album = mn.AlbumAdd(obj);
                if (album == null)
                {
                    return View("~/Views/Albums/Create.cshtml", form);
                    //return View(obj);
                }
                else
                {
                    return RedirectToAction("Details", "Albums", new { id = album.Id });
                }
            }
            catch
            {
                return View("~/Views/Albums/Create.cshtml", form);
            }
        }

        [Authorize(Roles = "Coordinator")]
        [Route("Artists/{id}/AddMediaItem", Name = "AddMediaItem")]
        public ActionResult AddMediaItem(int? id)
        {
            var form = GetMediaItemForm(id);
            if(form != null)
            {
                return View("~/Views/ArtistMediaItems/Create.cshtml", form);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        [Authorize(Roles = "Coordinator")]
        [Route("Artists/{id}/AddMediaItem")]
        public ActionResult AddMediaItem(int? id, ArtistMediaItemAddViewModel obj)
        {
            var form = GetMediaItemForm(id);
            if (form == null) return HttpNotFound();

            if (!ModelState.IsValid || id.GetValueOrDefault() != obj.ArtistId)
            {
                return View("~/Views/ArtistMediaItems/Create.cshtml", form);
            }

            var item = mn.ArtistMediaItemAdd(obj);

            if(item == null)
            {
                return View("~/Views/ArtistMediaItems/Create.cshtml", form);
            }
            else
            {
                return RedirectToAction("Details", new { id = obj.ArtistId });
            }
        }

        private AlbumAddFormViewModel GetAlbumForm(int? id, List<int> tracks)
        {
            var artist = mn.ArtistGetById(id.GetValueOrDefault());
            if (artist != null)
            {
                var album = new AlbumAddFormViewModel();

                album.ArtistName = artist.Name;
                album.GenreList = new SelectList(mn.GenreGetAll(), "Name", "GenreName");
                album.ArtistList = new MultiSelectList(
                    items: mn.ArtistGetAll(),
                    dataValueField: "Id",
                    dataTextField: "ArtistName",
                    selectedValues: new List<int> { artist.Id }
                );
                album.TrackList = new MultiSelectList(
                    items: mn.TrackGetAllByArtistId(artist.Id),
                    dataValueField: "Id",
                    dataTextField: "TrackName",
                    selectedValues: tracks
                );
                return album;
            }
            else
            {
                return null;
            }
        }

        private ArtistAddFormViewModel GetArtistForm()
        {
            var artist = new ArtistAddFormViewModel();
            artist.GenreList = new SelectList(mn.GenreGetAll(), "Name", "GenreName");
            return artist;
        }

        private ArtistMediaItemAddFormViewModel GetMediaItemForm(int? id)
        {
            var artist = mn.ArtistGetById(id.GetValueOrDefault());
            if(artist != null)
            {
                var mediaItem = new ArtistMediaItemAddFormViewModel();

                mediaItem.ArtistName = artist.Name;
                mediaItem.ArtistId = artist.Id;

                return mediaItem;
            }
            else
            {
                return null;
            }
        }
    }
}
