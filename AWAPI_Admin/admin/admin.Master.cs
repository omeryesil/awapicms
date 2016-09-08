using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using AWAPI_BusinessLibrary.library;

namespace AWAPI.Admin
{
    public partial class AdminMaster : System.Web.UI.MasterPage
    {
        #region PROPERTIES
        public Boolean IsSiteSelected
        {
            get
            {
                if (AWAPI.App_Code.SessionInfo.CurrentSite == null)
                    return false;
                return true;

            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            _currentServerTime.Text = DateTime.Now.ToString("MM/dd/yy HH:mm");
            _msg.Text = "";
            if (!IsPostBack)
            {
                PopulateSiteInfo();
                PopulatePageControls();
            }
        }

        void PopulateSiteInfo()
        {
            if (App_Code.SessionInfo.CurrentSite != null)
            {
                if (String.IsNullOrEmpty(App_Code.SessionInfo.CurrentSite.imageurl))
                {
                    _siteLogo.Visible = false;
                    _siteTitle.Text = "<h1>" + App_Code.SessionInfo.CurrentSite.title + " - Admin</h1>";
                }
                else
                {
                    _siteLogo.ImageUrl = App_Code.SessionInfo.CurrentSite.imageurl;
                    _siteLogo.Visible = true;
                }
            }
        }

        public static bool IsCurrentPage(string page)
        {
            return HttpContext.Current.Request.ServerVariables["URL"].ToLower().IndexOf(page.ToLower()) >= 0 ? true : false;
        }



        protected void Page_PreRender(object sender, EventArgs e)
        {
            _currentUser.Text =  App_Code.SessionInfo.CurrentUser.username + " - " +  App_Code.SessionInfo.CurrentUser.email;
        }

        /// <summary>
        /// Populates menu, controls, etc based on the user rights
        /// </summary>
        void PopulatePageControls()
        {
            bool siteEnabled = false;
            if (App_Code.SessionInfo.CurrentSite != null)
                siteEnabled = true;

            #region GET USER RIGHTS FOR EACH MODULE
            //Get Roles for each module
            AWAPI_Data.Data.awRole rlBlog = App_Code.UserInfo.GetUserRole(false, RoleLibrary.Module.blog.ToString());
            AWAPI_Data.Data.awRole rlContent = App_Code.UserInfo.GetUserRole(true, RoleLibrary.Module.content.ToString());
            AWAPI_Data.Data.awRole rlContentForm = App_Code.UserInfo.GetUserRole(false, RoleLibrary.Module.contentForm.ToString());
            AWAPI_Data.Data.awRole rlFiles = App_Code.UserInfo.GetUserRole(false, RoleLibrary.Module.file.ToString());
            AWAPI_Data.Data.awRole rlPolls = App_Code.UserInfo.GetUserRole(false, RoleLibrary.Module.poll.ToString());
            AWAPI_Data.Data.awRole rlUser = App_Code.UserInfo.GetUserRole(false, RoleLibrary.Module.user.ToString());
            AWAPI_Data.Data.awRole rlContest = App_Code.UserInfo.GetUserRole(false, RoleLibrary.Module.contest.ToString());
            AWAPI_Data.Data.awRole rlDataTransfer = App_Code.UserInfo.GetUserRole(false, RoleLibrary.Module.datatransfer.ToString());

            //AWAPI_Data.Data.awRole roleSite = App_Code.UserInfo.GetUserRole(false, "poll"); //poll is only available for the siteadmin
            #endregion

            #region POPULATE MENUS
            //Show hide menu's based on the rights
            menuLiDashboard.Visible = true;
            menuLiConfiguration.Visible = App_Code.SessionInfo.CurrentUser.isSuperAdmin;
            menuLiSites.Visible = App_Code.SessionInfo.CurrentUser.isSuperAdmin;
            menuLiBlogs.Visible = App_Code.SessionInfo.CurrentUser.isSuperAdmin || (siteEnabled && (rlBlog.canRead || rlBlog.canAdd || rlBlog.canDelete || rlBlog.canUpdate || rlBlog.canUpdateStatus));
            menuLiContents.Visible = App_Code.SessionInfo.CurrentUser.isSuperAdmin || (siteEnabled && (rlContent.canRead || rlContent.canAdd || rlContent.canDelete || rlContent.canUpdate || rlContent.canUpdateStatus));
            menuLiContentForms.Visible = App_Code.SessionInfo.CurrentUser.isSuperAdmin || (siteEnabled && (rlContentForm.canRead || rlContentForm.canAdd || rlContentForm.canDelete || rlContentForm.canUpdate || rlContentForm.canUpdateStatus));
            menuLiContest.Visible = App_Code.SessionInfo.CurrentUser.isSuperAdmin || (siteEnabled && (rlContest.canRead || rlContest.canAdd || rlContest.canDelete || rlContest.canUpdate || rlContest.canUpdateStatus));
            menuLiFiles.Visible =  App_Code.SessionInfo.CurrentUser.isSuperAdmin || (siteEnabled && (rlFiles.canRead || rlFiles.canAdd || rlFiles.canDelete || rlFiles.canUpdate || rlFiles.canUpdateStatus));
            menuLiUsers.Visible =  App_Code.SessionInfo.CurrentUser.isSuperAdmin || (siteEnabled && (rlUser.canRead || rlUser.canAdd || rlUser.canDelete || rlUser.canUpdate || rlUser.canUpdateStatus));
            menuLiPolls.Visible = App_Code.SessionInfo.CurrentUser.isSuperAdmin || (siteEnabled && (rlPolls.canRead || rlPolls.canAdd || rlPolls.canDelete || rlPolls.canUpdate || rlPolls.canUpdateStatus));
            menuLiDataTransfer.Visible = App_Code.SessionInfo.CurrentUser.isSuperAdmin || (siteEnabled && (rlDataTransfer.canRead || rlDataTransfer.canAdd || rlDataTransfer.canDelete || rlDataTransfer.canUpdate || rlDataTransfer.canUpdateStatus));

            //SET SELECTED CLASS
            menuLiDashboard.Attributes.Add("class", IsCurrentPage("default.aspx") == true ? "active" : "");
            menuLiConfiguration.Attributes.Add("class", IsCurrentPage("admin/configuration") == true ? "active" : "");
            menuLiSites.Attributes.Add("class", IsCurrentPage("admin/site") == true ? "active" : "");
            menuLiBlogs.Attributes.Add("class", IsCurrentPage("admin/blog") == true ? "active" : "");
            if (IsCurrentPage("admin/contentforms"))
                menuLiContentForms.Attributes.Add("class", "active");
            else if  (IsCurrentPage("admin/content"))
                menuLiContents.Attributes.Add("class", "active");
            menuLiContest.Attributes.Add("class", IsCurrentPage("admin/contest") == true ? "active" : "");
            menuLiFiles.Attributes.Add("class", IsCurrentPage("admin/file") == true ? "active" : "");
            menuLiUsers.Attributes.Add("class", IsCurrentPage("admin/users") == true ? "active" : "");
            menuLiPolls.Attributes.Add("class", IsCurrentPage("admin/poll") == true ? "active" : "");
            menuLiDataTransfer.Attributes.Add("class", IsCurrentPage("admin/datatransfer") == true ? "active" : "");
            #endregion

        }

        #region MESSAGE
        public enum MessageType
        {
            ERROR,
            INFO,
            WARNING
        }

        public void WriteMessage(MessageType msgType, string message)
        {
            System.Text.StringBuilder msg = new System.Text.StringBuilder();

            string className = "msgWarning";
            string title = "";
            switch (msgType)
            {
                case MessageType.INFO:
                    className = "msgInfo";
                    title = "";
                    break;
                case MessageType.ERROR:
                    className = "msgError";
                    title = "<b>ERROR : </b>";
                    break;
            }

            msg.Append("<span id='msgContainer' class='" + className + "'>");
            msg.Append(title);
            msg.Append(message);
            msg.Append("</span>");

            _msg.Text = msg.ToString();
        }
        #endregion


        protected void lbtnLogOut_Click(object sender, EventArgs e)
        {
            App_Code.UserInfo.Logout();
        }

        protected void ScriptManager1_AsyncPostBackError(object sender, AsyncPostBackErrorEventArgs e)
        {
            if (e.Exception.Data["ExtraInfo"] != null)
            {
                ScriptManager1.AsyncPostBackErrorMessage =
                    e.Exception.Message +
                    e.Exception.Data["ExtraInfo"].ToString();
            }
            else
            {
                ScriptManager1.AsyncPostBackErrorMessage =
                    "An unspecified error occurred.";
            }
        }
    }
}
