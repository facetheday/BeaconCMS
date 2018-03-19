using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Models
{
    public partial class ApiKey
    {
        public int Id { get; set; }
        public string ApiKeyValue { get; set; }
        public bool Status { get; set; }
    }
}