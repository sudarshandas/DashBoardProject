using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashBoardProject.Dtos
{
    public class DashboardCardDataDto
    {
        public string DocumentCount { get; set; }
        public decimal DocumentSumValue { get; set; }
        public bool IsPreviousFY { get; set; }
        public int  CardType { get; set; }
        public string  AdditionalString { get; set; }
    }
}