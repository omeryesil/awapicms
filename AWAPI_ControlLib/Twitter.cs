using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Generic;


namespace AWAPI_ControlLib
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:Twitter runat=server></{0}:Blog>")]
    public class Twitter : AWAPI_ControlLib.Classes.BaseControl, INamingContainer
    {
        public enum EMethods
        {
            GetStatus,
            GetStatusList,
        }

        #region Properties


        [Category("Setting")]
        [Description("Twitter Sercvice's URL address, defined in web.config file; AWAPI_Twitter_Service_URL)")]
        public string ServiceBaseUrl
        {
            get
            {
                return Classes.Configuration.TwitterServiceUrl;
            }
        }

        [Description("Content Service's URL with the parameters")]
        public string ServiceCall
        {
            get
            {
                string url = ServiceBaseUrl.ToLower() + GetParamsForServiceCall();
                return new AWAPI_ControlLib.Classes.AkamaiCaching().AddAkamai(url);
            }
        }

        [DefaultValue(EMethods.GetStatus)]
        [Description("")]
        public EMethods MethodName
        {
            get
            {
                if (ViewState["MethodName"] == null)
                    return EMethods.GetStatus;
                return (EMethods)ViewState["MethodName"];
            }
            set
            {
                ViewState["MethodName"] = value;
            }
        }

        [Description("XSL file's path. Example: ~/xslt/content.xslt")]
        public string XslPath
        {
            get
            {
                string path = "";
                if (ViewState["XslPath"] != null)
                    path = ViewState["XslPath"].ToString();
                return GetXSLUrl(path, false);
            }
            set
            { ViewState["XslPath"] = value; }
        }

        #endregion

        protected override void RenderContents(HtmlTextWriter output)
        {
            try
            {
                System.Xml.Xsl.XsltArgumentList xslArgs = Classes.XslTransformation.GetArgumentListFromUrl();

                StringBuilder transformed = new StringBuilder("\n");
                transformed.AppendFormat("<!-- START WIDGET: Id: {0} -->\n", this.ID);
                transformed.AppendFormat("<!-- XSLT: {0} -->\n", XslPath);
                transformed.AppendFormat("<!-- Call: {0} -->\n", ServiceCall);

                transformed.Append(Classes.XslTransformation.Transform(XslPath, xslArgs, ServiceCall, this.ID, CacheDuration));

                transformed.AppendFormat("\n<!-- END WIDGET ---------------------------- -->\n", ServiceCall);

                output.Write(transformed);
            }
            catch (Exception e)
            {
                string msg = "<font color='#ff0000'>ERROR:" + e.Message + "<br/>" +
                        "Control Id:" + this.ID + "<br/>" + "Service Call:" + ServiceCall + "<br/>" +
                        "XSLT Stylesheet Url:" + XslPath;

                if (e.InnerException != null)
                    msg += "<br/>" + e.InnerException.Message + "<br/>";
                msg += "</font>";
                output.Write(msg);
            }
        }

        public string GetXSLUrl(string path, bool addParams)
        {
            string strRtn = GetXSLUrl(path);
            if (addParams)
                strRtn += GetParamsForServiceCall();

            return strRtn;
        }


        string GetParamsForServiceCall()
        {
            PageSize = GetPropertyValue("pagesize", PageSize, 0);

            StringBuilder sb = new StringBuilder("");
            sb.Append("?method=" + MethodName);
            sb.Append("&siteid=" + SiteId);
            sb.Append("&type=" + XMLType.ToString());

            if (MaxTitleLength > 0) sb.Append("&maxtitlelength=" + MaxTitleLength);
            if (MaxDescriptionLength > 0) sb.Append("&maxdesclength=" + MaxDescriptionLength);
            if (PageSize > 0) sb.Append("&pagesize=" + PageSize);

            return sb.ToString();
        }

    }

}
