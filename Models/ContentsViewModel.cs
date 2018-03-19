using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS.Models
{
    public class ContentsViewModel
    {
        public IEnumerable<SelectListItem> Contents { get; set; }
        public string SelectedContentID { get; set; }
        public IEnumerable<SelectListItem> Notifications { get; set; }
        public string SelectedNotificationID { get; set; }
        public Guid BeaconID { get; set; }
    }
}