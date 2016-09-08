using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AWAPI_BusinessLibrary.library
{
    public class SecurityLibrary
    {

        /// <summary>
        /// Returns true if the referrer has right...
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public static bool IsValidReferrer(long siteId)
        {
            if (siteId <= 0)
                return false;

            AWAPI_BusinessLibrary.library.SiteLibrary _siteLib = new SiteLibrary();
            AWAPI_Data.Data.awSite site = _siteLib.Get(siteId);

            //check if site doesn't exist or disabled
            if (site == null || !site.isEnabled)
                return false;

            string domain = System.Web.HttpContext.Current.Request.UrlReferrer.Authority;// = "localhost:56624";

            //check if the domain is granted
            if (!String.IsNullOrEmpty(site.grantedDomains))
            {
                string[] grantedDomains = site.grantedDomains.Split(',');
                foreach (string dmn in grantedDomains)
                {
                    if (dmn.Trim().ToLower().Replace("www.", "").Replace("https://", "").Replace("http://", "") == domain.ToLower())
                        return true;
                }
                return false;
            }

            //check if the domain is banned
            if (!String.IsNullOrEmpty(site.bannedDomains))
            {
                string[] bannedDomains = site.bannedDomains.Split(',');
                foreach (string dmn in bannedDomains)
                    if (dmn.Trim().ToLower().Replace("www.", "").Replace("https://", "").Replace("http://", "") == domain.ToLower())
                        return false;
            }

            return true;
        }


        /// <summary>
        /// Access Key is required for insert/update methods,
        /// (We cannot get client's IP address from WCF)
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="accessKey"></param>
        /// <returns></returns>
        public static bool IsValidAccessKey(long siteId, string accessKey)
        {
            if (siteId <= 0 || String.IsNullOrEmpty(accessKey))
                return false;

            AWAPI_BusinessLibrary.library.SiteLibrary _siteLib = new SiteLibrary();
            AWAPI_Data.Data.awSite site = _siteLib.Get(siteId);

            //check if site doesn't exist or disabled
            if (site == null || !site.isEnabled)
                return false;

            if (site.accessKey.ToLower() == accessKey.ToLower())
                return true;
            return false;
        }

    }


}
