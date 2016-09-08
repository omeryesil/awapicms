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

namespace AWAPI.App_Code
{
    public class SessionInfo
    {
        public const string SESS_USER_OBJECT = "SESS_USER_OBJECT";
        public const string SESS_SITE_OBJECT = "SESS_SITE_OBJECT";
        public const string SESS_USER_ROLES_OBJECT = "SESS_USER_ROLES_OBJECT";


        public static AWAPI_Data.CustomEntities.UserExtended CurrentUser
        {
            get
            {
                //if in the session
                if (HttpContext.Current.Session[SESS_USER_OBJECT] != null)
                    return (AWAPI_Data.CustomEntities.UserExtended)HttpContext.Current.Session[SESS_USER_OBJECT];
                return null;
            }
            set
            {
                HttpContext.Current.Session[SESS_USER_OBJECT] = value;
            }
        }

        public static AWAPI_Data.Data.awSite CurrentSite
        {
            get
            {
                if (HttpContext.Current.Session[SESS_SITE_OBJECT] != null)
                    return (AWAPI_Data.Data.awSite)HttpContext.Current.Session[SESS_SITE_OBJECT];

                return null;
            }
            set
            {
                HttpContext.Current.Session[SESS_SITE_OBJECT] = value;
            }
        }

        public static System.Collections.Generic.IList<AWAPI_Data.Data.awRole> CurrentUserRoles
        {
            get
            {
                if (HttpContext.Current.Session[SESS_USER_ROLES_OBJECT] != null)
                    return (System.Collections.Generic.IList<AWAPI_Data.Data.awRole>)HttpContext.Current.Session[SESS_USER_ROLES_OBJECT];
                return null;
            }

            set
            {
                HttpContext.Current.Session[SESS_USER_ROLES_OBJECT] = value;
            }
        }



        public static void ClearSession()
        {
            App_Code.SessionInfo.CurrentUser = null;
            App_Code.SessionInfo.CurrentSite = null;
            App_Code.SessionInfo.CurrentUserRoles = null;
        }

        /// <summary>
        /// Fills the session objects based on the userId and the siteId
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="siteId"></param>
        public static bool FillSession(long userId, long siteId)
        {
            CurrentUser = null;
            CurrentSite = null;
            CurrentUserRoles = null;

            if (userId > 0)
            {
                CurrentUser = new AWAPI_BusinessLibrary.library.UserLibrary().Get(userId);
                if (siteId > 0)
                {
                    CurrentSite = new AWAPI_BusinessLibrary.library.SiteLibrary().Get(siteId);
                    CurrentUserRoles = new AWAPI_BusinessLibrary.library.RoleLibrary().GetUserRoleList(userId, siteId);
                }
            }

            //if the current user is null, then the form authnentication cookie will be deleted...
            if (CurrentUser == null)
                return false;
            return true;


        }

    }
}
