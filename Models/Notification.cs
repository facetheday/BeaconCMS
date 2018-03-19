using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Models
{
    public class Notification
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int DisplayFrequency { get; set; }
        public bool BeaconLostMode { get; set; }
    }
}