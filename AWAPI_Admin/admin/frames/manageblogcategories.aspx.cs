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

namespace  AWAPI.Admin.frames
{
    public partial class manageblogcategories : AWAPI.App_Code.AdminBasePage
    {
        AWAPI_BusinessLibrary.library.BlogLibrary _blogLib = new AWAPI_BusinessLibrary.library.BlogLibrary();

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
            _msg.Text = "";

            if (!IsPostBack)
                PopulateCategories();
        }

        void PopulateCategories()
        {
            _list.DataSource = _blogLib.GetBlogCategoryList(BlogId, false);
            _list.DataBind();
        }

        void ResetControls()
        {
            _id.Text = "";
            _title.Text = "";
            _description.Text = "";
            ShowHildeControls(false);
        }

        void ShowHildeControls(bool show)
        {
            btnDelete.Visible = show;
        }

        void PopulateField(Int64 catId)
        {

            AWAPI_Data.Data.awBlogCategory cat = _blogLib.GetBlogCategory(catId);

            _id.Text = cat.blogCategoryId.ToString();
            _title.Text = cat.title;
            _description.Text = cat.description;
            _isEnabled.Checked = cat.isEnabled;

            ShowHildeControls(true);
        }

        protected void _list_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int64 id = Convert.ToInt64(e.CommandArgument.ToString());
            switch (e.CommandName.ToLower())
            {
                case "editcat":
                    PopulateField(id);
                    break;

            }
        }

        protected void btnNew_Click(object sender, ImageClickEventArgs e)
        {
            ResetControls();
        }

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            Save();
        }

        void Save()
        {
            long id = 0;
            AWAPI_Data.Data.awBlogCategory cat = new AWAPI_Data.Data.awBlogCategory();

            try
            {

                cat.title = _title.Text;
                cat.description = _description.Text;
                cat.isEnabled = _isEnabled.Checked;

                //update current
                if (_id.Text == "")
                {
                    id = _blogLib.AddBlogCategory(BlogId, cat.title, cat.description, cat.isEnabled);
                    _id.Text = id.ToString();
                }
                else
                {
                    id = Convert.ToInt64(_id.Text);
                    _blogLib.UpdateBlogCategory(id, cat.title, cat.description, cat.isEnabled);
                }

                ShowHildeControls(true);
                PopulateCategories();
                App_Code.Misc.WriteMessage(_msg, "Blog category has been saved.");

            }
            catch (Exception ex)
            {
                _msg.Text = ex.Message;
                App_Code.Misc.WriteMessage(AWAPI.App_Code.Misc.MessageType.ERROR,  _msg, ex.Message);

            }
        }

        protected void btnDelete_Click(object sender, ImageClickEventArgs e)
        {
            Delete();
            App_Code.Misc.WriteMessage(_msg, "Blog category has been deleted.");
        }

        void Delete()
        {
            _blogLib.DeleteBlogCategory(Convert.ToInt64(_id.Text));
            ResetControls();
            PopulateCategories();
        }



    }
}
