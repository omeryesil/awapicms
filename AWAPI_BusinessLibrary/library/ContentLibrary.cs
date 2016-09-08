using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using AWAPI_Data.Data;
using AWAPI_Common.library;

namespace AWAPI_BusinessLibrary.library
{
    public class ContentLibrary
    {
        ContentContextDataContext _context = new ContentContextDataContext();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentId"></param>
        /// <returns></returns>
        public awContent Get(long contentId)
        {
            return Get(contentId, null);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentId"></param>
        /// <param name="cultureCode"></param>
        /// <returns></returns>
        public awContent Get(long contentId, string cultureCode)
        {
            return Get(contentId, cultureCode, false);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogId"></param>
        /// <returns></returns>
        public awContent Get(string alias, string cultureCode, bool returnDefaultForLanguage)
        {
            if (String.IsNullOrEmpty(alias))
                return null;

            var contList = from c in _context.awContents
                           where c.alias.ToLower().Equals(alias.ToLower())
                           select c;

            if (contList == null || contList.Count() == 0)
                return null;

            awContent cont = contList.ToList()[0];
            if (String.IsNullOrEmpty(cultureCode) || cont.awSite_.cultureCode == cultureCode)
                return cont;

            var contLangList = from cl in _context.awContentCultures
                               where cl.contentId.Equals(cont.contentId) &&
                                    cl.cultureCode.ToLower().Equals(cultureCode.ToLower())
                               select cl;

            if (contLangList == null || contLangList.Count() == 0)
            {
                if (!returnDefaultForLanguage)
                {
                    cont.title = null;
                    cont.description = null;
                    cont.link = null;
                    cont.imageurl = null;
                }
                return cont;
            }

            awContentCulture contLang = contLangList.FirstOrDefault<awContentCulture>();
            cont.title = (returnDefaultForLanguage) ? ((contLang.title == null) ? cont.title : contLang.title) : contLang.title;
            cont.description = (returnDefaultForLanguage) ? ((contLang.description == null) ? cont.description : contLang.description) : contLang.description;
            cont.link = (returnDefaultForLanguage) ? ((contLang.link == null) ? cont.link : contLang.link) : contLang.link;
            cont.imageurl = (returnDefaultForLanguage) ? ((contLang.imageurl == null) ? cont.imageurl : contLang.imageurl) : contLang.imageurl;
            return cont;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogId"></param>
        /// <returns></returns>
        public awContent Get(long contentId, string cultureCode, bool returnDefaultForLanguage)
        {
            if (contentId <= 0)
                return null;

            var contList = from c in _context.awContents
                           where c.contentId.Equals(contentId)
                           select c;

            if (contList == null || contList.Count() == 0)
                return null;

            awContent cont = contList.ToList()[0];
            if (String.IsNullOrEmpty(cultureCode) || cont.awSite_.cultureCode == cultureCode)
                return cont;

            var contLangList = from cl in _context.awContentCultures
                               where cl.contentId.Equals(cont.contentId) &&
                                    cl.cultureCode.ToLower().Equals(cultureCode.ToLower())
                               select cl;

            if (contLangList == null || contLangList.Count() == 0)
            {
                if (!returnDefaultForLanguage)
                {
                    cont.title = null;
                    cont.description = null;
                    cont.link = null;
                    cont.imageurl = null;
                }
                return cont;
            }

            awContentCulture contLang = contLangList.FirstOrDefault<awContentCulture>();
            cont.title = (returnDefaultForLanguage) ? ((contLang.title == null) ? cont.title : contLang.title) : contLang.title;
            cont.description = (returnDefaultForLanguage) ? ((contLang.description == null) ? cont.description : contLang.description) : contLang.description;
            cont.link = (returnDefaultForLanguage) ? ((contLang.link == null) ? cont.link : contLang.link) : contLang.link;
            cont.imageurl = (returnDefaultForLanguage) ? ((contLang.imageurl == null) ? cont.imageurl : contLang.imageurl) : contLang.imageurl;
            return cont;

        }

        public System.Collections.Generic.IList<awContent> GetList(long siteId, string where, string sortField)
        {
            return GetList(false, siteId, where, sortField, null);
        }

        public bool IsParentsAvailable(awContent content)
        {
            return IsParentsAvailable(false, content);
        }

        /// <summary>
        /// Checks if the current tag or the parent posts are available (isEnabled && publishing dates are ok)
        /// </summary>
        /// <param name="blog"></param>
        /// <returns></returns>
        public bool IsParentsAvailable(bool checkCurrentContentIsAvailable, awContent content)
        {
            DateTime dtNow = DateTime.Now;

            if (checkCurrentContentIsAvailable)
                if (content == null || !content.isEnabled ||
                    content.pubDate != null && content.pubDate > dtNow ||
                    content.pubEndDate != null && content.pubEndDate < dtNow)
                    return false;

            if (content.lineage == null || content.lineage.Trim() == "")
                return true;

            string[] parentIdsTmp = content.lineage.Split('|');
            System.Collections.ArrayList parentIds = new System.Collections.ArrayList();
            foreach (string parentId in parentIdsTmp)
                if (parentId != "")
                    parentIds.Add(Convert.ToInt64(parentId));

            var parents = from p in _context.awContents
                          where p.isEnabled &&
                                (p.pubDate == null || p.pubDate <= dtNow && p.pubEndDate == null || p.pubEndDate >= dtNow) &&
                                parentIds.ToArray().Contains(p.contentId)
                          select p;

            if (parents == null || parents.Count() < parentIds.Count)
                return false;

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="where"></param>
        /// <param name="sortField"></param>
        /// <returns></returns>
        public System.Collections.Generic.IList<awContent> GetList(bool onlyAvailables, long siteId, string where, string sortField, string cultureCode)
        {
            StringBuilder sb = new StringBuilder(" SELECT * FROM awContent ");
            StringBuilder sbWhere = new StringBuilder(" WHERE siteId=" + siteId);

            if (onlyAvailables)
            {
                string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                sbWhere.Append(" AND isEnabled =1 ");
                sbWhere.Append(" AND (pubDate    IS NULL OR pubDate<='" + date + "') ");
                sbWhere.Append(" AND (pubEndDate IS NULL OR pubEndDate>='" + date + "') ");
            }
            if (where.Trim().Length > 0)
                sbWhere.Append(" AND (" + where + ") ");

            sb.Append(sbWhere.ToString());

            if (sortField.Trim().Length > 0)
                sb.Append(" ORDER BY " + sortField);
            else
                sb.Append(" ORDER BY Lineage, sortOrder, title");

            var contents = _context.ExecuteQuery<awContent>(sb.ToString());
            if (contents == null)
                return null;

            //UPDATE CONTENT BASED ON THE CULTURE CODE -----------------------------------------

            //if the default culture == groups' culture code, 
            List<awContent> contentList = contents.ToList<awContent>();
            if (contentList.Count == 0 ||       //if list is empty
                String.IsNullOrEmpty(cultureCode) ||  //if culture code is epty
                String.IsNullOrEmpty(contentList[0].awSite_.cultureCode) ||  //if site's default culture code is empty
                contentList[0].awSite_.cultureCode == cultureCode)  //if default culture code is equal to culturecode
                return contentList;

            //GET ALL THE IDS
            ArrayList contentIds = new ArrayList();
            for (int i = 0; i < contentList.Count(); i++)
                contentIds.Add(contentList[i].contentId);
            var cLangList = from cl in _context.awContentCultures
                            where cl.cultureCode.Equals(cultureCode) &&
                                contentIds.ToArray().Contains(cl.contentId)
                            select cl;
            //UPDATE Title, Description, etc... 
            IList<awContentCulture> langList = cLangList.ToList<awContentCulture>();

            if (langList == null || cLangList.Count() == 0)
                return contentList;

            foreach (awContentCulture cl in cLangList)
            {
                var conts = from c in contentList
                            where c.contentId.Equals(cl.contentId)
                            select c;

                awContent cont = conts.FirstOrDefault<awContent>();
                if (cont == null)
                    continue;

                cont.title = String.IsNullOrEmpty(cl.title) ? cont.title : cl.title;
                cont.description = String.IsNullOrEmpty(cl.description) ? cont.description : cl.description;
                cont.imageurl = String.IsNullOrEmpty(cl.imageurl) ? cont.imageurl : cl.imageurl;
                cont.link = String.IsNullOrEmpty(cl.link) ? cont.link : cl.link;
            }


            return contentList;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="blogId"></param>
        /// <param name="titleLineage"></param>
        /// <returns></returns>
        public awContent GetByAliasLineage(long siteId, long contentId, string aliasLineage, string cultureCode)
        {

            if (siteId <= 0 || aliasLineage.Trim() == "")
                return null;

            //if blogId <=0 then we will look from the root, 
            //else we will get sub groups under the contentid

            if (contentId < 0)
                contentId = 0;
            string[] contentNames = aliasLineage.ToLower().Split('|');
            IList<awContent> contents = GetList(true, siteId, "", "", cultureCode);

            if (contents == null || contents.Count == 0)
                return null;

            long parentContentId = contentId;

            awContent cnt = null;
            foreach (string alias in contentNames)
            {
                var list = from c in contents
                           where c.isEnabled.Equals(true) &&
                                c.parentContentId.Equals(parentContentId) &&
                                c.alias.ToLower().Trim().Equals(alias)
                           orderby c.sortOrder
                           select c;
                if (list == null || list.Count() == 0)
                    return null;
                cnt = list.FirstOrDefault();
                parentContentId = cnt.contentId;
            }

            //check if all the parents are enabled
            return cnt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogId"></param>
        public void Delete(long contentId)
        {
            if (contentId <= 0)
                return;

            //GET CONTENTS TO BE DELETED --------------------------
            ArrayList contentIdsToBeDeleted = new ArrayList();
            string parentId = FormatLineage(contentId);
            var contents = from c in _context.awContents
                           where c.lineage.IndexOf(parentId) >= 0 ||
                                c.contentId.Equals(contentId)
                           select c;
            if (contents != null && contents.Count() > 0)
            {
                IList<awContent> tmp = contents.ToList();
                foreach (awContent cont in tmp)
                    contentIdsToBeDeleted.Add(cont.contentId);
            }


            if (contentIdsToBeDeleted != null && contentIdsToBeDeleted.Count > 0)
            {

                //DELETE CONTENT FORMS --------------------------------------
                //Delete field settings
                var formFields = from l in _context.awContentFormFieldSettings
                                 where contentIdsToBeDeleted.ToArray().Contains(l.awContentForm.awContent.contentId)
                                 select l;
                if (formFields != null)
                    _context.awContentFormFieldSettings.DeleteAllOnSubmit(formFields);

                //Delete group member relations
                var formRelations = from l in _context.awContentFormGroupMembers
                                    where contentIdsToBeDeleted.ToArray().Contains(l.awContentForm.awContent.contentId)
                                    select l;
                if (formRelations != null)
                    _context.awContentFormGroupMembers.DeleteAllOnSubmit(formRelations);

                //DELETE FORMS 
                var forms = from l in _context.awContentForms
                            where contentIdsToBeDeleted.ToArray().Contains(l.contentId)
                            select l;
                if (forms != null)
                    _context.awContentForms.DeleteAllOnSubmit(forms);


                //DELETE CONTENT LANGUAGES ----------------------------
                var contLang = from cl in _context.awContentCultures
                               where contentIdsToBeDeleted.ToArray().Contains(cl.contentId)
                               select cl;
                _context.awContentCultures.DeleteAllOnSubmit(contLang);



                //DELETE CUSTOM VALUES FOR THE CONTENT and ITS CHILDS
                var customFieldValues = from v in _context.awContentCustomFieldValues
                                        where contentIdsToBeDeleted.ToArray().Contains(v.contentId)
                                        select v;
                _context.awContentCustomFieldValues.DeleteAllOnSubmit(customFieldValues);

                //DELETE CUSTOM FIELDS FOR THE CONTENT and ITS CHILDS
                var customFields = from f in _context.awContentCustomFields
                                   where contentIdsToBeDeleted.ToArray().Contains(f.contentId)
                                   select f;

                _context.awContentCustomFields.DeleteAllOnSubmit(customFields);


                //DELETE TAGS 
                var tags = from t in _context.awSiteTaggedContents
                           where contentIdsToBeDeleted.ToArray().Contains(t.contentId)
                           select t;
                _context.awSiteTaggedContents.DeleteAllOnSubmit(tags);

            }

            //DELETE CONTENTS 
            _context.awContents.DeleteAllOnSubmit(contents);
            _context.SubmitChanges();
        }

        /// <summary>
        /// 
        /// </summary>

        /// <param name="alias"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="contentType"></param>
        /// <param name="siteId"></param>
        /// <param name="userId"></param>
        /// <param name="parentContentId"></param>
        /// <param name="eventStartDate"></param>
        /// <param name="endDate"></param>
        /// <param name="link"></param>
        /// <param name="imageurl"></param>
        /// <param name="sortOrder"></param>
        /// <param name="isEnabled"></param>
        /// <param name="isCommentable"></param>
        /// <param name="pubDate"></param>
        /// <param name="pubEndDate"></param>
        /// <returns></returns>
        public long Add(string alias, string title, string description,
                        string contentType, long siteId, long userId, Int64? parentContentId,
                        DateTime? eventStartDate, DateTime? endDate,
                        string link, string imageurl, int? sortOrder, bool isEnabled, bool isCommentable,
                        DateTime? pubDate, DateTime? pubEndDate)
        {
            long id = AWAPI_Common.library.MiscLibrary.CreateUniqueId();

            return Add(id, alias, title, description, contentType, siteId, userId, parentContentId,
                        eventStartDate, endDate, link, imageurl, sortOrder, isEnabled, isCommentable,
                        pubDate, pubEndDate);

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentId"></param>
        /// <param name="alias"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="contentType"></param>
        /// <param name="siteId"></param>
        /// <param name="userId"></param>
        /// <param name="parentContentId"></param>
        /// <param name="eventStartDate"></param>
        /// <param name="endDate"></param>
        /// <param name="link"></param>
        /// <param name="imageurl"></param>
        /// <param name="sortOrder"></param>
        /// <param name="isEnabled"></param>
        /// <param name="isCommentable"></param>
        /// <param name="pubDate"></param>
        /// <param name="pubEndDate"></param>
        /// <returns></returns>
        public long Add(long contentId, string alias, string title, string description,
                        string contentType, long siteId, long userId, Int64? parentContentId,
                        DateTime? eventStartDate, DateTime? endDate,
                        string link, string imageurl, int? sortOrder, bool isEnabled, bool isCommentable,
                        DateTime? pubDate, DateTime? pubEndDate)
        {
            awContent content = new awContent();

            if (String.IsNullOrEmpty(alias.Trim()))
                throw new Exception("Alias is required.");

            content.contentId = contentId;
            content.alias = alias.ToLower();
            content.title = title;
            content.description = description;

            content.contentType = String.IsNullOrEmpty(contentType) ? AWAPI_Common.values.TypeValues.ContentTypes.content.ToString() : contentType;
            content.userId = userId;
            content.siteId = siteId;
            content.parentContentId = parentContentId;
            content.lineage = CreateContentLineage(parentContentId); ;

            content.eventStartDate = eventStartDate;
            content.eventEndDate = endDate;
            content.link = link;
            content.imageurl = imageurl;
            content.sortOrder = sortOrder == null? 1:sortOrder;
            content.isEnabled = isEnabled;
            content.isCommentable = isCommentable;

            content.lastBuildDate = DateTime.Now;

            if (pubDate == null)
                content.pubDate = DateTime.Now;
            else
                content.pubDate = pubDate;
            content.pubEndDate = pubEndDate;
            content.createDate = DateTime.Now;

            _context.awContents.InsertOnSubmit(content);
            _context.SubmitChanges();

            return contentId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="alias"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="contentType"></param>
        /// <param name="userId"></param>
        /// <param name="parentContentId"></param>
        /// <param name="eventStartDate"></param>
        /// <param name="endDate"></param>
        /// <param name="link"></param>
        /// <param name="imageurl"></param>
        /// <param name="sortOrder"></param>
        /// <param name="isEnabled"></param>
        /// <param name="isCommentable"></param>
        /// <param name="pubDate"></param>
        /// <param name="pubEndDate"></param>
        /// <returns></returns>
        public bool Update(long id, string alias, string title, string description,
                        string contentType, long userId, long? parentContentId,
                        DateTime? eventStartDate, DateTime? endDate,
                        string link, string imageurl, int? sortOrder, bool isEnabled, bool isCommentable,
                        DateTime? pubDate, DateTime? pubEndDate)
        {
            awContent content = _context.awContents.FirstOrDefault(st => st.contentId.Equals(id));
            if (content == null)
                return false;

            bool parentHasBenChanged = parentContentId != content.parentContentId;

            if (String.IsNullOrEmpty(alias))
                throw new Exception("Alias is required.");

            content.alias = alias.ToLower();
            content.title = title;
            content.description = description;

            content.contentType = contentType;
            content.userId = userId;
            content.parentContentId = parentContentId;
            content.lineage = CreateContentLineage(parentContentId);

            content.eventStartDate = eventStartDate;
            content.eventEndDate = endDate;
            content.link = link;
            content.imageurl = imageurl;
            content.sortOrder = sortOrder == null ? 1 : sortOrder;
            content.isEnabled = isEnabled;
            content.isCommentable = isCommentable;

            content.lastBuildDate = DateTime.Now;
            if (pubDate != null)
                content.pubDate = pubDate;
            content.pubEndDate = pubEndDate;

            _context.SubmitChanges();

            //Update child contents' lineage.
            if (parentHasBenChanged)
                UpdateChildsLineage(content.contentId);

            return true;
        }

        /// <summary>
        /// When we change a content's parent through the Update method (above)
        /// we need to update all the child content's lineages...
        /// </summary>
        /// <param name="id"></param>
        private void UpdateChildsLineage(long id)
        {
            string lineage = FormatLineage(id);

            var list = from l in _context.awContents
                       where l.lineage.IndexOf(lineage) >= 0
                       select l;

            if (list == null || list.Count() == 0)
                return;

            foreach (awContent content in list)
            {
                if (content.parentContentId == null)
                    continue;

                awContent parent = Get(content.parentContentId.Value);

                content.lineage = CreateContentLineage(parent.contentId);
            }
            _context.SubmitChanges();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        public bool EnableDisable(long id, bool enable)
        {
            awContent content = _context.awContents.FirstOrDefault(st => st.contentId.Equals(id));

            if (content == null)
                return false;

            content.isEnabled = enable;
            content.lastBuildDate = DateTime.Now;

            _context.SubmitChanges();

            return true;
        }


        #region LANGUAGE
        public bool SaveContentLanguage(long contentId, string cultureCode, string title,
                            string description, string link, string imageUrl, long userId)
        {
            bool newRec = false;
            awContentCulture cl = _context.awContentCultures.
                                    FirstOrDefault(st => st.contentId.Equals(contentId) &&
                                            st.cultureCode.Equals(cultureCode));

            if (cl == null)
            {
                newRec = true;
                cl = new awContentCulture();
                cl.contentCultureId = AWAPI_Common.library.MiscLibrary.CreateUniqueId();
                cl.contentId = contentId;
                cl.cultureCode = cultureCode;
            }

            cl.title = title;
            cl.description = description;
            cl.link = link;
            cl.imageurl = imageUrl;
            cl.userId = userId;

            if (newRec)
                _context.awContentCultures.InsertOnSubmit(cl);

            _context.SubmitChanges();
            return true;
        }


        #endregion

        #region HELPER METHODS

        /// <summary>
        /// creates lineage string 
        /// example: |000000001|000000002|
        /// </summary>
        /// <param name="contentParentId"></param>
        /// <returns></returns>
        public string CreateContentLineage(Int64? contentParentId)
        {
            long? parentId = contentParentId;

            string lineage = "";
            int n = 0;
            while (true || n < 20)
            {
                if (parentId == null || parentId.Value == 0)
                    break;
                awContent content = Get(parentId.Value);

                if (content == null)
                    break;

                lineage = FormatLineage(parentId.Value) + lineage;
                parentId = content.parentContentId;
                n++;
            }
            lineage = lineage.Replace("||", "|");
            return lineage;
        }



        public string FormatLineage(Int64? contentParentId)
        {
            if (contentParentId == null || contentParentId <= 0)
                return "";

            return String.Format("|{0:00000000000000000000}|", contentParentId);
        }
        #endregion
    }
}
