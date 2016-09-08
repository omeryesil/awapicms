using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AWAPI_Data.Data;

namespace AWAPI.Admin
{
    public partial class PageBlogs : AWAPI.App_Code.AdminBasePage
    {
        AWAPI_BusinessLibrary.library.BlogLibrary _blogLib = new AWAPI_BusinessLibrary.library.BlogLibrary();


        public long CurrentBlogId
        {
            get
            {
                if (_blogId.Text == "")
                {
                    if (Request["blogid"] != null)
                    {
                        _blogId.Text = Request["blogid"];
                        return Convert.ToInt64(Request["blogid"]);
                    }
                    else
                        return -1;
                }
                return Convert.ToInt64(_blogId.Text);
            }
            set
            {
                _blogId.Text = value.ToString();
            }
        }

        public PageBlogs()
        {
            ModuleName = "blog";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateBlogs();
                PopulateBlogPosts(CurrentBlogId);
                SetBlogControls();

                PopulateTags();
                PopulateCategories();
            }
        }

        void PopulateBlogs()
        {
            long siteId = App_Code.SessionInfo.CurrentSite.siteId;
            var list = _blogLib.GetBlogList(siteId, "");

            if (list == null)
                blogList.DataSource = null;
            else
            {
                blogList.DataSource = list;

                if (CurrentBlogId <= 0 && list.Count > 0)
                    CurrentBlogId = list[0].blogId;

            }
            blogList.DataBind();
        }

        void PopulateTags()
        {
            _filterByTags.DataSource = null;
            _filterByTags.DataValueField = "blogtagid";
            _filterByTags.DataTextField = "title";

            var list = _blogLib.GetBlogTagList(CurrentBlogId, false, 0);
            if (list != null)
                _filterByTags.DataSource = list;

            _filterByTags.DataBind();

            _filterByTags.Items.Insert(0, new ListItem("All", ""));
            _filterByTags.SelectedIndex = 0;
        }

        void PopulateCategories()
        {
            _filterByCategory.DataSource = null;
            _filterByCategory.DataValueField = "blogCategoryId";
            _filterByCategory.DataTextField = "title";

            var list = _blogLib.GetBlogCategoryList(CurrentBlogId, false);
            if (list != null)
                _filterByCategory.DataSource = list;

            _filterByCategory.DataBind();

            _filterByCategory.Items.Insert(0, new ListItem("All", ""));
            _filterByCategory.SelectedIndex = 0;
        }


        void PopulateBlogPosts(long blogId)
        {
            gwArticles.DataSource = null;
            IList<AWAPI_Data.CustomEntities.BlogPostExtended> list = null;

            if (_filterByTags.SelectedIndex > 0)
                list = _blogLib.GetBlogPostListByTagId(Convert.ToInt64(_filterByTags.SelectedValue), false, _filterByText.Text.Trim());
            else if (_filterByCategory.SelectedIndex>0)
                list = _blogLib.GetPostListByCategoryId(Convert.ToInt64(_filterByCategory.SelectedValue), false, _filterByText.Text.Trim());
            else
                list = _blogLib.GetPostListByBlogId(blogId, false, _filterByText.Text.Trim());


            gwArticles.DataSource = list;

            gwArticles.DataBind();
        }

        protected void gwArticles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gwArticles.PageIndex = e.NewPageIndex;
            PopulateBlogPosts(CurrentBlogId);
        }

        protected void rowCbPublished_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            GridViewRow row = (GridViewRow)cb.NamingContainer;
            Label lblId = (Label)row.FindControl("rowLblContentId");
            long id = Convert.ToInt64(lblId.Text);

            _blogLib.UpdateBlogPostPublishStatus(id, cb.Checked);

            Label lblMsg = (Label)row.FindControl("rowLblPublishMsg");
            if (cb.Checked)
                lblMsg.Text = "Published";
            else
                lblMsg.Text = "Draft";
        }

        void SetBlogControls()
        {
            _blogTitle.Text = "Blog Posts";

            awBlog blog = _blogLib.GetBlog(CurrentBlogId);
            if (blog == null)
                return;

            _blogTitle.Text = blog.title + " - Posts";
        }

        protected void blogList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            long id = Convert.ToInt64(e.CommandArgument.ToString());
            switch (e.CommandName.ToLower())
            {
                case "selectblog":
                    CurrentBlogId = id;
                    SetBlogControls();
                    PopulateBlogPosts(id);
                    break;
            }
        }

        protected void btnNewContent_Click(object sender, EventArgs e)
        {
            if (CurrentBlogId <= 0)
            {
                AdminMaster.WriteMessage(AWAPI.Admin.AdminMaster.MessageType.ERROR, "Please select a blog before adding new post.");
                return;
            }

            Response.Redirect("blogpostdetail.aspx?blogid=" + CurrentBlogId);
        }

        protected void gwArticles_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            long id = Convert.ToInt64(e.CommandArgument.ToString());

            switch (e.CommandName.ToLower())
            {
                case "deletepost":
                    _blogLib.DeleteBlogPost(id);
                    PopulateBlogPosts(CurrentBlogId);
                    break;

            }
        }

        protected void lbtnFilter_Click(object sender, EventArgs e)
        {
            PopulateBlogPosts(CurrentBlogId);
        }

        protected void lbtnClear_Click(object sender, EventArgs e)
        {
            _filterByTags.SelectedIndex = 0;
            _filterByText.Text = "";

            PopulateBlogPosts(CurrentBlogId);
        }

        protected void _filterByCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            _filterByTags.SelectedIndex = 0;
            PopulateBlogPosts(CurrentBlogId);
        }

        protected void _filterByTags_SelectedIndexChanged(object sender, EventArgs e)
        {
            _filterByCategory.SelectedIndex = 0;
            PopulateBlogPosts(CurrentBlogId);
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

        protected void gwArticles_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HyperLink lbtn = (HyperLink)e.Row.FindControl("rowHplTitle");
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
