using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using W2022A6KI.Models;

namespace W2022A6KI.Controllers
{
    public class TracksController : Controller
    {
        private Manager mn = new Manager();

        // GET: Tracks
        [Authorize(Roles = "Staff")]
        public ActionResult Index()
        {
            return View(mn.TrackGetAll());
        }

        // GET: Tracks/Details/5
        [Authorize(Roles = "Staff")]
        public ActionResult Details(int? id)
        {
            var track = mn.TrackGetByIdWithDetail(id.GetValueOrDefault());
            if (track != null)
            {
                return View(track);
            }
            else
            {
                return HttpNotFound();
            }
        }

        // GET: Track/Edit/5
        [Authorize(Roles = "Clerk")]
        public ActionResult Edit(int? id)
        {
            var toEdit = mn.TrackGetById(id.GetValueOrDefault());
            if (toEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                var form = mn.mapper.Map<TrackBaseViewModel, TrackEditFormViewModel>(toEdit);
                return View(form);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Clerk")]
        public ActionResult Edit(int? id, TrackEditViewModel obj)
        {
            if(!ModelState.IsValid)
            {
                return RedirectToAction("Edit", new { id = obj.Id });
            }

            if(id.GetValueOrDefault() != obj.Id)
            {
                return RedirectToAction("Edit", new { id = obj.Id });
            }

            var edited = mn.TrackEdit(obj);

            if (edited != null) 
            {
                return RedirectToAction("Details", new { id = obj.Id });
            }
            else
            {
                return RedirectToAction("Edit", new { id = obj.Id });
            }
        }

        // GET: Tracks/Delete/5
        [Authorize(Roles = "Clerk")]
        public ActionResult Delete(int? id)
        {
            var track = mn.TrackGetById(id.GetValueOrDefault());
            if(track == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(track);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Clerk")]
        public ActionResult Delete(int? id, FormCollection collection)
        {
            var result = mn.TrackDelete(id.GetValueOrDefault());
            return RedirectToAction("Index");
        }

        [Route("clip/{id}")]
        [Authorize(Roles = "Staff")]
        public ActionResult SampleDetails(int? id)
        {
            var file = mn.TrackSampleGetById(id.GetValueOrDefault());
            if (file == null || file.SampleType == null)
            {
                return HttpNotFound();
            }
            else
            {
                return File(file.Sample, file.SampleType);
            }
        }

    }
}
