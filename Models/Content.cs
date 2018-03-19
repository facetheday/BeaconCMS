using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Models
{
    public class Content
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public List<URL> URL { get; set; }
    }

    public class URL
    {
        public int Id { get; set; }
        public Group Group { get; set; }
        public string UrlValue { get; set; }
        public int ProtectionPeriod { get; set; }
    }
}