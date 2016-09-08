using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AWAPI_Data.Data;

namespace AWAPI.Admin
{
    public partial class PageBlogPostDetail : AWAPI.App_Code.AdminBasePage
    {

        #region consts
        const string FORMAT_SELECTED_START = "<b><font color='#0000ff'>";
        const string FORMAT_SELECTED_END = "</font></b>";
        const string FORMAT_CONTENT_SAVED = " (<i>saved</i>)";
        const string FORMAT_DISABLED_START = "<font color='#bbbbbb'><i>";
        const string FORMAT_DISABLED_END = "</i></font>";
        const string FORMAT_ENABLED_IMAGE = "<img  src='img/blog-enabled.gif'/>";
        const string FORMAT_DISABLED_IMAGE = "<img src='img/blog-disabled.gif'/>";
        const string FORMAT_PUBLISHED_IMAGE = "<img style='border:none'  src='img/blog-published.gif'/>";
        const string FORMAT_NOT_PUBLISHED_IMAGE = "<img style='border:none'  src='img/blog-not-published.gif'/>";
        #endregion

        #region members
        AWAPI_BusinessLibrary.library.BlogLibrary _blogLib = new AWAPI_BusinessLibrary.library.BlogLibrary();

        public long _blogId
        {
            get
            {
                if (ViewState["blogid"] != null)
                    return Convert.ToInt64(ViewState["blogid"]);
                if (Request["blogid"] != null)
                    ViewState["blogid"] = Convert.ToInt64(Request["blogid"]);
                else
                    ViewState["blogid"] = -1;

                return Convert.ToInt64(ViewState["blogid"]);
            }
            set
            {
                ViewState["blogid"] = value;
            }
        }

        public long _idFromUrl
        {
            get
            {
                if (Request["postid"] != null)
                    return Convert.ToInt64(Request["postid"]);
                return 0;
            }
        }
        #endregion

        public PageBlogPostDetail()
        {
            ModuleName = "blogpost";
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (_blogId <= 0 && _idFromUrl > 0)
                {
                    AWAPI_Data.CustomEntities.BlogPostExtended blogPost = _blogLib.GetBlogPost(_idFromUrl);
                    if (blogPost == null)
                        return;

                    _blogId = blogPost.blogId;
                }

                ResetControls();
                PopulateCategories();
                PopulateTags();
                PopulateAuthorList();

                if (_idFromUrl > 0)
                    PopulateBlogPost(_idFromUrl);

                PopulateBlogInfo();
                PopulateBlogPostList();

                if (Request["commentid"] != null && _idFromUrl >= 0)
                {
                    System.Text.StringBuilder script = new System.Text.StringBuilder();
                    script.Append("$(document).ready(function(){ \n");
                    script.Append("    $.fn.colorbox({href:'frames/blogpostcomments.aspx?postid=" + _idFromUrl.ToString() +
                                            "&commentid=" + Request["commentid"] + "', open:true, width:'960px', height:'600px', iframe:true});");
                    script.Append("}); \n");
                    RegisterCustomScript(script);
                }
            }

            RegisterCustomScript();
            AddScriptToCategoryList();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            PopulateTags();
        }

        void AddScriptToCategoryList()
        {
            foreach (ListItem li in _categoryList.Items)
            {
                li.Attributes.Add("name", "categoryitem");
                li.Attributes.Add("tag", li.Text);
                li.Attributes.Add("onclick", "setElementFromCheckBoxList('categoryitem', '" + _categoryList.ClientID + "', '" + _categoryListInText.ClientID + "', ';')");
            }
        }

        void RegisterCustomScript()
        {
            System.Text.StringBuilder script = new System.Text.StringBuilder();
            script.Append("$(document).ready(function(){ \n");
            script.Append("    $(\"a[rel='selectfile']\").colorbox({width:'960px', height:'600px', iframe:true}); \n");
            script.Append("    $(\"a[rel='blogpostcomments']\").colorbox({width:'960px', height:'600px', iframe:true}); \n");
            script.Append("    $(\"a[rel='blogpostfiles']\").colorbox({width:'960px', height:'600px', iframe:true}); \n");
            script.Append("}); \n");

            //Add tags auto completed...
            if (!String.IsNullOrEmpty(_tagsAutoCompleteScript.Text))
            {

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("var data = '" + _tagsAutoCompleteScript.Text + "'.split(',');\n");
                sb.Append("$('#" + _tags.ClientID + "').autocomplete(data, {\n");
                sb.Append("    width: 320, max: 10, multiple: true, multipleSeparator: ',', scroll: true, scrollHeight: 300 });\n");
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), sb.ToString(), true);
            }

            RegisterCustomScript(script);
        }

        /// <summary>
        /// 
        /// </summary>
        void PopulateBlogInfo()
        {
            _blogTitle.Text = "";

            if (_blogId <= 0)
                return;

            awBlog blog = _blogLib.GetBlog(_blogId);
            if (blog == null)
                return;

            if (blog.siteId != App_Code.SessionInfo.CurrentSite.siteId)
            {
                Response.Redirect("blogs.aspx");
                Response.End();
                return;
            }

            _blogTitle.Text = blog.title;
            _blogTitle.NavigateUrl = "blogs.aspx?blogid=" + _blogId;
        }

        void PopulateAuthorList()
        {
            _authorList.Items.Clear();
            AWAPI_BusinessLibrary.library.UserLibrary userLib = new AWAPI_BusinessLibrary.library.UserLibrary();
            System.Collections.Generic.IList<AWAPI_Data.CustomEntities.UserExtended> list
                                    = userLib.GetList(App_Code.SessionInfo.CurrentSite.siteId, "", "");

            var tmp = from l in list
                      orderby l.firstName, l.lastName
                      select l;

            foreach (AWAPI_Data.CustomEntities.UserExtended usr in tmp)
            {
                ListItem li = new ListItem();
                li.Text = usr.firstName + " " + usr.lastName + " - " + usr.username + " ";
                li.Value = usr.userId.ToString();
                if (usr.userId == App_Code.SessionInfo.CurrentUser.userId)
                    li.Selected = true;

                _authorList.Items.Add(li);
            }
            _authorList.Items.Insert(0, new ListItem("- Please select the author", ""));
        }

        /// <summary>
        /// 
        /// </summary>
        void PopulateBlogPostList()
        {
            string filter = txbFilter.Text.ToLower().Trim();
            gwPostList.DataSource = null;

            System.Collections.Generic.IList<AWAPI_Data.CustomEntities.BlogPostExtended>
                    list = _blogLib.GetBlogPostsByBlogId(_blogId);

            if (filter.Length == 0)
                gwPostList.DataSource = list;
            else
            {
                var l = from p in list
                        where p.title.ToLower().IndexOf(filter) >= 0 ||
                            p.description.IndexOf(filter) >= 0 ||
                            p.summary.IndexOf(filter) >= 0
                        select p;
                if (l != null)
                    gwPostList.DataSource = l.ToList();
            }

            gwPostList.DataBind();
        }

        #region CATEGORIES
        /// <summary>
        /// 
        /// </summary>
        void PopulateCategories()
        {
            var cats = _blogLib.GetBlogCategoryList(_blogId, false);

            _categoryList.Items.Clear();
            if (cats != null && cats.Count > 0)
            {
                foreach (AWAPI_Data.Data.awBlogCategory cat in cats)
                {
                    if (!cat.isEnabled)
                        continue;

                    ListItem li = new ListItem(cat.title, cat.blogCategoryId.ToString());
                    _categoryList.Items.Add(li);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postId"></param>
        void SelectCategories(long postId)
        {
            _categoryListInText.Text = "";
            IList<AWAPI_Data.Data.awBlogCategory> catList = null;
            catList = _blogLib.GetBlogCategoryPostList(postId, false);

            if (catList == null || catList.Count() == 0)
                return;
            foreach (ListItem li in _categoryList.Items)
            {
                li.Selected = false;
                var l = from c in catList
                        where c.blogCategoryId.Equals(Convert.ToInt64(li.Value))
                        select c;
                if (l != null && l.Count() > 0)
                {
                    if (_categoryListInText.Text.Length > 0 &&
                        !_categoryListInText.Text.EndsWith(","))
                        _categoryListInText.Text += ", ";

                    _categoryListInText.Text += li.Text;
                    li.Selected = true;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogPostId"></param>
        void SaveCategories(long blogPostId)
        {
            System.Collections.ArrayList ids = new System.Collections.ArrayList();

            foreach (ListItem li in _categoryList.Items)
                if (li.Selected)
                    ids.Add(Convert.ToInt64(li.Value));

            _blogLib.AddPostToBlogCategories(ids, blogPostId);

        }
        #endregion

        #region TAGS
        /// <summary>
        /// 
        /// </summary>
        void PopulateTags()
        {
            _tagsAutoCompleteScript.Text = "";
            IList<AWAPI_Data.CustomEntities.BlogTagExtended> list = _blogLib.GetBlogTagList(_blogId, false, 0);
            _tagList.DataSource = list;
            _tagList.DataBind();

            if (list == null || list.Count == 0)
                return;

            //Auto Complete part ---------------------------------
            System.Text.StringBuilder sbTags = new System.Text.StringBuilder();
            foreach (AWAPI_Data.CustomEntities.BlogTagExtended tag in list)
            {
                if (sbTags.Length > 0)
                    sbTags.Append(",");
                sbTags.Append(tag.title.Trim());
            }
            _tagsAutoCompleteScript.Text = sbTags.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postId"></param>
        void SelectTags(long postId)
        {
            _tags.Text = "";

            IList<AWAPI_Data.Data.awBlogTag> tagList = null;
            tagList = _blogLib.GetBlogPostTagList(postId, false);

            if (tagList == null || tagList.Count() == 0)
                return;

            foreach (awBlogTag tag in tagList)
            {
                if (_tags.Text.Trim().Length > 0)
                    _tags.Text += ", ";

                _tags.Text += tag.title;

                //ListItem li = _tagList.Items.FindByValue(tag.blogTagId.ToString());
                //li.Selected = true;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogPostId"></param>
        void SaveTags(long blogPostId)
        {
            System.Collections.ArrayList ids = new System.Collections.ArrayList();

            string[] tags = _tags.Text.Trim().ToLower().Split(',');
            _blogLib.AddPostToBlogTags(tags, blogPostId);
        }

        #endregion


        void ResetControls()
        {
            _id.Text = "";
            _title.Text = "";
            _description.Value = "";
            _summary.Text = "";
            //_geoTag.Text = "";
            _imageUrl.Text = "";
            _categoryListInText.Text = "";
            _tags.Text = "";

            _image.ImageUrl = "";
            _pubDate.Text = DateTime.Now.ToString("MM/dd/yyyy HH:mm"); ;
            _pubEndDate.Text = "";

            _isCommentable.Checked = false;

            hplPopupBlogPostComment_.NavigateUrl = "";
            hplPopupBlogPostFile_.NavigateUrl = "";


            //foreach (ListItem li in _tagList.Items)
            //    li.Selected = false;

            foreach (ListItem li in _categoryList.Items)
                li.Selected = false;

            if (_authorList.Items.Count > 0 && _authorList.Items.FindByValue(App_Code.SessionInfo.CurrentUser.userId.ToString()) != null)
                _authorList.SelectedValue = App_Code.SessionInfo.CurrentUser.userId.ToString();

            ShowHideContentButtons(false);
        }

        void ShowHideContentButtons(bool show)
        {
            ShowHideControl(btnDeleteBlogPost_, show);
            ShowHideControl(lblDeleteBlogPost_, show);

            ShowHideControl(hplPopupBlogPostComment_, show);
            ShowHideControl(lblPopupBlogPostComment_, show);

            ShowHideControl(hplPopupBlogPostFile_, show);
            ShowHideControl(lblPopupBlogPostFile_, show);


            ShowHideControl(_image, show);

            if (show)
            {
                hplPopupBlogPostComment_.NavigateUrl = "~/admin/frames/blogpostcomments.aspx?postid=" + _id.Text;
                hplPopupBlogPostFile_.NavigateUrl = "~/admin/frames/manageblogfiles.aspx?postid=" + _id.Text;
            }
        }

        void PopulateBlogPost(Int64 blogPostId)
        {
            ResetControls();
            AWAPI_Data.CustomEntities.BlogPostExtended blogPost = _blogLib.GetBlogPost(blogPostId);

            if (blogPost == null)
                return;

            if (_blogId <= 0)
                _blogId = blogPost.blogId;

            _id.Text = blogPost.blogPostId.ToString();
            _title.Text = blogPost.title;
            _summary.Text = blogPost.summary;
            _description.Value = blogPost.description;

            _isPublished.Checked = blogPost.isPublished;
            _imageUrl.Text = blogPost.imageurl;

            SetImage(blogPost.imageurl);

            _isCommentable.Checked = blogPost.isCommentable;

            if (blogPost.pubDate != null) _pubDate.Text = blogPost.pubDate.Value.ToString("MM/dd/yyyy HH:mm");
            if (blogPost.pubEndDate != null) _pubEndDate.Text = blogPost.pubEndDate.Value.ToString("MM/dd/yyyy HH:mm");

            if (_authorList.Items.FindByValue(blogPost.authorUserId.ToString()) != null)
                _authorList.SelectedValue = blogPost.authorUserId.ToString();
            else if (_authorList.Items.FindByValue(blogPost.userId.ToString()) != null)
                _authorList.SelectedValue = blogPost.userId.ToString();
            else
                _authorList.SelectedIndex = 0;

            SelectCategories(blogPost.blogPostId);
            SelectTags(blogPost.blogPostId);

            ShowHideContentButtons(true);
        }

        void SetImage(string imageUrl)
        {
            if (!String.IsNullOrEmpty(imageUrl))
            {
                _image.Attributes.Add("style", "display:'';");
                if (imageUrl.ToLower().IndexOf(AWAPI_BusinessLibrary.library.ConfigurationLibrary.Config.fileServiceUrl.ToLower()) >= 0)
                    _image.ImageUrl = imageUrl + "&size=150x150";
                else
                    _image.ImageUrl = imageUrl;
            }

        }


        string AddCharacterForLineage(string lineage)
        {
            System.Text.StringBuilder space = new System.Text.StringBuilder("");
            if (lineage != null && lineage.Trim().Length > 0)
            {
                string[] parentContents = lineage.Split('|');
                for (int i = 1; i < parentContents.Length; i++)
                    space.Append("___");
            }
            else
                return "___";

            return space.ToString();

        }

        protected void btnDelete_Click(object sender, ImageClickEventArgs e)
        {
            DeleteBlogPost(Convert.ToInt64(_id.Text));
            PopulateBlogPostList();
            AdminMaster.WriteMessage(AWAPI.Admin.AdminMaster.MessageType.INFO, "Blog post has been deleted.");
        }

        void DeleteBlogPost(long blogPostId)
        {
            _blogLib.DeleteBlogPost(blogPostId);

            ResetControls();
        }

        protected void btnNewContent_Click(object sender, EventArgs e)
        {
            ResetControls();
        }


        protected void btnSaveBlogPostContent_Click(object sender, ImageClickEventArgs e)
        {
            Save();
            PopulateBlogPostList();
        }

        protected void Save()
        {
            long blogPostId = 0;
            if (_blogId <= 0)
            {
                AdminMaster.WriteMessage(AWAPI.Admin.AdminMaster.MessageType.ERROR, "Blog hasn't been selected. Please click on 'blog posts' to select a blog before creating a blog post.");
                return;
            }

            awBlogPost blogPost = new awBlogPost();
            blogPost.userId = App_Code.SessionInfo.CurrentUser.userId;
            if (_authorList.SelectedIndex == 0)
                blogPost.authorUserId = blogPost.userId;
            else
                blogPost.authorUserId = Convert.ToInt64(_authorList.SelectedValue);
            blogPost.title = _title.Text;
            blogPost.description = _description.Value;
            blogPost.summary = _summary.Text;
            blogPost.geoTag = "";
            blogPost.pubDate = AWAPI_Common.library.MiscLibrary.ConvertStringToDate(_pubDate.Text);
            blogPost.pubEndDate = AWAPI_Common.library.MiscLibrary.ConvertStringToDate(_pubEndDate.Text, true);
            blogPost.blogId = _blogId;

            blogPost.imageurl = _imageUrl.Text;

            blogPost.isPublished = _isPublished.Checked;
            blogPost.isCommentable = _isCommentable.Checked;

            if (_id.Text.Trim().Length == 0)
            {
                blogPostId = _blogLib.AddBlogPost(blogPost.blogId, blogPost.userId, blogPost.authorUserId,
                            blogPost.title, blogPost.description, blogPost.summary, blogPost.imageurl,
                            blogPost.geoTag, blogPost.isPublished, blogPost.isCommentable,
                            blogPost.pubDate, blogPost.pubEndDate);
                _id.Text = blogPostId.ToString();
            }
            else
            {
                blogPostId = Convert.ToInt64(_id.Text);
                _blogLib.UpdateBlogPost(blogPostId, blogPost.userId, blogPost.authorUserId,
                            blogPost.title, blogPost.description,
                            blogPost.summary, blogPost.imageurl, blogPost.geoTag, blogPost.isPublished,
                            blogPost.isCommentable, blogPost.pubDate, blogPost.pubEndDate);

            }

            SetImage(blogPost.imageurl);


            //Save tags and the tags
            SaveCategories(blogPostId);
            SaveTags(blogPostId);

            //Repopulate tags
            PopulateTags();

            //Check Tags and the Categories
            SelectCategories(blogPostId);
            SelectTags(blogPostId);

            ShowHideContentButtons(true);

            AdminMaster.WriteMessage(AWAPI.Admin.AdminMaster.MessageType.INFO, "Blog post has been saved.");
        }

        protected void lbtnFilter_Click(object sender, EventArgs e)
        {
            PopulateBlogPostList();
        }

        protected void lbtnClear_Click(object sender, EventArgs e)
        {
            txbFilter.Text = "";
            PopulateBlogPostList();
        }

        protected void gwPostList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.ToLower())
            {
                case "selectblogpost":
                    PopulateBlogPost(Convert.ToInt64(e.CommandArgument.ToString()));
                    break;
            }
        }

        protected void gwPostList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gwPostList.PageIndex = e.NewPageIndex;
            PopulateBlogPostList();

        }

        protected void gwPostList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lbtn = (LinkButton)e.Row.FindControl("lbtnSelect");
                if (lbtn != null)
                {
                    bool published = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "isPublished"));
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
