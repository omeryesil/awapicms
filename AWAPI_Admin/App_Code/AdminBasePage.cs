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
using AWAPI.App_Code;
using AWAPI_BusinessLibrary.library;
using AWAPI_Data.Data;

namespace AWAPI.App_Code
{
    public class AdminBasePage : System.Web.UI.Page
    {
        #region FOR PAGE ROLES
        public AWAPI_Data.Data.awRole PageRole = null;
        public string ModuleName = "";
        #endregion

        public AWAPI.Admin.AdminMaster AdminMaster
        {
            get { return (AWAPI.Admin.AdminMaster)this.Master; }
        }

        protected override void OnLoad(EventArgs e)
        {
            //if (!IsLoggedIn)
            //{
            //    Server.Transfer("/admin/login.aspx");
            //    Response.End();
            //}
            //else
            //{
            //if (!AWAPI.App_Code.UserInfo.IsAuthenticated() )
            //    Response.Redirect ("~/admin/login.aspx?ReturnUrl=" + 
            //                       Server.UrlEncode(Request.Url.ToString()));

            if (!IsPostBack)
            {
                if (!CheckPageRights(ModuleName, ref PageRole))
                {
                    Response.Redirect("~/admin/nopermission.aspx");
                    Response.End();
                }
            }
            base.OnLoad(e);
            //}
        }

        protected override void OnPreRender(System.EventArgs e)
        {
            //if (!CheckPageRights(ModuleName, LookForWholeModuleName, ref PageRole))
            //{
            //    Response.Redirect("~/admin/nopermission.aspx");
            //    Response.End();
            //}
            // PreRender code
            base.OnPreRender(e);
        }


        public void RegisterCustomScript(System.Text.StringBuilder script)
        {
            script.Append("$(document).ready(function(){ \n");
            script.Append("    $(\"a[rel='selectfile']\").colorbox({width:'960px', height:'600px', iframe:true}); \n");
            script.Append("    $(\"a[rel='blogpostcomments']\").colorbox({width:'960px', height:'600px', iframe:true}); \n");
            script.Append("}); \n");

            if (script.Length > 0)
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "", script.ToString(), true);

        }



        /// <summary>
        /// if True then user has the right to see the page,
        /// else the page will be redirected to 'nopermission.aspx'
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public bool CheckPageRights(string moduleName, ref awRole moduleRole)
        {
            moduleRole = null;

            if (String.IsNullOrEmpty(moduleName))
                return true;

            if (App_Code.SessionInfo.CurrentUser == null)
                return false;

            if (App_Code.SessionInfo.CurrentUser.isSuperAdmin)
                return true;

            awRole role = App_Code.UserInfo.GetUserRole(false, moduleName);
            if (!role.canRead &&
                !role.canAdd &&
                !role.canDelete &&
                !role.canRead &&
                !role.canUpdate &&
                !role.canUpdateStatus)
                return false;

            //Show/Hide Controls 
            ImageButton btnDelete = null;
            ImageButton btnNew = null;         //canAdd
            ImageButton btnSave = null;     //canUpdate
            HyperLink hplPopUp = null;      //have any right
            CheckBox cbStatus = null;         //canUpdateStatus

            Label lblDelete = null;
            Label lblNew = null;
            Label lblSave = null;
            Label lblPopUp = null;


            ContentPlaceHolder content = null;
            if (this.Master != null)
                content = (ContentPlaceHolder)this.Master.FindControl("contentPlaceHolder");


            string[] modules = Enum.GetNames(typeof(AWAPI_BusinessLibrary.library.RoleLibrary.Module));
            foreach (string module in modules)
            {
                awRole subRole = App_Code.UserInfo.GetUserRole(false, module);

                //Show/Hide Controls 
                if (content != null)
                {
                    btnDelete = (ImageButton)content.FindControl("btnDelete" + module + "_");   //canDelete
                    btnNew = (ImageButton)content.FindControl("btnNew" + module + "_");         //canAdd
                    btnSave = (ImageButton)content.FindControl("btnSave" + module + "_");       //canUpdate
                    hplPopUp = (HyperLink)content.FindControl("hplPopUp" + module + "_");         //have any right
                    cbStatus = (CheckBox)content.FindControl("cbStatus" + module + "_");           //canUpdateStatus

                    lblDelete = (Label)content.FindControl("lblDelete" + module + "_");
                    lblNew = (Label)content.FindControl("lblNew" + module + "_");
                    lblSave = (Label)content.FindControl("lblSave" + module + "_");
                    lblPopUp = (Label)content.FindControl("lblPopup" + module + "_");
                }
                else
                {
                    btnDelete = (ImageButton)this.Page.FindControl("btnDelete" + module + "_");   //canDelete
                    btnNew = (ImageButton)this.Page.FindControl("btnNew" + module + "_");         //canAdd
                    btnSave = (ImageButton)this.Page.FindControl("btnSave" + module + "_");       //canUpdate
                    hplPopUp = (HyperLink)this.Page.FindControl("hplPopUp" + module + "_");         //have any right
                    cbStatus = (CheckBox)this.Page.FindControl("cbStatus" + module + "_");           //canUpdateStatus

                    lblDelete = (Label)this.Page.FindControl("lblDelete" + module + "_");
                    lblNew = (Label)this.Page.FindControl("lblNew" + module + "_");
                    lblSave = (Label)this.Page.FindControl("lblSave" + module + "_");
                    lblPopUp = (Label)this.Page.FindControl("lblPopup" + module + "_");
                }

                if (btnDelete != null) btnDelete.Visible = subRole.canDelete;
                if (btnNew != null) btnNew.Visible = subRole.canAdd;
                if (btnSave != null) btnSave.Visible = subRole.canAdd || subRole.canUpdate || subRole.canUpdateStatus;
                if (hplPopUp != null) hplPopUp.Visible = subRole.canAdd || subRole.canUpdate || subRole.canUpdateStatus;

                if (lblDelete != null) lblDelete.Visible = subRole.canDelete;
                if (lblNew != null) lblNew.Visible = subRole.canAdd;
                if (lblSave != null) lblSave.Visible = subRole.canAdd || subRole.canUpdate || subRole.canUpdateStatus;
                if (lblPopUp != null) lblPopUp.Visible = subRole.canAdd || subRole.canUpdate || subRole.canUpdateStatus;
                if (cbStatus != null) cbStatus.Enabled = subRole.canUpdateStatus;
            }


            moduleRole = role;
            return true;
        }

        public void ShowHideControl(ImageButton control, bool show)
        {
            if (show)
                control.Attributes.Remove("style");
            else
                control.Attributes.Add("style", "display:none");
        }


        public void ShowHideControl(System.Web.UI.WebControls.CheckBox control, bool show)
        {
            if (show)
                control.Attributes.Remove("style");
            else
                control.Attributes.Add("style", "display:none");
        }


        public void ShowHideControl(Image control, bool show)
        {
            if (show)
                control.Attributes.Remove("style");
            else
                control.Attributes.Add("style", "display:none");
        }


        public void ShowHideControl(Label control, bool show)
        {
            if (show)
                control.Attributes.Remove("style");
            else
                control.Attributes.Add("style", "display:none");
        }

        public void ShowHideControl(HyperLink control, bool show)
        {
            if (show)
                control.Attributes.Remove("style");
            else
                control.Attributes.Add("style", "display:none");
        }



    }
}
