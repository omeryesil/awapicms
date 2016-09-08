using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace AWAPI.App_Code
{
    public class SiteEventArgument : EventArgs
    {
        long _siteId;

        public long SiteId
        {
            get { return _siteId; }
            set { _siteId = value; }
        }

        public SiteEventArgument(long siteId)
        {
            _siteId = siteId;
        }


    }
}
