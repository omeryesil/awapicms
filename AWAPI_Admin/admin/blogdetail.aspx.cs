using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AWAPI_Data.Data;
using AWAPI_Common.values;
using AWAPI_BusinessLibrary.library;


namespace AWAPI.Admin
{
    public partial class PageBlogDetail : AWAPI.App_Code.AdminBasePage
    {

        #region members
        AWAPI_BusinessLibrary.library.BlogLibrary _blogLib = new AWAPI_BusinessLibrary.library.BlogLibrary();

        public long _blogIdFromUrl
        {
            get
            {
                if (Request["blogid"] != null)
                    return Convert.ToInt64(Request["blogid"]);
                return 0;
            }
        }
        #endregion


        public PageBlogDetail()
        {
            ModuleName = "blog";
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateBlogs();
                PopulateEmailTemplates();
                ResetControls();

                if (_blogIdFromUrl > 0)
                    GetBlog(_blogIdFromUrl);
            }

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            RegisterCustomScript();
        }

        void PopulateBlogs()
        {
            long siteId = App_Code.SessionInfo.CurrentSite.siteId;
            var list = _blogLib.GetBlogList(siteId, "");

            if (list == null)
                blogList.DataSource = null;
            else
                blogList.DataSource = list.ToList();
            blogList.DataBind();
        }

        void PopulateEmailTemplates()
        {
            AWAPI_BusinessLibrary.library.EmailTemplateLib lib = new EmailTemplateLib();
            _emailTemplate.DataTextField = "title";
            _emailTemplate.DataValueField = "emailTemplateId";
            _emailTemplate.DataSource = lib.GetList(App_Code.SessionInfo.CurrentSite.siteId);
            _emailTemplate.DataBind();

            _emailTemplate.Items.Insert(0, new ListItem("-Select a template", ""));
            _emailTemplate.SelectedIndex = 0;

        }

        void RegisterCustomScript()
        {
            System.Text.StringBuilder script = new System.Text.StringBuilder();

            //if (_blogId.Text != "")
            //    script.Append("parent.location.hash = '" + _blogId.Text + "'");

            script.Append("$(document).ready(function(){ \n");

            script.Append("    $(\"a[rel='manageblogcategories']\").colorbox({width:'960px', height:'570px', iframe:true}); \n");
            script.Append("    $(\"a[rel='managetags']\").colorbox({width:'960px', height:'570px', iframe:true}); \n");
            script.Append("    $(\"a[rel='selectfile']\").colorbox({width:'960px', height:'620px', iframe:true}); \n");
            script.Append("    aliasNameTrigger('" + _alias.ClientID + "', '" + _blogTitle.ClientID + "'); \n");
            script.Append("}); \n");

            if (script.Length > 0)
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "", script.ToString(), true);
        }


        void ResetControls()
        {
            _blogId.Text = "";
            _alias.Text = "";
            _blogTitle.Text = "";
            _blogDescription.Value = "";
            _blogImageUrl.Text = "";
            _blogImage.ImageUrl = "";
            _blogImage.Attributes.Add("style", "display:none;");
            _blogPostPage.Text = "";

            _enableCommentEmailNotifier.Checked = false;
            _commentEmailTo.Text = "";
            _emailTemplate.SelectedIndex = 0;

            ShowHideContentButtons(false);
        }

        void ShowHideContentButtons(bool show)
        {
            ShowHideControl(_categoriesLink, show);
            ShowHideControl(_categoriesLinkText, show);

            ShowHideControl(_tagsLink, show);
            ShowHideControl(_tagsLinkText, show);

            ShowHideControl(btnDeleteBlog_, show);
            ShowHideControl(lblDeleteBlog_, show);

            _blogTitle.Enabled = true;
            _blogDescription.Visible = true;

            _blogIsEnabled.Enabled = true;
            _blogImageUrl.Enabled = true;

            pnlCommentEmailNotifier.Visible = _enableCommentEmailNotifier.Checked;

            if (show)
            {
                _categoriesLink.NavigateUrl = "~/admin/frames/manageblogcategories.aspx?blogid=" + _blogId.Text;
                _tagsLink.NavigateUrl = "~/admin/frames/manageblogtags.aspx?blogid=" + _blogId.Text;
            }

        }

        void GetBlog(Int64 blogId)
        {
            ResetControls();
            awBlog blog = _blogLib.GetBlog(blogId);

            if (blog == null)
                return;

            _blogId.Text = blog.blogId.ToString();
            _alias.Text = blog.alias;
            _blogTitle.Text = blog.title;
            _blogDescription.Value = blog.description;
            _blogIsEnabled.Checked = blog.isEnabled;
            _blogImageUrl.Text = blog.imageurl;
            _blogPostPage.Text = blog.blogPostPage;

            if (blog.imageurl != null && blog.imageurl.Trim() != "")
            {
                _blogImage.Attributes.Add("style", "display:'';");
                if (blog.imageurl.ToLower().IndexOf("/handler/") >= 0)
                    _blogImage.ImageUrl = blog.imageurl + "&size=150x150";
                else
                    _blogImage.ImageUrl = blog.imageurl;
            }

            _enableCommentEmailNotifier.Checked = blog.enableCommentEmailNotifier;
            _commentEmailTo.Text = blog.commentEmailTo;
            if (blog.commentEmailTemplateId != null &&
                _emailTemplate.Items.FindByValue(blog.commentEmailTemplateId.Value.ToString()) != null)
                _emailTemplate.SelectedValue = blog.commentEmailTemplateId.Value.ToString();

            ShowHideContentButtons(true);
        }


        protected void btnDelete_Click(object sender, ImageClickEventArgs e)
        {
            DeleteBlog(Convert.ToInt64(_blogId.Text));
            AdminMaster.WriteMessage(AWAPI.Admin.AdminMaster.MessageType.INFO, "Blog has been deleted.");
        }

        void DeleteBlog(long blogId)
        {
            _blogLib.DeleteBlog(blogId);

            ResetControls();
            PopulateBlogs();
        }
        protected void btnNew_Click(object sender, EventArgs e)
        {
            ResetControls();
        }

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Save();
            }
            catch (Exception ex)
            {
                AdminMaster.WriteMessage(AWAPI.Admin.AdminMaster.MessageType.ERROR, ex.Message);
            }
        }

        void Save()
        {
            long blogId = 0;

            awBlog blog = new awBlog();
            blog.alias = _alias.Text.ToLower();
            blog.title = _blogTitle.Text;
            blog.description = _blogDescription.Value;

            blog.siteId = App_Code.SessionInfo.CurrentSite.siteId;
            blog.userId = App_Code.SessionInfo.CurrentUser.userId;
            blog.imageurl = _blogImageUrl.Text;
            blog.isEnabled = _blogIsEnabled.Checked;
            blog.blogPostPage = _blogPostPage.Text;

            blog.enableCommentEmailNotifier = _enableCommentEmailNotifier.Checked;
            blog.commentEmailTo = _commentEmailTo.Text;
            if (_emailTemplate.SelectedIndex > 0)
                blog.commentEmailTemplateId = Convert.ToInt64(_emailTemplate.SelectedValue);
            else
                blog.commentEmailTemplateId = null;

            if (_blogId.Text.Trim().Length == 0)
            {
                blogId = _blogLib.AddBlog(blog.siteId, blog.userId, blog.alias, blog.title, blog.description,
                                    blog.imageurl, blog.isEnabled, blog.enableCommentEmailNotifier, 
                                    blog.commentEmailTo, blog.commentEmailTemplateId, blog.blogPostPage);
                _blogId.Text = blogId.ToString();
            }
            else
            {
                blogId = Convert.ToInt64(_blogId.Text);
                _blogLib.UpdateBlog(blogId, blog.alias, blog.title, blog.description, blog.isEnabled, 
                                    blog.imageurl, blog.enableCommentEmailNotifier, 
                                    blog.commentEmailTo, blog.commentEmailTemplateId, blog.blogPostPage);

            }
            _blogImage.ImageUrl = blog.imageurl + "&size=150x150";

            ShowHideContentButtons(true);

            PopulateBlogs();

            AdminMaster.WriteMessage(AWAPI.Admin.AdminMaster.MessageType.INFO, "Blog has been saved.");
        }


        protected void btnRefreshTree_Click(object sender, ImageClickEventArgs e)
        {
            PopulateBlogs();
        }

        protected void blogList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            long id = Convert.ToInt64(e.CommandArgument.ToString());

            switch (e.CommandName.ToLower())
            {
                case "editblog":
                    GetBlog(id);
                    break;
            }
        }

        protected void blogList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lbtn = (LinkButton)e.Row.FindControl("lbtnSelectBlog");
                if (lbtn != null)
                {
                    bool enabled = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "isEnabled"));
                    if (!enabled)
                        lbtn.ForeColor = System.Drawing.Color.Gray;
                }
            }
        }

        protected void _enableCommentEmailNotifier_CheckedChanged(object sender, EventArgs e)
        {
            pnlCommentEmailNotifier.Visible = _enableCommentEmailNotifier.Checked;
        }



    }
}
