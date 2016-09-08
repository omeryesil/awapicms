using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace AWAPI
{
    public class Global : System.Web.HttpApplication
    {
        public static long UserId = 0;
        public static long SiteId = 0;

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {
            App_Code.UserInfo.AuthenticateFromCookie();
        }


        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //Upgrade Database 


        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }


        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}