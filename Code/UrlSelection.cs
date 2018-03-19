using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Models;

namespace CMS.Code
{
    public static class UrlSelection
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Selects the URL.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="beaconId">The beacon identifier.</param>
        /// <returns></returns>
        public static URL SelectUrl(User user, Guid beaconId)
        {
            // List of URLs assigned to given beacon and user
            var urls = db.Beacons.Where(y => y.Id == beaconId)
                .Select(x => x.Content.URL.Where(u => u.Group.ID == user.Group.ID))
                .FirstOrDefault().ToList();

            // select distinct recent URLs ordered by date 
            var recentUrls = db.Log.Where(x => x.User_Id == user.Id && x.Beacon_Id == beaconId && x.Group_Id == user.Group.ID)
                .GroupBy(i => i.Url_Id)
                .Select(i => new
                {
                    date = i.Max(z => z.RequestDate),
                    urlId = i.Key
                })
                .OrderBy(d => d.date);

            // if there's any assigned url which is not in urls history, return it
            foreach (URL url in urls)
            {
                if (!recentUrls.Select(x => x.urlId).Contains(url.Id))
                    return url;
            }

            // return the oldest url from recentUrls which is in assigned urls
            foreach (var i in recentUrls)
            {
                if (urls.Select(x => x.Id).Contains(i.urlId))
                {
                    int protectionPeriod = urls.Where(x => x.Id == i.urlId).Select(y => y.ProtectionPeriod).FirstOrDefault();

                    // Protection Period
                    if (((DateTime.Now - i.date).TotalMinutes > protectionPeriod) || (protectionPeriod == 0))
                    {
                        return urls.Where(x => x.Id == i.urlId).FirstOrDefault();
                    }
                }
            }

            // Else return random url
            Random random = new Random();
            int randomIndex = random.Next(0, urls.Count());

            return urls.Skip(randomIndex).Take(1).First();
        }

        /// <summary>
        /// Selects the URL.
        /// </summary>
        /// <param name="beaconId">The beacon identifier.</param>
        /// <returns></returns>
        public static URL SelectUrl(Guid beaconId)
        {
            // List of URLs assigned to given beacon and user
            var urls = db.Beacons.Where(y => y.Id == beaconId)
                .Select(x => x.Content.URL)
                .FirstOrDefault().ToList();

            // select distinct recent URLs ordered by date 
            var recentUrls = db.Log.Where(x => x.Beacon_Id == beaconId)
                .GroupBy(i => i.Url_Id)
                .Select(i => new
                {
                    date = i.Max(z => z.RequestDate),
                    urlId = i.Key
                })
                .OrderBy(d => d.date);

            // if there's any assigned url which is not in urls history, return it
            foreach (URL url in urls)
            {
                if (!recentUrls.Select(x => x.urlId).Contains(url.Id))
                    return url;
            }

            // return the oldest url from recentUrls which is in assigned urls
            foreach (var i in recentUrls)
            {
                if (urls.Select(x => x.Id).Contains(i.urlId))
                {
                    int protectionPeriod = urls.Where(x => x.Id == i.urlId).Select(y => y.ProtectionPeriod).FirstOrDefault();

                    // Protection Period
                    if (((DateTime.Now - i.date).TotalMinutes > protectionPeriod) || (protectionPeriod == 0))
                    {
                        return urls.Where(x => x.Id == i.urlId).FirstOrDefault();
                    }
                }
            }

            // Else return random url
            Random random = new Random();
            int randomIndex = random.Next(0, urls.Count());

            return urls.Skip(randomIndex).Take(1).First();
        }
    }
}