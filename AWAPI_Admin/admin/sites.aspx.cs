using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using AWAPI_Data.Data;

namespace AWAPI.Admin
{
    public partial class PageSites : AWAPI.App_Code.AdminBasePage
    {
        AWAPI_BusinessLibrary.library.SiteLibrary _siteLib = new AWAPI_BusinessLibrary.library.SiteLibrary();

        long SiteId
        {
            get
            {
                if (Request["siteid"] == null)
                    return 0;
                return Convert.ToInt64(Request["siteid"]);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!App_Code.SessionInfo.CurrentUser.isSuperAdmin)
                Response.Redirect("~/admin/nopermission.aspx");

            if (!IsPostBack)
            {
                ResetControls();
                PopulateSiteList();
                //PopulateSiteOwners();
                PopulateSiteCultures();

                if (SiteId > 0)
                    PopulateSite(SiteId);
                else if (App_Code.SessionInfo.CurrentSite != null)
                    PopulateSite(App_Code.SessionInfo.CurrentSite.siteId);
                    
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
            script.Append("    $(\"a[rel='managetags']\").colorbox({width:'500', height:'450', iframe:true}); \n");
            script.Append("    $(\"a[rel='managecultures']\").colorbox({width:'500', height:'500', iframe:true}); \n");
            script.Append("    $(\"a[rel='manageenvironments']\").colorbox({width:'900', height:'600', iframe:true}); \n");
            script.Append("    $(\"a[rel='manageemailtemplates']\").colorbox({width:'900', height:'620', iframe:true}); \n");
            script.Append("    aliasNameTrigger('" + _alias.ClientID + "', '" + _title.ClientID + "'); \n");
            script.Append("}); \n");

            if (script.Length > 0)
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "", script.ToString(), true);
        }

        /// <summary>
        /// 
        /// </summary>
        public void PopulateSiteList()
        {
            gwSiteList.DataSource = _siteLib.GetUserSiteList(App_Code.SessionInfo.CurrentUser.userId);
            gwSiteList.DataBind();
        }

        protected void gwSiteList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string value = e.CommandArgument.ToString();
            switch (e.CommandName.ToLower().Trim())
            {
                case "edit_site":
                    PopulateSite(Convert.ToInt64(value));
                    break;
            }
        }

        void PopulateSiteCultures()
        {
            AWAPI_BusinessLibrary.library.CultureLibrary lib = new AWAPI_BusinessLibrary.library.CultureLibrary();

            _cultureCode.DataValueField = "cultureCode";
            _cultureCode.DataTextField = "title";

            _cultureCode.DataSource = lib.GetList();
            _cultureCode.DataBind();

            _cultureCode.Items.Insert(0, new ListItem("None", ""));
            _cultureCode.SelectedIndex = 0;
        }

        void PopulateSiteEmailTemplates(long siteId, long? selectedUserConfirmEmailTemplateId, long? selectedResetPasswordEmailTemplateId)
        {
            AWAPI_BusinessLibrary.library.EmailTemplateLib lib = new AWAPI_BusinessLibrary.library.EmailTemplateLib();
            IList<awEmailTemplate> list = lib.GetList(siteId);

            
            _userConfirmationEmailTemplate.Items.Clear();            
            _userConfirmationEmailTemplate.DataValueField = "emailTemplateId";
            _userConfirmationEmailTemplate.DataTextField = "title";                       
            _userConfirmationEmailTemplate.DataSource = list;
            _userConfirmationEmailTemplate.DataBind();
            _userConfirmationEmailTemplate.Items.Insert(0, new ListItem("None", ""));
            if (selectedUserConfirmEmailTemplateId != null && selectedUserConfirmEmailTemplateId > 0)
                _userConfirmationEmailTemplate.SelectedValue = selectedUserConfirmEmailTemplateId.ToString();
            else
                _userConfirmationEmailTemplate.SelectedIndex = 0;


            _userResetPasswordEmailTemplate.Items.Clear();
            _userResetPasswordEmailTemplate.DataValueField = "emailTemplateId";
            _userResetPasswordEmailTemplate.DataTextField = "title";
            _userResetPasswordEmailTemplate.DataSource = list;
            _userResetPasswordEmailTemplate.DataBind();
            _userResetPasswordEmailTemplate.Items.Insert(0, new ListItem("None", ""));
            if (selectedResetPasswordEmailTemplateId!= null &&  selectedResetPasswordEmailTemplateId > 0)
                _userResetPasswordEmailTemplate.SelectedValue = selectedResetPasswordEmailTemplateId.ToString();
            else
                _userResetPasswordEmailTemplate.SelectedIndex = 0;
            
        }

        void ResetControls()
        {
            _siteId.Text = "";
            _alias.Text = "";
            _title.Text = "";
            _description.Text = "";
            _link.Text = "";
            _imageUrl.Text = "";
            _grantedIPs.Text = "";
            _bannedIPs.Text = "";
            _accessKey.Text = Guid.NewGuid().ToString().ToLower();
            _twitterUsername.Text = "";
            _twitterPassword.Text = "";
            _fileAmazonS3BucketName.Text = "";

            _cultureCode.SelectedIndex = 0;

            _userConfirmationEmailTemplate.DataSource = null;
            _userConfirmationEmailTemplate.DataBind();

            _userResetPasswordEmailTemplate.DataSource = null;
            _userResetPasswordEmailTemplate.DataBind();

            ShowHideControls(false);
        }

        void ShowHideControls(bool show)
        {
            ShowHideControl(btnDeleteSite_, show);
            ShowHideControl(lblDeleteSite_, show);

            ShowHideControl(_tagsLink, show);
            ShowHideControl(_tagsLinkText, show);

            ShowHideControl(_culturesLink, show);
            ShowHideControl(_culturesLinkText, show);

            ShowHideControl(_environmentLink, show);
            ShowHideControl(_environmentsText, show);

            ShowHideControl(hplEmailTemplate_, show);
            ShowHideControl(lblEmailTemplate_, show);

            if (show)
            {
                _tagsLink.NavigateUrl = "~/admin/frames/managetags.aspx?siteid=" + _siteId.Text;
                _culturesLink.NavigateUrl = "~/admin/frames/managecultures.aspx?siteid=" + _siteId.Text;
                _environmentLink.NavigateUrl = "~/admin/frames/manageenvironments.aspx?siteid=" + _siteId.Text;
                hplEmailTemplate_.NavigateUrl = "~/admin/frames/manageemailtemplates.aspx?siteid=" + _siteId.Text;
            }
        }

        void PopulateSite(long siteId)
        {
            ResetControls();

            AWAPI_Data.Data.awSite site = _siteLib.Get(siteId);
            if (site == null)
                return;
            _siteId.Text = site.siteId.ToString();
            _alias.Text = site.alias;
            _title.Text = site.title;
            _description.Text = site.description;
            _link.Text = site.link;
            _imageUrl.Text = site.imageurl;
            _enabled.Checked = site.isEnabled;
            _maxBlogs.Text = site.maxBlogs.ToString();
            _maxUsers.Text = site.maxUsers.ToString();
            _maxContent.Text = site.maxContents.ToString();
            _grantedIPs.Text = site.grantedDomains;
            _bannedIPs.Text = site.bannedDomains;
            _accessKey.Text = site.accessKey;
            _twitterUsername.Text = site.twitterUsername;
            _twitterPassword.Text = site.twitterPassword;
            _fileAmazonS3BucketName.Text = site.fileAmazonS3BucketName;

            if (site.cultureCode != null && _cultureCode.Items.FindByValue(site.cultureCode) != null)
                _cultureCode.SelectedValue = site.cultureCode;

            PopulateSiteEmailTemplates(siteId, site.userConfirmationEmailTemplateId, site.userResetPasswordEmailTemplateId);

            ShowHideControls(true);
        }

        protected void btnSaveSite_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Save();
        }

        void Save()
        {
            long siteId = 0;

            AWAPI_Data.Data.awSite site = new AWAPI_Data.Data.awSite();

            try
            {
                site.alias = _alias.Text;
                site.title = _title.Text;
                site.description = _description.Text;
                site.isEnabled = _enabled.Checked;
                site.link = _link.Text;
                site.imageurl = _imageUrl.Text;
                site.maxBlogs = Convert.ToInt32(_maxBlogs.Text);
                site.maxUsers = Convert.ToInt32(_maxUsers.Text);
                site.maxContents = Convert.ToInt32(_maxContent.Text);
                site.grantedDomains = _grantedIPs.Text;
                site.bannedDomains = _bannedIPs.Text;
                site.accessKey = _accessKey.Text;
                site.twitterUsername = _twitterUsername.Text;
                site.twitterPassword = _twitterPassword.Text;
                site.fileAmazonS3BucketName = _fileAmazonS3BucketName.Text;
                if (_userConfirmationEmailTemplate.SelectedIndex>0)
                    site.userConfirmationEmailTemplateId = Convert.ToInt64(_userConfirmationEmailTemplate.SelectedValue);
                if (_userResetPasswordEmailTemplate.SelectedIndex>0)
                    site.userResetPasswordEmailTemplateId = Convert.ToInt64(_userResetPasswordEmailTemplate.SelectedValue);

                if (_cultureCode.SelectedValue != "")
                    site.cultureCode = _cultureCode.SelectedValue;

                site.userId = App_Code.SessionInfo.CurrentUser.userId;

                if (_siteId.Text.Trim().Length == 0)
                {
                    siteId = _siteLib.Add(site.userId.Value,
                                        site.alias, site.title, site.description, site.isEnabled, site.link,
                                        site.imageurl, site.maxBlogs, site.maxUsers, site.maxContents, site.cultureCode,
                                        site.grantedDomains, site.bannedDomains, site.accessKey,
                                        site.twitterUsername, site.twitterPassword, site.fileAmazonS3BucketName,
                                        site.userConfirmationEmailTemplateId, site.userResetPasswordEmailTemplateId,
                                        site.pubDate);
                    _siteId.Text = siteId.ToString();
                }
                else
                {
                    siteId = Convert.ToInt64(_siteId.Text);
                    _siteLib.Update(siteId, site.userId.Value,
                                    site.alias, site.title, site.description, site.isEnabled,
                                    site.link, site.imageurl,
                                    site.maxBlogs, site.maxUsers, site.maxContents, site.cultureCode,
                                    site.grantedDomains, site.bannedDomains, site.accessKey,
                                    site.twitterUsername, site.twitterPassword, site.fileAmazonS3BucketName, 
                                    site.userConfirmationEmailTemplateId, site.userResetPasswordEmailTemplateId,
                                    site.pubDate);

                }

                ShowHideControls(true);

                PopulateSiteList();

                AdminMaster.WriteMessage(AWAPI.Admin.AdminMaster.MessageType.INFO, "Site has been saved.");
            }
            catch (Exception ex)
            {
                AdminMaster.WriteMessage(AWAPI.Admin.AdminMaster.MessageType.ERROR, ex.Message);
            }
        }

        protected void btnNewSite_Click(object sender, EventArgs e)
        {
            ResetControls();
        }

        protected void ibtnNewGuId_Click(object sender, ImageClickEventArgs e)
        {
            _accessKey.Text = Guid.NewGuid().ToString().ToLower();
        }

        protected void gwSiteList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lbtn = (LinkButton)e.Row.FindControl("lbtnSelect");
                if (lbtn != null)
                {
                    bool enabled = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "isEnabled"));
                    if (!enabled)
                        lbtn.ForeColor = System.Drawing.Color.Gray;
                }
            }
        }




    }
}
