using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Certification.Model
{
    public class UserInfo
    {
        public string UID { get; set; }
        public string Department { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Site { get; set; }
        public int IsGroup { get; set; }
    }
}