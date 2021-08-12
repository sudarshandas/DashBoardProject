using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashBoardProject.Dtos
{
    public class UserMenuDto
    {
        public string MenuName { get; set; }
        public string ParentMenuName { get; set; }
        public string DisplayName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
    }
}