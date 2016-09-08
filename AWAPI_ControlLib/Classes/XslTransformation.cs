using System.Data;
using System.Web;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;


namespace AWAPI_ControlLib.Classes
{
    public class XslTransformation
    {
        public static XsltArgumentList GetArgumentListFromUrl()
        {
            XsltArgumentList arg = new XsltArgumentList();

            if (HttpContext.Current.Request.Url.Query == null ||
                HttpContext.Current.Request.Url.Query.Trim().Length == 0)
                return arg;

            string uriPrms = HttpContext.Current.Request.Url.Query;

            if (uriPrms.Trim().Length > 0)
            {
                string bfr = uriPrms.Replace("?", "&");
                string[] prms = bfr.Split('&');
                foreach (string prmAndValue in prms)
                {
                    int equalPos = prmAndValue.IndexOf("="); //don't use split, may contain '=' in the value
                    if (equalPos > 0)
                    {
                        string prmName = System.Web.HttpUtility.HtmlDecode(prmAndValue.Substring(0, equalPos)).Trim();

                        if (arg.GetParam(prmName, "") == null)
                        {
                            string value = prmAndValue.Substring(equalPos + 1, prmAndValue.Length - equalPos - 1);
                            arg.AddParam(prmName, "", value);
                        }
                    }
                }
            }
            return arg;
        }

        public static string TransformXSLToHTML(string xslFilePath, string xmlUrl)
        {
            XsltArgumentList arg = GetArgumentListFromUrl();
            return Transform(xslFilePath, arg, xmlUrl, "", "");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="xslt"></param>
        /// <param name="pathDoc"></param>
        /// <param name="cacheDuration">This value is taken by the CacheAspect</param>
        /// <returns></returns>
        [Classes.CacheAspect("AWAPI_WidgetCachedDuration")]
        public static string Transform(string xslFilePath, XsltArgumentList arg, string xmlUrl, string controlId, string cacheDuration)
        {
            System.Xml.Xsl.XslCompiledTransform xslTransform = new System.Xml.Xsl.XslCompiledTransform();
            xslTransform.Load(xslFilePath);

            //Add helper class
            XslFunctions func = new XslFunctions(HttpContext.Current.Request.Url.ToString());
            if (arg == null)
                arg = new XsltArgumentList();
            arg.AddExtensionObject("awp:functions", func);

            //TRANSFORM
            System.IO.StringWriter sw = new System.IO.StringWriter();
            xslTransform.Transform(xmlUrl, arg, sw);
            return System.Web.HttpUtility.HtmlDecode(sw.ToString());
        }

        public static string TransformXSLToHTML(string xslFilePath, DataSet ds)
        {
            if (ds == null || ds.Tables.Count == 0)
                return "";

            string xmlData = ds.GetXml();
            XPathDocument pathDoc = new XPathDocument(new System.IO.StringReader(xmlData));
            return TransformXSLToHTML(xslFilePath, pathDoc);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xslt"></param>
        /// <param name="pathDoc"></param>
        /// <param name="uriPrms"></param>
        /// <returns></returns>
        public static string TransformXSLToHTML(string xslFilePath, XPathDocument pathDoc)
        {
            System.Xml.Xsl.XslCompiledTransform xslTransform = new System.Xml.Xsl.XslCompiledTransform();
            XsltArgumentList arg = GetArgumentListFromUrl();

            //SET SETTINGS
            XmlReaderSettings set = new XmlReaderSettings();
            set.ProhibitDtd = false;
            set.XmlResolver = null;
            XmlReader xslRdr = XmlReader.Create(xslFilePath);
            xslTransform.Load(xslRdr);

            //TRANSFORM
            System.IO.StringWriter sw = new System.IO.StringWriter();
            xslTransform.Transform(pathDoc, arg, sw);
            return System.Web.HttpUtility.HtmlDecode(sw.ToString());
        }

    }


}
