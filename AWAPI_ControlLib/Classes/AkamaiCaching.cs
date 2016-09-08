using System;
using System.Text.RegularExpressions;
using System.Web;


namespace AWAPI_ControlLib.Classes
{
    public class AkamaiCaching
    {
        #region PROPERTIES
        public bool AkamaiEnabled
        {
            get
            {
                if (System.Configuration.ConfigurationManager.AppSettings["AWAPI_Cache_AkamaiEnabled"] != null &&
                        AkamaiURL != "" &&
                        (System.Configuration.ConfigurationManager.AppSettings["AWAPI_Cache_AkamaiEnabled"] == "1" ||
                        System.Configuration.ConfigurationManager.AppSettings["AWAPI_Cache_AkamaiEnabled"].ToLower() == "true"))
                    return true;
                return false;

            }
        }

        public string AkamaiURL
        {
            get
            {
                if (System.Configuration.ConfigurationManager.AppSettings["AWAPI_Cache_AkamaiURL"] != null)
                    return System.Configuration.ConfigurationManager.AppSettings["AWAPI_Cache_AkamaiURL"].ToLower();
                return "";
            }
        }

        /// <summary>
        /// If set, then the the link will be added to the akamai url
        /// </summary>
        public string DomainNameForLocalCall
        {
            get
            {
                if (System.Configuration.ConfigurationManager.AppSettings["AWAPI_Cache_AkamaiDomainName"] != null)
                    return System.Configuration.ConfigurationManager.AppSettings["AWAPI_Cache_AkamaiDomainName"].ToLower();
                return "";
            }
        }
        #endregion

        public string AddAkamai(string sourceUrl)
        {
            //if already akamaid
            if (String.IsNullOrEmpty(sourceUrl))
                return "";

            if (sourceUrl.ToLower().IndexOf("akamai") > 0)
                return sourceUrl;

            sourceUrl = sourceUrl.Replace("~", "");

            if (AkamaiEnabled &&
                sourceUrl.IndexOf(".axd") < 0)
            {
                string akamaiURL = AkamaiURL; // ConfigurationManager.AppSettings["AkamaiURL"].ToLower();
                string dns = "";

                //if domain name isn't in the original url...
                if (sourceUrl.IndexOf(DomainNameForLocalCall.ToLower()) < 0 &&
                    sourceUrl.IndexOf(".ca/") < 0 &&
                    sourceUrl.IndexOf(".com/") < 0 &&
                    sourceUrl.IndexOf(".net/") < 0 &&
                    sourceUrl.IndexOf(".org/") < 0 &&
                    sourceUrl.IndexOf(".gov/") < 0)
                {
                    dns = DomainNameForLocalCall;
                    if (dns == "")
                    {
                        dns = HttpContext.Current.Request.Url.Host;
                        if (HttpContext.Current.Request.Url.Port.ToString() != "80")
                            dns += ":" + HttpContext.Current.Request.Url.Port.ToString();
                    }
                }

                if (!sourceUrl.StartsWith("/") && sourceUrl.IndexOf("http") < 0)
                    sourceUrl = "/" + sourceUrl;

                return akamaiURL + dns + sourceUrl.Replace("https://", "").Replace("http://", "");
            }
            return sourceUrl;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public string ReplaceForAkamai(string source, string pattern)
        {
            if (!AkamaiEnabled)
                return source;

            Regex r = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            source = r.Replace(source, ReplaceAll);
            return source;
        }


        /// <summary>
        /// Replaces all the links with akamai call
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private string ReplaceAll(Match m)
        {
            string rtn = m.ToString();

            if (rtn.ToLower().Trim().IndexOf(AkamaiURL) >= 0)
                return rtn;

            string dns = "";
            string matchValue = m.ToString().ToLower().Trim();

            if (matchValue.IndexOf("www") < 0 && matchValue.IndexOf(".ca") < 0 &&
                matchValue.IndexOf(".com") < 0 && matchValue.IndexOf(".net") < 0 &&
                matchValue.IndexOf(".axd") < 0)
            {
                dns = DomainNameForLocalCall;
                if (dns == "")
                {
                    dns = HttpContext.Current.Request.Url.Host;
                    if (HttpContext.Current.Request.Url.Port.ToString() != "80")
                        dns += ":" + HttpContext.Current.Request.Url.Port.ToString();
                }
                if (!matchValue.StartsWith("/"))
                    matchValue = "/" + matchValue;

                rtn = AkamaiURL + dns + matchValue.Replace("http://", "");
            }
            return rtn;
        }


    }

}
