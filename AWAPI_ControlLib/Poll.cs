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
    [ToolboxData("<{0}:Blog runat=server></{0}:Blog>")]
    public class Poll : AWAPI_ControlLib.Classes.BaseControl, INamingContainer
    {
        public enum EMethods
        {
            GetPoll,
            GetPollList
        }

        #region Properties

        [Category("Setting")]
        [Description("Content Sercvice's URL address. Must be defined in web.config file; AWAPI_Content_Service_URL)")]
        public string ServiceBaseUrl
        {
            get
            {
                return Classes.Configuration.PollServiceUrl;
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

        [DefaultValue(EMethods.GetPoll)]
        [Description("")]
        public EMethods MethodName
        {
            get
            {
                if (ViewState["MethodName"] == null)
                    return EMethods.GetPoll;
                return (EMethods)ViewState["MethodName"];
            }
            set
            {
                ViewState["MethodName"] = value;
            }
        }

        [Description("")]
        [DefaultValue(0)]
        public long PollId
        {
            get
            {
                if (ViewState["PollId"] != null)
                    return Convert.ToInt64(ViewState["PollId"]);
                return -1;
            }
            set
            { ViewState["PollId"] = value; }
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
                if (HttpContext.Current.Request["showurl"] != null)
                    output.Write("<br/><span style='color:red;background-color:#ffffff;'>" + ServiceCall + "<br/></span>");

                System.Xml.Xsl.XsltArgumentList xslArgs = Classes.XslTransformation.GetArgumentListFromUrl();
                string transformed = Classes.XslTransformation.Transform(XslPath, xslArgs, ServiceCall, this.ID, CacheDuration);
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


        public string GetXSLUrl(string path, bool addParams)
        {
            string strRtn = GetXSLUrl(path);
            if (addParams)
                strRtn += GetParamsForServiceCall();

            return strRtn;
        }



        string GetParamsForServiceCall()
        {
            PollId = GetPropertyValue("PollId", PollId, 0);
            PageSize = GetPropertyValue("pagesize", PageSize, 0);
            PageIndex = GetPropertyValue("pageindex", PageIndex, 0);

            StringBuilder sb = new StringBuilder("");
            sb.Append("?method=" + MethodName);
            sb.Append("&siteid=" + SiteId);
            sb.Append("&type=" + XMLType.ToString());

            if (PollId > 0) sb.Append("&blogid=" + PollId);
            if (Culture.Trim() != "") sb.Append("&culture=" + Culture);
            if (PageSize > 0) sb.Append("&pagesize=" + PageSize);
            if (PageIndex > 0) sb.Append("&pageindex=" + PageIndex);   
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
            
            if (HttpContext.Current.Request["currpostid"] != null)
                sb.Append("&currpostid=" + HttpContext.Current.Request["currpostid"].ToString());
            else
                if (HttpContext.Current.Request["postid"] != null)
                    sb.Append("&currpostid=" + HttpContext.Current.Request["postid"].ToString());

            return sb.ToString();
        }

    }

}
