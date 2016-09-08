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
    public partial class manageblogtags : AWAPI.App_Code.AdminBasePage
    {
        AWAPI_BusinessLibrary.library.BlogLibrary _tagLib = new AWAPI_BusinessLibrary.library.BlogLibrary();

        Int64 BlogId
        {
            get
            {
                if (Request["blogid"] == null)
                    return 0;
                return Convert.ToInt64(Request["blogid"]);
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                PopulateTags();
        }

        void PopulateTags()
        {
            System.Collections.Generic.IList<AWAPI_Data.CustomEntities.BlogTagExtended> list  = _tagLib.GetBlogTagList(BlogId, false, 0);


            _tagList.DataSource = list;
            _tagList.DataBind();
        }

        void ResetControls()
        {
            _tagId.Text = "";
            _title.Text = "";

            _tagPosts.DataSource = null;
            _tagPosts.DataBind();

            ShowHildeControls(false);
        }

        void ShowHildeControls(bool show)
        {
            btnDelete.Visible = show;
        }

        void PopulateTag(Int64 blogTagId)
        {
            ResetControls();

            AWAPI_Data.Data.awBlogTag tag = _tagLib.GetBlogTag(blogTagId);
            _tagId.Text = tag.blogTagId.ToString();
            _title.Text = tag.title;


            System.Collections.Generic.IList<AWAPI_Data.CustomEntities.BlogPostExtended> postList = _tagLib.GetBlogPostListByTagId(blogTagId, false, "");
            _tagPosts.DataSource = postList;
            _tagPosts.DataBind();

            ShowHildeControls(true);
        }

        protected void _tagList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int64 siteTagId = Convert.ToInt64(e.CommandArgument.ToString());
            switch (e.CommandName.ToLower())
            {
                case "edittag":
                    PopulateTag(siteTagId);
                    break;
            }
        }

        protected void btnAdd_Click(object sender, ImageClickEventArgs e)
        {
            ResetControls();
        }

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            SaveTag();
        }

        void SaveTag()
        {
            long blogTagId = 0;
            if (_tagId.Text != "")
            {
                blogTagId = Convert.ToInt64(_tagId.Text);
                _tagLib.UpdateBlogTag(blogTagId, _title.Text);
            }
            else
            {
                blogTagId = _tagLib.AddBlogTag(BlogId, _title.Text);
                _tagId.Text = blogTagId.ToString();
            }
            ShowHildeControls(true);
            PopulateTags();

            App_Code.Misc.WriteMessage(_msg, "Tag has been saved.");
        }

        protected void btnDelete_Click(object sender, ImageClickEventArgs e)
        {
            DeleteTag();
            App_Code.Misc.WriteMessage(_msg, "Tag has been deleted.");
        }

        void DeleteTag()
        {
            _tagLib.DeleteBlogTag(Convert.ToInt64(_tagId.Text));

            ResetControls();
            PopulateTags();
        }



    }
}
