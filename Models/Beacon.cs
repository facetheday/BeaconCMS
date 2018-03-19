using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Models
{
    public class Beacon
    {
        public Guid Id { get; set; }
        public string BeaconID { get; set; }
        public int Major { get; set; }
        public int Minor { get; set; }
        public Content Content { get; set; }
        public Notification Notification { get; set; }
    }

    public class BeaconURL
    {
        public Beacon Beacon { get; set; }
        public URL Url { get; set; }
    }

    public class BeaconResult
    {
        public Guid Id { get; set; }
        public string BeaconID { get; set; }
        public int Major { get; set; }
        public int Minor { get; set; }
        public string Url { get; set; }
        public string Group { get; set; }
        public string DeviceId { get; set; }
        public Notification Notification { get; set; }
        public int DisplayTimeout { get; set; }
    }
}