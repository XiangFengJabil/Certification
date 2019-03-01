using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Certification.Model
{
    public class ApproverInfo
    {
        public int SNO { get; set; }
        public int FlowId { get; set; }
        public string UID { get; set; }
        public string ApproveType { get; set; }
        public int IsGroup { get; set; }
        public int SeqNo { get; set; }
        public string DisplayName { get; set; }
        public string Mail { get; set; }
    }
}