using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using AWAPI_Common.library;

namespace AWAPI.App_Code
{
    public class UserInfo
    {

        #region PROPERTIES

        #endregion


        public static bool IsAuthenticated()
        {
            return
                HttpContext.Current.User.Identity.IsAuthenticated &&
                App_Code.SessionInfo.CurrentSite != null &&
                App_Code.SessionInfo.CurrentUser != null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userNameOrEmail"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static AWAPI_Data.CustomEntities.UserExtended ValidateUser(string userNameOrEmail, string password)
        {
            AWAPI_BusinessLibrary.library.UserLibrary usr = new AWAPI_BusinessLibrary.library.UserLibrary();
            AWAPI_Data.CustomEntities.UserExtended user = usr.Login(userNameOrEmail, password);

            return user;
        }

        /// <summary>
        /// Returns current user's role for the speficifed module
        /// </summary>
        /// <param name="lookForWholeWord">
        /// if true then modulename has to be matched (blog == blog)
        /// else module name should include that word (example:  blog, blogs, blogposts, blogcomments,, etc)
        /// </param>
        /// <param name="moduleName"></param>
        /// <returns></returns>
        public static AWAPI_Data.Data.awRole GetUserRole(bool lookForWholeWord, string moduleName)
        {
            AWAPI_Data.Data.awRole rtnRole = new AWAPI_Data.Data.awRole();

            if (App_Code.SessionInfo.CurrentUser == null ||
                App_Code.SessionInfo.CurrentUserRoles == null ||
                String.IsNullOrEmpty(moduleName))
                return rtnRole;

            if (App_Code.SessionInfo.CurrentUser.isSuperAdmin)
            {
                rtnRole.canAdd = true;
                rtnRole.canDelete = true;
                rtnRole.canRead = true;
                rtnRole.canUpdate = true;
                rtnRole.canUpdateStatus = true;
                return rtnRole;
            }


            var role = from r in App_Code.SessionInfo.CurrentUserRoles
                       where (lookForWholeWord && r.module.ToLower().Trim().Equals(moduleName.ToLower().Trim())) ||
                             (!lookForWholeWord && r.module.ToLower().Trim().IndexOf(moduleName.ToLower().Trim()) >= 0)
                       select r;

            if (role == null || role.Count() == 0)
                return rtnRole;


            return role.FirstOrDefault<AWAPI_Data.Data.awRole>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="canAdd"></param>
        /// <param name="canDelete"></param>
        /// <param name="canRead"></param>
        /// <param name="canUpdate"></param>
        /// <param name="canUpdateStatus"></param>
        /// <param name="lookForWholeWord"></param>
        /// <param name="moduleName"></param>
        /// <returns></returns>
        public static bool GetUserRole(bool? canAdd,
                                bool? canDelete,
                                bool? canRead,
                                bool? canUpdate,
                                bool? canUpdateStatus, bool lookForWholeWord, string moduleName)
        {


            AWAPI_Data.Data.awRole rtnRole = GetUserRole(lookForWholeWord, moduleName);

            return ((canAdd != null && canAdd == rtnRole.canAdd) || canAdd == null) &&        //if canadd==null or equal to the user's role
                ((canDelete != null && canDelete == rtnRole.canDelete) || canDelete == null) &&
                ((canRead != null && canRead == rtnRole.canRead) || canRead == null) &&
                ((canUpdate != null && canUpdate == rtnRole.canUpdate) || canUpdate == null) &&
                ((canUpdateStatus != null && canUpdateStatus == rtnRole.canUpdateStatus) || canUpdateStatus == null);
        }


        #region AUTHENTICATION

        
        /// <summary>
        /// If session is null and the user is authenticated,
        /// get the userid and the siteid (if defined) from the cookie...
        /// </summary>
        public static void AuthenticateFromCookie()
        {

            if (HttpContext.Current.User != null &&
                HttpContext.Current.User.Identity.IsAuthenticated &&
                App_Code.SessionInfo.CurrentUser == null)
            {

                //If authenticated, get userId and siteId, and fill the session objects..
                if (HttpContext.Current.User.Identity is FormsIdentity)
                {
                    long userId = 0;
                    long siteId = 0;

                    FormsIdentity identity = (FormsIdentity)HttpContext.Current.User.Identity;
                    FormsAuthenticationTicket ticket = identity.Ticket;

                    if (!String.IsNullOrEmpty(ticket.UserData))
                    {
                        string[] userInfo = ticket.UserData.Split('|');
                        if (userInfo.Length > 0 && !String.IsNullOrEmpty(userInfo[0]))
                            userId = Convert.ToInt64(userInfo[0]);
                        if (userInfo.Length > 1 && !String.IsNullOrEmpty(userInfo[1]))
                            siteId = Convert.ToInt64(userInfo[1]);

                    }
                    //if user is deleted or disabled, then the user will be logged out...
                    if (!App_Code.SessionInfo.FillSession(userId, siteId))
                        App_Code.UserInfo.Logout();
                }
            }
        }

        /// <summary>
        /// Clear session and the authentication cookie
        /// </summary>
        public static void Logout()
        {
            App_Code.SessionInfo.ClearSession();
            FormsAuthentication.SignOut();

            HttpContext.Current.Response.Redirect("~/admin/login.aspx");
        }

        /// <summary>
        /// Update the siteid in the authentication cookie
        /// </summary>
        /// <param name="siteId"></param>
        public static void UpdateSiteIdInAuthenticationCookie(long siteId)
        {
            if (App_Code.SessionInfo.CurrentUser == null || !HttpContext.Current.User.Identity.IsAuthenticated)
                return;

            string newUserInfo = App_Code.SessionInfo.CurrentUser.userId.ToString() + "|" + siteId.ToString();
            HttpCookie oldCookie = FormsAuthentication.GetAuthCookie(App_Code.SessionInfo.CurrentUser.userId.ToString(), true);

            FormsIdentity identity = (FormsIdentity)HttpContext.Current.User.Identity;
            FormsAuthenticationTicket oldTicket = identity.Ticket;

            FormsAuthenticationTicket newTicket = new FormsAuthenticationTicket(1, // Ticket 
                                App_Code.SessionInfo.CurrentUser.userId.ToString(),// Username to be associated with this 
                                oldTicket.IssueDate, // Date/time ticket was 
                                oldTicket.Expiration, // Date and time the cookie will 
                                true, // if user has chcked rememebr me then create persistent 
                                newUserInfo, // store the user data, in this case roles of the 
                                FormsAuthentication.FormsCookiePath); // Cookie path specified in the web.config file in <Forms> tag if any.


            string hashCookies = FormsAuthentication.Encrypt(newTicket);
            HttpCookie newCookie = new HttpCookie(FormsAuthentication.FormsCookieName, hashCookies);
            HttpContext.Current.Response.Cookies.Add(newCookie);
        }

        /// <summary>
        /// Authenticates the user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="siteId"></param>
        public static void AuthenticateUser(long userId, long siteId)
        {
            string userInfo = userId.ToString() + "|" + siteId.ToString();

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, // Ticket 
                                userId.ToString(),// Username to be associated with this 
                                DateTime.Now, // Date/time ticket was 
                                DateTime.Now.AddDays(7), // Date and time the cookie will 
                                true, // if user has chcked rememebr me then create persistent 
                                userInfo, // store the user data, in this case roles of the 
                                FormsAuthentication.FormsCookiePath); // Cookie path specified in the web.config file in <Forms> tag if any.
            // To give more security it is suggested to hash it

            string hashCookies = FormsAuthentication.Encrypt(ticket);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hashCookies);
            // Hashed ticket
            // Add the cookie to the response, user browser
            HttpContext.Current.Response.Cookies.Add(cookie);

        }
        #endregion
    }
}
