using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AWAPI_Data.Data;


namespace AWAPI.Admin
{
    public partial class PagePolls : App_Code.AdminBasePage
    {
        #region MEMBERS
        AWAPI_BusinessLibrary.library.PollLibrary _pollLibrary = new AWAPI_BusinessLibrary.library.PollLibrary();

        public long PollId
        {
            get
            {
                if (Request["pollid"] != null)
                    return Convert.ToInt64(Request["pollId"]);
                return 0;
            }
        }
        #endregion

        public PagePolls()
        {
            ModuleName = "poll";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ResetControls();
                PopulateList();
                PopulateLanguges();

                if (PollId > 0)
                    PopulatePoll(PollId, "");
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
            //script.Append("    $(\"a[rel='managepollchoices']\").colorbox({width:'850px', height:'470px', iframe:true}); \n");
            script.Append("}); \n");

            if (script.Length > 0)
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "", script.ToString(), true);
        }


        void PopulateLanguges()
        {
            AWAPI_BusinessLibrary.library.CultureLibrary lib = new AWAPI_BusinessLibrary.library.CultureLibrary();
            _culture.DataSource = lib.GetListBySiteId(App_Code.SessionInfo.CurrentSite.siteId);
            _culture.DataValueField = "cultureCode";
            _culture.DataTextField = "title";
            _culture.DataBind();

            _culture.Items.Insert(0, new ListItem("None", ""));
            if (!String.IsNullOrEmpty(App_Code.SessionInfo.CurrentSite.cultureCode) &&
                _culture.Items.FindByValue(App_Code.SessionInfo.CurrentSite.cultureCode) != null)
            {
                _culture.SelectedValue = App_Code.SessionInfo.CurrentSite.cultureCode;
                _culture.SelectedItem.Text = _culture.SelectedItem.Text + " (default)";
            }
            else
                _culture.SelectedIndex = 0;
        }

        #region POLL HEADER
        void ResetControls()
        {
            _id.Text = "";
            _title.Text = "";
            _description.Text = "";
            _answeredQuestion.Text = "";
            _isEnabled.Checked = false;
            _isMultipleChoice.Checked = false;
            //_publishEndDate.Text = "";
            _publishStartDate.Text = "";
            _totalNumberOfVotes.Text = "0";

            ResetChoiceControls();
            gwChoiceList.DataSource = null;
            gwChoiceList.DataBind();

            pnlChoices.Visible = false;

            ShowHideControls(false);
        }

        void ResetCultureControl()
        {
            //SET Default Language
            if (!String.IsNullOrEmpty(App_Code.SessionInfo.CurrentSite.cultureCode) &&
                            _culture.Items.FindByValue(App_Code.SessionInfo.CurrentSite.cultureCode) != null)
                _culture.SelectedValue = App_Code.SessionInfo.CurrentSite.cultureCode;
            else
                _culture.SelectedIndex = 0;
        }

        void ShowHideControls(bool show)
        {
            //ShowHideControl(_choicesLink, show);
            //ShowHideControl(_choicesLinkText, show);

            ShowHideControl(btnDeletePoll_, show);
            ShowHideControl(lblDeletePoll_, show);

            _culture.Enabled = show;

            //if (show)
            //{
            //    _choicesLink.NavigateUrl = "~/admin/frames/managepollchoices.aspx?pollid=" + _id.Text;
            //}

            pnlChoices.Visible = show;

        }

        void PopulateList()
        {
            gwList.DataSource = _pollLibrary.GetPollList(App_Code.SessionInfo.CurrentSite.siteId, "", "", false);
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
                case "selectpoll":
                    PopulatePoll(Convert.ToInt64(e.CommandArgument.ToString()), "");
                    break;

            }
        }

        void PopulatePoll(Int64 blogId, string cultureCode)
        {
            ResetControls();

            //set the default culture code if it is emmpty
            if (String.IsNullOrEmpty(cultureCode) &&        //culture code is null
                _culture.Items.FindByValue(App_Code.SessionInfo.CurrentSite.cultureCode) != null)   //session culture code exists in the list
                _culture.SelectedValue = App_Code.SessionInfo.CurrentSite.cultureCode;

            AWAPI_Data.CustomEntities.PollExtended poll = _pollLibrary.GetPoll(blogId, cultureCode, false);

            if (poll == null)
                return;

            _id.Text = poll.pollId.ToString();
            _title.Text = poll.title;
            _description.Text = poll.description;
            _answeredQuestion.Text = poll.answeredQuestion;
            _isEnabled.Checked = poll.isEnabled;
            //_isPublic.Checked = poll.isPublic;
            _isMultipleChoice.Checked = poll.isMultipleChoice;
            if (poll.pubDate != null) _publishStartDate.Text = poll.pubDate.Value.ToString("MM/dd/yyyy HH:mm");
            //if (poll.pubEndDate != null) _publishEndDate.Text = poll.pubEndDate.Value.ToString("MM/dd/yyyy HH:mm");

            PopulateChoiceList();

            ShowHideControls(true);
        }

        protected void btnDeletePoll__Click(object sender, ImageClickEventArgs e)
        {
            _pollLibrary.DeletePoll(Convert.ToInt64(_id.Text));
            ResetControls();
            PopulateList();

            ResetCultureControl();
        }

        protected void btnNewPoll__Click(object sender, ImageClickEventArgs e)
        {
            ResetControls();
            ResetCultureControl();
        }

        protected void btnSavePoll__Click(object sender, ImageClickEventArgs e)
        {
            Save();
        }

        void Save()
        {
            long pollId = 0;

            AWAPI_Data.Data.awPoll poll = new AWAPI_Data.Data.awPoll();

            try
            {
                poll.title = _title.Text;
                poll.description = _description.Text;
                poll.answeredQuestion = _answeredQuestion.Text;
                poll.isEnabled = _isEnabled.Checked;
                poll.isPublic = true; //_isPublic.Checked;
                poll.isMultipleChoice = _isMultipleChoice.Checked;
                poll.pubDate = AWAPI_Common.library.MiscLibrary.ConvertStringToDate(_publishStartDate.Text);
                //poll.pubEndDate = AWAPI_Common.library.MiscLibrary.ConvertStringToDate(_publishEndDate.Text, true);
                poll.siteId = App_Code.SessionInfo.CurrentSite.siteId;
                poll.userId = App_Code.SessionInfo.CurrentUser.userId;


                if (_id.Text.Trim() != "" &&           //if content is selected
                    !String.IsNullOrEmpty(_culture.SelectedValue) &&
                    !String.IsNullOrEmpty(App_Code.SessionInfo.CurrentSite.cultureCode) &&
                    App_Code.SessionInfo.CurrentSite.cultureCode != _culture.SelectedValue)
                {
                    _pollLibrary.UpdatePollForCulture(Convert.ToInt64(_id.Text), _culture.SelectedValue, poll.description, poll.answeredQuestion);
                }
                else
                {
                    if (_id.Text.Trim().Length == 0)
                    {
                        pollId = _pollLibrary.AddPoll(poll.siteId, poll.userId,
                                                poll.title, poll.description, poll.answeredQuestion, poll.isEnabled, poll.isPublic,
                                                poll.isMultipleChoice, poll.pubDate, poll.pubEndDate);

                        _id.Text = pollId.ToString();
                    }
                    else
                    {
                        pollId = Convert.ToInt64(_id.Text);
                        _pollLibrary.UpdatePoll(pollId, poll.title, poll.description, poll.answeredQuestion,
                                            poll.isEnabled, poll.isPublic, poll.isMultipleChoice,
                                            poll.pubDate, poll.pubEndDate);

                    }
                }

                ShowHideControls(true);

                PopulateList();

                AdminMaster.WriteMessage(AWAPI.Admin.AdminMaster.MessageType.INFO, "Poll has been saved.");
            }
            catch (Exception ex)
            {
                AdminMaster.WriteMessage(AWAPI.Admin.AdminMaster.MessageType.ERROR, ex.Message);
            }
        }

        protected void _culture_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_id.Text != "")
                PopulatePoll(Convert.ToInt64(_id.Text), _culture.SelectedValue);

        }
        #endregion

        #region POLL CHOICE
        void PopulateChoiceList()
        {
            gwChoiceList.DataSource = null;
            if (_id.Text != "")
            {
                int totalNumberOfChocies = 0;
                Int64 pollId = Convert.ToInt64(_id.Text);
                string cultureCode = _culture.SelectedValue;
                IList<AWAPI_Data.CustomEntities.PollChoiceExtended> list = _pollLibrary.GetPollChoiceList(pollId, cultureCode);
                if (list != null && list.Count > 0)
                {
                    foreach (AWAPI_Data.CustomEntities.PollChoiceExtended choice in list)
                        totalNumberOfChocies += choice.numberOfVotes;
                    _totalNumberOfVotes.Text = totalNumberOfChocies.ToString();
                }
                gwChoiceList.DataSource = list;
            }
            gwChoiceList.DataBind();
        }

        void ResetChoiceControls()
        {
            _choiceId.Text = "";
            _choiceTitle.Text = "";
            _choiceDescription.Text = "";

            ShowHideChoiceControls(false);
        }

        void ShowHideChoiceControls(bool show)
        {
            btnDeletePoll_Choice.Visible = show;
            lblDeletePoll_Choice.Visible = show;
        }

        void PopulateChoice(Int64 choiceId)
        {
            string cultureCode = _culture.SelectedValue.ToLower();

            //set the default culture code if it is emmpty
            if (String.IsNullOrEmpty(cultureCode) &&        //culture code is null
                _culture.Items.FindByValue(App_Code.SessionInfo.CurrentSite.cultureCode) != null)   //session culture code exists in the list
                _culture.SelectedValue = App_Code.SessionInfo.CurrentSite.cultureCode;

            AWAPI_Data.Data.awPollChoice pc = _pollLibrary.GetPollChoice(choiceId, cultureCode);

            _choiceId.Text = pc.pollChoiceId.ToString();
            _choiceTitle.Text = pc.title;
            _choiceDescription.Text = pc.description;
            _choiceSortOrder.Text = pc.sortOrder.ToString();

            ShowHideChoiceControls(true);
        }

        protected void gwChoiceListFieldList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int64 choiceId = Convert.ToInt64(e.CommandArgument.ToString());
            switch (e.CommandName.ToLower())
            {
                case "editchoice":
                    PopulateChoice(choiceId);
                    break;

            }
        }

        protected void btnNewPoll_Choice_Click(object sender, ImageClickEventArgs e)
        {
            ResetChoiceControls();
        }

        protected void btnSavePoll_Choice_Click(object sender, ImageClickEventArgs e)
        {
            SaveChoice();
        }

        void SaveChoice()
        {
            Int64 pollId = Convert.ToInt64(_id.Text);
            Int64 id = 0;
            AWAPI_Data.Data.awPollChoice pc = new AWAPI_Data.Data.awPollChoice();


            if (_choiceTitle.Text.Trim() == "")
            {
                AdminMaster.WriteMessage(AWAPI.Admin.AdminMaster.MessageType.ERROR, "Choice title is required.");
                return;
            }


            try
            {
                pc.title = _choiceTitle.Text;
                pc.description = _choiceDescription.Text;
                pc.sortOrder = Convert.ToInt32(_choiceSortOrder.Text);

                if (_choiceId.Text.Trim() != "" && !String.IsNullOrEmpty(_culture.SelectedValue) &&
                    !String.IsNullOrEmpty(App_Code.SessionInfo.CurrentSite.cultureCode) && App_Code.SessionInfo.CurrentSite.cultureCode != _culture.SelectedValue)
                    _pollLibrary.UpdatePollChoiceForCulture(Convert.ToInt64(_choiceId.Text), _culture.SelectedValue, pc.title, pc.description);
                else
                {
                    if (_choiceId.Text.Trim() == "")
                        id = _pollLibrary.AddPollChoice(pollId, pc.title, pc.description, pc.sortOrder);
                    else
                    {
                        id = Convert.ToInt64(_choiceId.Text);
                        _pollLibrary.UpdatePollChoice(id, pc.title, pc.description, pc.sortOrder);
                    }
                    _choiceId.Text = id.ToString();

                }
                ShowHideChoiceControls(true);
                PopulateChoiceList();

                AdminMaster.WriteMessage(AWAPI.Admin.AdminMaster.MessageType.INFO, "Poll choice has been saved.");
            }
            catch (Exception ex)
            {
                AdminMaster.WriteMessage(AWAPI.Admin.AdminMaster.MessageType.ERROR, ex.Message);
            }
        }

        protected void btnDeletePoll_Choice_Click(object sender, ImageClickEventArgs e)
        {
            DeleteChoice();
        }

        void DeleteChoice()
        {
            Int64 pollId = Convert.ToInt64(_choiceId.Text);
            _pollLibrary.DeletePollChoice(pollId);
            ResetChoiceControls();
            PopulateChoiceList();

            AdminMaster.WriteMessage(AWAPI.Admin.AdminMaster.MessageType.INFO, "Poll choice has been deleted.");
        }

        #endregion

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

        protected void gwChoiceList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Image choiceBar = (Image)e.Row.FindControl("_choiceBar");
                if (choiceBar != null)
                {
                    if (!String.IsNullOrEmpty(_totalNumberOfVotes.Text))
                    {
                        int totalVotes = Convert.ToInt32(_totalNumberOfVotes.Text);
                        if (totalVotes > 0)
                        {
                            int numberOfVotes = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "numberOfVotes"));
                            double percentage = numberOfVotes * 75 / totalVotes;
                            if (percentage > 0)
                            {
                                choiceBar.Width = new Unit(percentage, UnitType.Pixel);
                                choiceBar.Visible = true;
                            }

                        }
                    }
                }

            }
        }

    }
}
