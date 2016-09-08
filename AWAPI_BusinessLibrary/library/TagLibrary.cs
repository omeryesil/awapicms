using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using AWAPI_Data.Data;
using AWAPI_Common.library;

namespace AWAPI_BusinessLibrary.library
{
    public class TagLibrary
    {
        ContentContextDataContext _context = new ContentContextDataContext();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteTagId"></param>
        /// <returns></returns>
        public awSiteTag Get(long siteTagId)
        {
            if (siteTagId <= 0)
                return null;
            var tag = _context.GetTable<awSiteTag>()
                        .Where(st => st.siteTagId.Equals(siteTagId));

            if (tag == null && tag.ToList().Count() == 0)
                return null;
            return tag.First();
        }


        public System.Collections.Generic.IEnumerable<awSiteTag> GetList(long siteId, string where)
        {
            StringBuilder sb = new StringBuilder(" SELECT * FROM awSiteTag ");
            StringBuilder sbWhere = new StringBuilder(" WHERE siteId=" + siteId);

            if (where.Trim().Length > 0)
                sbWhere.Append(" AND (" + where + ") ");

            sb.Append(sbWhere.ToString());
            sb.Append(" ORDER BY title");

            var tags = _context.ExecuteQuery<awSiteTag>(sb.ToString());
            return tags;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteTagId"></param>
        public void Delete(long siteTagId)
        {
            if (siteTagId <= 0)
                return;

            //Delete taggedContent relations
            DeleteTaggedContentByTagId(siteTagId);

            //delete blog and sub groups
            var tags = from t in _context.awSiteTags
                       where t.siteTagId.Equals(siteTagId)
                       select t;

            _context.awSiteTags.DeleteAllOnSubmit(tags);
            _context.SubmitChanges();
        }


        /// <summary>
        ///  Delete tag and blog relations
        /// </summary>
        /// <param name="siteTagId"></param>
        public void DeleteTaggedContentByTagId(long siteTagId)
        {
            if (siteTagId <= 0)
                return;

            var tags = from t in _context.awSiteTaggedContents
                       where t.siteTagId.Equals(siteTagId)
                       select t;

            _context.awSiteTaggedContents.DeleteAllOnSubmit(tags);
            _context.SubmitChanges();
        }

        /// <summary>
        ///  Delete tag and blog relations
        /// </summary>
        /// <param name="siteTagId"></param>
        public void DeleteTaggedContentByContentId(long contentId)
        {
            if (contentId <= 0)
                return;

            var tags = from t in _context.awSiteTaggedContents
                       where t.contentId.Equals(contentId) 
                       select t;

            _context.awSiteTaggedContents.DeleteAllOnSubmit(tags);
            _context.SubmitChanges();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public long Add(string title, long siteId)
        {
            awSiteTag tag = new awSiteTag();
            tag.siteTagId = AWAPI_Common.library.MiscLibrary.CreateUniqueId();

            tag.siteId = siteId;
            tag.title = title;
            tag.createDate = DateTime.Now;

            _context.awSiteTags.InsertOnSubmit(tag);
            _context.SubmitChanges();

            return tag.siteTagId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteTagId"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public bool Update(long siteTagId, string title)
        {
            awSiteTag tag = _context.awSiteTags.FirstOrDefault(st => st.siteTagId.Equals(siteTagId));

            if (tag == null)
                return false;

            tag.title = title;
            _context.SubmitChanges();

            return true;
        }

        #region TAGGED CONTENTS -------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteTagId"></param>
        /// <param name="blogId"></param>
        /// <returns></returns>
        public long TagContent(long siteTagId, long contentId)
        {
            awSiteTaggedContent tag = new awSiteTaggedContent();

            tag.siteTaggedContentId = AWAPI_Common.library.MiscLibrary.CreateUniqueId();
            tag.siteTagId = siteTagId;
            tag.contentId = contentId;

            _context.awSiteTaggedContents.InsertOnSubmit(tag);
            _context.SubmitChanges();

            return tag.siteTaggedContentId;        
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="tagIds"></param>
        /// <param name="blogId"></param>
        public void TagContent(IList<long> tagIds, long contentId)
        {
            if (contentId <= 0)
                return;

            //delete the current tags
            DeleteTaggedContentByContentId(contentId);

            if (tagIds == null || tagIds.ToList().Count == 0)
                return;
            
            //add the new ones
            foreach (long siteTagId in tagIds)
            {
                TagContent(tagIds, contentId);  
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogId"></param>
        /// <returns></returns>
        public System.Collections.Generic.IEnumerable<awSiteTag> GetContentTagList(long contentId)
        {
            if (contentId<=0)
                return null;

            var tags = from t in _context.awSiteTaggedContents
                       where t.contentId.Equals(contentId)
                       select t.awSiteTag;

            return tags;

        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tags">| separated tag names </param>
        /// <returns></returns>
        public System.Collections.Generic.IList<awContent> GetTaggedContentList(string tagNames)
        {
            if (tagNames.Trim().Length == 0)
                return null;

            System.Collections.ArrayList rtnList = new System.Collections.ArrayList();
            string[] names = tagNames.ToLower().Split('|');

            var contentIds = from t in _context.awSiteTaggedContents
                             where names.Contains(t.awSiteTag.title.ToLower())
                             select t.awContent;
            if (contentIds == null || contentIds.ToList().Count == 0)
                return null;

            return contentIds.ToList();
        }


        #endregion 

    }
}
