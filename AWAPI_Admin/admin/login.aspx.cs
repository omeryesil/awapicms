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
using AWAPI.App_Code;

namespace AWAPI.Admin
{
    public partial class PageLogin : System.Web.UI.Page
    {
        long UserId
        {
            get
            {
                if (ViewState["UserId"] == null)
                    return 0;
                return Convert.ToInt64(ViewState["UserId"]);
            }
            set
            {
                ViewState["UserId"] = value;
            }
        }

        long SiteId
        {
            get
            {
                if (ViewState["SiteId"] == null)
                    return 0;
                return Convert.ToInt64(ViewState["SiteId"]);
            }
            set
            {
                ViewState["SiteId"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblMsg.Text = "";

            if (!IsPostBack)
            {
                PrepareControls();

                App_Code.SessionInfo.ClearSession();
                ShowHidePanels(_pnlLogin);
            }

        }

        void PrepareControls()
        { 
            string title = ConfigurationManager.AppSettings["AWAPI_Login_Title"];
            string logo = ConfigurationManager.AppSettings["AWAPI_Login_Logo"];

            loginTitle.Visible = false;
            loginLogo.Visible = false;

            if (!String.IsNullOrEmpty(logo))
            {
                loginLogo.Visible = true;
                loginLogo.ImageUrl = logo;
            }
            else if (!String.IsNullOrEmpty(title))
            {
                loginTitle.Visible = true;
                loginTitle.Text = title + "<br/><br/>";
            }
            else
                loginLogo.Visible = true;
        }

        void ShowHidePanels(Panel pnl)
        {
            _pnlLogin.Visible = false;
            _pnlSelectSite.Visible = false;

            pnl.Visible = true;
        }



        protected void lbtnLogin_Click(object sender, EventArgs e)
        {
            AWAPI_Data.CustomEntities.UserExtended user = UserInfo.ValidateUser(txbEmail.Text, txbPassword.Text);

            if (user != null)
            {
                UserId = user.userId;

                PopulateSites();

                //If there is only one poll then select it and continue
                if (_siteList.Items.Count == 1)
                {
                    _siteList.SelectedIndex = 0;
                    SelectASiteAndContinue();
                    return;
                }
                ShowHidePanels(_pnlSelectSite);
            }
            else
                lblMsg.Text = "Email and password don't match. Please try again.";
        }


        public void PopulateSites()
        {
            AWAPI_BusinessLibrary.library.SiteLibrary siteLib = new AWAPI_BusinessLibrary.library.SiteLibrary();

            _siteList.DataTextField = "title";
            _siteList.DataValueField = "siteid";
            _siteList.DataSource = siteLib.GetUserSiteList(UserId);
            _siteList.DataBind();
        }


        protected void lbtnBack_Click(object sender, EventArgs e)
        {
            ShowHidePanels(_pnlLogin);
        }


        protected void lbtnContinue_Click(object sender, EventArgs e)
        {
            SelectASiteAndContinue();
        }

        /// <summary>
        /// Authenticate the user, and save the info in the cookie if rememberme enabled..
        /// </summary>
        protected void SelectASiteAndContinue()
        {
            if (_siteList.Items.Count > 0 && !String.IsNullOrEmpty(_siteList.SelectedValue))
                SiteId = Convert.ToInt64(_siteList.SelectedValue);

            App_Code.UserInfo.AuthenticateUser(UserId, SiteId);
            App_Code.SessionInfo.FillSession(UserId, SiteId);

            // check if it exists, if not then redirect to default page
            if (Request.QueryString["ReturnUrl"] == null)
                Response.Redirect("~/admin/default.aspx");
            else
                Response.Redirect(Request.QueryString["ReturnUrl"]);
        }


    }
}
