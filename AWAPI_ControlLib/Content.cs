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
    [ToolboxData("<{0}:Content runat=server></{0}:Content>")]
    public class Content : AWAPI_ControlLib.Classes.BaseControl, INamingContainer
    {
        public enum EMethods
        {
            Get,
            GetList
        }

        #region Properties


        [Category("Setting")]
        [Description("Content Sercvice's URL address. Must be defined in web.config file; AWAPI_Content_Service_URL)")]
        public string ServiceBaseUrl
        {
            get
            {
                return Classes.Configuration.ContentServiceUrl;
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

        [DefaultValue(EMethods.Get)]
        [Description("")]
        public EMethods MethodName
        {
            get
            {
                if (ViewState["MethodName"] == null)
                    return EMethods.Get;
                return (EMethods)ViewState["MethodName"];
            }
            set
            {
                ViewState["MethodName"] = value;
            }
        }


        [Description("")]
        [DefaultValue(0)]
        public long ContentId
        {
            get
            {
                if (ViewState["ContentId"] == null)
                    return -1;
                return Convert.ToInt64(ViewState["ContentId"]);
            }
            set
            { ViewState["ContentId"] = value; }
        }

        [Description("Alias name. Used for Get method. If ContentId is empty, then this value will be taken")]
        [DefaultValue("")]
        public string Alias
        {
            get
            {
                if (ViewState["Alias"] == null)
                    return "";
                return ViewState["Alias"].ToString();
            }
            set
            { ViewState["Alias"] = value; }
        }


        /// <summary>
        /// if method ='GetList'
        ///     example:
        ///         -2 : returns 1 level parents
        ///         -1 : returns siblings
        ///         0  : returns 1 level childs
        ///  if method = 'Get'
        ///     example:
        ///         0 : returns itself
        ///         -1: returns parent
        ///         -2: returns 2 level deep parent
        /// </summary>
        [Description("Specifies lineage's level. For example if method is 'Get' and Deep is '-1' then the parent content will be returned")]
        [DefaultValue(0)]
        public long Deep
        {
            get
            {
                if (ViewState["Deep"] == null)
                    return -1;
                return Convert.ToInt64(ViewState["Deep"]);
            }
            set
            { ViewState["Deep"] = value; }
        }

        [Description("")]
        [DefaultValue(0)]
        public long DefaultContentId
        {
            get
            {
                if (ViewState["DefaultContentId"] == null)
                    return -1;
                return Convert.ToInt64(ViewState["DefaultContentId"]);
            }
            set
            { ViewState["DefaultContentId"] = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        [Browsable(true)]
        [Description("Content tree based on the alias. Example: Products|Seasonal|Fruits")]
        public string Lineage
        {
            get
            {
                if (ViewState["Lineage"] == null)
                    return "";
                else
                    return ViewState["Lineage"].ToString();
            }
            set
            {
                ViewState["Lineage"] = value;
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
            ContentId = GetPropertyValue("ContentId", ContentId, DefaultContentId);
            Alias = GetPropertyValue("Alias", Alias, "");
            Lineage = GetPropertyValue("Lineage", Lineage, "");
            //Where = GetPropertyValue("where", Where, "");
            //SortBy = GetPropertyValue("sortby", SortBy, "");
            PageSize = GetPropertyValue("pagesize", PageSize, 0);
            PageIndex = GetPropertyValue("pageindex", PageIndex, 0);
            Culture = GetPropertyValue("culture", Culture, "");

            StringBuilder sb = new StringBuilder("");
            sb.Append("?method=" + MethodName);
            sb.Append("&siteid=" + SiteId);
            sb.Append("&type=" + XMLType.ToString());

            if (ContentId > 0) sb.Append("&contentid=" + ContentId);
            if (Culture.Trim() != "") sb.Append("&culture=" + Culture);
            if (Alias.Trim().Length > 0) sb.Append("&alias=" + Alias);
            if (Lineage.Trim().Length > 0) sb.Append("&lineage=" + Lineage);
            if (MaxTitleLength > 0) sb.Append("&maxtitlelength=" + MaxTitleLength);
            if (MaxDescriptionLength > 0) sb.Append("&maxdesclength=" + MaxDescriptionLength);
            if (PageSize > 0) sb.Append("&pagesize=" + PageSize);
            if (PageIndex > 0) sb.Append("&pageindex=" + PageIndex);
            if (Parameters.Trim().Length> 0) sb.Append("&prms=" + Parameters);
            if (Where.Trim().Length > 0) sb.Append("&where=" + Where);
            if (SortBy.Trim().Length > 0) sb.Append("&sortby=" + SortBy);

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
            
            if (HttpContext.Current.Request["currcontentid"] != null)
                sb.Append("&currcontentid=" + HttpContext.Current.Request["currcontentid"].ToString());
            else
                if (HttpContext.Current.Request["contentid"] != null)
                    sb.Append("&currcontentid=" + HttpContext.Current.Request["contentid"].ToString());

            return sb.ToString();
        }

    }

    public class GenericMethods
    {

        #region XSL TRANSFORMATION -------------------------------------------------------------

        #endregion

        public static string GetSubSentence(string source, int maxLength)
        {
            if (maxLength <= 0) return source;

            string sourceWithOutHTML = System.Text.RegularExpressions.Regex.
                                        Replace(source, @"<(.|\n)*?>", string.Empty);

            if (sourceWithOutHTML.Trim().Length == 0)
                return source;

            StringBuilder sbRtn = new StringBuilder("");
            string[] words = sourceWithOutHTML.Split(' ');

            foreach (string word in words)
            {
                if (sbRtn.Length + word.Length >= maxLength)
                    return sbRtn.ToString();
                sbRtn.Append(word);
                sbRtn.Append(" ");
            }
            return sbRtn.ToString();
        }



        public static string ConvertDateTimeToGMT(DateTime dt)
        {
            return dt.ToString("r");
        }





    }


}
