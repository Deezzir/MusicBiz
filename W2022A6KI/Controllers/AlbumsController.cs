
using System.Web.Mvc;
using W2022A6KI.Models;

namespace W2022A6KI.Controllers
{
    [Authorize(Roles = "Staff")]
    public class AlbumsController : Controller
    {
        private Manager mn = new Manager();

        // GET: Albums
        public ActionResult Index()
        {
            return View(mn.AlbumGetAll());
        }

        // GET: Albums/Details/5
        public ActionResult Details(int? id)
        {
            var album = mn.AlbumGetByIdWithDetail(id.GetValueOrDefault());
            if (album != null)
            {
                return View(album);
            }
            else
            {
                return HttpNotFound();
            }
        }

        // GET: Albums/5/AddTrack
        [Authorize(Roles = "Clerk")]
        [Route("Albums/{id}/AddTrack", Name = "AddTrack")]
        public ActionResult AddTrack(int? id)
        {
            var track = GetTrackForm(id);
            if (track != null)
            {
                return View("~/Views/Tracks/Create.cshtml", track);
                //return View(track);
            }
            else
            {
                return HttpNotFound();
            }
        }

        // POST: Albums/5/AddTrack
        [HttpPost]
        [Authorize(Roles = "Clerk")]
        [Route("Albums/{id}/AddTrack")]
        public ActionResult AddTrack(int? id, TrackAddViewModel obj)
        {
            var form = GetTrackForm(id);
            if (form == null) return HttpNotFound();

            if (!ModelState.IsValid)
            {
                //return View(obj);
                return View("~/Views/Tracks/Create.cshtml", form);
            }
            try
            {
                var track = mn.TrackAdd(obj);
                if (track == null)
                {
                    return View("~/Views/Tracks/Create.cshtml", form);
                    //return View(obj);
                }
                else
                {
                    return RedirectToAction("Details", "Tracks", new { id = track.Id });
                }
            }
            catch
            {
                return View("~/Views/Tracks/Create.cshtml", form);
            }
        }

        private TrackAddFormViewModel GetTrackForm(int? id)
        {
            var album = mn.AlbumGetById(id.GetValueOrDefault());
            if (album != null)
            {
                var track = new TrackAddFormViewModel();

                track.AlbumName = album.Name;
                track.AlbumId = album.Id;
                track.GenreList = new SelectList(mn.GenreGetAll(), "Name", "GenreName");

                return track;
            }
            else
            {
                return null;
            }
        }
    }
}
