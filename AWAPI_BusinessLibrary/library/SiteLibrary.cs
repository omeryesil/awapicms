using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using AWAPI_Data.Data;

namespace AWAPI_BusinessLibrary.library
{

    public class SiteLibrary
    {
        SiteContextDataContext _context = new SiteContextDataContext();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public awSite Get(long siteId)
        {

            var sites = _context.GetTable<awSite>()
                        .Where(st => st.siteId.Equals(siteId));

            if (sites == null || sites.Count() == 0)
                return null;
            return sites.FirstOrDefault<awSite>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sortField"></param>
        /// <returns></returns>
        public System.Collections.Generic.IList<awSite> GetList(string sortField)
        {
            var list = from l in _context.awSites
                       orderby l.title
                       select l;

            if (list == null || list.Count() == 0)
                return null;

            return list.ToArray<awSite>();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public System.Collections.Generic.IEnumerable<awSite> GetUserSiteList(long userId)
        {
            DataSet ds = new DataSet();

            var siteIds = from su in _context.awSiteUsers
                          where su.userId.Equals(userId)
                          select su.siteId;

            var sites = from st in _context.awSites
                        where st.userId.Equals(userId) ||
                            siteIds.Contains(st.siteId)
                        orderby st.title
                        select st;

            return sites;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public void Delete(long siteId)
        {
            //delete files


            //delete posts


            //delete roleGroups 


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="alias"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="isEnabled"></param>
        /// <param name="link">http://awapi.com</param>
        /// <param name="imageurl"></param>
        /// <param name="maxBlogs"></param>
        /// <param name="maxUsers"></param>
        /// <param name="maxContents"></param>
        /// <param name="cultureCode"></param>
        /// <param name="grantedDomains"></param>
        /// <param name="bannedDomains"></param>
        /// <param name="accessKey"></param>
        /// <param name="twitterUsername"></param>
        /// <param name="twitterPassword"></param>
        /// <param name="userConfirmationEmailTemplateId"></param>
        /// <param name="userResetPasswordEmailTemplateId"></param>
        /// <param name="pubDate"></param>
        /// <returns></returns>
        public long Add(long userId,
                        string alias, string title, string description,
                        bool isEnabled, string link, string imageurl,
                        int maxBlogs, int maxUsers, int maxContents, string cultureCode,
                        string grantedDomains, string bannedDomains, string accessKey,
                        string twitterUsername, string twitterPassword, string fileAmazonS3BucketName,
                        long? userConfirmationEmailTemplateId, long? userResetPasswordEmailTemplateId,
                        DateTime? pubDate)
        {
            if (AliasExistForOtherSite(null, alias))
                throw new Exception("Alias already defined for another site.");

            long id = AWAPI_Common.library.MiscLibrary.CreateUniqueId();
            awSite site = new awSite();

            if (pubDate == null)
                pubDate = DateTime.Now;

            site.siteId = id;
            site.alias = alias.ToLower().Trim();
            site.title = title.Trim();
            site.description = description.Trim();
            site.isEnabled = isEnabled;
            site.link = link.Trim();
            site.imageurl = imageurl.Trim();
            site.maxBlogs = maxBlogs;
            site.maxUsers = maxUsers;
            site.maxContents = maxContents;
            site.pubDate = pubDate;
            site.lastBuildDate = DateTime.Now;
            site.createDate = DateTime.Now;
            site.userId = userId;
            site.cultureCode = cultureCode.Trim();
            site.grantedDomains = grantedDomains.Trim();
            site.bannedDomains = bannedDomains.Trim();
            site.accessKey = accessKey.Trim();
            site.twitterUsername = twitterUsername.Trim();
            site.twitterPassword = twitterPassword.Trim();
            site.fileAmazonS3BucketName = fileAmazonS3BucketName.Trim();

            site.userConfirmationEmailTemplateId = userConfirmationEmailTemplateId;
            site.userResetPasswordEmailTemplateId = userResetPasswordEmailTemplateId;

            _context.awSites.InsertOnSubmit(site);
            _context.SubmitChanges();

            return id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        public bool Update(long siteId, long userId,
                        string alias, string title, string description,
                        bool isEnabled, string link, string imageurl,
                        int maxBlogs, int maxUsers, int maxContents, string cultureCode,
                        string grantedDomains, string bannedDomains, string accessKey,
                        string twitterUsername, string twitterPassword, string fileAmazonS3BucketName,
                        long? userConfirmationEmailTemplateId, long? userResetPasswordEmailTemplateId,
                        DateTime? pubDate)
        {
            if (AliasExistForOtherSite(siteId, alias))
                throw new Exception("Alias already defined for another site.");


            awSite site = _context.awSites.FirstOrDefault(st => st.siteId.Equals(siteId));

            if (site == null)
                return false;

            if (pubDate == null)
                pubDate = DateTime.Now;

            site.userId = userId;
            site.alias = alias.ToLower().Trim();;
            site.title = title.Trim();;
            site.description = description.Trim();;
            site.isEnabled = isEnabled;
            site.link = link.Trim();;
            site.imageurl = imageurl.Trim();;
            site.maxBlogs = maxBlogs;
            site.maxUsers = maxUsers;
            site.maxContents = maxContents;
            site.cultureCode = cultureCode.Trim();;
            site.grantedDomains = grantedDomains.Trim();;
            site.bannedDomains = bannedDomains.Trim();;
            site.accessKey = accessKey.Trim();;
            site.twitterUsername = twitterUsername.Trim();;
            if (twitterPassword.Trim() != "")
                site.twitterPassword = new AWAPI_Common.library.SecurityLibrary().EncodeString(twitterPassword.Trim());
            site.fileAmazonS3BucketName = fileAmazonS3BucketName.Trim();

            site.userConfirmationEmailTemplateId = userConfirmationEmailTemplateId;
            site.userResetPasswordEmailTemplateId = userResetPasswordEmailTemplateId;

            site.pubDate = pubDate;
            site.lastBuildDate = DateTime.Now;

            _context.SubmitChanges();

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="alias"></param>
        /// <returns></returns>
        public bool AliasExistForOtherSite(long? siteId, string alias)
        {
            if (siteId == null || siteId == 0)
            {
                var list = from s in _context.awSites
                           where s.alias.ToLower().Equals(alias.ToLower())
                           select s;
                if (list == null || list.Count() == 0)
                    return false;
            }
            else
            {
                var list = from s in _context.awSites
                           where s.alias.ToLower().IndexOf(alias.ToLower()) >= 0 &&
                                !s.siteId.Equals(siteId.Value)
                           select s.alias;
                if (list != null || list.Count() == 0)
                    return false;
            }
            return true;
        }

    }
}
