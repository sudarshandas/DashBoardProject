using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DashBoardProject.Security
{
    public class DashboardAuthorize : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            //string userId = Convert.ToString(context.HttpContext.Session["G_UserID"]);

            //if (string.IsNullOrEmpty(userId))
            //{
            //    context.HttpContext.Session.Abandon(); // it will clear the session at the end of request
            //    context.HttpContext.Session.Clear();
            //    context.HttpContext.Session.RemoveAll();

            //    context.HttpContext.Response.Redirect("/Account/LogIn", true);
            //}
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string userId = Convert.ToString(filterContext.HttpContext.Session["G_UserID"]);

            if (string.IsNullOrEmpty(userId))
            {
                filterContext.HttpContext.Session.Abandon(); // it will clear the session at the end of request
                filterContext.HttpContext.Session.Clear();
                filterContext.HttpContext.Session.RemoveAll();

                filterContext.HttpContext.Response.Redirect("/Account/LogIn", true);
                filterContext.HttpContext.Response.End();
            }
            //base.OnActionExecuting(filterContext);
        }
    }
}