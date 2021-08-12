using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DashBoardProject.Dtos
{
    public class LoginDto
    {
        [Required(ErrorMessage ="Please enter username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter password")]
        public string UserPassword { get; set; }
    }
}