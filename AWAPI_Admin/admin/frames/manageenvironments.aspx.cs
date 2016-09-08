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
    public partial class manageEnvironments : AWAPI.App_Code.AdminBasePage
    {
        AWAPI_BusinessLibrary.library.EnvironmentLibrary _environmentLib = new AWAPI_BusinessLibrary.library.EnvironmentLibrary();

        Int64 SiteId
        {
            get
            {
                if (Request["siteId"] == null)
                    return 0;
                return Convert.ToInt64(Request["siteId"]);
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _msg.Text = "";

            if (!IsPostBack)
            {
                ResetControls();
                PopulateList();
            }
        }

        void PopulateList()
        {
            _enviromentList.DataSource = _environmentLib.GetList(SiteId);
            _enviromentList.DataBind();

        }

        void ResetControls()
        {
            _id.Text = "";
            _serviceUrl.Text = "";
            _title.Text = "";
            _description.Text = "";
            _publicKey.Text = "";

            ShowHideControls(false);
        }

        void ShowHideControls(bool show)
        {
            ShowHideControl(btnDeleteEnvironment_, show);
        }

        protected void btnNew_Click(object sender, ImageClickEventArgs e)
        {
            ResetControls();
        }

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            AWAPI_Data.Data.awEnvironment env = new AWAPI_Data.Data.awEnvironment();
            Int64 id = 0;

            try
            {
                env.userId = App_Code.SessionInfo.CurrentUser.userId;
                env.title = _title.Text;
                env.description = _description.Text;
                env.serviceUrl = _serviceUrl.Text;
                env.publicKey = _publicKey.Text;

                if (_id.Text.Trim() == "")  //add new 
                {
                    id = _environmentLib.Add(SiteId, env.userId, env.title, env.description, env.serviceUrl, env.publicKey);
                }
                else  //Update  
                {
                    id = Convert.ToInt64(_id.Text);
                    _environmentLib.Update(id, env.userId, env.title, env.description, env.serviceUrl, env.publicKey);
                }
                _id.Text = id.ToString();

                PopulateList();
                ShowHideControls(true);

                App_Code.Misc.WriteMessage(_msg, "Environment has been saved.");
            }
            catch (Exception ex)
            {
                App_Code.Misc.WriteMessage(App_Code.Misc.MessageType.ERROR, _msg, ex.Message);
            }

        }

        protected void _enviromentList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            _enviromentList.PageIndex = e.NewPageIndex;
            PopulateList();
        }

        protected void btnDelete_Click(object sender, ImageClickEventArgs e)
        {
            _environmentLib.Delete(Convert.ToInt64(_id.Text));
            PopulateList();

            App_Code.Misc.WriteMessage(_msg, "Environment has been deleted.");
        }

        protected void _enviromentList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.ToLower())
            {
                case "editenviroment":
                    Populate(Convert.ToInt64(e.CommandArgument.ToString()));
                    break;
            }
        }

        void Populate(long EnvironmentId)
        {
            ResetControls();
            AWAPI_Data.Data.awEnvironment env = _environmentLib.Get(EnvironmentId);
            if (env == null)
                return;

            _id.Text = EnvironmentId.ToString();
            _title.Text = env.title;
            _description.Text = env.description;
            _serviceUrl.Text = env.serviceUrl;
            _publicKey.Text = env.publicKey;

            ShowHideControls(true);
        }

        protected void _searchStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateList();
        }

    }
}
