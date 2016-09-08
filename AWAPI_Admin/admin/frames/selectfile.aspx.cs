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
    public partial class selectfile : AWAPI.App_Code.AdminBasePage
    {
        AWAPI_BusinessLibrary.library.FileLibrary _fileLib = new AWAPI_BusinessLibrary.library.FileLibrary();

        #region PROPERTIES
        public long Last_Selected_GroupId
        {
            get
            {
                if (Session["SELECT_FILE_LAST_SELECTED_GROUP_ID"] == null)
                    return 0;
                return Convert.ToInt64(Session["SELECT_FILE_LAST_SELECTED_GROUP_ID"]);
            }
            set
            {
                Session["SELECT_FILE_LAST_SELECTED_GROUP_ID"] = value;
            }
        }

        public string Last_SearchText
        {
            get
            {
                if (Session["SELECT_FILE_LAST_SEARCH_TEXT"] == null)
                    return "";
                return Session["SELECT_FILE_LAST_SEARCH_TEXT"].ToString();
            }
            set
            {
                Session["SELECT_FILE_LAST_SEARCH_TEXT"] = value;
            }
        }

        


        public string ImageServerLink
        {
            get {
                return AWAPI_BusinessLibrary.library.ConfigurationLibrary.Config.fileServiceUrl + "?id=";
            }
        }


        public string FileType
        {
            get
            {
                if (Request["type"] != null)
                    return Request["type"];
                return "";
            }
        }

        public string CallbackFunction
        {
            get
            {
                if (Request["callback"] != null)
                    return Request["callback"];
                return "";
            }
        }

        public string TargetControlId
        {
            get
            {
                if (Request["targetcontrolid"] != null)
                    return Request["targetcontrolid"];
                return "";
            }
        }

        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Text = "";

            if (!IsPostBack)
            {
                txbFilter.Text = Last_SearchText;
                long groupId = CreateGroupIfNotExists();
                PopulateFileGroups(groupId);

                PopulateFiles();

                CreateGroupIfNotExists();
            }
        }


        long CreateGroupIfNotExists()
        {
            long id = Last_Selected_GroupId;

            string grupName = "General";
            if (Request["group"] != null && Request["group"].Trim() != "")
                grupName = Request["group"];

            AWAPI_Data.Data.awFileGroup group = _fileLib.GetFileGroup(App_Code.SessionInfo.CurrentSite.siteId, grupName.ToLower());
            if (group == null)
            {
                group = new AWAPI_Data.Data.awFileGroup();
                group.title = grupName;
                group.description = "Automatically created.";
                group.siteId = App_Code.SessionInfo.CurrentSite.siteId;
                group.userId = App_Code.SessionInfo.CurrentUser.userId;

                id = _fileLib.AddFileGroup(group.siteId.Value, group.userId.Value, group.title, group.description, null);
            }
            return group.fileGroupId;
        }

        void PopulateFileGroups(long selectedGroupId)
        {
            ddlGroupList.DataValueField = "fileGroupId";
            ddlGroupList.DataTextField = "title";
            ddlGroupList.DataSource = _fileLib.GetFileGroupListBySiteId(App_Code.SessionInfo.CurrentSite.siteId);
            ddlGroupList.DataBind();

            ddlGroupList.Items.Insert(0, new ListItem("All File Groups", "0"));

            if (ddlGroupList.Items.FindByValue(selectedGroupId.ToString()) != null)
                ddlGroupList.SelectedValue = selectedGroupId.ToString();
            else
                ddlGroupList.SelectedIndex = 0;

            //_fileGroupId.Text = ddlGroupList.SelectedValue;
        }


        void PopulateFiles()
        {
            string filterText = txbFilter.Text.Trim().ToLower();

            System.Collections.Generic.IList<AWAPI_Data.Data.awFile> files = null;
            if (ddlGroupList.SelectedValue == "0")
                files = _fileLib.GetList(App_Code.SessionInfo.CurrentSite.siteId, filterText);
            else
                files = _fileLib.GetListByGroupId(Convert.ToInt64(ddlGroupList.SelectedValue), filterText);

            var result = from f in files
                         where f.isEnabled == true &&
                            ((FileType.Trim().Length > 0 && f.contentType.ToLower().IndexOf(FileType) >= 0) ||
                            FileType.Trim().Length == 0)
                         select f;

            _fileList.DataSource = result;
            _fileList.DataBind();
        }

        protected void _fileList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }

        protected void ddlGroupList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //_fileGroupId.Text = ddlGroupList.SelectedValue;
            Last_Selected_GroupId = Convert.ToInt64(ddlGroupList.SelectedValue);
            PopulateFiles();
        }


        void ShowHidePanels(Panel pnl)
        {
            pnlExisting.Visible = false;
            pnlUploadNew.Visible = false;

            pnl.Visible = true;

            if (pnl == pnlUploadNew)
            {
                _fileId.Text = "";
                _fileTitle.Text = "";
                _fileDescription.Text = "";
                _fileImg.Visible = false;
                _fileImg.ImageUrl = "";
                _fileSize.SelectedIndex = 0;
            }

        }

        protected void lbtnUploadNew_Click(object sender, EventArgs e)
        {
            ShowHidePanels(pnlUploadNew);
        }

        protected void btlnCancel_Click(object sender, EventArgs e)
        {
            ShowHidePanels(pnlExisting);
        }

        protected void btnSaveFile__Click(object sender, EventArgs e)
        {
            long groupId = 0;

            try
            {
                if (ddlGroupList.SelectedIndex > 0)
                    groupId = Convert.ToInt64(ddlGroupList.SelectedValue);

                //if group isn't selected, create a new group
                if (groupId == 0)
                {
                    //check if 'General' uploads exists get it else create new 
                    AWAPI_Data.Data.awFileGroup group = _fileLib.GetFileGroup(App_Code.SessionInfo.CurrentSite.siteId, "general");
                    if (group != null)
                        groupId = group.fileGroupId;
                    else
                    {
                        groupId = _fileLib.AddFileGroup(App_Code.SessionInfo.CurrentSite.siteId,
                                             App_Code.SessionInfo.CurrentUser.userId,
                                             "General", "General file uploads", null);
                        PopulateFileGroups(groupId);
                    }

                }

                if (_fileUpload == null || !_fileUpload.HasFile)
                    lblMessage.Text = "Select a file to upload.";
                else
                {

                    //byte[] bytes = new byte[_fileUpload.PostedFile.InputStream.Length];
                    //_fileUpload.PostedFile.InputStream.Read(bytes, 0, (int)_fileUpload.PostedFile.InputStream.Length);
                    //string content = Convert.ToBase64String(bytes);

                    ArrayList groupIds = new ArrayList();
                    groupIds.Add(groupId);

                    long id = _fileLib.Upload(App_Code.SessionInfo.CurrentSite.siteId,
                                    (long[])groupIds.ToArray(typeof(long)), App_Code.SessionInfo.CurrentUser.userId,
                                    _fileTitle.Text, _fileDescription.Text, _fileUpload.PostedFile.FileName, _fileUpload.PostedFile.InputStream, true, _fileSize.SelectedValue);

                    _fileId.Text = id.ToString();

                    _fileUrl.Text = AWAPI_BusinessLibrary.library.ConfigurationLibrary.Config.fileServiceUrl +
                                        "?id=" + id.ToString();

                    _fileImg.ImageUrl = _fileUrl.Text + "&size=150x150";
                    _selectedFileTitle.Text = _fileTitle.Text;

                    PopulateFiles();
                    ShowHidePanels(pnlExisting);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "ERROR: " + ex.Message;
            }
        }

        protected void lbtnFilter_Click(object sender, EventArgs e)
        {
            Last_SearchText = txbFilter.Text;
            PopulateFiles();
        }

        protected void lbtnClear_Click(object sender, EventArgs e)
        {
            txbFilter.Text = "";
            PopulateFiles();
        }

        protected void _fileList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item != null)
            {
                HyperLink hpl = (HyperLink)e.Item.FindControl("hplImage");
                Image img = (Image)e.Item.FindControl("image");
                if (hpl != null && img != null)
                {
                    string path = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "path")).ToLower();
                    long fileId = Convert.ToInt64(DataBinder.Eval(e.Item.DataItem, "fileid"));
                    string title = Convert.ToString(DataBinder.Eval(e.Item.DataItem, "title")).ToLower();

                    string imageUrl = AWAPI_BusinessLibrary.library.ConfigurationLibrary.Config.fileServiceUrl + "?id=" + fileId.ToString(); //AWAPI_BusinessLibrary.library.FileLibrary.GetUrl(fileId, path, "");
                    hpl.NavigateUrl = "javascript:void(0);";
                    hpl.Attributes.Add("onclick", "SetFile('" + imageUrl + "', '" + title.Replace("'", "").Replace("\"", "") + "')");

                    img.ImageUrl = imageUrl + "&size=100x100"; //AWAPI_BusinessLibrary.library.FileLibrary.GetUrl(fileId, path, "100x100"); ;
                }
            }
        }

    }
}
