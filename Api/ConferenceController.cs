using CMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CMS.Api
{
    public class ConferenceController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET api/<controller>
        public IEnumerable<Conference> Get()
        {
            return db.Conferences;
        }

        // GET api/<controller>/5
        public Conference Get(int id)
        {
            Conference conf = db.Conferences.Where(x => x.MajorID == id).Single();
            if (conf == null) NotFound();
            return conf;
        }
    }
}