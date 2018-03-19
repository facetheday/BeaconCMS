using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CMS.Models;

namespace CMS.Api
{
    public class GroupController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET api/<controller>
        public IEnumerable<Group> Get()
        {
            return db.Groups;
        }

        // GET api/<controller>/5
        public Group Get(int id)
        {
            return db.Groups.Where(x => x.ID == id).FirstOrDefault();
        }
    }
}