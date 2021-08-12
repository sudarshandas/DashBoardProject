using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashBoardProject.Models
{
    public class Users
    {
        public long UserID { get; set; }
        public string UserPersonName { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
    }
}