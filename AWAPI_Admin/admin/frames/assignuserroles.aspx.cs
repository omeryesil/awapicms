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

namespace AWAPI.Admin.frames
{
    public partial class RoleList : AWAPI.App_Code.AdminBasePage
    {
        AWAPI_BusinessLibrary.library.RoleLibrary _roleLib = new AWAPI_BusinessLibrary.library.RoleLibrary();

        public long UserId
        {
            get
            {
                if (Request["userid"] == null)
                    return 0;
                return Convert.ToInt64(Request["userid"]);
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            _msg.Text = "";
            if (!IsPostBack)
            {
                PopulateUser();
                PopulateRoles();
            }
        }

        void PopulateUser()
        {
            if (UserId > 0)
            {
                AWAPI_Data.CustomEntities.UserExtended usr = new AWAPI_BusinessLibrary.library.UserLibrary().Get(UserId);
                if (usr != null)
                    _pageTitle.Text = "User Roles - " + usr.username;
           
            }

        }

        void PopulateRoles()
        {
            var roles = _roleLib.GetList();


            //var roleList = _roleLib.GetMembersRights(UserId, App_Code.SessionInfo.CurrentSite.siteId);

            _roleList.DataSource = roles;
            _roleList.DataBind();
        }

        protected void _roleList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                long roleId = (long)DataBinder.Eval(e.Row.DataItem, "roleId");

                CheckBox assigned = (CheckBox)e.Row.FindControl("_assigned");

                AWAPI_Data.Data.awRoleMember rm = _roleLib.UserHasRole(roleId, App_Code.SessionInfo.CurrentSite.siteId, 
                                                                    UserId);
                if (rm == null)
                    assigned.Checked = false;
                else
                    assigned.Checked = true;
            }
        }

        protected void btnSavenAssignRole__Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                SaveUserRoles();
                App_Code.Misc.WriteMessage(_msg, "Roles have been saved.");
            }
            catch (Exception ex)
            {
                App_Code.Misc.WriteMessage(App_Code.Misc.MessageType.ERROR, _msg, ex.Message);
            }
        }

        void SaveUserRoles()
        {
            foreach (GridViewRow row in _roleList.Rows)
            {
                long roleId = Convert.ToInt64(((Label)row.FindControl("_roleId")).Text);

                CheckBox assigned = (CheckBox)row.FindControl("_assigned");

                if (assigned.Checked)
                {
                    _roleLib.AssignRoleToUser(roleId,
                                        App_Code.SessionInfo.CurrentSite.siteId, UserId);

                }
                else
                {
                    _roleLib.RemoveRoleFromUser(roleId, App_Code.SessionInfo.CurrentSite.siteId,UserId);
                }
            }
        }


    }
}
