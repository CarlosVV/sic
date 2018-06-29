using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace Nagnoi.SiC.Web.App_Start {
    public class BaseAuthenticationFilter : AuthorizeAttribute {

        public override void OnAuthorization(AuthorizationContext filterContext) {       
            bool skipAuthorization = filterContext.
                                                    ActionDescriptor.
                                                    IsDefined(typeof(AllowAnonymousAttribute), true)
                                                    ||                      filterContext.ActionDescriptor.
                                                     ControllerDescriptor.
                                                    IsDefined(typeof(AllowAnonymousAttribute), true);

            if (skipAuthorization)
            {
                return;
            }


            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
                {
                    filterContext.HttpContext.Response.StatusCode = 401;
                    filterContext.Result = new HttpStatusCodeResult(401, "Please login to continue");
                    filterContext.HttpContext.Response.End();

                    //FormsAuthentication.SignOut();
                }
            }
        
        }        
    }
}