using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CMS.Models;

namespace CMS.Controllers
{
    public class ConferencesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Conferences
        public ActionResult Index()
        {
            return View(db.Conferences.ToList());
        }

        // GET: Conferences/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Conference conference = db.Conferences.Find(id);
            if (conference == null)
            {
                return HttpNotFound();
            }
            return View(conference);
        }

        // GET: Conferences/Create
        public ActionResult Create()
        {
            TempData["DuplicateMajorID"] = false;

            ConfereneceBeaconViewModel confereneceBeaconViewModel = new ConfereneceBeaconViewModel();
            confereneceBeaconViewModel.Beacons = PopulateMajorIDDropDown();

            return View(confereneceBeaconViewModel);
        }

        // POST: Conferences/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ConferenceID,Conference,Beacons,SelectedMajorID")] ConfereneceBeaconViewModel confereneceBeaconViewModel)
        {
            TempData["DuplicateMajorID"] = false;

            Conference conference = new Conference();
            conference.MajorID = Convert.ToInt32(confereneceBeaconViewModel.SelectedMajorID);
            conference.Name = confereneceBeaconViewModel.Conference.Name;

            if (ModelState.IsValid)
            {
                if (!db.Conferences.Where(x => x.MajorID == conference.MajorID).Any())
                {
                    db.Conferences.Add(conference);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["DuplicateMajorID"] = true;
                    confereneceBeaconViewModel.Beacons = PopulateMajorIDDropDown();
                }
            }

            return View(confereneceBeaconViewModel); ;
        }

        // GET: Conferences/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Conference conference = db.Conferences.Find(id);

            ConfereneceBeaconViewModel confereneceBeaconViewModel = new ConfereneceBeaconViewModel();
            confereneceBeaconViewModel.ConferenceID = conference.ID;
            confereneceBeaconViewModel.Conference = conference;
            confereneceBeaconViewModel.Beacons = PopulateMajorIDDropDown();

            if (confereneceBeaconViewModel.SelectedMajorID != null)
                confereneceBeaconViewModel.SelectedMajorID = conference.MajorID.ToString();

            return View(confereneceBeaconViewModel);
        }

        // POST: Conferences/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ConferenceID,Conference,Beacons,SelectedMajorID")] ConfereneceBeaconViewModel confereneceBeaconViewModel)
        {
            confereneceBeaconViewModel.Beacons = PopulateMajorIDDropDown();
            int majorID = Convert.ToInt32(confereneceBeaconViewModel.SelectedMajorID);
            Conference conference = db.Conferences.Find(confereneceBeaconViewModel.ConferenceID);
            conference.MajorID = majorID;

            if (ModelState.IsValid)
            {
                if (!db.Conferences.Where(x => x.MajorID == conference.MajorID).Any())
                {
                    db.Entry(conference).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(confereneceBeaconViewModel);
        }

        // GET: Conferences/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Conference conference = db.Conferences.Find(id);
            if (conference == null)
            {
                return HttpNotFound();
            }
            return View(conference);
        }

        // POST: Conferences/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Conference conference = db.Conferences.Find(id);
            db.Conferences.Remove(conference);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Populates the major identifier drop down.
        /// </summary>
        /// <returns></returns>
        private List<SelectListItem> PopulateMajorIDDropDown()
        {
            List<SelectListItem> itemList = new List<SelectListItem>();

            foreach (Beacon item in db.Beacons.GroupBy(x => x.Major).Select(x => x.FirstOrDefault()))
            {
                SelectListItem newItem = new SelectListItem()
                {
                    Text = item.Major.ToString(),
                    Value = item.Major.ToString(),
                };
                itemList.Add(newItem);
            }

            return itemList;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
