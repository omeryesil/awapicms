using System;
using System.ServiceModel.Activation;
using AWAPI_BusinessLibrary.library;
using System.Linq;
using AWAPI_Data.Data;
using System.Collections.Generic;

namespace AWAPI_Services.wcf
{
    // NOTE: If you change the class name "ContentService" here, you must also update the reference to "ContentService" in Web.config.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class ContentService : IContentService
    {
        ContentLibrary _contentLibrary = new ContentLibrary();
        ContentCustomFieldLibrary _contentCustomField = new ContentCustomFieldLibrary();

        #region CONTENT
        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteTagId"></param>
        /// <returns></returns>
        public AWAPI_Data.CustomEntities.ContentExtended Get(long contentId)
        {
            awContent tmp = _contentLibrary.Get(contentId);
            if (tmp == null)
                return null;

            AWAPI_Data.CustomEntities.ContentExtended cont = new AWAPI_Data.CustomEntities.ContentExtended();

            cont.contentId = tmp.contentId;
            cont.alias = tmp.alias;
            cont.title = tmp.title;
            cont.description = tmp.description;
            cont.lineage = tmp.lineage;
            cont.sortOrder = tmp.sortOrder;
            cont.pubDate = tmp.pubDate;
            cont.pubEndDate = tmp.pubEndDate;
            cont.contentType = tmp.contentType;
            cont.eventStartDate = tmp.eventStartDate;
            cont.eventEndDate = tmp.eventEndDate;
            cont.imageurl = tmp.imageurl;
            cont.isCommentable = tmp.isCommentable;
            cont.isEnabled = tmp.isEnabled;
            cont.link = tmp.link;
            cont.parentContentId = tmp.parentContentId;
            cont.lastBuildDate = tmp.lastBuildDate;
            cont.createDate = tmp.createDate;
            cont.siteId = tmp.siteId;
            cont.userId = tmp.userId;

            return cont;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="where"></param>
        /// <param name="sortField"></param>
        /// <returns></returns>
        public IList<AWAPI_Data.CustomEntities.ContentExtended> GetList(long siteId, string where, string sortField)
        {
            System.Collections.Generic.IList<awContent> list =
                _contentLibrary.GetList(siteId, where, sortField);

            if (list == null || list.Count == 0)
                return null;

            var tmp = from l in list
                      select new AWAPI_Data.CustomEntities.ContentExtended
                      {
                          contentId = l.contentId,
                          alias = l.alias,
                          title = l.title,
                          description = l.description,
                          lineage = l.lineage,
                          sortOrder = l.sortOrder,
                          pubDate = l.pubDate,
                          pubEndDate = l.pubEndDate,
                          contentType = l.contentType,
                          eventStartDate = l.eventStartDate,
                          eventEndDate = l.eventEndDate,
                          imageurl = l.imageurl,
                          isCommentable = l.isCommentable,
                          isEnabled = l.isEnabled,
                          link = l.link,
                          parentContentId = l.parentContentId,
                          lastBuildDate = l.lastBuildDate,
                          createDate = l.createDate,
                          siteId = l.siteId,
                          userId = l.userId
                      };

            return tmp.ToList<AWAPI_Data.CustomEntities.ContentExtended>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteTagId"></param>
        public void Delete(long contentId)
        {
            _contentLibrary.Delete(contentId);
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
                        string contentType, long siteId, long userId, long parentContentId,
                        DateTime? eventStartDate, DateTime? endDate,
                        string link, string imageurl, int sortOrder, bool isEnabled, bool isCommentable,
                        DateTime? pubDate, DateTime? pubEndDate)
        {
            return _contentLibrary.Add(alias, title, description, contentType, siteId, userId, parentContentId,
                        eventStartDate, endDate, link, imageurl, sortOrder, isEnabled, isCommentable,
                        pubDate, pubEndDate);
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
                        string contentType, long userId, long parentContentId,
                        DateTime? eventStartDate, DateTime? endDate,
                        string link, string imageurl, int sortOrder, bool isEnabled, bool isCommentable,
                        DateTime? pubDate, DateTime? pubEndDate)
        {
            return _contentLibrary.Update(id, alias, title, description, contentType, userId, parentContentId,
                        eventStartDate, endDate, link, imageurl, sortOrder, isEnabled, isCommentable,
                        pubDate, pubEndDate);
        }
        #endregion

        #region CUSTOM FIELD METHODS
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldId"></param>
        /// <returns></returns>
        public AWAPI_Data.Data.awContentCustomField GetField(long fieldId)
        {
            return _contentCustomField.GetField(fieldId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteTagId"></param>
        /// <returns></returns>
        public System.Collections.Generic.IList<AWAPI_Data.CustomEntities.ContentCustomField> GetFieldList(long contentId)
        {
            return _contentCustomField.GetFieldList(contentId, true);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="fld"></param>
        /// <returns></returns>
        public long SaveField(awContentCustomField fld)
        {
            return _contentCustomField.SaveField(fld);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldId"></param>
        public void DeleteField(int fieldId)
        {
            _contentCustomField.DeleteField(fieldId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteTagId"></param>
        public void DeleteFieldsByContentId(long contentId)
        {
            _contentCustomField.DeleteFieldsByContentId(contentId);

        }
        #endregion FIELDS

        #region CUSTOM FIELD VALUES

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueId"></param>
        /// <returns></returns>
        public awContentCustomFieldValue GetFieldValue(int valueId)
        {
            return _contentCustomField.GetFieldValue(valueId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueId"></param>
        /// <returns></returns>
        public awContentCustomFieldValue GetFieldValueByContentId(long contentId, int fieldId, string cultureCode)
        {
            return _contentCustomField.GetFieldValue(contentId, fieldId, cultureCode);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteTagId"></param>
        /// <returns></returns>
        public System.Collections.Generic.IList<AWAPI_Data.CustomEntities.ContentCustomFieldValueExtended> GetFieldValueList(long contentId, string cultureCode)
        {
            return _contentCustomField.GetFieldValueList(contentId, cultureCode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteTagId"></param>
        /// <param name="fieldId"></param>
        /// <param name="userId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public long UpdateFieldValue(long contentId, int fieldId, long userId, string value, string cultureCode)
        {
            return _contentCustomField.UpdateFieldValue(contentId, fieldId, userId, value, cultureCode);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueId"></param>
        public void DeleteFieldValue(int valueId)
        {
            _contentCustomField.DeleteFieldValue(valueId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldId"></param>
        public void DeleteFieldValues(int fieldId)
        {
            _contentCustomField.DeleteFieldValues(fieldId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteTagId"></param>
        public void DeleteFieldValuesByContentId(long contentId)
        {
            _contentCustomField.DeleteFieldValuesByContentId(contentId);
        }

        #endregion

    }
}
