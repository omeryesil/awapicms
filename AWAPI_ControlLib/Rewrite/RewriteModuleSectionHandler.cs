using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Web;
using System.Xml;


namespace AWAPI_ControlLib.RewriteUrl
{
    public class RewriteModuleSectionHandler : IConfigurationSectionHandler
    {

        private XmlNode _xmlSection;
        private string _rewriteBase;
        private bool _rewriteOn = false;
        private bool _dontRedirectIfExists = false;

        public XmlNode XmlSection
        {
            get { return _xmlSection; }
        }

        public string RewriteBase
        {
            get { return _rewriteBase; }
        }


        /// <summary>
        /// If true, then rewrites the url
        /// </summary>
        public bool RewriteOn
        {
            get { return _rewriteOn; }
        }

        /// <summary>
        /// if true, the checks the file if already exists, 
        /// if exists, doesn't rewrites the url...
        /// </summary>
        public bool DontRedirectIfExists
        {
            get { return _dontRedirectIfExists; }
        }



        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            // set base path for rewriting module to
            // application root
            _rewriteBase = HttpContext.Current.Request.ApplicationPath + "/";

            // process configuration section 
            // from web.config
            try
            {
                _xmlSection = section;
                _rewriteOn = Convert.ToBoolean(section.SelectSingleNode("rewriteOn").InnerText);
                if (section.SelectSingleNode("dontRewriteIfExists") != null)
                    _dontRedirectIfExists = Convert.ToBoolean(section.SelectSingleNode("dontRewriteIfExists").InnerText);

            }
            catch (Exception ex)
            {
                throw (new Exception("Error while processing RewriteModule configuration section.", ex));
            }
            return this;
        }
    }
}
