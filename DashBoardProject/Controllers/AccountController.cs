using DashBoardProject.Dtos;
using DashBoardProject.Models;
using DashBoardProject.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DashBoardProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpGet]
        public ActionResult Login()
        {
            if(Session["G_UserID"] != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {
            if (string.IsNullOrEmpty(loginDto.UserName) || string.IsNullOrEmpty(loginDto.UserPassword))
            {
                ModelState.Clear();

                ModelState.AddModelError("error", "Please enter username and password");
                return View(loginDto);
            }

            var user = await _accountRepository.GetUserByUserNameAndPassword(loginDto.UserName, loginDto.UserPassword);

            if (user == null)
            {
                ModelState.AddModelError("error", "Error in login, invalid details...");
                return View(loginDto);
            }

            var lstuserMenu =await _accountRepository.GetUserMenu(user.UserID);

            if(lstuserMenu == null)
            {
                ModelState.AddModelError("error", "User rights is not defined");
                return View(loginDto);
            }

            Session["G_UserID"] = user.UserID;
            Session["G_UserName"] = user.UserName;
            Session["G_UserPassword"] = user.UserPassword;
            Session["G_UserPersonName"] = user.UserPersonName;
            Session["G_UserMenu"] = lstuserMenu; // In _layout page we will create menu

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Logout()
        {
            Session.Abandon(); // it will clear the session at the end of request
            Session.Clear();
            Session.RemoveAll();
            return RedirectToAction("LogIn");
        }
    }
}