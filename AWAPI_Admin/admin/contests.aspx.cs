using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AWAPI_Data.Data;


namespace AWAPI.Admin
{
    public partial class PageContests : App_Code.AdminBasePage
    {

        AWAPI_BusinessLibrary.library.ContestLibrary _contestLibrary = new AWAPI_BusinessLibrary.library.ContestLibrary();

        public long ContestId
        {
            get
            {
                if (Request["contestid"] != null)
                    return Convert.ToInt64(Request["contestid"]);
                return 0;
            }
        }

        public PageContests()
        {
            ModuleName = "contest";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hplPopupContestGroupEntry_.Visible = false;
                PopulateEmailTemplates();
                ResetControls();
                PopulateContestGroupList();
                PopulateList();
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            RegisterCustomScript();
        }

        /// <summary>
        /// 
        /// </summary>
        void RegisterCustomScript()
        {
            System.Text.StringBuilder script = new System.Text.StringBuilder();

            script.Append("$(document).ready(function(){ \n");
            script.Append("    $(\"a[rel='contestentries']\").colorbox({width:'960', height:'600', iframe:true}); \n");
            script.Append("    $(\"a[rel='contestusers']\").colorbox({width:'960', height:'600', iframe:true}); \n");
            script.Append("    $(\"a[rel='managecontestgroups']\").colorbox({width:'800', height:'470', iframe:true}); \n");
            script.Append("}); \n");

            if (script.Length > 0)
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "", script.ToString(), true);
        }


        void ResetControls()
        {
            _id.Text = "";
            _title.Text = "";
            _description.Text = "";
            _isEnabled.Checked = false;
            _maxEntry.Text = "0";
            _entryPerUser.Text = "0";
            _entryPerUserPeriodValue.Text = "0";
            _entryPerUserPeriodType.SelectedIndex = 0;
            _numberOfWinners.Text = "0";
            _publishStartDate.Text = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
            _publishEndDate.Text = "";
            hplPopupContestEntry_.NavigateUrl = "";

            _sendEmailToModeratorRecipes.Text = "";
            _sendEmailToModeratorTemplate.SelectedIndex = 0;

            _sendEmailToModeratorRecipes.Text = "";
            _sendEmailToModeratorTemplate.SelectedIndex = 0;
            _sendEmailAfterSubmissionTemplate.SelectedIndex = 0;
            _sendEmailAfterApproveEntryEmailTemplate.SelectedIndex = 0;
            _sendEmailAfterDeleteEntryEmailTemplate.SelectedIndex = 0;

            ResetSelectedFileGroups();

            ShowHideControls(false);
        }

        void ResetSelectedFileGroups()
        {
            foreach (ListItem li in _contestGroupList.Items)
                li.Selected = false;
        }

        void ShowHideControls(bool show)
        {
            ShowHideControl(hplPopupContestEntry_, show);
            ShowHideControl(lblPopupContestEntry_, show);

            ShowHideControl(hplPopupContestUsers_, show);
            ShowHideControl(lblPopupContestUsers_, show);

            ShowHideControl(btnDeleteContest_, show);
            ShowHideControl(lblDeleteContest_, show);

            if (show)
            {
                hplPopupContestEntry_.NavigateUrl = "~/admin/frames/contestentries.aspx?contestid=" + _id.Text;
                hplPopupContestUsers_.NavigateUrl = "~/admin/frames/contestusers.aspx?contestid=" + _id.Text;
            }
        }

        void PopulateEmailTemplates()
        {
            AWAPI_BusinessLibrary.library.EmailTemplateLib lib = new AWAPI_BusinessLibrary.library.EmailTemplateLib();
            IList<AWAPI_Data.Data.awEmailTemplate> list = lib.GetList(App_Code.SessionInfo.CurrentSite.siteId);

            _sendEmailToModeratorTemplate.DataTextField = "title";
            _sendEmailToModeratorTemplate.DataValueField = "emailTemplateId";
            _sendEmailToModeratorTemplate.DataSource = list;
            _sendEmailToModeratorTemplate.DataBind();
            _sendEmailToModeratorTemplate.Items.Insert(0, new ListItem("-Select a template", ""));
            _sendEmailToModeratorTemplate.SelectedIndex = 0;


            _sendEmailAfterSubmissionTemplate.DataTextField = "title";
            _sendEmailAfterSubmissionTemplate.DataValueField = "emailTemplateId";
            _sendEmailAfterSubmissionTemplate.DataSource = list;
            _sendEmailAfterSubmissionTemplate.DataBind();
            _sendEmailAfterSubmissionTemplate.Items.Insert(0, new ListItem("-Select a template", ""));
            _sendEmailAfterSubmissionTemplate.SelectedIndex = 0;


            _sendEmailAfterApproveEntryEmailTemplate.DataTextField = "title";
            _sendEmailAfterApproveEntryEmailTemplate.DataValueField = "emailTemplateId";
            _sendEmailAfterApproveEntryEmailTemplate.DataSource = list;
            _sendEmailAfterApproveEntryEmailTemplate.DataBind();
            _sendEmailAfterApproveEntryEmailTemplate.Items.Insert(0, new ListItem("-Select a template", ""));
            _sendEmailAfterApproveEntryEmailTemplate.SelectedIndex = 0;


            _sendEmailAfterDeleteEntryEmailTemplate.DataTextField = "title";
            _sendEmailAfterDeleteEntryEmailTemplate.DataValueField = "emailTemplateId";
            _sendEmailAfterDeleteEntryEmailTemplate.DataSource = list;
            _sendEmailAfterDeleteEntryEmailTemplate.DataBind();
            _sendEmailAfterDeleteEntryEmailTemplate.Items.Insert(0, new ListItem("-Select a template", ""));
            _sendEmailAfterDeleteEntryEmailTemplate.SelectedIndex = 0;


        }

        void PopulateContestGroupList()
        {
            var list = _contestLibrary.GetContestGroupList(App_Code.SessionInfo.CurrentSite.siteId, "");

            _contestGroupList.DataTextField = "title";
            _contestGroupList.DataValueField = "contestGroupId";
            _contestGroupList.DataSource = list;
            _contestGroupList.DataBind();

            _searchGroupList.DataValueField = "contestGroupId";
            _searchGroupList.DataTextField = "title";
            _searchGroupList.DataSource = list;
            _searchGroupList.DataBind();
            _searchGroupList.Items.Insert(0, new ListItem("All", ""));
            _searchGroupList.SelectedIndex = 0;
        }


        void PopulateList()
        {
            gwList.DataSource = null;

            IList<AWAPI_Data.CustomEntities.ContestExtended> list = null;
            if (_searchGroupList.SelectedIndex == 0)
                list = _contestLibrary.GetContestList(App_Code.SessionInfo.CurrentSite.siteId, false);
            else
                list = _contestLibrary.GetContestListByGroupId(App_Code.SessionInfo.CurrentSite.siteId, Convert.ToInt64(_searchGroupList.SelectedValue), false);

            gwList.DataSource = list;
            gwList.DataBind();

        }

        protected void gwList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gwList.PageIndex = e.NewPageIndex;
            PopulateList();
        }

        protected void gwList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.ToLower())
            {
                case "selectcontest":
                    PopulateContest(Convert.ToInt64(e.CommandArgument.ToString()));
                    break;
            }
        }

        void PopulateContest(Int64 blogId)
        {
            ResetControls();
            AWAPI_Data.CustomEntities.ContestExtended contest = _contestLibrary.GetContest(blogId, false);

            if (contest == null)
                return;

            _id.Text = contest.contestId.ToString();
            _title.Text = contest.title;
            _description.Text = contest.description;
            _isEnabled.Checked = contest.isEnabled;

            _maxEntry.Text = contest.maxEntry.ToString();
            _entryPerUser.Text = contest.entryPerUser.ToString();
            _entryPerUserPeriodValue.Text = contest.entryPerUserPeriodValue.ToString();
            _entryPerUserPeriodType.SelectedIndex = 0;

            _numberOfWinners.Text = contest.numberOfWinners.ToString();

            if (contest.pubDate != null) _publishStartDate.Text = contest.pubDate.Value.ToString("MM/dd/yyyy HH:mm");
            if (contest.pubEndDate != null) _publishEndDate.Text = contest.pubEndDate.Value.ToString("MM/dd/yyyy HH:mm");

            //Send email to the moderators after contest entry creation
            _sendEmailToModeratorRecipes.Text = contest.sendEmailToModeratorRecipes;
            if (contest.sendEmailToModeratorTemplateId != null &&
                _sendEmailToModeratorTemplate.Items.FindByValue(contest.sendEmailToModeratorTemplateId.ToString()) != null)
                _sendEmailToModeratorTemplate.SelectedValue = contest.sendEmailToModeratorTemplateId.ToString();

            //Send email to the contender after contest entry creation
            if (contest.sendEmailAfterSubmissionTemplateId != null &&
                _sendEmailAfterSubmissionTemplate.Items.FindByValue(contest.sendEmailAfterSubmissionTemplateId.ToString()) != null)
                _sendEmailAfterSubmissionTemplate.SelectedValue = contest.sendEmailAfterSubmissionTemplateId.ToString();

            //Send email to the contender if the contest entry has been approved
            if (contest.sendEmailAfterApproveEntryTemplateId != null &&
                _sendEmailAfterApproveEntryEmailTemplate.Items.FindByValue(contest.sendEmailAfterApproveEntryTemplateId.ToString()) != null)
                _sendEmailAfterApproveEntryEmailTemplate.SelectedValue = contest.sendEmailAfterApproveEntryTemplateId.ToString();

            //Send email to the contender if the contest entry has been denied
            if (contest.sendEmailAfterDeleteEntryTemplateId != null &&
                _sendEmailAfterDeleteEntryEmailTemplate.Items.FindByValue(contest.sendEmailAfterDeleteEntryTemplateId.ToString()) != null)
                _sendEmailAfterDeleteEntryEmailTemplate.SelectedValue = contest.sendEmailAfterDeleteEntryTemplateId.ToString();


            PopulateSelectedGroups(contest.contestId);

            ShowHideControls(true);
        }

        void PopulateSelectedGroups(long contestId)
        {
            ResetSelectedFileGroups();

            System.Collections.Generic.IList<AWAPI_Data.Data.awContestGroup> list = _contestLibrary.GetContestGroupListByContestId(contestId);

            foreach (AWAPI_Data.Data.awContestGroup grp in list)
            {
                ListItem li = _contestGroupList.Items.FindByValue(grp.contestGroupId.ToString());
                if (li != null)
                    li.Selected = true;
            }

        }

        protected void btnDeleteContest_Click(object sender, ImageClickEventArgs e)
        {
            _contestLibrary.DeleteContest(Convert.ToInt64(_id.Text));
            ResetControls();
            PopulateList();
        }

        protected void btnNewContest_Click(object sender, ImageClickEventArgs e)
        {
            ResetControls();
        }

        protected void btnSaveContest_Click(object sender, ImageClickEventArgs e)
        {
            Save();
        }

        void Save()
        {
            System.Collections.ArrayList groupIds = new System.Collections.ArrayList();
            long contestId = 0;


            AWAPI_Data.Data.awContest contest = new AWAPI_Data.Data.awContest();

            try
            {
                contest.title = _title.Text;
                contest.description = _description.Text;
                contest.isEnabled = _isEnabled.Checked;
                contest.maxEntry = Convert.ToInt32(_maxEntry.Text);
                contest.entryPerUser = Convert.ToInt32(_entryPerUser.Text);
                contest.entryPerUserPeriodType = _entryPerUserPeriodType.SelectedValue;
                contest.entryPerUserPeriodValue = Convert.ToInt32(_entryPerUserPeriodValue.Text);
                contest.numberOfWinners = Convert.ToInt32(_numberOfWinners.Text);

                contest.pubDate = AWAPI_Common.library.MiscLibrary.ConvertStringToDate(_publishStartDate.Text);
                contest.pubEndDate = AWAPI_Common.library.MiscLibrary.ConvertStringToDate(_publishEndDate.Text, true);

                contest.sendEmailToModeratorRecipes = _sendEmailToModeratorRecipes.Text;
                if (_sendEmailToModeratorTemplate.SelectedIndex > 0)
                    contest.sendEmailToModeratorTemplateId = Convert.ToInt64(_sendEmailToModeratorTemplate.SelectedValue.ToString());

                if (_sendEmailAfterSubmissionTemplate.SelectedIndex > 0)
                    contest.sendEmailAfterSubmissionTemplateId = Convert.ToInt64(_sendEmailAfterSubmissionTemplate.SelectedValue.ToString());

                if (_sendEmailAfterApproveEntryEmailTemplate.SelectedIndex > 0)
                    contest.sendEmailAfterApproveEntryTemplateId = Convert.ToInt64(_sendEmailAfterApproveEntryEmailTemplate.SelectedValue.ToString());

                if (_sendEmailAfterDeleteEntryEmailTemplate.SelectedIndex > 0)
                    contest.sendEmailAfterDeleteEntryTemplateId = Convert.ToInt64(_sendEmailAfterDeleteEntryEmailTemplate.SelectedValue.ToString());

                contest.siteId = App_Code.SessionInfo.CurrentSite.siteId;
                contest.userId = App_Code.SessionInfo.CurrentUser.userId;

                if (_id.Text.Trim().Length == 0)
                {
                    contestId = _contestLibrary.AddContest(contest.siteId, contest.userId.Value,
                                            contest.title, contest.description, contest.isEnabled, contest.maxEntry,
                                            contest.entryPerUser, contest.entryPerUserPeriodValue, contest.entryPerUserPeriodType,
                                            contest.numberOfWinners, contest.pubDate, contest.pubEndDate,
                                            contest.sendEmailToModeratorRecipes, contest.sendEmailToModeratorTemplateId,
                                            contest.sendEmailAfterSubmissionTemplateId,
                                            contest.sendEmailAfterApproveEntryTemplateId,
                                            contest.sendEmailAfterDeleteEntryTemplateId);

                    _id.Text = contestId.ToString();
                }
                else
                {
                    contestId = Convert.ToInt64(_id.Text);
                    _contestLibrary.UpdateContest(contestId, contest.title, contest.description, contest.isEnabled, contest.maxEntry,
                                            contest.entryPerUser, contest.entryPerUserPeriodValue, contest.entryPerUserPeriodType,
                                            contest.numberOfWinners, contest.pubDate, contest.pubEndDate,
                                            contest.sendEmailToModeratorRecipes, contest.sendEmailToModeratorTemplateId,
                                            contest.sendEmailAfterSubmissionTemplateId,
                                            contest.sendEmailAfterApproveEntryTemplateId,
                                            contest.sendEmailAfterDeleteEntryTemplateId);

                }

                //add contest to group
                foreach (ListItem li in _contestGroupList.Items)
                    if (li.Selected)
                        groupIds.Add(Convert.ToInt64(li.Value));

                if (groupIds.Count > 0)
                    _contestLibrary.AddContestToGroups((long[])groupIds.ToArray(typeof(long)), contestId);

                ShowHideControls(true);

                PopulateList();

                AdminMaster.WriteMessage(AWAPI.Admin.AdminMaster.MessageType.INFO, "Contest has been saved.");
            }
            catch (Exception ex)
            {
                AdminMaster.WriteMessage(AWAPI.Admin.AdminMaster.MessageType.ERROR, ex.Message);
            }
        }

        protected void btnRefresh_Click(object sender, ImageClickEventArgs e)
        {
            hplPopupContestGroupEntry_.Visible = false;
            PopulateContestGroupList();
        }

        protected void _searchGroupList_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateList();

            if (_searchGroupList.SelectedIndex == 0)
                hplPopupContestGroupEntry_.Visible = false;
            else
            {
                hplPopupContestGroupEntry_.Visible = true;
                hplPopupContestGroupEntry_.NavigateUrl = "~/admin/frames/contestentries.aspx?groupid=" + _searchGroupList.SelectedValue;
            }
        }

        protected void gwList_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lbtn = (LinkButton)e.Row.FindControl("lbtnSelect");
                if (lbtn != null)
                {
                    bool published = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "isEnabled"));
                    DateTime? pubStart = null,
                                pubEndDate = null;
                    if (DataBinder.Eval(e.Row.DataItem, "pubDate") != null)
                        pubStart = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "pubDate"));
                    if (DataBinder.Eval(e.Row.DataItem, "pubEndDate") != null)
                        pubEndDate = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "pubEndDate"));

                    int result = AWAPI_Common.library.MiscLibrary.IsDateBetween(DateTime.Now, pubStart, pubEndDate);

                    if (!published || result != 1)
                        lbtn.ForeColor = System.Drawing.Color.Gray;
                }
            }
        }

    }
}
