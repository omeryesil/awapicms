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
    public partial class managefilegroups : AWAPI.App_Code.AdminBasePage
    {
        AWAPI_BusinessLibrary.library.FileLibrary _fileLib = new AWAPI_BusinessLibrary.library.FileLibrary();

        Int64 GroupId
        {
            get
            {
                if (Request["Link"] == null)
                    return 0;
                return Convert.ToInt64(Request["Link"]);
            }

        }

        public managefilegroups()
        {
            ModuleName = "filegroup";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _msg.Text = "";

            if (!IsPostBack)
                PopulateFileGroups();
        }


        void PopulateFileGroups()
        {
            _list.DataSource = _fileLib.GetFileGroupListBySiteId(App_Code.SessionInfo.CurrentSite.siteId);
            _list.DataBind();
        }

        void PopulateFileGroup(long groupId)
        {
            ResetControls();

            AWAPI_Data.Data.awFileGroup group = _fileLib.GetFileGroup(groupId);
            if (group != null)
            {
                _groupId.Text = group.fileGroupId.ToString();
                _groupTitle.Text = group.title;
                _groupDescription.Text = group.description;

                ShowHildeControls(true);

            }
        }

        void ResetControls()
        {
            _groupId.Text = "";
            _groupDescription.Text = "";
            _groupTitle.Text = "";

            ShowHildeControls(false);
        }



        void ShowHildeControls(bool show)
        {
            ShowHideControl(btnDeleteFileGroup_, show);
        }


        protected void _list_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int64 id = Convert.ToInt64(e.CommandArgument.ToString());
            switch (e.CommandName.ToLower())
            {
                case "editfilegroup":
                    PopulateFileGroup(id);
                    break;

            }
        }

        protected void btnNew_Click(object sender, ImageClickEventArgs e)
        {
            ResetControls();
        }


        void AddFileGroup(string groupTitle, string groupDescription)
        {
            long groupId = _fileLib.AddFileGroup(App_Code.SessionInfo.CurrentSite.siteId, App_Code.SessionInfo.CurrentUser.userId,
                                                groupTitle, groupDescription, DateTime.Now);
            PopulateFileGroups();
            PopulateFileGroup(groupId);

            App_Code.Misc.WriteMessage(_msg, "File group has been saved.");
        }


        void UpdateFileGroup(long groupId, string groupTitle, string groupDescription)
        {
            _fileLib.UpdateFileGroup(groupId, groupTitle, groupDescription);
            PopulateFileGroups();
            PopulateFileGroup(groupId);

            App_Code.Misc.WriteMessage(_msg, "File group has been saved.");

        }


        void DeleteFileGroup(long groupId)
        {
            _fileLib.DeleteFileGroup(groupId);
            PopulateFileGroups();

            App_Code.Misc.WriteMessage(_msg, "File group has been deleted.");
        }

        protected void btnSaveFileGroup_Click(object sender, ImageClickEventArgs e)
        {
            if (_groupId.Text.Trim().Length == 0) //add new one
            {
                AddFileGroup(_groupTitle.Text, _groupDescription.Text);
            }
            else //update the current one
            {
                UpdateFileGroup(Convert.ToInt64(_groupId.Text), _groupTitle.Text, _groupDescription.Text);
            }
        }

        protected void btnDeleteFileGroup_Click(object sender, ImageClickEventArgs e)
        {
            if (_groupId.Text.Trim().Length > 0)
                DeleteFileGroup(Convert.ToInt64(_groupId.Text));

        }



        protected void btnNewFileGroup_Click(object sender, EventArgs e)
        {
            ResetControls();
        }



    }
}
