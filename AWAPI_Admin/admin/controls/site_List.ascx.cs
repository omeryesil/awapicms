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
using AWAPI_Data.Data;
using System.Collections.Generic;

namespace  AWAPI.Admin.controls
{
    public partial class site_List : App_Code.AdminBaseControl
    {
        AWAPI_BusinessLibrary.library.SiteLibrary _siteLib = new AWAPI_BusinessLibrary.library.SiteLibrary();

        #region EVENTS
        public delegate void SiteHandler(object sender, AWAPI.App_Code.SiteEventArgument e);
        public event SiteHandler SiteSelected;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Populate();
        }
        #endregion

        protected void OnSiteSelected(AWAPI.App_Code.SiteEventArgument e)
        {
            if (SiteSelected != null)
                SiteSelected(this, e);
        }

        public void Populate()
        {
            gwSiteList.DataSource = _siteLib.GetUserSiteList(App_Code.SessionInfo.CurrentUser.userId);
            gwSiteList.DataBind();
        }

        protected void gwSiteList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string value = e.CommandArgument.ToString();
            switch (e.CommandName.ToLower().Trim())
            {
                case "select_site":
                    SelectSite(Convert.ToInt64(value));
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        void SelectSite(long siteId)
        {
            OnSiteSelected(new AWAPI.App_Code.SiteEventArgument(siteId));
        }
    }
}