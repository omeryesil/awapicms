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
using AWAPI_BusinessLibrary.library;
using AWAPI_Data.Data;

namespace AWAPI.Admin
{
    public partial class PageDefault : AWAPI.App_Code.AdminBasePage
    {
        AWAPI_BusinessLibrary.library.BlogLibrary _blogLib = new AWAPI_BusinessLibrary.library.BlogLibrary();
        AWAPI_BusinessLibrary.library.FileLibrary _fileLib = new FileLibrary();
        AWAPI_BusinessLibrary.library.ShareItLibrary _shareItLib = new ShareItLibrary();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateInfo();
                PreparePageControls();
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            //RegisterCustomScript();
        }

        void RegisterCustomScript()
        {
            System.Text.StringBuilder script = new System.Text.StringBuilder();

            //if (_contentId.Text != "")
            //    script.Append("parent.location.hash = '" + _contentId.Text + "'");

            script.Append("$(document).ready(function(){ \n");
            //script.Append("    $(\"a[rel='updateprofile']\").colorbox({width:'960', height:'750', iframe:true}); \n");
            script.Append("}); \n");

            if (script.Length > 0)
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "", script.ToString(), true);

        }

        void PreparePageControls()
        {
            #region GET USER RIGHTS FOR EACH MODULE
            //Get Roles for each module
            AWAPI_Data.Data.awRole rlBlogPosts = App_Code.UserInfo.GetUserRole(true, RoleLibrary.Module.blogpost.ToString());
            AWAPI_Data.Data.awRole rlBlogComment = App_Code.UserInfo.GetUserRole(true, RoleLibrary.Module.blogpostcomment.ToString());

            AWAPI_Data.Data.awRole rlContent = App_Code.UserInfo.GetUserRole(false, RoleLibrary.Module.content.ToString());
            AWAPI_Data.Data.awRole rlFiles = App_Code.UserInfo.GetUserRole(false, RoleLibrary.Module.file.ToString());
            AWAPI_Data.Data.awRole rlPolls = App_Code.UserInfo.GetUserRole(false, RoleLibrary.Module.poll.ToString());
            AWAPI_Data.Data.awRole rlUsers = App_Code.UserInfo.GetUserRole(false, RoleLibrary.Module.user.ToString());
            //AWAPI_Data.Data.awRole roleSite = App_Code.UserInfo.GetUserRole(false, "poll"); //poll is only available for the siteadmin
            #endregion

            pnlShareIt.Visible = false;
            PopulateShareIt();

            pnlPendingComments.Visible = false;
            if (App_Code.SessionInfo.CurrentUser.isSuperAdmin || rlBlogComment.canAdd ||
                rlBlogComment.canDelete || rlBlogComment.canRead || rlBlogComment.canUpdate || rlBlogComment.canUpdateStatus)
                PopulateRecentComments();

            pnlRecentPosts.Visible = false;
            if (App_Code.SessionInfo.CurrentUser.isSuperAdmin || rlBlogPosts.canAdd ||
                rlBlogPosts.canDelete || rlBlogPosts.canRead || rlBlogPosts.canUpdate || rlBlogPosts.canUpdateStatus)
                PopulateRecentPosts();

            pnlRecentImages.Visible = false;
            if (App_Code.SessionInfo.CurrentUser.isSuperAdmin || rlFiles.canAdd ||
                rlFiles.canDelete || rlFiles.canRead || rlFiles.canUpdate || rlFiles.canUpdateStatus)
                PopulateRecentImages();

            pnlUnApprovedFiles.Visible = false;
            if (App_Code.SessionInfo.CurrentUser.isSuperAdmin || rlFiles.canAdd ||
                rlFiles.canDelete || rlFiles.canUpdate || rlFiles.canUpdateStatus)
                PopulateUnApprovedFiles();
        }

        protected void PopulateInfo()
        {
            _userName.Text = AWAPI.App_Code.SessionInfo.CurrentUser.firstName + " " +  AWAPI.App_Code.SessionInfo.CurrentUser.lastName;

            if (AWAPI.App_Code.SessionInfo.CurrentUser.imageurl != null &&
                AWAPI.App_Code.SessionInfo.CurrentUser.imageurl != "")
                _userImage.ImageUrl = AWAPI_BusinessLibrary.library.FileLibrary.GetUrl(AWAPI.App_Code.SessionInfo.CurrentUser.imageurl, "108x108");
            _userEmail.Text = AWAPI.App_Code.SessionInfo.CurrentUser.email;

            if (App_Code.SessionInfo.CurrentSite != null)
                _currentsiteId.Text = App_Code.SessionInfo.CurrentSite.siteId.ToString();

            if (AWAPI_BusinessLibrary.library.ConfigurationLibrary.Config.fileSaveOnAmazonS3)
            {
                _serverFileLocation.ImageUrl = "~/admin/img/button/1616/amazon-ws-icon.png";
                _serverFileLocation.ToolTip = "Files will be saved on Amazon S3";
                _serverFileLocationTitle.Text = "Files will be saved on Amazon S3";
            }
            else
            {
                _serverFileLocation.ImageUrl = "~/admin/img/button/1616/hardrive-icon.gif";
                _serverFileLocation.ToolTip = "Files will be saved on Local Server";
                _serverFileLocationTitle.Text = "Files will be saved on Local Server";
            }


            _serverBlogHandlerLink.NavigateUrl = AWAPI_BusinessLibrary.library.ConfigurationLibrary.Config.blogServiceUrl;
            _serverBlogHandlerLink.Text = AWAPI_BusinessLibrary.library.ConfigurationLibrary.Config.blogServiceUrl;

            _serverContentHandlerLink.NavigateUrl = AWAPI_BusinessLibrary.library.ConfigurationLibrary.Config.contentServiceUrl;
            _serverContentHandlerLink.Text = AWAPI_BusinessLibrary.library.ConfigurationLibrary.Config.contentServiceUrl;

            _serverFileHandlerLink.NavigateUrl = AWAPI_BusinessLibrary.library.ConfigurationLibrary.Config.fileServiceUrl;
            _serverFileHandlerLink.Text = AWAPI_BusinessLibrary.library.ConfigurationLibrary.Config.fileServiceUrl;

            _serverTwitterHandlerLink.NavigateUrl = AWAPI_BusinessLibrary.library.ConfigurationLibrary.Config.twitterServiceUrl;
            _serverTwitterHandlerLink.Text = AWAPI_BusinessLibrary.library.ConfigurationLibrary.Config.twitterServiceUrl;

            _serverWeatherHandlerLink.NavigateUrl = AWAPI_BusinessLibrary.library.ConfigurationLibrary.Config.weatherServiceUrl;
            _serverWeatherHandlerLink.Text = AWAPI_BusinessLibrary.library.ConfigurationLibrary.Config.weatherServiceUrl;
        }

        protected void PopulateShareIt()
        {
            if (App_Code.SessionInfo.CurrentSite == null || App_Code.SessionInfo.CurrentUser == null)
                return;

            System.Collections.Generic.IList<AWAPI_Data.CustomEntities.ShareItExtended> list =
                               _shareItLib.GetUsersShareIts(App_Code.SessionInfo.CurrentSite.siteId, App_Code.SessionInfo.CurrentUser.userId);

            _shareIt.DataSource = list;
            _shareIt.DataBind();

            if (_shareIt.Rows.Count > 0)
                pnlShareIt.Visible = true;
        }

        protected void PopulateRecentComments()
        {
            if (App_Code.SessionInfo.CurrentSite == null)
                return;

            _pendingComments.DataSource = null;
            var list = _blogLib.GetBlogPostCommentListBySiteId(App_Code.SessionInfo.CurrentSite.siteId);
            if (list != null && list.Count > 0)
            {
                var list2 = (from l in list
                             where l.status.Equals(0)
                             select l).Take(5);
                if (list2 != null)
                    _pendingComments.DataSource = list2;
            }
            _pendingComments.DataBind();
            if (_pendingComments.Rows.Count > 0)
                pnlPendingComments.Visible = true;
        }

        protected void PopulateRecentPosts()
        {
            if (App_Code.SessionInfo.CurrentSite == null)
                return;

            _recentPosts.DataSource = null;
            var list = _blogLib.GetBlogPostList(App_Code.SessionInfo.CurrentSite.siteId);
            if (list != null && list.Count > 0)
            {
                var list2 = (from l in list
                             select l).Take(5);

                if (list2 != null)
                    _recentPosts.DataSource = list2;
            }

            _recentPosts.DataBind();

            if (_recentPosts.Rows.Count > 0)
                pnlRecentPosts.Visible = true;
        }

        protected void PopulateRecentImages()
        {
            if (App_Code.SessionInfo.CurrentSite == null)
                return;

            _recentImages.DataSource = null;

            var list = _fileLib.GetList(App_Code.SessionInfo.CurrentSite.siteId, "");

            if (list != null && list.Count > 0)
            {
                var list2 = (from l in list
                             where l.contentType.ToLower().IndexOf("image") >= 0
                             orderby l.createDate descending
                             select l).Take(1);

                if (list2 != null)
                    _recentImages.DataSource = list2;
            }

            _recentImages.DataBind();

            if (_recentImages.Items.Count > 0)
                pnlRecentImages.Visible = true;
        }

        protected void PopulateUnApprovedFiles()
        {
            if (App_Code.SessionInfo.CurrentSite == null)
                return;

            _recentImages.DataSource = null;

            var list = _fileLib.GetList(App_Code.SessionInfo.CurrentSite.siteId, "");

            if (list != null && list.Count > 0)
            {
                var list2 = from l in list
                            where l.isEnabled.Equals(false)
                            orderby l.createDate descending
                            select new
                            {
                                l.fileId,
                                l.title,
                                l.description,
                                l.createDate,
                                username = l.awUser_File== null?"": l.awUser_File.username
                            };

                if (list2 != null)
                    _recentlyUploadedImages.DataSource = list2;
            }

            _recentlyUploadedImages.DataBind();

            if (_recentlyUploadedImages.Rows.Count > 0)
                pnlUnApprovedFiles.Visible = true;
        }

        protected void _recentImages_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item)
            {
                Image img = (Image)e.Item.FindControl("_recentImage");
                long id = Convert.ToInt64(DataBinder.Eval(e.Item.DataItem, "fileid").ToString());
                string path = DataBinder.Eval(e.Item.DataItem, "path").ToString();
                img.ImageUrl = AWAPI_BusinessLibrary.library.FileLibrary.GetUrl(id, path, "150x150");


            }
        }
    }
}
