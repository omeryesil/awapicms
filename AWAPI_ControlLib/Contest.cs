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
    [ToolboxData("<{0}:Contest runat=server></{0}:Content>")]
    public class Contest : AWAPI_ControlLib.Classes.BaseControl, INamingContainer
    {
        public enum EMethods
        {
            GetEntryList,
            GetEntrydailySummary
        }

        #region Properties


        [Category("Setting")]
        [Description("Contest Sercvice's URL address. Must be defined in web.config file; AWAPI_Contest_Service_URL)")]
        public string ServiceBaseUrl
        {
            get
            {
                return Classes.Configuration.ContestServiceUrl;
            }
        }

        [Description("Contest Service's URL with the parameters")]
        public string ServiceCall
        {
            get
            {
                string url = ServiceBaseUrl.ToLower() + GetParamsForServiceCall();
                return new AWAPI_ControlLib.Classes.AkamaiCaching().AddAkamai(url);
            }
        }

        [DefaultValue(EMethods.GetEntryList)]
        [Description("")]
        public EMethods MethodName
        {
            get
            {
                if (ViewState["MethodName"] == null)
                    return EMethods.GetEntryList;
                return (EMethods)ViewState["MethodName"];
            }
            set
            {
                ViewState["MethodName"] = value;
            }
        }


        [Description("yyyy-MM-dd")]
        public string ContestEntryDate
        {
            get
            {
                if (ViewState["ContestEntryDate"] == null)
                    return "";
                return ViewState["ContestEntryDate"].ToString();
            }
            set
            {
                ViewState["ContestEntryDate"] = value;
            }
        }


        [Description("")]
        [DefaultValue(0)]
        public long ContestId
        {
            get
            {
                if (ViewState["ContestId"] == null)
                    return -1;
                return Convert.ToInt64(ViewState["ContestId"]);
            }
            set
            { ViewState["ContestId"] = value; }
        }

        [Description("")]
        [DefaultValue(0)]
        public long ContestGroupId
        {
            get
            {
                if (ViewState["ContestGroupId"] == null)
                    return -1;
                return Convert.ToInt64(ViewState["ContestGroupId"]);
            }
            set
            { ViewState["ContestGroupId"] = value; }
        }

        [Description("")]
        [DefaultValue(0)]
        public long UserId
        {
            get
            {
                if (ViewState["UserId"] == null)
                    return -1;
                return Convert.ToInt64(ViewState["UserId"]);
            }
            set
            { ViewState["UserId"] = value; }
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


        //public string GetFilePath(string path, bool addParams)
        //{
        //    string strRtn = path;

        //    if (path.IndexOf("http:") < 0)
        //    {
        //        string root = "http://" +
        //                    HttpContext.Current.Request.Url.Host +
        //                    ":" + HttpContext.Current.Request.Url.Port;

        //        strRtn = path.Replace("~", "");
        //        if (!strRtn.StartsWith("/"))
        //            strRtn += "/";

        //        strRtn = root + strRtn;
        //    }

        //    if (addParams)
        //        strRtn += GetParamsForServiceCall();

        //    return strRtn;
        //}


        string GetParamsForServiceCall()
        {
            ContestId = GetPropertyValue("ContestId", ContestId, 0);
            ContestGroupId = GetPropertyValue("ContestGroupId", ContestGroupId, 0);
            UserId = GetPropertyValue("UserId", UserId, 0);

            PageSize = GetPropertyValue("pagesize", PageSize, 0);
            PageIndex = GetPropertyValue("pageindex", PageIndex, 0);

            StringBuilder sb = new StringBuilder("");
            sb.Append("?method=" + MethodName);
            sb.Append("&siteid=" + SiteId);
            sb.Append("&type=" + XMLType.ToString());

            if (ContestId > 0) sb.Append("&contestid=" + ContestId);
            if (ContestGroupId > 0) sb.Append("&groupid=" + ContestGroupId);
            if (UserId > 0) sb.Append("&userid=" + UserId);

            if (PageSize > 0) sb.Append("&pagesize=" + PageSize);
            if (PageIndex > 0) sb.Append("&pageindex=" + PageIndex);
            if (ContestEntryDate.Trim().Length > 0) sb.Append("&date=" + ContestEntryDate);
            if (Parameters.Trim().Length> 0) sb.Append("&prms=" + Parameters);

            if (IgnoredUriParameters.Trim() != "*" && 
                HttpContext.Current.Request.Url.Query != null &&
                HttpContext.Current.Request.Url.Query.Length > 0)
            {
                string[] prms = HttpContext.Current.Request.Url.Query.Replace("?", "").Split('&');

                //if paremeter isn't already added to the returning string
                foreach (string prm in prms)
                {
                    if (prm.Trim().Length == 0)
                        continue;

                    string prmName = (prm.Split('='))[0];

                    if (!IsInIgnoreList(prmName))
                        if (sb.ToString().ToLower().IndexOf(prmName.ToLower()) < 0)
                            sb.Append("&" + prm);
                }
            }

            return sb.ToString();
        }

    }
}
