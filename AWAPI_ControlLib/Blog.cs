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
    public class Blog : AWAPI_ControlLib.Classes.BaseControl, INamingContainer
    {
        public enum EMethods
        {
            GetArchiveList,
            GetArchivedPostList,
            GetBlog,
            GetBlogList,
            GetCategoryList,
            GetPost,
            GetPostList,
            GetPostFileList,
            GetCommentList,
            GetTagList
        }

        #region Properties

        [Category("Setting")]
        [Description("Content Sercvice's URL address. Must be defined in web.config file; AWAPI_Content_Service_URL)")]
        public string ServiceBaseUrl
        {
            get
            {
                return Classes.Configuration.BlogServiceUrl;
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

        [DefaultValue(EMethods.GetPost)]
        [Description("")]
        public EMethods MethodName
        {
            get
            {
                if (ViewState["MethodName"] == null)
                    return EMethods.GetPost;
                return (EMethods)ViewState["MethodName"];
            }
            set
            {
                ViewState["MethodName"] = value;
            }
        }

        [Description("")]
        [DefaultValue(0)]
        public long BlogId
        {
            get
            {
                if (ViewState["BlogId"] != null)
                    return Convert.ToInt64(ViewState["BlogId"]);

                long id = GetPropertyValue("blogid", -1, -1);
                if (id > 0)
                    return id;

                if (System.Configuration.ConfigurationManager.AppSettings["AWAPI_BlogId"] != null)
                    return Convert.ToInt64(System.Configuration.ConfigurationManager.AppSettings["AWAPI_BlogId"]);
                return -1;
            }
            set
            { ViewState["BlogId"] = value; }
        }

        [Description("")]
        [DefaultValue(0)]
        public long BlogCategoryId
        {
            get
            {
                if (ViewState["BlogCategoryId"] != null)
                    return Convert.ToInt64(ViewState["BlogCategoryId"]);

                long id = GetPropertyValue("blogcatid", -1, 0);
                return id;
            }
            set
            { ViewState["BlogCategoryId"] = value; }
        }

        [Description("")]
        [DefaultValue(0)]
        public long PostId
        {
            get
            {
                if (ViewState["PostId"] != null)
                    return Convert.ToInt64(ViewState["PostId"]);
                long id = HttpContext.Current.Request["postid"] == null ? -1 : Convert.ToInt64(HttpContext.Current.Request["postid"]);
                
                id = GetPropertyValue("postid", id, DefaultPostId);
                return id;


            }
            set
            { ViewState["PostId"] = value; }
        }


        [Description("")]
        [DefaultValue(0)]
        public long TagId
        {
            get
            {
                if (ViewState["tagid"] != null)
                    return Convert.ToInt64(ViewState["tagid"]);

                long id = GetPropertyValue("TagId", -1, 0);
                return id;
            }
            set
            { ViewState["tagid"] = value; }
        }




        [Description("")]
        [DefaultValue(0)]
        public long DefaultBlogId
        {
            get
            {
                if (ViewState["DefaultBlogId"] == null)
                    return -1;
                return Convert.ToInt64(ViewState["DefaultBlogId"]);
            }
            set
            { ViewState["DefaultBlogId"] = value; }
        }



        [Description("")]
        [DefaultValue(0)]
        public long DefaultPostId
        {
            get
            {
                if (ViewState["DefaultPostId"] == null)
                    return -1;
                return Convert.ToInt64(ViewState["DefaultPostId"]);
            }
            set
            { ViewState["DefaultPostId"] = value; }
        }


        [Description("Keyword search. This is different than the Where. In the Where we can add logic; AND, OR, etc...")]
        public string Search
        {
            get
            {
                if (ViewState["Search"] != null)
                    return ViewState["Search"].ToString();

                string value = GetPropertyValue("search", "", "");
                return value;
            }
            set
            {
                ViewState["Search"] = value;
            }
        }

        [Description("Additional where statement for archieve (should be: month-year. Example: 1-2010")]
        public string Archive
        {
            get
            {
                if (ViewState["archive"] != null)
                    return ViewState["archive"].ToString();
                string value = GetPropertyValue("archive", "", "");
                return value;
            }
            set
            {
                ViewState["archive"] = value;
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
                //if (HttpContext.Current.Request["showurl"] != null)
                //    output.Write("<br/><span style='color:red;background-color:#ffffff;'>" + ServiceCall + "<br/></span>");

                System.Xml.Xsl.XsltArgumentList xslArgs = Classes.XslTransformation.GetArgumentListFromUrl();
                StringBuilder transformed = new StringBuilder("\n");
                transformed.AppendFormat("<!-- START WIDGET: Id: {0} -->\n", this.ID);
                transformed.AppendFormat("<!-- XSLT: {0} -->\n", XslPath);
                transformed.AppendFormat("<!-- Call: {0} -->\n", ServiceCall);
                transformed.Append(Classes.XslTransformation.Transform(XslPath, xslArgs, ServiceCall, this.ID, CacheDuration));
                transformed.AppendFormat("\n<!-- END WIDGET ---------------------------- -->\n", ServiceCall);
                output.Write(transformed.ToString());
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


        //public string GetXSLUrl(string path, bool addParams)
        //{
        //    string strRtn = path;

        //    if (path.IndexOf("http:") < 0)
        //    {
        //        string root = HttpContext.Current.Request.Url.Scheme + "://" + 
        //              HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath; 

        //        strRtn = path.Replace("~", "");
        //        if (!strRtn.StartsWith("/") && !root.EndsWith("/"))
        //            strRtn += "/";

        //        if (strRtn.StartsWith("/") && root.EndsWith("/"))
        //            strRtn = strRtn.Remove(0, 1);

        //        strRtn = root + strRtn;
        //    }

        //    if (addParams)
        //        strRtn += GetParamsForServiceCall();

        //    return strRtn;
        //}

        string GetArchivedPostList()
        {
            StringBuilder sb = new StringBuilder();
            //SortBy = GetPropertyValue("sortby", SortBy, "");
            PageSize = GetPropertyValue("pagesize", PageSize, 0);
            PageIndex = GetPropertyValue("pageindex", PageIndex, 0);
            //Archive = GetPropertyValue("archive", Archive, "");

            if (PageSize > 0) sb.Append("&pagesize=" + PageSize);
            if (PageIndex > 0) sb.Append("&pageindex=" + PageIndex);
            if (SortBy.Trim().Length > 0) sb.Append("&sortby=" + SortBy);

            if (HttpContext.Current.Request["currpostid"] != null)
                sb.Append("&currpostid=" + HttpContext.Current.Request["currpostid"].ToString());
            else
                if (HttpContext.Current.Request["postid"] != null)
                    sb.Append("&currpostid=" + HttpContext.Current.Request["postid"].ToString());

            return sb.ToString();        
        
        }

        string GetBlogPostList()
        {
            StringBuilder sb = new StringBuilder();

            //TagId = GetPropertyValue("TagId", TagId, 0);
            //BlogCategoryId = GetPropertyValue("blogcatid", BlogCategoryId, 0);
            //Search = GetPropertyValue("search", Search, "");
            //Where = GetPropertyValue("where", Where, "");
            //SortBy = GetPropertyValue("sortby", SortBy, "");
            PageSize = GetPropertyValue("pagesize", PageSize, 0);
            PageIndex = GetPropertyValue("pageindex", PageIndex, 0);

            if (BlogCategoryId > 0) sb.Append("&blogcatid=" + BlogCategoryId);
            if (Search.Trim().Length > 0) sb.Append("&search=" + Search);
            if (Where.Trim().Length > 0) sb.Append("&where=" + Where);
            if (TagId > 0) sb.Append("&tagid=" + TagId);
            if (PageSize > 0) sb.Append("&pagesize=" + PageSize);
            if (PageIndex > 0) sb.Append("&pageindex=" + PageIndex);
            if (SortBy.Trim().Length > 0) sb.Append("&sortby=" + SortBy);

            if (HttpContext.Current.Request["currpostid"] != null)
                sb.Append("&currpostid=" + HttpContext.Current.Request["currpostid"].ToString());
            else
                if (HttpContext.Current.Request["postid"] != null)
                    sb.Append("&currpostid=" + HttpContext.Current.Request["postid"].ToString());

            return sb.ToString();
        }

        string GetBlogPostFileList()
        {
            StringBuilder sb = new StringBuilder();
            if (PostId > 0) sb.Append("&postid=" + PostId);
            return sb.ToString();
        }

        string GetBlogCommentList()
        {
            StringBuilder sb = new StringBuilder();

            //PostId = GetPropertyValue("PostId", PostId, DefaultPostId);
            PageSize = GetPropertyValue("pagesize", PageSize, 0);
            PageIndex = GetPropertyValue("pageindex", PageIndex, 0);

            if (PostId > 0) sb.Append("&postid=" + PostId);
            if (PageSize > 0) sb.Append("&pagesize=" + PageSize);
            if (PageIndex > 0) sb.Append("&pageindex=" + PageIndex);

            return sb.ToString();
        }

        string GetBlogPost()
        {
            StringBuilder sb = new StringBuilder();

           // PostId = GetPropertyValue("PostId", PostId, DefaultPostId);
            if (PostId > 0) sb.Append("&postid=" + PostId);

            return sb.ToString();
        }

        string GetTagList()
        {
            StringBuilder sb = new StringBuilder();

            PageSize = GetPropertyValue("pagesize", PageSize, 0);
            if (PageSize > 0) sb.Append("&pagesize=" + PageSize);
            return sb.ToString();
        }

        string GetUriParameters(string sourceUrl)
        {
            StringBuilder sb = new StringBuilder();
            string src = sourceUrl.ToLower();

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

                    string prmName = (prm.Split('='))[0].ToLower();

                    //if parameter hasn't been added 
                    if (src.IndexOf("&" + prmName + "=") < 0)
                    {
                        //if the parementer is not in the ignore list
                        if (!IsInIgnoreList(prmName))
                            if (sb.ToString().ToLower().IndexOf(prmName) < 0)
                                sb.Append("&" + prm);
                    }
                }
            }

            return sb.ToString();

        }

        string GetParamsForServiceCall()
        {
            //BlogId = GetPropertyValue("BlogId", BlogId, DefaultBlogId);

            StringBuilder sb = new StringBuilder("");
            sb.Append("?method=" + MethodName);
            sb.Append("&siteid=" + SiteId);
            sb.Append("&type=" + XMLType.ToString());
            if (BlogId > 0) sb.Append("&blogid=" + BlogId);

            if (MethodName == EMethods.GetPostList)
                sb.Append(GetBlogPostList());
            else if (MethodName == EMethods.GetPost)
                sb.Append(GetBlogPost());
            else if (MethodName == EMethods.GetCommentList)
                sb.Append(GetBlogCommentList());
            else if (MethodName == EMethods.GetArchivedPostList)
                sb.Append(GetArchivedPostList());
            else if (MethodName == EMethods.GetPostFileList)
                sb.Append(GetBlogPostFileList());

            if (MaxTitleLength > 0) sb.Append("&maxtitlelength=" + MaxTitleLength);
            if (MaxDescriptionLength > 0) sb.Append("&maxdesclength=" + MaxDescriptionLength);
            if (Parameters.Trim().Length > 0) sb.Append("&prms=" + Parameters);

            sb.Append(GetUriParameters(sb.ToString()));

            return sb.ToString();
        }
    }

}
