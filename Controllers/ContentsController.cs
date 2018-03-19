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
    public class ContentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Contents
        public ActionResult Index()
        {
            return View(db.Contents.ToList());
        }

        // GET: Contents/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Content content = db.Contents.Include(x => x.URL.Select(z => z.Group)).Where(y => y.ID == id).FirstOrDefault();
            if (content == null)
            {
                return HttpNotFound();
            }
            return View(content);
        }

        // GET: Contents/Create
        //[Authorize]
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult CreatePersonalizedContent(Guid? id)
        {
            ContentGroupViewModel contentGroupViewModel = new ContentGroupViewModel();
            contentGroupViewModel.Groups = PopulateGroupIDDropDown();
            contentGroupViewModel.ContentID = id ?? default(Guid);
            contentGroupViewModel.Content = db.Contents.Find(id);

            return View(contentGroupViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePersonalizedContent([Bind(Include = "ContentID,Groups,SelectedGroupID,URL,ProtectionPeriod")] ContentGroupViewModel contentGroupViewModel)
        {
            int groupId = Convert.ToInt32(contentGroupViewModel.SelectedGroupID);

            Group group = db.Groups.Find(groupId);

            URL groupURI = new URL()
            {
                Group = group,
                UrlValue = contentGroupViewModel.URL,
                ProtectionPeriod = contentGroupViewModel.ProtectionPeriod
            };

            Content content = db.Contents.Include(x => x.URL).Where(y => y.ID == contentGroupViewModel.ContentID).FirstOrDefault();

            if (ModelState.IsValid)
            {
                content.URL.Add(groupURI);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = contentGroupViewModel.ContentID });
            }

            return View(contentGroupViewModel); ;
        }

        // POST: Contents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,URL,Name")] Content content)
        {
            if (ModelState.IsValid)
            {
                content.ID = Guid.NewGuid();
                db.Contents.Add(content);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(content);
        }

        // GET: Contents/Edit/5
        //[Authorize]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Content content = db.Contents.Find(id);
            if (content == null)
            {
                return HttpNotFound();
            }
            return View(content);
        }

        // POST: Contents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,URL,Name")] Content content)
        {
            if (ModelState.IsValid)
            {
                db.Entry(content).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(content);
        }

        // GET: Contents/Delete/5
        //[Authorize]
        public ActionResult Delete(Guid? id)
        {
            TempData["Deletable"] = true;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Content content = db.Contents.Find(id);
            if (content == null)
            {
                return HttpNotFound();
            }
            return View(content);
        }

        // POST: Contents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            TempData["Deletable"] = true;

            Content content = db.Contents.Include(x => x.URL)
                .Where(y => y.ID == id).FirstOrDefault();

            if (db.Beacons.Where(x => x.Content.ID == id).Any()
                || content.URL.Any())
            {
                TempData["Deletable"] = false;
                return View(content);
            }

            db.Contents.Remove(content);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // Personalized content delete
        public ActionResult DeletePersonalizedContent(int? id, Guid? contentID)
        {
            URL groupURI = db.GroupURIs.Find(id);

            db.GroupURIs.Remove(groupURI);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = contentID });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private List<SelectListItem> PopulateGroupIDDropDown()
        {
            List<SelectListItem> itemList = new List<SelectListItem>();

            foreach (Group item in db.Groups.GroupBy(x => x.Name).Select(x => x.FirstOrDefault()))
            {
                SelectListItem newItem = new SelectListItem()
                {
                    Text = item.Name.ToString(),
                    Value = item.ID.ToString(),
                };
                itemList.Add(newItem);
            }

            return itemList;
        }
    }
}
