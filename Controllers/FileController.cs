using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.Azure;
using CMS.Models;
using System.Net;
using System.Data.Entity;

namespace CMS.Controllers
{
    public class FileController : Controller
    {
        private CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
            CloudConfigurationManager.GetSetting("StorageConnectionString"));
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: File
        public ActionResult Index()
        {
            return View(db.BlobContents.ToList());
        }

        [HttpPost]
        public ActionResult Upload(string contentName)
        {
            {
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference("contentcontainer");

                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];

                    if (file != null && file.ContentLength > 0)
                    {
                        CloudBlockBlob blockBlob = container.GetBlockBlobReference(file.FileName);
                        blockBlob.UploadFromStream(file.InputStream);

                        BlobContent bcontent = new BlobContent
                        {
                            Name = contentName,
                            URL = blockBlob.Uri.ToString(),
                            ContainerName = container.Name,
                            CreateDate = DateTime.Now,
                            FileName = blockBlob.Name
                        };

                        db.BlobContents.Add(bcontent);
                        db.SaveChanges();
                    }
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BlobContent blob = db.BlobContents.Find(id);
            if (blob == null)
            {
                return HttpNotFound();
            }
            return View(blob);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BlobContent blob = db.BlobContents.Find(id);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("contentcontainer");
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blob.FileName);

            blockBlob.DeleteIfExists();

            db.BlobContents.Remove(blob);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}