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
    public partial class managecontestgroups : AWAPI.App_Code.AdminBasePage
    {
        AWAPI_BusinessLibrary.library.ContestLibrary _contestLib = new AWAPI_BusinessLibrary.library.ContestLibrary();

        Int64 GroupId
        {
            get
            {
                if (Request["Link"] == null)
                    return 0;
                return Convert.ToInt64(Request["Link"]);
            }

        }

        public managecontestgroups()
        {
            ModuleName = "contestgroup";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _msg.Text = "";

            if (!IsPostBack)
                PopulateContestGroups();
        }


        void PopulateContestGroups()
        {
            _list.DataSource = _contestLib.GetContestGroupList(App_Code.SessionInfo.CurrentSite.siteId, "");
            _list.DataBind();
        }

        void PopulateContestGroup(long groupId)
        {
            ResetControls();

            AWAPI_Data.Data.awContestGroup group = _contestLib.GetContestGroup(groupId);
            if (group != null)
            {
                _groupId.Text = group.contestGroupId.ToString();
                _title.Text = group.title;
                _description.Text = group.description;
                _isEnabled.Checked = group.isEnabled;

                if (group.pubDate != null) _publishStartDate.Text = group.pubDate.Value.ToString("MM/dd/yyyy HH:mm");
                if (group.pubEndDate != null) _publishEndDate.Text = group.pubEndDate.Value.ToString("MM/dd/yyyy HH:mm");

                ShowHildeControls(true);

            }
        }

        void ResetControls()
        {
            _groupId.Text = "";
            _description.Text = "";
            _title.Text = "";
            _publishStartDate.Text = "";
            _publishEndDate.Text = "";
            _numberOfWinners.Text = "1";
            _isEnabled.Checked = false;

            ShowHildeControls(false);
        }



        void ShowHildeControls(bool show)
        {
            ShowHideControl(btnDeleteContestGroup_, show);
        }


        protected void _list_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int64 id = Convert.ToInt64(e.CommandArgument.ToString());
            switch (e.CommandName.ToLower())
            {
                case "editcontestgroup":
                    PopulateContestGroup(id);
                    break;

            }
        }

        protected void btnNew_Click(object sender, ImageClickEventArgs e)
        {
            ResetControls();
        }


        void DeleteContestGroup(long groupId)
        {
            _contestLib.DeleteContestGroup(groupId);
            ResetControls();
            PopulateContestGroups();

            App_Code.Misc.WriteMessage(_msg, "Contest group has been deleted.");
        }

        protected void btnSaveContestGroup_Click(object sender, ImageClickEventArgs e)
        {
            long groupId = 0;

            AWAPI_Data.Data.awContestGroup grp = new AWAPI_Data.Data.awContestGroup();
            grp.title = _title.Text;
            grp.description = _description.Text;
            grp.isEnabled = _isEnabled.Checked;
            grp.numberOfWinners = Convert.ToInt32(_numberOfWinners.Text);
            grp.pubDate = AWAPI_Common.library.MiscLibrary.ConvertStringToDate(_publishStartDate.Text);
            grp.pubEndDate = AWAPI_Common.library.MiscLibrary.ConvertStringToDate(_publishEndDate.Text, true);

            if (_groupId.Text.Trim().Length == 0)
            {
                groupId = _contestLib.AddContestGroup(App_Code.SessionInfo.CurrentSite.siteId,
                                                         App_Code.SessionInfo.CurrentUser.userId,
                                                        grp.title, grp.description,
                                                        grp.isEnabled, grp.numberOfWinners,
                                                        grp.pubDate, grp.pubEndDate);
            }
            else
            {
                groupId = Convert.ToInt64(_groupId.Text);
                _contestLib.UpdateContestGroup(groupId,
                                            grp.title, grp.description,
                                            grp.isEnabled, grp.numberOfWinners,
                                            grp.pubDate, grp.pubEndDate);
            
            }

            PopulateContestGroups();
            PopulateContestGroup(groupId);

            App_Code.Misc.WriteMessage (App_Code.Misc.MessageType.INFO, _msg, "Contest group has been saved.");
        }

        protected void btnDeleteContestGroup_Click(object sender, ImageClickEventArgs e)
        {
            if (_groupId.Text.Trim().Length > 0)
                DeleteContestGroup(Convert.ToInt64(_groupId.Text));

        }


        protected void btnNewContestGroup_Click(object sender, EventArgs e)
        {
            ResetControls();
        }



    }
}
