using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using AWAPI_Data.Data;
using AWAPI_Common.library;

namespace AWAPI_BusinessLibrary.library
{
    public class ContentFormLibrary
    {
        ContentContextDataContext _context = new ContentContextDataContext();

        #region STATIC FIELD DEFINITIONS
        public enum StaticFieldsName
        {
            Alias,
            Title,
            Description,
            IsEnabled,
            IsCommentable,
            SortOrder,
            Link,
            Imageurl,
            PubDate,
            PubEndDate,
            EventStartDate,
            EventEndDate
        }
        #endregion

        #region CONTENT FORMS
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentFormId"></param>
        /// <param name="onlyEnabled"></param>
        /// <returns></returns>
        public AWAPI_Data.Data.awContentForm Get(long contentFormId, bool onlyEnabled)
        {
            DateTime now = DateTime.Now;
            var forms = from l in _context.awContentForms
                        where l.contentFormId.Equals(contentFormId) &&
                        (!onlyEnabled ||
                          (
                            onlyEnabled &&
                            l.awSite_.isEnabled &&
                            l.awContent.isEnabled &&
                            (l.awContent.pubDate.Equals(null) || (l.awContent.pubDate != null && l.awContent.pubDate <= now)) &&
                            (l.awContent.pubEndDate.Equals(null) || (l.awContent.pubEndDate != null && l.awContent.pubEndDate > now))
                            )
                         )
                        select l;

            return forms.FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentId"></param>
        /// <param name="onlyEnabled"></param>
        /// <returns></returns>
        public AWAPI_Data.Data.awContentForm GetByContentId(long contentId, bool onlyEnabled)
        {
            DateTime now = DateTime.Now;
            var forms = from l in _context.awContentForms
                        where l.contentId.Equals(contentId) &&
                        (!onlyEnabled ||
                          (
                            onlyEnabled &&
                            l.awSite_.isEnabled &&
                            l.awContent.isEnabled &&
                            (l.awContent.pubDate.Equals(null) || (l.awContent.pubDate != null && l.awContent.pubDate <= now)) &&
                            (l.awContent.pubEndDate.Equals(null) || (l.awContent.pubEndDate != null && l.awContent.pubEndDate > now))
                            )
                         )
                        select l;

            return forms.FirstOrDefault();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="onlyEnabled"></param>
        /// <returns></returns>
        public System.Collections.Generic.IList<AWAPI_Data.Data.awContentForm> GetList(long siteId, bool onlyEnabled)
        {
            var forms = from l in _context.awContentForms
                        where l.siteId.Equals(siteId) &&
                           (!onlyEnabled ||
                           (
                                 onlyEnabled &&
                                 l.awSite_.isEnabled &&
                                 l.isEnabled
                             )
                          )
                        orderby l.title
                        select l;

            if (forms == null || forms.Count() == 0)
                return null;

            return forms.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="contentId"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="isEnabled"></param>
        /// <param name="applyToSub"></param>
        /// <param name="canCreateNew"></param>
        /// <param name="canUpdate"></param>
        /// <param name="canDelete"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public long Add(long siteId, long contentId, string title, string description,
                        bool isEnabled, bool applyToSub, bool canCreateNew, bool canUpdate, bool canDelete, long userId)
        {
            awContentForm form = new awContentForm();

            form.contentFormId = AWAPI_Common.library.MiscLibrary.CreateUniqueId();
            form.siteId = siteId;
            form.contentId = contentId;
            form.title = title;
            form.description = description;
            form.isEnabled = isEnabled;
            form.applyToSub = applyToSub;
            form.canCreateNew = canCreateNew;
            form.canUpdate = canUpdate;
            form.canDelete = canDelete;
            form.userId = userId;
            form.lastBuildDate = DateTime.Now;
            form.createDate = DateTime.Now;

            _context.awContentForms.InsertOnSubmit(form);

            //adds static fields (fields from the content table) and custom content fields
            AddFieldsToForm(contentId, form.contentFormId, false);

            _context.SubmitChanges();


            return form.contentFormId;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentFormId"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="isEnabled"></param>
        /// <param name="applyToSub"></param>
        /// <param name="canCreateNew"></param>
        /// <param name="canUpdate"></param>
        /// <param name="canDelete"></param>
        /// <param name="userId"></param>
        public void Update(long contentFormId, string title, string description,
                bool isEnabled, bool applyToSub, bool canCreateNew, bool canUpdate, bool canDelete, long userId)
        {
            awContentForm form = (from l in _context.awContentForms
                                  where l.contentFormId.Equals(contentFormId)
                                  select l).FirstOrDefault<awContentForm>();
            if (form == null)
                return;

            form.title = title;
            form.description = description;
            form.isEnabled = isEnabled;
            form.applyToSub = applyToSub;
            form.canCreateNew = canCreateNew;
            form.canUpdate = canUpdate;
            form.canDelete = canDelete;
            form.userId = userId;
            form.lastBuildDate = DateTime.Now;
            _context.SubmitChanges();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentFormId"></param>
        public void Delete(Int64 contentFormId)
        {
            //Delete field settings
            var fields = from l in _context.awContentFormFieldSettings
                         where l.contentFormId.Equals(contentFormId)
                         select l;
            if (fields != null)
                _context.awContentFormFieldSettings.DeleteAllOnSubmit(fields);

            //Delete group member relations
            var relations = from l in _context.awContentFormGroupMembers
                            where l.contentFormId.Equals(contentFormId)
                            select l;
            if (relations != null)
                _context.awContentFormGroupMembers.DeleteAllOnSubmit(relations);

            //Delete the form
            var forms = from l in _context.awContentForms
                        where l.contentFormId.Equals(contentFormId)
                        select l;
            _context.awContentForms.DeleteAllOnSubmit(forms);

            _context.SubmitChanges();
        }


        #endregion

        #region FIELD SETTINGS
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fs"></param>
        /// <returns></returns>
        public long AddFieldSetting(AWAPI_Data.Data.awContentFormFieldSetting fs)
        {
            if (fs.contentFormId <= 0)
                throw new Exception("ContentFormId is required");
            if (fs.staticFieldName.Trim() == "" &&
                (fs.contentCustomFieldId == null || fs.contentCustomFieldId <= 0))
                throw new Exception("staticFieldName or contentCustomFieldId must be entered");

            //do not add if already exist
            var exist = from l in _context.awContentFormFieldSettings
                        where
                            l.contentFormId.Equals(fs.contentFormId) &&
                            (
                               (fs.isContentCustomField && l.contentCustomFieldId.Equals(fs.contentCustomFieldId)) ||
                               (!fs.isContentCustomField && l.staticFieldName.ToLower().Equals(fs.staticFieldName.ToLower()))
                            )
                        select l;
            if (exist != null && exist.Count() > 0)
                return exist.ToList()[0].contentFormFieldSettingId;

            fs.contentFormFieldSettingId = AWAPI_Common.library.MiscLibrary.CreateUniqueId();

            _context.awContentFormFieldSettings.InsertOnSubmit(fs);
            return fs.contentFormId;
        }

        /// <summary>
        /// if not exists add static and custom content fields to the form,,,
        /// NOTE: this method DOES NOT call SUBMITCHANGES
        /// </summary>
        /// <param name="contentId"></param>
        /// <param name="contentFormId"></param>
        public void AddFieldsToForm(long contentId, long contentFormId, bool submitChanges)
        {
            //Cleanup if exists
            CleanupCustomFields();

            //Add static fields first
            int sortOrder = 1;
            foreach (string field in Enum.GetNames(typeof(StaticFieldsName)))
            {
                awContentFormFieldSetting set = new awContentFormFieldSetting();
                set.contentFormFieldSettingId = AWAPI_Common.library.MiscLibrary.CreateUniqueId();
                set.contentFormId = contentFormId;
                set.staticFieldName = field;
                set.isContentCustomField = false;
                set.isEnabled = false;
                set.isVisible = false;
                set.sortOrder = sortOrder;

                AddFieldSetting(set);

                sortOrder++;
            }

            //add custom fields
            awContent content = new ContentLibrary().Get(contentId);

            System.Collections.Generic.IEnumerable<AWAPI_Data.CustomEntities.ContentCustomField> cstFlds = new ContentCustomFieldLibrary().GetFieldList(contentId, true);
            if (cstFlds != null && cstFlds.Count() > 0)
            {
                var customFields = from r in cstFlds
                                   where r.isEnabled &&
                                         (r.applyToSubContents == false && r.fieldContentId.Equals(contentId) ||
                                         r.applyToSubContents && r.fieldContentId.Equals(content.parentContentId))
                                   orderby r.sortOrder
                                   select r;
                if (customFields != null && customFields.Count() > 0)
                {
                    cstFlds = customFields.ToList();
                    foreach (AWAPI_Data.CustomEntities.ContentCustomField cf in cstFlds)
                    {
                        awContentFormFieldSetting set = new awContentFormFieldSetting();
                        set.contentFormFieldSettingId = AWAPI_Common.library.MiscLibrary.CreateUniqueId();
                        set.contentFormId = contentFormId;
                        set.contentCustomFieldId = cf.customFieldId;
                        set.staticFieldName = "";
                        set.isContentCustomField = true;
                        set.isEnabled = false;
                        set.isVisible = false;
                        set.sortOrder = sortOrder;

                        AddFieldSetting(set);

                        sortOrder = sortOrder + 10;
                    }
                }
            }

            if (submitChanges)
                _context.SubmitChanges();
        }

        /// <summary>
        /// Deletes disabled or non existing custom fields...
        /// </summary>
        public void CleanupCustomFields()
        {
            var list = from l in _context.awContentFormFieldSettings
                       where l.contentCustomFieldId != null &&
                            (l.awContentCustomField == null || !l.awContentCustomField.isEnabled)
                       select l;
            if (list == null && list.Count() == 0)
                return;

            _context.awContentFormFieldSettings.DeleteAllOnSubmit(list);
            _context.SubmitChanges();
        }

        /// <summary>
        ///
        /// </summary>
        public void RemoveCustomField(long contentCustomFieldId)
        {
            var list = from l in _context.awContentFormFieldSettings
                       where l.contentCustomFieldId.Equals(contentCustomFieldId)
                       select l;
            if (list == null && list.Count() == 0)
                return;

            _context.awContentFormFieldSettings.DeleteAllOnSubmit(list);
            _context.SubmitChanges();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentFormId"></param>
        /// <returns></returns>
        public IList<AWAPI_Data.CustomEntities.ContentFormFieldSettingExtended> GetFieldList(long contentFormId)
        {
            var list = from l in _context.awContentFormFieldSettings
                       where l.contentFormId.Equals(contentFormId)
                       orderby l.sortOrder
                       select new AWAPI_Data.CustomEntities.ContentFormFieldSettingExtended
                       {
                           contentFormFieldSettingId = l.contentFormFieldSettingId,
                           contentFormId = l.contentFormId,
                           title = l.isContentCustomField != true ? l.staticFieldName : l.awContentCustomField.title,
                           description = l.isContentCustomField != true ? l.staticFieldName : l.awContentCustomField.description,
                           fieldType = l.isContentCustomField != true ? GetFieldType(l.staticFieldName) : l.awContentCustomField.fieldType,
                           contentCustomFieldId = l.contentCustomFieldId,
                           isContentCustomField = l.isContentCustomField,
                           isEnabled = l.isEnabled,
                           isVisible = l.isVisible,
                           sortOrder = l.sortOrder,
                           staticFieldName = l.staticFieldName,
                           maximumLength = GetFieldMaximumLength(30, l.staticFieldName, l.isContentCustomField, l.isContentCustomField != true ? null : l.awContentCustomField.maximumLength)
                       };

            if (list == null)
                return null;

            return list.ToList();
        }

        public int? GetFieldMaximumLength(int? defaultValue, string staticFieldName, bool isCustomField, int? customFieldLength)
        {
            int? len = defaultValue;

            if (String.IsNullOrEmpty(staticFieldName) && !isCustomField)
                return len;

            if (isCustomField)
                return customFieldLength;

            switch (staticFieldName.ToLower().Trim())
            {
                case "alias": len = 50; break;
                case "title": len = 70; break;
                case "description": len = 250; break;
                case "sortorder": len = 10; break;
                case "link": len = 100; break;
                case "imageurl": len = 100; break;
                case "pubdate": len = 20; break;
                case "pubenddate": len = 20; break;
                case "eventstartdate": len = 20; break;
                case "eventenddate": len = 20; break;
            }

            return len;
        }

        /// <summary>
        /// Returns type of the field (textbox, checkbox, radiobox, etc...)
        /// </summary>
        /// <param name="staticFieldName"></param>
        /// <returns></returns>
        public string GetFieldType(string staticFieldName)
        {
            string rtn = "textbox";

            if (!String.IsNullOrEmpty(staticFieldName))
            {
                string fieldName = staticFieldName.ToLower().Trim();

                if (fieldName == "iscommentable" ||
                    fieldName == "isenabled")
                    return "checkbox";

                if (fieldName == "pubdate" ||
                    fieldName == "pubenddate" ||
                    fieldName == "eventstartdate" ||
                    fieldName == "eventenddate")
                    return "datetime";
            }

            return rtn;
        }


        public void UpdateFieldEnableStatus(long contentFormFieldSettingId, bool isEnabled)
        {
            awContentFormFieldSetting set = (from l in _context.awContentFormFieldSettings
                                             where l.contentFormFieldSettingId.Equals(contentFormFieldSettingId)
                                             select l).FirstOrDefault();

            if (set == null)
                return;

            set.isEnabled = isEnabled;
            _context.SubmitChanges();
        }

        public void UpdateFieldVisibleStatus(long contentFormFieldSettingId, bool isVisible)
        {
            awContentFormFieldSetting set = (from l in _context.awContentFormFieldSettings
                                             where l.contentFormFieldSettingId.Equals(contentFormFieldSettingId)
                                             select l).FirstOrDefault();

            if (set == null)
                return;

            set.isVisible = isVisible;
            _context.SubmitChanges();
        }

        public void MoveFieldUp(long contentFormId, long contentFormFieldSettingId)
        {
            var list = from l in _context.awContentFormFieldSettings
                       where l.contentFormId.Equals(contentFormId)
                       orderby l.sortOrder
                       select l;

            if (list == null || list.Count() == 0)
                return;

            IList<awContentFormFieldSetting> list2 = list.ToList();
            int index = -1;
            for (int n = 0; n < list2.Count; n++)
                if (list2[n].contentFormFieldSettingId == contentFormFieldSettingId)
                {
                    index = n;
                    break;
                }

            //not found or item is already the first one
            if (index <= -1)
                return;

            long prevItemId = list2[index - 1].contentFormFieldSettingId;

            awContentFormFieldSetting set = (from l in _context.awContentFormFieldSettings
                                             where l.contentFormFieldSettingId.Equals(contentFormFieldSettingId)
                                             select l).First();

            awContentFormFieldSetting prevSet = (from l in _context.awContentFormFieldSettings
                                                 where l.contentFormFieldSettingId.Equals(prevItemId)
                                                 select l).First();

            int currentSortOrder = set.sortOrder;

            set.sortOrder = prevSet.sortOrder;
            prevSet.sortOrder = currentSortOrder;

            _context.SubmitChanges();
        }

        public void MoveFieldDown(long contentFormId, long contentFormFieldSettingId)
        {
            var list = from l in _context.awContentFormFieldSettings
                       where l.contentFormId.Equals(contentFormId)
                       orderby l.sortOrder
                       select l;

            if (list == null || list.Count() == 0)
                return;

            IList<awContentFormFieldSetting> list2 = list.ToList();
            int index = -1;
            for (int n = 0; n < list2.Count; n++)
                if (list2[n].contentFormFieldSettingId == contentFormFieldSettingId)
                {
                    index = n;
                    break;
                }

            //not found or item is already the first one
            if (index == -1 || index >= list2.Count - 1)
                return;

            long nextItemId = list2[index + 1].contentFormFieldSettingId;

            awContentFormFieldSetting set = (from l in _context.awContentFormFieldSettings
                                             where l.contentFormFieldSettingId.Equals(contentFormFieldSettingId)
                                             select l).First();

            awContentFormFieldSetting nextSet = (from l in _context.awContentFormFieldSettings
                                                 where l.contentFormFieldSettingId.Equals(nextItemId)
                                                 select l).First();

            int currentSortOrder = set.sortOrder;

            set.sortOrder = nextSet.sortOrder;
            nextSet.sortOrder = currentSortOrder;

            _context.SubmitChanges();
        }


        #endregion


    }
}
