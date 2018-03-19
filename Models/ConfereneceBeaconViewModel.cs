using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS.Models
{
    public class ConfereneceBeaconViewModel
    {
        public int ConferenceID { get; set; }
        public Conference Conference { get; set; }
        public IEnumerable<SelectListItem> Beacons { get; set; }
        public string SelectedMajorID { get; set; }
    }

    public class ContentGroupViewModel
    {
        public Guid ContentID { get; set; }
        public Content Content { get; set; }
        public IEnumerable<SelectListItem> Groups { get; set; }
        public string SelectedGroupID { get; set; }
        public string URL { get; set; }
        public int ProtectionPeriod { get; set; }
    }
}