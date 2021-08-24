using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashBoardProject.Dtos
{
    public class DashboardCardMoreInfoDetailsDto
    {
        public long ChannelId { get; set; }
        public string UnitName { get; set; }
        public string Value { get; set; }
    }
}