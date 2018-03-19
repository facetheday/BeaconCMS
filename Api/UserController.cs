using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CMS.Models;

namespace CMS.Api
{
    public class UserController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Posts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Post([FromBody]JArray value)
        {
            try
            {
                dynamic data = JsonConvert.DeserializeObject(value.ToString());
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, "Data received succesfully.");

                for (int i = 0; i < data.Count; i++)
                {
                    dynamic item = data[i];
                    string groupName = item.Group;
                    string userName = item.UserName;
                    Group group = db.Groups.Where(x => x.Name.Equals(groupName, StringComparison.InvariantCulture)).FirstOrDefault();

                    if (group == null)
                    {
                        response = Request.CreateResponse(HttpStatusCode.Conflict, "Unknown group category for user " + userName);
                        return response;
                    }

                    User user = new Models.User
                    {
                        Id = Guid.NewGuid(),
                        UserName = userName,
                        Password = item.Password,
                        Email = item.Email,
                        Group = group
                    };

                    if (db.User.Where(x => x.UserName == user.UserName).Any())
                    {
                        response = Request.CreateResponse(HttpStatusCode.Conflict, "UserName " + user.UserName + " already in database");
                        return response;
                    }
                    else
                    {
                        db.User.Add(user);
                        db.SaveChanges();
                    }
                }

                return response;
            }
            catch (Exception e)
            {
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest, e.Message);
                return response;
            }
        }
    }
}
