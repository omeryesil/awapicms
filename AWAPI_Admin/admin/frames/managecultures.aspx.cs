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
    public partial class managecultures : AWAPI.App_Code.AdminBasePage
    {
        AWAPI_BusinessLibrary.library.SiteLibrary _siteLib = new AWAPI_BusinessLibrary.library.SiteLibrary();
        AWAPI_BusinessLibrary.library.CultureLibrary _cultureLib = new AWAPI_BusinessLibrary.library.CultureLibrary();

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
            {
                PopulateCultures();
                PopulateSiteCultures();
            }
        }

        void PopulateCultures()
        {
            var tagList = _cultureLib.GetList();

            _list.DataSource = tagList;
            _list.DataBind();
        }

        void PopulateSiteCultures()
        {
            _culturesInSite.DataSource = _cultureLib.GetListBySiteId(SiteId);
            _culturesInSite.DataBind();
        
        }


        void AddCulture(string cultureCode)
        {
            _cultureLib.AddToSite(SiteId, cultureCode.ToLower());

            App_Code.Misc.WriteMessage(_msg, "Culture has been saved.");
            PopulateSiteCultures();
        }


        protected void _list_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string cultureCode = e.CommandArgument.ToString();
            switch (e.CommandName.ToLower())
            {
                case "addculture":
                    AddCulture(cultureCode);
                    break;
            }
        }

        void DeleteLanguageFromSite(string cultureCode)
        {
            _cultureLib.RemoveFromSite(SiteId, cultureCode);
            PopulateSiteCultures();

            App_Code.Misc.WriteMessage(_msg, "Culture has been deleted.");
        }

        protected void _culturesInSite_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string cultureCode = e.CommandArgument.ToString();
            switch (e.CommandName.ToLower())
            {
                case "removeculture":
                    DeleteLanguageFromSite(cultureCode);
                    break;
            }
        }

    }
}
