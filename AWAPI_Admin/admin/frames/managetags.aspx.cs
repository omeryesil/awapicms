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
    public partial class managetags : AWAPI.App_Code.AdminBasePage
    {
        AWAPI_BusinessLibrary.library.TagLibrary _tagLib = new AWAPI_BusinessLibrary.library.TagLibrary();

        Int64 SiteId
        {
            get
            {
                if (Request["siteid"] == null)
                    return 0;
                return Convert.ToInt64(Request["siteid"]);
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                PopulateTags();
        }

        void PopulateTags()
        {
            var tagList = _tagLib.GetList(SiteId, "");

            _tagList.DataSource = tagList;
            _tagList.DataBind();
        }

        void ResetControls()
        {
            _tagId.Text = "";
            _title.Text = "";

            ShowHildeControls(false);
        }

        void ShowHildeControls(bool show)
        {
            btnDelete.Visible = show;
        }

        void PopulateTag(Int64 siteTagId)
        {
            ResetControls();

            AWAPI_Data.Data.awSiteTag tag = _tagLib.Get(siteTagId);
            _tagId.Text = tag.siteTagId.ToString();
            _title.Text = tag.title;

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
            long siteTagId = 0;
            if (_tagId.Text != "")
            {
                siteTagId = Convert.ToInt64(_tagId.Text);
                _tagLib.Update(siteTagId, _title.Text);
            }
            else
            {
                siteTagId = _tagLib.Add(_title.Text, SiteId);
                _tagId.Text = siteTagId.ToString();
            }
            ShowHildeControls(true);
            PopulateTags();
        }

        protected void btnDelete_Click(object sender, ImageClickEventArgs e)
        {
            DeleteTag();
        }

        void DeleteTag()
        {
            _tagLib.Delete(Convert.ToInt64(_tagId.Text));

            ResetControls();
            PopulateTags();
        }



    }
}
