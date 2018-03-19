using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Models
{
    public class Log
    {
        public int Id { get; set; }
        public Guid Beacon_Id { get; set; }
        public Guid User_Id { get; set; }
        public int Group_Id { get; set; }
        public int Url_Id { get; set; }
        public DateTime RequestDate { get; set; }
    }
}