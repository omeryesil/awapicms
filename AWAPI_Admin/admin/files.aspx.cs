using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using AWAPI_Common.library;
using AWAPI_Data.CustomEntities;
using System.Linq;

namespace AWAPI.Admin
{
    public partial class PageFiles : AWAPI.App_Code.AdminBasePage
    {
        AWAPI_BusinessLibrary.library.FileLibrary _fileLib = new AWAPI_BusinessLibrary.library.FileLibrary();

        public long FileId
        {
            get
            {
                if (String.IsNullOrEmpty(Request["fileId"]))
                    return 0;
                return Convert.ToInt64(Request["fileid"]);
            }
        }

        public PageFiles()
        {
            ModuleName = "file";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ResetControls();
                PopulateFileGroupList();
                PopulateFileList();

                if (FileId > 0)
                    PopulateFile(FileId);
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            RegisterCustomScript();
        }

        void RegisterCustomScript()
        {
            System.Text.StringBuilder script = new System.Text.StringBuilder();

            //if (_contentId.Text != "")
            //    script.Append("parent.location.hash = '" + _contentId.Text + "'");

            script.Append("$(document).ready(function(){ \n");
            script.Append("    $(\"a[rel='managefilegroups']\").colorbox({width:'850px', height:'550px', iframe:true}); \n");
            script.Append("    $(\"a[rel='manageshareit']\").colorbox({width:'850px', height:'550px', iframe:true}); \n");
            script.Append("}); \n");

            if (script.Length > 0)
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "", script.ToString(), true);
        }

        void ResetControls()
        {
            _fileId.Text = "";
            _fileTitle.Text = "";
            _fileDescription.Text = "";
            _fileImg.ImageUrl = "";
            _fileImg.Visible = false;
            _fileLink.NavigateUrl = "";
            _fileLink.Visible = false;
            _shareIt.Visible = false;
            _shareIt.ToolTip = "Share this file with other users.";
            _filePreview.Text = "";
            _fileUploadInfo.Text = "";
            _fileSize.SelectedIndex = 0;

            ResetSelectedFileGroups();

            ShowHideControl(btnDeleteFile_, false);
            ShowHideControl(lblDeleteFile_, false);
        }

        void ResetSelectedFileGroups()
        {
            foreach (ListItem li in _fileGroupList.Items)
                li.Selected = false;
        }

        void PopulateFileGroupList()
        {
            var list = _fileLib.GetFileGroupListBySiteId(App_Code.SessionInfo.CurrentSite.siteId);

            _fileGroupList.DataTextField = "title";
            _fileGroupList.DataValueField = "fileGroupId";
            _fileGroupList.DataSource = list;
            _fileGroupList.DataBind();

            ddlGroupList.DataValueField = "filegroupid";
            ddlGroupList.DataTextField = "title";
            ddlGroupList.DataSource = list;
            ddlGroupList.DataBind();
            ddlGroupList.Items.Insert(0, new ListItem("All", ""));
            ddlGroupList.SelectedIndex = 0;
        }


        void PopulateFileList()
        {
            System.Collections.Generic.IList<AWAPI_Data.Data.awFile> list = null;

            _fileList.DataSource = null;
            string filterText = txbFilter.Text.Trim();

            if (ddlGroupList.SelectedIndex > 0)
                list = _fileLib.GetListByGroupId(Convert.ToInt64(ddlGroupList.SelectedValue), filterText);
            else
                list = _fileLib.GetList(App_Code.SessionInfo.CurrentSite.siteId, filterText);

            _fileList.DataSource = list;
            _fileList.DataBind();
        }

        protected void btnSaveFile_Click(object sender, ImageClickEventArgs e)
        {
            System.Collections.ArrayList groupIds = new System.Collections.ArrayList();

            //try
            //{
            foreach (ListItem li in _fileGroupList.Items)
                if (li.Selected)
                    groupIds.Add(Convert.ToInt64(li.Value));

            //if (groupIds.Count == 0)
            //    AdminMaster.WriteMessage(AWAPI.Admin.AdminMaster.MessageType.ERROR, "At least one group must be selected.");
            //else
            //{
            if (_fileId.Text.Trim().Length == 0) //add new one
            {
                if (_fileUpload == null && !_fileUpload.HasFile)
                    throw new Exception("Please select a file to upload.");
                AddFile((long[])groupIds.ToArray(typeof(long)), _fileTitle.Text, _fileDescription.Text, _fileIsEnabled.Checked, _fileUpload, _fileSize.SelectedValue);
            }
            else
                UpdateFile((long[])groupIds.ToArray(typeof(long)),
                            Convert.ToInt64(_fileId.Text),
                            _fileTitle.Text, _fileDescription.Text, _fileIsEnabled.Checked, _fileUpload, _fileSize.SelectedValue);


            PopulateFile(Convert.ToInt64(_fileId.Text.ToString()));
            //}
            //}
            //catch (Exception ex)
            //{
            //    AdminMaster.WriteMessage(AWAPI.Admin.AdminMaster.MessageType.ERROR, ex.Message);
            //}
        }

        void AddFile(long[] groupIds, string title, string description, bool isEnabled, System.Web.UI.WebControls.FileUpload fileUpload, string size)
        {
            if (fileUpload == null || !fileUpload.HasFile)
                AdminMaster.WriteMessage(AWAPI.Admin.AdminMaster.MessageType.ERROR, "Select a file to upload.");
            else if (title.Trim().Length == 0)
                AdminMaster.WriteMessage(AWAPI.Admin.AdminMaster.MessageType.ERROR, "File title is required.");
            else
            {

                //byte[] bytes = new byte[fileUpload.PostedFile.InputStream.Length];
                //fileUpload.PostedFile.InputStream.Read(bytes, 0, (int)fileUpload.PostedFile.InputStream.Length);
                //string content = Convert.ToBase64String(bytes);

                long id = _fileLib.Upload(App_Code.SessionInfo.CurrentSite.siteId,
                                groupIds, App_Code.SessionInfo.CurrentUser.userId,
                                title, description, fileUpload.PostedFile.FileName, fileUpload.PostedFile.InputStream, isEnabled, size);

                _fileId.Text = id.ToString();

                ShowHideControl(btnDeleteFile_, true);
                ShowHideControl(lblDeleteFile_, true);

                PopulateFileList();

                AdminMaster.WriteMessage(AWAPI.Admin.AdminMaster.MessageType.INFO, "File has been saved.");
            }
        }


        void UpdateFile(long[] groupIds, long fileId, string title, string description, bool isEnabled, System.Web.UI.WebControls.FileUpload fileUpload, string size)
        {
            if (title.Trim().Length == 0)
                AdminMaster.WriteMessage(AWAPI.Admin.AdminMaster.MessageType.ERROR, "File title is required.");
            else
            {
                if (fileUpload.HasFile)
                    _fileLib.Update(Convert.ToInt64(_fileId.Text), groupIds,
                                    title, description, fileUpload.PostedFile.FileName, fileUpload.PostedFile.InputStream, isEnabled, size);
                else
                    _fileLib.Update(Convert.ToInt64(_fileId.Text), groupIds,
                            title, description, fileUpload.PostedFile.FileName, null, isEnabled, "");


                PopulateFileList();
                AdminMaster.WriteMessage(AWAPI.Admin.AdminMaster.MessageType.INFO, "File has been saved.");

            }
        }

        void PopulateSelectedGroups(long fileId)
        {
            ResetSelectedFileGroups();

            System.Collections.Generic.IList<AWAPI_Data.Data.awFileGroup> list = _fileLib.GetFileGroupListByFileId(fileId);

            foreach (AWAPI_Data.Data.awFileGroup grp in list)
            {
                ListItem li = _fileGroupList.Items.FindByValue(grp.fileGroupId.ToString());
                if (li != null)
                    li.Selected = true;
            }

        }


        void PopulateFile(long fileId)
        {
            ResetControls();
            AWAPI_Data.Data.awFile file = _fileLib.Get(fileId);

            if (file == null)
                return;

            _fileId.Text = file.fileId.ToString();
            _fileTitle.Text = file.title;
            _fileDescription.Text = file.description;
            _fileIsEnabled.Checked = file.isEnabled;

            System.Text.StringBuilder uploadInfo = new System.Text.StringBuilder();

            if (file.awUser_File != null)
                uploadInfo.Append("<br/>Uploaded by :<b>" + file.awUser_File.firstName + " " + file.awUser_File.lastName + "</b>");

            uploadInfo.Append("<br/><b>" + file.createDate.ToString() + "</b>");
            uploadInfo.Append("<br/>ContetType: <b>" + file.contentType + "</b>");
            uploadInfo.Append("<br/>Size: <b>" + file.contentSize / 1024 + "KB</b>");
            _fileUploadInfo.Text = uploadInfo.ToString();


            if (file.contentType.IndexOf("image") >= 0)
            {
                _fileImg.Visible = true;
                _fileImg.ImageUrl = AWAPI_BusinessLibrary.library.FileLibrary.GetUrl(fileId, file.path, "150x150");
            }


            string fileUrl = "";
            if (file.path != null)
            {
                if (!file.isOnLocal || file.path.IndexOf("http://") >= 0)
                    fileUrl = file.path;
                else
                    fileUrl = AWAPI_BusinessLibrary.library.ConfigurationLibrary.Config.fileServiceUrl + "?id=" + fileId;
                _fileLink.NavigateUrl = fileUrl;

                _fileLink.Visible = true;

                if (file.contentType == "video/x-flv")
                {
                    string previewUrl = AWAPI_BusinessLibrary.library.ConfigurationLibrary.Config.fileServiceUrl + "?id=" + fileId + "&method=getvideopreview";
                    _filePreview.Text =
                            "<object type='application/x-shockwave-flash' data='includes/swf/player_flv_maxi.swf' width='400' height='300'>" +
                            "<param name='movie' value='includes/swf/player_flv_maxi.swf' />\n" +
                            "<param name='allowFullScreen' value='true' />\n" +
                            "<param name='FlashVars' value='startimage=" + Server.UrlEncode(previewUrl) + "&amp;width=400&height=300&showstop=1&showvolume=1&showtime=1&amp;showfullscreen=1&amp;bgcolor1=189ca8&amp;bgcolor2=0E82CB&amp;playercolor=0E82CB&amp;flv=" + fileUrl + "' />" +
                            "</object>";
                }

                //flv=/medias/KyodaiNoGilga.flv&width=320&height=240&showstop=1&showvolume=1&showtime=1&startimage=/medias/startimage_en.jpg&showfullscreen=1&bgcolor1=189ca8&bgcolor2=085c68&playercolor=085c68
            }

            _shareIt.Visible = true;
            _shareIt.Link = _fileLink.NavigateUrl; // App_Code.Misc.GetUrlOnly() + "?fileid=" + file.fileId.ToString();

            PopulateSelectedGroups(fileId);

            ShowHideControl(btnDeleteFile_, true);
            ShowHideControl(lblDeleteFile_, true);

        }

        protected void btnDeleteFile_Click(object sender, ImageClickEventArgs e)
        {
            if (_fileId.Text.Trim().Length > 0)
                DeleteFile(Convert.ToInt64(_fileId.Text));
        }

        void DeleteFile(long fileId)
        {
            _fileLib.Delete(fileId);
            ResetControls();

            PopulateFileList();
            AdminMaster.WriteMessage(AWAPI.Admin.AdminMaster.MessageType.INFO, "File has been deleted.");
        }


        protected void btnNewFile_Click(object sender, EventArgs e)
        {
            ResetControls();
        }

        protected void lbtnFilter_Click(object sender, EventArgs e)
        {
            PopulateFileList();
        }

        protected void _fileList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.ToLower())
            {
                case "selectfile":
                    long fileId = Convert.ToInt64(e.CommandArgument.ToString());
                    PopulateFile(fileId);
                    break;
            }
        }

        protected void lbtnClear_Click(object sender, EventArgs e)
        {
            ddlGroupList.SelectedIndex = 0;
            txbFilter.Text = "";
            PopulateFileList();
        }

        protected void _fileList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            _fileList.PageIndex = e.NewPageIndex;
            PopulateFileList();
        }

        protected void btnRefreshTree_Click(object sender, ImageClickEventArgs e)
        {
            PopulateFileGroupList();
            PopulateFileList();
        }

        protected void ddlGroupList_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateFileList();
        }

        protected void _fileList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lbtn = (LinkButton)e.Row.FindControl("lbtnSelect");

                bool enabled = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "isEnabled"));
                if (!enabled)
                    lbtn.ForeColor = System.Drawing.Color.Gray;

                //set file icon
                string contentType = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "contenttype")).ToLower().Trim();
                Image img = (Image)e.Row.FindControl("_fileIcon");

                img.ImageUrl = App_Code.Misc.GetContentIcon(contentType, "1616");
                img.ToolTip = "Content type: " + contentType;
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in _fileList.Rows)
            {
                LinkButton button = (LinkButton)row.FindControl("lbtnSelect");
                CheckBox cb = (CheckBox)row.FindControl("_deleteFile");

                if (cb != null && button != null && cb.Checked)
                {
                    long id = Convert.ToInt64(button.CommandArgument);
                    DeleteFile(id);
                }                           
            }

            ResetControls();
        }
    }
}
