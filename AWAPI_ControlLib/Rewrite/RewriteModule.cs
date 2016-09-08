using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Configuration;
using System.Xml;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.IO;
using System.Collections.Specialized;
using System.Xml.XPath;
using System.Reflection;


namespace AWAPI_ControlLib.RewriteUrl
{
    class RewriteModule : IHttpModule
    {
        public void Dispose() { }

        public void Init(HttpApplication context)
        {
            // it is necessary to 
            context.BeginRequest += new EventHandler(RewriteModule_BeginRequest);
            context.PreRequestHandlerExecute += new EventHandler(RewriteModule_PreRequestHandlerExecute);
        }

        #region FOR POSTBACK
        void RewriteModule_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            HttpContext context = application.Context;
            if (context.Handler is Page)
            {
                Page page = (Page)context.Handler;
                page.PreRender += new EventHandler(RegisterControlAdapters);
            }
        }

        private void RegisterControlAdapters(object sender, EventArgs e)
        {
            Page page = (Page)sender;
            page.PreRender -= new EventHandler(RegisterControlAdapters);
            if (page.Form != null)
            {
                // attach new instance of control adapter  
                FieldInfo adapterFieldInfo = page.Form.GetType().GetField("_adapter",
                                             BindingFlags.NonPublic | BindingFlags.Instance);
                if (adapterFieldInfo != null)
                {
                    HtmlFormAdapter adapter = new HtmlFormAdapter();
                    FieldInfo controlFieldInfo = adapter.GetType().GetField("_control",
                                                 BindingFlags.NonPublic | BindingFlags.Instance);
                    if (controlFieldInfo != null)
                    {
                        controlFieldInfo.SetValue(adapter, page.Form);
                        adapterFieldInfo.SetValue(page.Form, adapter);
                    }
                }
            }
        }
        #endregion


        private void RewriteModule_BeginRequest(object sender, EventArgs e)
        {
            AWAPI_ControlLib.Rewrite.Rewrite rewriteLib = new AWAPI_ControlLib.Rewrite.Rewrite();
            AWAPI_ControlLib.Rewrite.RewriteConfig rewriteConfig = rewriteLib.Load();
            if (rewriteConfig == null || rewriteConfig.RewriteRules==null || rewriteConfig.RewriteRules.Count == 0)
                return;
            
            string path = HttpContext.Current.Request.Path; ;
            string extension = Path.GetExtension(path).ToLower();
            bool redirectOnly = false; //if true, it won't rewrite, but redirect..

            // there us nothing to process
            if (path.Length == 0) return;

            // module is turned off in web.config
            if (!rewriteConfig.RewriteOn) 
                return;

            // don't rewrite the url if the file exists. 
            // this value is set in web.config
            if (rewriteConfig.DontRewriteIfExists)
                if (System.IO.File.Exists(path))
                    return;

            //Check if the extension is ok to redirect (we don't want to redirect everything)
            //If we want to redirect all kind of files, we should add <extension value=".*"/>
            if (extension != "" && rewriteConfig.RewriteExtensions != null)
            {
                bool extensionOk = false;

                foreach (AWAPI_ControlLib.Rewrite.RewriteExtension ext in rewriteConfig.RewriteExtensions)
                {
                    if (ext.Extension == ".*" || extension == ext.Extension)
                    {
                        extensionOk = true;
                        break;
                    }
                }
                if (!extensionOk)
                    return;
            }

            // load rewriting rules from web.config
            // and loop through rules collection until first match
            DateTime now = DateTime.Now;
            foreach (AWAPI_ControlLib.Rewrite.RewriteRule rule in rewriteConfig.RewriteRules)
            {
                try
                {

                    redirectOnly = rule.RedirectOnly;
                    string redirectParam = rule.RedirectParam;
                    if (redirectOnly && HttpContext.Current.Request[redirectParam] != null)
                        return;

                    //blog1/p/(\d+)_(.+).htm
                    string pattern = rule.Source; 
                    Regex re = new Regex(pattern, RegexOptions.IgnoreCase);
                    Match match = re.Match(path);
                    if (match.Success)
                    {
                        //if this is a scheduled rule
                        if (!String.IsNullOrEmpty(rule.StartTime) && !String.IsNullOrEmpty(rule.EndTime) &&
                            !String.IsNullOrEmpty(rule.DestionationBefore) && !String.IsNullOrEmpty(rule.DestionationAfter))
                        {
                            DateTime startDate = DateTime.ParseExact(rule.StartTime, "yyyy-MM-dd HH:mm:ss", null);
                            DateTime endDate = DateTime.ParseExact(rule.EndTime, "yyyy-MM-dd HH:mm:ss", null);

                            if (now < startDate)
                                path = re.Replace(path, rule.DestionationBefore);
                            else if (now > endDate)
                                path = re.Replace(path, rule.DestionationAfter);
                            else
                            {
                                //if destination is not set, return
                                if (String.IsNullOrEmpty(rule.Destination))
                                    return;
                                path = re.Replace(path, rule.Destination);
                            }
                        }
                        else
                            path = re.Replace(path, rule.Destination);

                        if (path.Length != 0)
                        {
                            // check for QueryString parameters
                            if (HttpContext.Current.Request.QueryString.Count != 0)
                            {
                                // if there are Query String papameters
                                // then append them to current path
                                string sign = (path.IndexOf('?') == -1) ? "?" : "&";
                                path = path + sign + HttpContext.Current.Request.QueryString.ToString();
                            }
                            // new path to rewrite to
                            string rew = rewriteConfig.Baseurl + path;
                            // save original path to HttpContext for further use
                            HttpContext.Current.Items.Add("OriginalUrl", HttpContext.Current.Request.RawUrl);
                            // rewrite
                            //HttpContext.Current.Response.Write(rew);
                            if (redirectOnly)
                            {
                                string url = rew;
                                if (rew.IndexOf("http")<0)
                                    url = rew.Replace("///", "/");    
                                
                                //we should prevent redirecting the page,,,
                                if (url.IndexOf("?") >0)
                                    url += "&";
                                else 
                                    url += "?";

                                url += redirectParam + "=1";

                                HttpContext.Current.Response.Redirect(url);
                                HttpContext.Current.Response.End();
                            }
                            else 
                                HttpContext.Current.RewritePath(rew);
                        }
                        return;
                    }
                }
                catch (Exception ex)
                {
                    //throw (new Exception("Incorrect rule.", ex));
                }
            }
            return;
        }

        public string GetDomainUrl()
        {
            string domain;
            Uri url = HttpContext.Current.Request.Url;
            domain = url.AbsoluteUri.Replace(url.PathAndQuery, string.Empty);
            return domain;
        }


        //private bool ValidateToParse()
        //{
        //    string path = HttpContext.Current.Request.Url.LocalPath;
        //    _extension = Path.GetExtension(path).ToLower();

        //    if (System.IO.File.Exists(path))
        //        return false;

        //    if (_extension == "")
        //    {
        //        if (!path.EndsWith("\\"))
        //            path += "\\";
        //        if (System.IO.File.Exists(path + "default.aspx") ||
        //            System.IO.File.Exists(path + "default.asp") ||
        //            System.IO.File.Exists(path + "index.html") ||
        //            System.IO.File.Exists(path + "index.htm"))
        //            return false;
        //    }

        //    return true;
        //}

    }


    public class HtmlFormAdapter : System.Web.UI.Adapters.ControlAdapter
    {
        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(new HtmlFormWriter(writer));
        }
        private class HtmlFormWriter : HtmlTextWriter
        {
            public HtmlFormWriter(HtmlTextWriter writer)

                : base(writer)
            {

                this.InnerWriter = writer.InnerWriter;

            }

            public HtmlFormWriter(TextWriter writer)

                : base(writer)
            {

                this.InnerWriter = writer;

            }

            public override void WriteAttribute(string key, string value, bool fEncode)
            {

                if (string.Compare(key, "action") == 0)
                {

                    value = HttpContext.Current.Request.RawUrl;

                }

                base.WriteAttribute(key, value, fEncode);

            }

        }

    }
}
