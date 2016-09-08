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
    public partial class manageblogfiles : AWAPI.App_Code.AdminBasePage
    {
        AWAPI_BusinessLibrary.library.BlogLibrary _blogLib = new AWAPI_BusinessLibrary.library.BlogLibrary();
        AWAPI_BusinessLibrary.library.FileLibrary _fileLib = new AWAPI_BusinessLibrary.library.FileLibrary();

        Int64 BlogPostId
        {
            get
            {
                if (Request["postid"] == null)
                    return 0;
                return Convert.ToInt64(Request["postid"]);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateFileGroupList();
                PopulateFileList();
                PopulateBlogPostFileList();
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "", "$(document).ready(function(){ screenshotPreview(); });", true);
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

            if (list != null || list.Count > 0)
            {
                var tmp = from l in list
                          where l.isEnabled
                          select l;
                if (tmp == null || tmp.Count() == 0)
                    list = null;
                else
                    list = tmp.ToList();
            }

            _fileList.DataSource = list;
            _fileList.DataBind();
        }

        void PopulateFileGroupList()
        {
            var list = _fileLib.GetFileGroupListBySiteId(App_Code.SessionInfo.CurrentSite.siteId);
            ddlGroupList.DataValueField = "filegroupid";
            ddlGroupList.DataTextField = "title";
            ddlGroupList.DataSource = list;
            ddlGroupList.DataBind();
            ddlGroupList.Items.Insert(0, new ListItem("All", ""));
            ddlGroupList.SelectedIndex = 0;
        }

        void PopulateBlogPostFileList()
        {
            _blogPostFileList.DataSource = _blogLib.GetBlogPostFileList(BlogPostId);
            _blogPostFileList.DataBind();
        }

        void ResetControls()
        {
            _blogPostFileList.DataSource = null;
            _blogPostFileList.DataBind();

            ShowHildeControls(false);
        }

        void ShowHildeControls(bool show)
        {
        }

        protected void ddlGroupList_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateFileList();
        }

        protected void lbtnFilter_Click(object sender, EventArgs e)
        {
            PopulateFileList();
        }

        protected void lbtnClear_Click(object sender, EventArgs e)
        {
            txbFilter.Text = "";
            PopulateFileList();
        }



        protected void _blogPostFileList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            long fileId = Convert.ToInt64(e.CommandArgument.ToString());

            switch (e.CommandName.ToLower())
            {
                case "deletefile":
                    DeleteFileFromBlogPost(fileId);
                    break;
            }
        }



        void DeleteFileFromBlogPost(long fileId)
        {
            try
            {
                _blogLib.RemoveFileBlogPostRelationByFileId(fileId);
                PopulateBlogPostFileList();
                App_Code.Misc.WriteMessage(_msg, "File has been removed from the post.");
            }
            catch (Exception ex)
            {
                App_Code.Misc.WriteMessage(App_Code.Misc.MessageType.ERROR, _msg, ex.Message);
            }

        }

        void AddFileToBlogPost(long fileId)
        {
            try
            {
                _blogLib.SaveFileToBlogPost(BlogPostId, fileId, 0);
                PopulateBlogPostFileList();
                App_Code.Misc.WriteMessage(_msg, "File has been added to the post.");
            }
            catch (Exception ex)
            {
                App_Code.Misc.WriteMessage(App_Code.Misc.MessageType.ERROR, _msg, ex.Message);
            }

        }

        protected void _fileList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            long fileId = Convert.ToInt64(e.CommandArgument.ToString());


            switch (e.CommandName.ToLower())
            {
                case "addfiletopost":
                    AddFileToBlogPost(fileId);
                    break;
            }
        }

        protected void _fileList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lbtn = (LinkButton)e.Row.FindControl("lbtnSelect");
                string contentType = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "contenttype")).ToLower().Trim();
                Image img = (Image)e.Row.FindControl("_fileIcon");
                long fileId = Convert.ToInt64(DataBinder.Eval(e.Row.DataItem, "fileid"));

                //set previoew image
                if (contentType.IndexOf("image") >= 0)
                    lbtn.Attributes.Add("rel", AWAPI_BusinessLibrary.library.ConfigurationLibrary.Config.fileServiceUrl + 
                                            "?id=" + fileId + "&size=100x100");
                else if (contentType.IndexOf("video") >= 0)
                    lbtn.Attributes.Add("rel", AWAPI_BusinessLibrary.library.ConfigurationLibrary.Config.fileServiceUrl +
                                            "?id=" + fileId + "&method=getvideopreview&size=100x100");


                //set file icon
                img.ImageUrl = App_Code.Misc.GetContentIcon(contentType, "1616");
                img.ToolTip = "Content type: " + contentType;
            }
        }

        protected void _fileList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            _fileList.PageIndex = e.NewPageIndex;
            PopulateFileList();
        }

        protected void _blogPostFileList_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            _blogPostFileList.PageIndex = e.NewSelectedIndex;
            PopulateBlogPostFileList();
                
            
        }

        protected void _blogPostFileList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            _blogPostFileList.PageIndex = e.NewPageIndex;
            PopulateBlogPostFileList();
        }

    }
}
