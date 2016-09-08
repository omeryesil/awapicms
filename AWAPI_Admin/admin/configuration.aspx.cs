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
    public partial class PageConfiguration : AWAPI.App_Code.AdminBasePage
    {
        AWAPI_BusinessLibrary.library.ConfigurationLibrary _configLib = new AWAPI_BusinessLibrary.library.ConfigurationLibrary();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!App_Code.SessionInfo.CurrentUser.isSuperAdmin)
                Response.Redirect("~/admin/nopermission.aspx");

            if (!IsPostBack)
            {
                ResetControls();
                PopulateList();

                Populate(AWAPI_BusinessLibrary.library.ConfigurationLibrary.CurrentConfigurationId);
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
            script.Append("    aliasNameTrigger('" + _alias.ClientID + "', '" + _title.ClientID + "'); \n");
            script.Append("}); \n");

            if (script.Length > 0)
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "", script.ToString(), true);
        }


        /// <summary>
        /// 
        /// </summary>
        public void PopulateList()
        {
            _list.DataSource = _configLib.GetList();
            _list.DataBind();
        }

        protected void _list_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string value = e.CommandArgument.ToString();
            switch (e.CommandName.ToLower().Trim())
            {
                case "edit_config":
                    Populate(Convert.ToInt64(value));
                    break;
            }
        }
        void ResetControls()
        {
            _configurationId.Text = "";
            _alias.Text = "";
            _title.Text = "";
            _description.Text = "";

            _fileServiceUrl.Text = "";
            _contentServiceUrl.Text = "";
            _blogServiceUrl.Text = "";
            _pollServiceUrl.Text = "";
            _automatedTaskServiceUrl.Text = "";
            _twitterServiceUrl.Text = "";
            _weatherServiceUrl.Text = "";
            _adminBaseUrl.Text = "";
            _fileDirectory.Text = "";
            _fileMimeXMLPath.Text = "";
            _fileSaveOnAmazonS3.Checked = false;
            _fileAmazonS3AccessKey.Text = "";
            _fileAmazonS3BucketName.Text = "";
            _fileAmazonS3SecreyKey.Text = "";
            _smtpServer.Text = "";

            pnlAmaxonS3.Visible = false;

            ShowHideControls(false);
        }

        void ShowHideControls(bool show)
        {
            ShowHideControl(btnDeleteConfiguration_, show);
            ShowHideControl(lblDeleteConfiguration_, show);
        }

        void Populate(long configId)
        {
            ResetControls();

            AWAPI_Data.Data.awConfiguration config = _configLib.Get(configId);
            if (config == null)
                return;

            _configurationId.Text = config.configurationId.ToString();
            _alias.Text = config.alias;
            _title.Text = config.title;
            _description.Text = config.description;

            _fileServiceUrl.Text = config.fileServiceUrl;
            _contentServiceUrl.Text = config.contentServiceUrl;
            _blogServiceUrl.Text = config.blogServiceUrl;
            _pollServiceUrl.Text = config.pollServiceUrl;
            _automatedTaskServiceUrl.Text = config.automatedTaskServiceUrl;
            _twitterServiceUrl.Text = config.twitterServiceUrl;
            _weatherServiceUrl.Text = config.weatherServiceUrl;
            _adminBaseUrl.Text = config.adminBaseUrl;
            _fileDirectory.Text = config.fileDirectory;
            _fileMimeXMLPath.Text = config.fileMimeXMLPath;
            _fileSaveOnAmazonS3.Checked = config.fileSaveOnAmazonS3;
            _fileAmazonS3AccessKey.Text = config.fileAmazonS3AccessKey;
            _fileAmazonS3BucketName.Text = config.fileAmazonS3BucketName;
            _fileAmazonS3SecreyKey.Text = config.fileAmazonS3SecreyKey;
            _smtpServer.Text = config.smtpServer;


            pnlAmaxonS3.Visible = config.fileSaveOnAmazonS3;

            ShowHideControls(true);
        }

        protected void btnSaveConfiguration_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            Save();
        }

        void Save()
        {
            long configurationId = _configurationId.Text == "" ? 0 : Convert.ToInt64(_configurationId.Text);

            AWAPI_Data.Data.awConfiguration config = new AWAPI_Data.Data.awConfiguration();

            try
            {
                config.alias = _alias.Text; 
                config.title = _title.Text;
                config.description = _description.Text;

                config.fileServiceUrl = _fileServiceUrl.Text;
                config.contentServiceUrl = _contentServiceUrl.Text;
                config.blogServiceUrl = _blogServiceUrl.Text;
                config.pollServiceUrl = _pollServiceUrl.Text;
                config.automatedTaskServiceUrl = _automatedTaskServiceUrl.Text;
                config.twitterServiceUrl = _twitterServiceUrl.Text;
                config.weatherServiceUrl = _weatherServiceUrl.Text;
                config.adminBaseUrl = _adminBaseUrl.Text;
                config.fileDirectory = _fileDirectory.Text;
                config.fileMimeXMLPath = _fileMimeXMLPath.Text;
                config.fileSaveOnAmazonS3 = _fileSaveOnAmazonS3.Checked;
                config.fileAmazonS3AccessKey = _fileAmazonS3AccessKey.Text;
                config.fileAmazonS3BucketName = _fileAmazonS3BucketName.Text;
                config.fileAmazonS3SecreyKey = _fileAmazonS3SecreyKey.Text;
                config.smtpServer = _smtpServer.Text;

                configurationId = _configLib.Save(configurationId,
                            config.alias, config.title, config.description, config.fileServiceUrl,
                            config.contentServiceUrl, config.blogServiceUrl, config.pollServiceUrl,
                            config.automatedTaskServiceUrl, config.twitterServiceUrl, config.weatherServiceUrl,
                            config.adminBaseUrl, config.fileDirectory, config.fileMimeXMLPath, config.fileSaveOnAmazonS3,
                            config.fileAmazonS3BucketName, config.fileAmazonS3AccessKey, config.fileAmazonS3SecreyKey,
                            config.smtpServer);

                _configurationId.Text = configurationId.ToString();

                ShowHideControls(true);

                PopulateList();

                AdminMaster.WriteMessage(AWAPI.Admin.AdminMaster.MessageType.INFO, "Configiration has been saved.");
            }
            catch (Exception ex)
            {
                AdminMaster.WriteMessage(AWAPI.Admin.AdminMaster.MessageType.ERROR, ex.Message);
            }
        }

        protected void btnNewConfiguration_Click(object sender, EventArgs e)
        {
            ResetControls();
        }

        protected void _list_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lbtn = (LinkButton)e.Row.FindControl("lbtnSelect");
                if (lbtn != null)
                {
                    long id = Convert.ToInt64(DataBinder.Eval(e.Row.DataItem, "configurationId"));
                    if (id == AWAPI_BusinessLibrary.library.ConfigurationLibrary.CurrentConfigurationId)
                        lbtn.Text += " (current)";
                }
            }
        }

        protected void btnDeleteConfiguration__Click(object sender, ImageClickEventArgs e)
        {
            long configurationId = Convert.ToInt64(_configurationId.Text);

            if (AWAPI_BusinessLibrary.library.ConfigurationLibrary.CurrentConfigurationId == configurationId)
                AdminMaster.WriteMessage(AWAPI.Admin.AdminMaster.MessageType.INFO, "The configuration you want to delete has been set for the admin panel.<br/>Please update the web.config to delete the configuration.");
            else
            {
                _configLib.Delete(configurationId);
                ResetControls();
            }
        }

        protected void _fileSaveOnAmazonS3_CheckedChanged(object sender, EventArgs e)
        {
            pnlAmaxonS3.Visible = _fileSaveOnAmazonS3.Checked;
        }




    }
}
