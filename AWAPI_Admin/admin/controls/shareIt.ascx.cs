using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AWAPI.admin.controls
{
    public partial class shareIt : System.Web.UI.UserControl
    {
        public string Link
        {
            get
            {
                if (ViewState["link"] == null)
                    return "";
                return ViewState["link"].ToString();
            }

            set
            {
                ViewState["link"] = value;
                hplShareIt.NavigateUrl = "~/admin/frames/manageshareit.aspx?link=" + Server.UrlEncode(value);
            }
        }

        public string ToolTip
        {
            get
            {
                return hplShareIt.ToolTip;
            }

            set {
                hplShareIt.ToolTip = value;            
            }
        
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}