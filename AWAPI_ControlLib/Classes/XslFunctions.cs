using System;
using System.Web;

namespace AWAPI_ControlLib.Classes
{

    /// <summary>
    /// This class will be added to the xslt.
    /// </summary>
    public class XslFunctions
    {
        string _url = "";

        public XslFunctions()
        {
            if (HttpContext.Current != null && HttpContext.Current.Request != null)
                _url = HttpContext.Current.Request.Url.ToString();
        }

        public XslFunctions(string currentUrl)
        {
            _url = currentUrl.Trim().ToLower();
        }


        #region URL methods
        public string GetUrl()
        {
            return _url;
        }

        public string GetDomainUrl()
        {
            string domain;
            Uri url = HttpContext.Current.Request.Url;
            domain = url.AbsoluteUri.Replace(url.PathAndQuery, string.Empty);
            if (!domain.EndsWith("/"))
                domain += "/";

            return domain;
        }

        public string GetUriParam(string paramName)
        {
            if (String.IsNullOrEmpty(_url))
                return "";

            Uri uri = new Uri(_url);
            if (String.IsNullOrEmpty(uri.Query))
                return "";

            return HttpUtility.ParseQueryString(uri.Query).Get(paramName);
        }

        public string UrlEncode(string source)
        {
            return HttpContext.Current.Server.UrlEncode(source);
        }

        public string UrlDecode(string source)
        {
            return HttpContext.Current.Server.UrlDecode(source);
        }

        public string HtmlEncode(string source)
        {
            return HttpContext.Current.Server.HtmlEncode(source);
        }

        public string HtmlDecode(string source)
        {
            return HttpContext.Current.Server.HtmlDecode(source);
        }

        #endregion

        #region PAGE SPECIFIC
        public string SetPageTitle(string first, string second, string last)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<script language='javascript' type='text/javascript'>\n");
            sb.Append("document.title = \"" + first + second + last + "\"; \n");
            sb.Append("</script>\n");

            return sb.ToString();
        }

        public string AddToPageTitle(string first, string second)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<script language='javascript' type='text/javascript'>\n");
            sb.Append("document.title = document.title + \"" + first + second + "\"; \n");
            sb.Append("</script>\n");

            return sb.ToString();
        }
        #endregion

        #region STRING
        public string Trim(string source)
        {
            return source.Trim();
        }

        public string LTrim(string source)
        {
            return source.TrimStart();
        }

        public string Replace(string source, string replace, string replaceWith)
        {
            return System.Text.RegularExpressions.Regex.Replace(source, replace, replaceWith, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        }

        public int IndexOf(string source, string lookFor)
        {
            return source.ToLower().IndexOf(lookFor.ToLower());
        }

        public string ToLower(string source)
        {
            return source.ToLower();
        }

        public string ToUpper(string source)
        {
            return source.ToUpper();
        }

        public string SubString(string source, int start, int length)
        {
            return source.Substring(start, length);
        }

        public int Compare(string strA, string strB)
        {
            return string.Compare(strA, strB);
        }

        public string FormatNumber(string source, string format)
        {
            if (string.IsNullOrEmpty(format))
                return source;

            return string.Format(format, source);
        }
        #endregion

        #region DATE
        public string FormatDate(string dateTime, string format)
        {
            if (string.IsNullOrEmpty(dateTime))
                return "";

            return System.Convert.ToDateTime(dateTime).ToString(format);
        }

        public int DayOfWeek(string dateTime)
        {
            return Convert.ToInt32(System.Convert.ToDateTime(dateTime).DayOfWeek);
        }
        #endregion


        /// <summary>
        /// Returns appconfig 
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public string GetAppSetting(string keyName)
        {
            return System.Configuration.ConfigurationManager.AppSettings[keyName];
        }

        public string CreateUniqueLink(string id, string title, string extension)
        {
            string extensionSeperator = extension.IndexOf(".") >= 0 ? "" : ".";

            string t = String.IsNullOrEmpty(title) ? "" : "_" + FormatAliasName(title);

            if (extension.Length > 0)
                return id + t + extensionSeperator + extension;
            else
                return id + t;
        }

        public string FormatAliasName(string source)
        {
            source = source.Length > 40 ? source.Substring(0, 39).Trim() : source.Trim();
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[^a-zA-Z0-9]");
            string alias = reg.Replace(source.ToLower(), "-").Replace("--", "-");

            return alias;
        }
    }
}
