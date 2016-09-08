using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;
using AWAPI_Data.Data;
using AWAPI_Data.CustomEntities;

namespace AWAPI_Services.wcf
{
    // NOTE: If you change the interface name "IContentService" here, you must also update the reference to "IContentService" in Web.config.
    [ServiceContract]
    public interface IContentService
    {
        #region CONTENT
        [OperationContract]
        AWAPI_Data.CustomEntities.ContentExtended Get(long contentId);

        [OperationContract]
        System.Collections.Generic.IList<AWAPI_Data.CustomEntities.ContentExtended> GetList(long siteId, string where, string sortField);

        [OperationContract]
        void Delete(long contentId);

        [OperationContract]
        long Add(string alias, string title, string description,
                string contentType, long siteId, long userId, long parentContentId,
                DateTime? eventStartDate, DateTime? endDate,
                string link, string imageurl, int sortOrder, bool isEnabled, bool isCommentable,
                DateTime? pubDate, DateTime? pubEndDate);

        [OperationContract]
        bool Update(long id, string alias, string title, string description,
                        string contentType, long userId, long parentContentId,
                        DateTime? eventStartDate, DateTime? endDate,
                        string link, string imageurl, int sortOrder, bool isEnabled, bool isCommentable,
                        DateTime? pubDate, DateTime? pubEndDate);
        #endregion

        #region CUSTOM FIELD METHODS

        [OperationContract]
        AWAPI_Data.Data.awContentCustomField GetField(long fieldId);

        [OperationContract]
        System.Collections.Generic.IList<AWAPI_Data.CustomEntities.ContentCustomField> GetFieldList(long contentId);

        [OperationContract]
        long SaveField(awContentCustomField fld);

        [OperationContract]
        void DeleteField(int fieldId);

        [OperationContract]
        void DeleteFieldsByContentId(long contentId);

        #endregion FIELDS

        #region CUSTOM FIELD VALUES

        [OperationContract]
        AWAPI_Data.Data.awContentCustomFieldValue GetFieldValue(int valueId);

        [OperationContract]
        awContentCustomFieldValue GetFieldValueByContentId(long contentId, int fieldId, string cultureCode);

        [OperationContract]
        System.Collections.Generic.IList<AWAPI_Data.CustomEntities.ContentCustomFieldValueExtended> GetFieldValueList(long contentId, string cultureCode);

        [OperationContract]
        long UpdateFieldValue(long contentId, int fieldId, long userId, string value, string cultureCode);

        [OperationContract]
        void DeleteFieldValue(int valueId);

        [OperationContract]
        void DeleteFieldValues(int fieldId);

        [OperationContract]
        void DeleteFieldValuesByContentId(long contentId);

        #endregion
    }
}
