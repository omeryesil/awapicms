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
    public partial class selectsite : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void siteList_SiteSelected(object sender, AWAPI.App_Code.SiteEventArgument e)
        {

            AWAPI_BusinessLibrary.library.SiteLibrary lib = new AWAPI_BusinessLibrary.library.SiteLibrary();
            App_Code.SessionInfo.CurrentSite = lib.Get(e.SiteId);

            App_Code.UserInfo.UpdateSiteIdInAuthenticationCookie(e.SiteId);

            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "", " window.parent.closeColorBox(true); ", true);
        }

    }
}
