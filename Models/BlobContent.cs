using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Models
{
    public class BlobContent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContainerName { get; set; }
        public string FileName { get; set; }
        public string URL { get; set; }
        public DateTime CreateDate { get; set; }
    }
}