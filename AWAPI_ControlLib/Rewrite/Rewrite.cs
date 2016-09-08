using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;
using System.Xml;

namespace AWAPI_ControlLib.Rewrite
{
    public class Rewrite
    {
        public string XmlPath
        {
            get
            {
                string path = System.Web.HttpContext.Current.Server.MapPath("~/awapi_rewriteurl.xml");
                if (System.IO.File.Exists(path))
                    return path;

                path = System.Web.HttpContext.Current.Server.MapPath("/awapi_rewriteurl.xml");
                return path;
            }
        }

        public RewriteConfig Load()
        {
            RewriteConfig config = new RewriteConfig();
            XPathDocument docNav = new XPathDocument(XmlPath);
            if (docNav == null)
                return null;

            XPathNavigator nav = docNav.CreateNavigator();

            //get main settings 
            string rewriteOn = GetNodeValue(nav, "/root/rewriteon");
            string dontRewriteIfExists = GetNodeValue(nav, "/root/dontrewriteifexists");


            config.RewriteOn = String.IsNullOrEmpty(rewriteOn) ? false : Convert.ToBoolean(rewriteOn);
            config.DontRewriteIfExists = String.IsNullOrEmpty(dontRewriteIfExists) ? true : Convert.ToBoolean(dontRewriteIfExists);
            config.Baseurl = GetNodeValue(nav, "/root/baseurl");

            //get extensions 
            XPathNodeIterator it = nav.Select("/root/extensions/extension");
            if (it != null)
            {
                config.RewriteExtensions = new List<RewriteExtension>();
                while (it.MoveNext())
                {
                    RewriteExtension extension = new RewriteExtension();
                    extension.Extension = GetNodeValue(it.Current, "extension", "value");
                    config.RewriteExtensions.Add(extension);
                }
            }

            //Get Rules
            it = nav.Select("/root/rules/rule");
            if (it != null)
            {
                config.RewriteRules = new List<RewriteRule>();
                while (it.MoveNext())
                {
                    RewriteRule rule = new RewriteRule();
                    rule.Source = GetNodeValue(it.Current, "rule", "source");
                    rule.Destination = GetNodeValue(it.Current, "rule", "destination");
                    rule.DestionationAfter = GetNodeValue(it.Current, "rule", "destionationafter");
                    rule.DestionationBefore = GetNodeValue(it.Current, "rule", "destionationbefore");
                    rule.EndTime = GetNodeValue(it.Current, "rule", "endtime");

                    string redirectOny= GetNodeValue(it.Current, "rule", "redirectonly");
                    if (!String.IsNullOrEmpty(redirectOny))
                        rule.RedirectOnly = Convert.ToBoolean(redirectOny);
                    rule.RedirectParam = GetNodeValue(it.Current, "rule", "redirectparam");
                    rule.StartTime = GetNodeValue(it.Current, "rule", "starttime");

                    config.RewriteRules.Add(rule);
                }
            }

            return config;
        }

        string GetNodeValue(XPathNavigator nav, string path, string attribute)
        {
            if (nav == null)
                return "";
            XmlNamespaceManager ns = new XmlNamespaceManager(nav.NameTable);
            XPathNodeIterator it = nav.Select(path);
            if (it == null)
                return "";

            it.MoveNext();

            return it.Current.GetAttribute(attribute, ns.DefaultNamespace);
        }


        string GetNodeValue(XPathNavigator nav, string path)
        {
            if (nav == null)
                return "";
            XmlNamespaceManager ns = new XmlNamespaceManager(nav.NameTable);
            XPathNodeIterator it = nav.Select(path);
            if (it == null)
                return "";

            it.MoveNext();

            return it.Current.Value;
        }




    }



    public class RewriteRule
    {
        private string _source = "";
        private string _destination = "";
        private string _startTime = "";
        private string _destionationBefore = "";
        private string _endTime = "";
        private string _destionationAfter = "";
        private bool _redirectOnly = false;
        private string _redirectParam = "";


        public string Source
        {
            get { return _source; }
            set { _source = value; }
        }
        public string Destination
        {
            get { return _destination; }
            set { _destination = value; }
        }


        public string StartTime
        {
            get { return _startTime; }
            set { _startTime = value; }
        }

        public string DestionationBefore
        {
            get { return _destionationBefore; }
            set { _destionationBefore = value; }
        }


        public string EndTime
        {
            get { return _endTime; }
            set { _endTime = value; }
        }

        public string DestionationAfter
        {
            get { return _destionationAfter; }
            set { _destionationAfter = value; }
        }


        public bool RedirectOnly
        {
            get { return _redirectOnly; }
            set { _redirectOnly = value; }
        }

        public string RedirectParam
        {
            get { return _redirectParam; }
            set { _redirectParam = value; }
        }



    }

    public class RewriteExtension
    {
        private string _extension = "";

        public string Extension
        {
            get { return _extension; }
            set { _extension = value; }
        }
    }

    public class RewriteConfig
    {
        private bool _rewriteOn = false;
        private string _baseurl = "/";
        private bool _dontRewriteIfExists = true;
        private System.Collections.Generic.List<RewriteRule> _rewriteRules = null;
        private System.Collections.Generic.List<RewriteExtension> _rewriteExtensions = null;


        public string Baseurl
        {
            get { return _baseurl; }
            set {
                if (value == null || Convert.ToString(value) == "")
                    _baseurl = "/";
                else                 
                    _baseurl = value; }
        }


        public bool RewriteOn
        {
            get { return _rewriteOn; }
            set { _rewriteOn = value; }
        }

        public bool DontRewriteIfExists
        {
            get { return _dontRewriteIfExists; }
            set { _dontRewriteIfExists = value; }
        }

        public System.Collections.Generic.List<RewriteRule> RewriteRules
        {
            get { return _rewriteRules; }
            set { _rewriteRules = value; }
        }


        public System.Collections.Generic.List<RewriteExtension> RewriteExtensions
        {
            get { return _rewriteExtensions; }
            set { _rewriteExtensions = value; }
        }
    }


}
