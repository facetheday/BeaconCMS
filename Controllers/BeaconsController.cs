using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using CMS.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;

namespace CMS.Controllers
{
    public class BeaconsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Beacons
        public ActionResult Index(string sortOrder, string searchByBeaconId, string searchByMajor)
        {
            ViewBag.majorIdParm = sortOrder == "major" ? "major_desc" : "major";
            ViewBag.minorIdParm = sortOrder == "minor" ? "minor_desc" : "minor";
            ViewBag.contentParm = sortOrder == "content" ? "content_desc" : "content";
            ViewBag.notificationParm = sortOrder == "notification" ? "notification_desc" : "notification";
            
            var beacons = db.Beacons.Include(x => x.Notification).Include(y => y.Content);

            if (!String.IsNullOrEmpty(searchByBeaconId))
                beacons = beacons.Where(x => x.BeaconID.Contains(searchByBeaconId));
            if (!String.IsNullOrEmpty(searchByMajor))
            {
                if (Int32.TryParse(searchByMajor, out int searchMajor))
                    beacons = beacons.Where(x => x.Major == searchMajor);
            }

            switch (sortOrder)
            {
                case "major":
                    beacons = beacons.OrderBy(x => x.Major);
                    break;
                case "major_desc":
                    beacons = beacons.OrderByDescending(x => x.Major);
                    break;
                case "minor":
                    beacons = beacons.OrderBy(x => x.Minor);
                    break;
                case "minor_desc":
                    beacons = beacons.OrderByDescending(x => x.Minor);
                    break;
                case "content":
                    beacons = beacons.OrderBy(x => x.Content.Name);
                    break;
                case "content_desc":
                    beacons = beacons.OrderByDescending(x => x.Content.Name);
                    break;
                case "notification":
                    beacons = beacons.OrderBy(x => x.Notification.Title);
                    break;
                case "notification_desc":
                    beacons = beacons.OrderByDescending(x => x.Notification.Title);
                    break;
                default:
                    beacons = beacons.OrderByDescending(x => x.Content.Name);
                    break;
            }

            return View(beacons.ToList());
        }

        // GET: Beacons/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Beacon beacon = db.Beacons.Include(x => x.Notification).Include(y => y.Content).Single(z => z.Id == id);

            if (beacon == null)
            {
                return HttpNotFound();
            }
            return View(beacon);
        }

        // GET: Beacons/Edit/5
        //[Authorize]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Beacon beacon = db.Beacons.Find(id);
            ContentsViewModel contentsViewModel = new ContentsViewModel();
            contentsViewModel.Contents = PopulateContentDropDown();
            contentsViewModel.Notifications = PopulateNotificationDropDown();
            
            if (beacon.Notification != null)
                contentsViewModel.SelectedNotificationID = beacon.Notification.ID.ToString();

            if (beacon.Content != null)
                contentsViewModel.SelectedContentID = beacon.Content.ID.ToString();

            contentsViewModel.BeaconID = beacon.Id;

            if (contentsViewModel == null)
            {
                return HttpNotFound();
            }
            return View(contentsViewModel);
        }

        // POST: Beacons/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BeaconID,SelectedContentID,SelectedNotificationID")] ContentsViewModel contentsViewModel)
        {
            int NotificationId = Convert.ToInt32(contentsViewModel.SelectedNotificationID);
            contentsViewModel.Contents = PopulateContentDropDown();
            contentsViewModel.Notifications = PopulateNotificationDropDown();
            Beacon beacon = db.Beacons.Find(contentsViewModel.BeaconID);
            Content content = db.Contents.Find(Guid.Parse(contentsViewModel.SelectedContentID));
            Notification notification = db.Notifications.Find(NotificationId);
            beacon.Content = content;
            beacon.Notification = notification;

            if (ModelState.IsValid)
            {
                db.Entry(beacon).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contentsViewModel);
        }

        // GET: Beacons/Delete/5
        //[Authorize]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Beacon beacon = db.Beacons.Find(id);
            if (beacon == null)
            {
                return HttpNotFound();
            }
            return View(beacon);
        }

        // POST: Beacons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Beacon beacon = db.Beacons.Find(id);
            db.Beacons.Remove(beacon);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        string Baseurl = "https://api.kontakt.io/";
        /// <summary>
        /// Imports the beacons.
        /// </summary>
        /// <returns></returns>
        public async System.Threading.Tasks.Task<ActionResult> ImportBeacons()
        {
            using (var client = new HttpClient())
            {
                // Base URL
                client.BaseAddress = new Uri(Baseurl);
                // Headers
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Api-Key", "rFOXRuKjNYQYXlhxGNuAbYBvMUcuXyLw");
                client.DefaultRequestHeaders.Add("Accept", "application/vnd.com.kontakt+json;version=10");
                // Get resource 
                HttpResponseMessage res = await client.GetAsync("beacon?proximity=f7826da6-4fa2-4e98-8024-bc5b71e0893e&major=60919&published=true");

                if (res.IsSuccessStatusCode)
                {  
                    var response = res.Content.ReadAsStringAsync().Result;
                    dynamic data = JsonConvert.DeserializeObject(response);

                    for (int i = 0; i < data.beacons.Count; i++)
                    {
                        dynamic item = data.beacons[i];
                        Guid Uuid = item.id;

                        if (!db.Beacons.Where(x => x.Id == Uuid).Any())
                        {
                            Beacon newBeacon = new Beacon();
                            newBeacon.Id = item.id;
                            newBeacon.BeaconID = item.uniqueId;
                            newBeacon.Major = item.major;
                            newBeacon.Minor = item.minor;
                            db.Beacons.Add(newBeacon);
                        }
                        else
                        {
                            Beacon updateBeacon = db.Beacons.Find(Uuid);
                            updateBeacon.BeaconID = item.uniqueId;
                            updateBeacon.Major = item.major;
                            updateBeacon.Minor = item.minor;
                            db.Entry(updateBeacon).State = EntityState.Modified;
                        }
                    }
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Populates the content drop down.
        /// </summary>
        /// <returns></returns>
        private List<SelectListItem> PopulateContentDropDown()
        {
            List<SelectListItem> itemList = new List<SelectListItem>();

            foreach (Content item in db.Contents)
            {
                SelectListItem newItem = new SelectListItem()
                {
                    Text = item.Name,
                    Value = item.ID.ToString(),
                };
                itemList.Add(newItem);
            }

            return itemList;
        }

        /// <summary>
        /// Populates the notification drop down.
        /// </summary>
        /// <returns></returns>
        private List<SelectListItem> PopulateNotificationDropDown()
        {
            List<SelectListItem> itemList = new List<SelectListItem>();

            foreach (Notification item in db.Notifications)
            {
                SelectListItem newItem = new SelectListItem()
                {
                    Text = item.Title,
                    Value = item.ID.ToString(),
                };
                itemList.Add(newItem);
            }

            return itemList;
        }
    }
}
