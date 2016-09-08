using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using AWAPI_BusinessLibrary.library;

namespace AWAPI_Services.wcf
{
    // NOTE: If you change the class name "SiteService" here, you must also update the reference to "SiteService" in Web.config.
    public class SiteService : ISiteService
    {
        SiteLibrary _siteLibrary = new SiteLibrary();



        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public AWAPI_Data.Data.awSite Get(long siteId)
        {
            return _siteLibrary.Get(siteId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public System.Collections.Generic.IEnumerable<AWAPI_Data.Data.awSite> GetList(string where, string sortField)
        {
            return _siteLibrary.GetList(where, sortField);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public void Delete(long siteId)
        {
            _siteLibrary.Delete(siteId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="ownerUserId"></param>
        /// <param name="alias"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="isEnabled"></param>
        /// <param name="link"></param>
        /// <param name="imageurl"></param>
        /// <param name="maxBlogs"></param>
        /// <param name="maxUsers"></param>
        /// <param name="maxContent"></param>
        /// <param name="defaultLanguage"></param>
        /// <param name="pubDate"></param>
        /// <returns></returns>
        public bool Update(long siteId, long ownerUserId, string alias,  string title, string description,
                        bool isEnabled, string link, string imageurl, 
                        int maxBlogs, int maxUsers, int maxContent, string defaultLanguage, 
                        DateTime? pubDate)
        {
            return _siteLibrary.Update(siteId, ownerUserId, alias, title, description,
                        isEnabled, link, imageurl, maxBlogs, maxUsers, maxContent, defaultLanguage,  pubDate);

        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ownerUserId"></param>
        /// <param name="alias"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="isEnabled"></param>
        /// <param name="link"></param>
        /// <param name="imageurl"></param>
        /// <param name="maxBlogs"></param>
        /// <param name="maxUsers"></param>
        /// <param name="maxContent"></param>
        /// <param name="defaultLanguage"></param>
        /// <param name="pubDate"></param>
        /// <returns></returns>
        public long Add(long ownerUserId, string alias,
                        string title, string description,
                        bool isEnabled, string link,
                        string imageurl,
                        int maxBlogs, int maxUsers, int maxContent, string defaultLanguage, 
                        DateTime? pubDate)
        {
            return _siteLibrary.Add(ownerUserId, alias,
                        title, description, isEnabled, link, imageurl, maxBlogs, maxUsers, maxContent, defaultLanguage, pubDate);
        }


    }
}
