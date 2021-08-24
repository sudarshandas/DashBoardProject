using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashBoardProject.Dtos
{
    public class DashboardCardInfoDetailsDto
    {
        public long ChannelId { get; set; }
        public string CompanyName { get; set; }
        public string Value { get; set; }
        public bool hasChildChannel { get; set; }
        public List<DashboardCardMoreInfoDetailsDto> childInfo { get; set; }
    }
}