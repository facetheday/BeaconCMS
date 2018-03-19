using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using CMS.Models;
using CMS.Code;
using System.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CMS
{
    public class BeaconController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private int _timeout = Convert.ToInt32(ConfigurationManager.AppSettings["DisplayTimeout"]);

        // GET api/<controller>
        [HttpGet]
        public IEnumerable<Beacon> GetList()
        {
            var beacons = db.Beacons.Include(x => x.Content.URL.Select(z => z.Group)).Include(y => y.Notification);
            return beacons;
        }

        /// <summary>
        /// Gets the by major identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<BeaconResult> GetByMajorID(int id)
        {
            var beacons = db.Beacons.Include(x => x.Content).Include(y => y.Notification).Where(x => x.Major == id);

            var result = db.Beacons.Select(b => new BeaconResult
            {
                Id = b.Id,
                Url = b.Content.URL.Select(x => x.UrlValue).FirstOrDefault(),
                Notification = b.Notification,
                BeaconID = b.BeaconID,
                Major = b.Major,
                Minor = b.Minor,
                DisplayTimeout = _timeout
            }).Where(x => x.Major == id);

            return result;
        }

        // GET api/<controller>/5
        private IQueryable<BeaconResult> GetById(Guid id)
        {
            URL urlToAssign = UrlSelection.SelectUrl(id);

            var result = db.Beacons.Select(b => new BeaconResult
            {
                Id = b.Id,
                Url = urlToAssign.UrlValue,
                Notification = b.Notification,
                BeaconID = b.BeaconID,
                Major = b.Major,
                Minor = b.Minor,
                DisplayTimeout = _timeout
            }).Where(x => x.Id == id);

            Log logRecord = new Log
            {
                Beacon_Id = id,
                RequestDate = DateTime.Now,
                Url_Id = urlToAssign.Id
            };

            db.Log.Add(logRecord);
            db.SaveChanges();

            return result;
        }

        /// <summary>
        /// Gets the by by user TODO!
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<BeaconResult> Get([FromUri] Guid id, [FromUri] string username = "", [FromUri] string password = "", [FromUri] string deviceId = "")
        {
            // Beacon validation
            if (!db.Beacons.Where(x => x.Id == id).Any())
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("No Beacon found")
                });
            }

            // If personal parameters empty, call GetById method
            if ((string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) && string.IsNullOrEmpty(deviceId))
                return GetById(id);

            User user = db.User.Where(x => x.UserName == username && x.Password == password || x.DeviceId == deviceId)
                .Include(y => y.Group).FirstOrDefault();

            // User validation
            if (user == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("No User found")
                });
            }

            // Url assignment
            URL urlToAssign = UrlSelection.SelectUrl(user, id);
            
            var result = db.Beacons.Select(b => new BeaconResult
            {
                Id = b.Id,
                Url = urlToAssign.UrlValue,
                Notification = b.Notification,
                Group = user.Group.Name,
                BeaconID = b.BeaconID,
                Major = b.Major,
                Minor = b.Minor,
                DisplayTimeout = _timeout,
                DeviceId = user.DeviceId
            }).Where(x => x.Id == id);

            Log logRecord = new Log
            {
                Beacon_Id = id,
                User_Id = user.Id,
                Group_Id = user.Group.ID,
                RequestDate = DateTime.Now,
                Url_Id = urlToAssign.Id
            };

            db.Log.Add(logRecord);
            db.SaveChanges();

            return result;
        }
    }
}