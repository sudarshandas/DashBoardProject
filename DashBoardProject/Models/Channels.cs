using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashBoardProject.Models
{
    public class Channels
    {
        public long ChannelID { get; set; }
        public string ChannelDesc { get; set; }
        public DateTime ChannelDateCreated { get; set; }
        public DateTime ChannelLastUpdated { get; set; }
        public int ChannelDeleted { get; set; }
        public long ChannelParentID { get; set; }
        public long ChannelLastUpdByUserID { get; set; }
        public long ChannelCreatedByUserID { get; set; }
        public string ChannelCode { get; set; }
        public string ChannelName { get; set; }
        public int ShowInDashBoard { get; set; }
        public int ShowInReport { get; set; }
        public long MasterContactID { get; set; }
    }
}