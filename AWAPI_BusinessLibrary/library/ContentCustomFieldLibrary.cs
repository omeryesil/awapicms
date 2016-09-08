using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using AWAPI_Data.Data;
using AWAPI_Common.library;

namespace AWAPI_BusinessLibrary.library
{
    public class ContentCustomFieldLibrary
    {
        ContentContextDataContext _context = new ContentContextDataContext();

        #region CUSTOM FIELD METHODS
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldId"></param>
        /// <returns></returns>
        public AWAPI_Data.Data.awContentCustomField GetField(long fieldId)
        {
            var fields = from f in _context.awContentCustomFields
                         where f.customFieldId.Equals(fieldId)
                         select f;

            return fields.FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteTagId"></param>
        /// <returns></returns>
        public System.Collections.Generic.IList<AWAPI_Data.CustomEntities.ContentCustomField> GetFieldList(long contentId, bool onlyEnabled)
        {
            var fields = from f in _context.awContentCustomFields
                         from c in _context.awContents
                         where (
                                    (f.applyToSubContents == true && c.parentContentId.Equals(f.contentId)) ||
                                    f.contentId.Equals(contentId)
                                )
                                && c.contentId.Equals(contentId) 
                                && (!onlyEnabled || onlyEnabled && f.isEnabled)
                         orderby f.sortOrder
                         select new AWAPI_Data.CustomEntities.ContentCustomField
                         {
                             fieldContentId = f.awContent.contentId,
                             fieldContentTitle = f.awContent.title,
                             valueContentId = c.contentId,
                             valueTitle = c.title,
                             customFieldId = f.customFieldId,
                             title = f.title,
                             description = f.description,
                             fieldType = f.fieldType,
                             applyToSubContents = f.applyToSubContents,
                             maximumLength = f.maximumLength,
                             minimumValue = f.minimumValue,
                             maximumValue = f.maximumValue,
                             defaultValue = f.defaultValue,
                             regularExpression = f.regularExpression,
                             sortOrder = f.sortOrder,
                             isEnabled = f.isEnabled,
                             createDate = f.createDate
                         };

            if (fields == null || fields.Count() == 0)
                return null;

            //IList <AWAPI_Data.CustomEntities.ContentCustomField> list = fields.ToList();
            //if (String.IsNullOrEmpty(defaultLanguage) ||
            //     String.IsNullOrEmpty(cultureCode) ||
            //     defaultLanguage.ToLower() == cultureCode.ToLower())
            //    return list;

            return fields.ToList();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="forceToAdd"></param>
        /// <param name="sourceFld"></param>
        /// <returns></returns>
        public long SaveField(AWAPI_Data.Data.awContentCustomField sourceFld)
        {
            awContentCustomField fld = null;
            bool addNew = false;
            if (sourceFld == null)
                return 0;

            if (sourceFld.customFieldId > 0)
                fld = GetField(sourceFld.customFieldId);

            if (fld == null)
            {
                fld = new awContentCustomField();
                if (sourceFld.contentId <= 0 || sourceFld.userId <= 0)
                    throw new Exception("Content Id and User Id must be set in order the create a new custom field");

                addNew = true;
                fld.customFieldId = AWAPI_Common.library.MiscLibrary.CreateUniqueId();
                fld.contentId = sourceFld.contentId;
                fld.createDate = DateTime.Now;

            }

            fld.title = sourceFld.title;
            fld.description = sourceFld.description;
            fld.applyToSubContents = sourceFld.applyToSubContents;
            fld.fieldType = sourceFld.fieldType;
            fld.maximumLength = sourceFld.maximumLength;
            fld.maximumValue = sourceFld.maximumValue;
            fld.minimumValue = sourceFld.minimumValue;
            fld.defaultValue = sourceFld.defaultValue;
            fld.regularExpression = sourceFld.regularExpression;
            fld.sortOrder = sourceFld.sortOrder;
            fld.isEnabled = sourceFld.isEnabled;
            fld.userId = sourceFld.userId;
            fld.lastBuildDate = DateTime.Now;

            if (addNew)
                _context.awContentCustomFields.InsertOnSubmit(fld);
            _context.SubmitChanges();

            if (addNew && !fld.isEnabled)
                new ContentFormLibrary().RemoveCustomField(fld.customFieldId);

            return fld.customFieldId;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="customFieldId"></param>
        public void DeleteField(Int64 customFieldId)
        {
            //first Delete the values
            var vls = from v in _context.awContentCustomFieldValues
                      where v.customFieldId.Equals(customFieldId)
                      select v;
            _context.awContentCustomFieldValues.DeleteAllOnSubmit(vls);


            //Delete custom form field relation
            var frmFields = from l in _context.awContentFormFieldSettings
                           where l.isContentCustomField && l.contentCustomFieldId.Equals(customFieldId)
                           select l;
            _context.awContentFormFieldSettings.DeleteAllOnSubmit(frmFields);

            //Delete the field
            var fld = from f in _context.awContentCustomFields
                      where f.customFieldId.Equals(customFieldId)
                      select f;
            _context.awContentCustomFields.DeleteAllOnSubmit(fld);

            _context.SubmitChanges();

            //new ContentFormLibrary().RemoveCustomField(customFieldId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteTagId"></param>
        public void DeleteFieldsByContentId(long contentId)
        {
            ContentLibrary contLib = new ContentLibrary();

            //DELETE VALUES FOR THE CONTENT and ITS CHILDS
            var values = from v in _context.awContentCustomFieldValues
                         join f in _context.awContentCustomFields on v.customFieldId equals f.customFieldId
                         where f.contentId.Equals(contentId)
                         select v;

            //DELETE FIELDS FOR THE CONTENT and ITS CHILDS
            var fields = from f in _context.awContentCustomFields
                         where f.contentId.Equals(contentId)
                         select f;

            _context.awContentCustomFieldValues.DeleteAllOnSubmit(values);
            _context.awContentCustomFields.DeleteAllOnSubmit(fields);
            _context.SubmitChanges();
        }
        #endregion FIELDS

        #region VALUES

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueId"></param>
        /// <returns></returns>
        public awContentCustomFieldValue GetFieldValue(Int64 valueId)
        {
            var fields = from v in _context.awContentCustomFieldValues
                         where v.customFieldValueId.Equals(valueId)
                         select v;

            if (fields == null || fields.Count() == 0)
                return null;
            return fields.First();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentId"></param>
        /// <param name="fieldId"></param>
        /// <param name="cultureCode"></param>
        /// <returns></returns>
        public awContentCustomFieldValue GetFieldValue(Int64 contentId, Int64 fieldId, string cultureCode)
        {
            var list = from l in _context.awContentCustomFieldValues
                       where l.contentId.Equals(contentId) &&
                            l.customFieldId.Equals(fieldId) &&
                            (String.IsNullOrEmpty(cultureCode) ||
                             !String.IsNullOrEmpty(cultureCode) && l.cultureCode.Equals(cultureCode))
                       select l;

            if (list == null)
                return null;

            return list.FirstOrDefault();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentId"></param>
        /// <param name="cultureCode"></param>
        /// <returns></returns>
        public IList<AWAPI_Data.CustomEntities.ContentCustomFieldValueExtended> GetFieldValueList(long contentId, string cultureCode)
        {
            var fieldsValues = from f in _context.awContentCustomFields
                               from c in _context.awContents
                               from v in _context.awContentCustomFieldValues
                               where (
                                      (f.applyToSubContents == true && c.parentContentId.Equals(f.contentId)) || f.contentId.Equals(contentId))
                                      && c.contentId.Equals(contentId)
                                      && v.customFieldId.Equals(f.customFieldId)
                                      && v.contentId.Equals(contentId)
                                      && (String.IsNullOrEmpty(cultureCode) ||
                                          !String.IsNullOrEmpty(cultureCode) && v.cultureCode.Equals(cultureCode))
                               orderby f.sortOrder
                               select new AWAPI_Data.CustomEntities.ContentCustomFieldValueExtended
                               {
                                   contentId = v.contentId,
                                   createDate = v.createDate,
                                   cultureCode = v.cultureCode,
                                   customFieldId = v.customFieldId,
                                   title = v.awContentCustomField.title,
                                   customFieldValueId = v.customFieldValueId,
                                   fieldValue = v.fieldValue,
                                   lastBuildDate = v.lastBuildDate,
                                   userId = v.userId
                               };

            if (fieldsValues == null)
                return null;

            return fieldsValues.ToList(); ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteTagId"></param>
        /// <param name="fieldId"></param>
        /// <param name="userId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public long UpdateFieldValue(long contentId, long fieldId, long userId, string value, string cultureCode)
        {
            var flds = from f in _context.awContentCustomFieldValues
                       where f.customFieldId.Equals(fieldId) &&
                             f.contentId.Equals(contentId)
                       select f;

            awContentCustomFieldValue fld = _context.awContentCustomFieldValues.
                                FirstOrDefault(v => v.customFieldId.Equals(fieldId) &&
                                                    v.contentId.Equals(contentId) &&
                                                    v.cultureCode.Equals(cultureCode));

            //if doesn't exist then Add a new one, else udpate the current one
            if (fld == null)
            {
                fld = new awContentCustomFieldValue();
                fld.customFieldValueId = MiscLibrary.CreateUniqueId();
                fld.cultureCode = cultureCode;
                fld.customFieldId = fieldId;
                fld.contentId = contentId;
                fld.fieldValue = value;
                fld.userId = userId;
                fld.lastBuildDate = DateTime.Now;
                fld.createDate = DateTime.Now;

                _context.awContentCustomFieldValues.InsertOnSubmit(fld);
            }
            else
            {
                fld.fieldValue = value;
                fld.userId = userId;
                fld.lastBuildDate = DateTime.Now;
            }

            _context.SubmitChanges();

            return fld.customFieldValueId;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueId"></param>
        public void DeleteFieldValue(int valueId)
        {
            //Delete the field
            var vls = from v in _context.awContentCustomFieldValues
                      where v.customFieldValueId.Equals(valueId)
                      select v;
            _context.awContentCustomFieldValues.DeleteAllOnSubmit(vls);

            _context.SubmitChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldId"></param>
        public void DeleteFieldValues(Int64 fieldId)
        {
            //Delete the field
            var vls = from v in _context.awContentCustomFieldValues
                      where v.customFieldId.Equals(fieldId)
                      select v;
            _context.awContentCustomFieldValues.DeleteAllOnSubmit(vls);

            _context.SubmitChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteTagId"></param>
        public void DeleteFieldValuesByContentId(long contentId)
        {

            //first Delete the values
            var values = from f in _context.awContentCustomFields
                         from c in _context.awContents
                         from v in _context.awContentCustomFieldValues
                         where (
                                (f.applyToSubContents == true && c.parentContentId.Equals(f.contentId)) ||
                                f.contentId.Equals(contentId)
                                    )
                                && c.contentId.Equals(contentId)
                                && v.customFieldId.Equals(f.customFieldId)
                                && v.contentId.Equals(contentId)
                         orderby f.sortOrder
                         select v;


            _context.awContentCustomFieldValues.DeleteAllOnSubmit(values);
            _context.SubmitChanges();
        }

        #endregion

    }
}
