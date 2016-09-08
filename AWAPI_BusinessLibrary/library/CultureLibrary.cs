using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AWAPI_Data.Data;


namespace AWAPI_BusinessLibrary.library
{
    /// <summary>
    /// . Does not contain culture functions for the Contents module. 
    ///     Contents module requires performance, it is its' own language tables..
    /// . There are two main tables for the culture.
    ///     awCulture
    ///     awCultureValue
    /// </summary>
    public class CultureLibrary
    {
        AWAPI_Data.Data.SiteContextDataContext _context = new SiteContextDataContext();

        #region CULTURE
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public System.Collections.Generic.IEnumerable<awCulture> GetList()
        {
            var list = from lan in _context.awCultures
                       orderby lan.title
                       select lan;
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public System.Collections.Generic.IEnumerable<awCulture> GetListBySiteId(Int64 siteId)
        {
            var list = from lan in _context.awSiteCultures
                       where lan.siteId.Equals(siteId)
                       orderby lan.awCulture.title
                       select lan.awCulture;
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="cultureCode"></param>
        public void AddToSite(long siteId, string cultureCode)
        {
            //check if already exist
            var list = from cult in _context.awSiteCultures
                       where cult.siteId.Equals(siteId) &&
                            cult.cultureCode.ToLower().Equals(cultureCode.ToLower())
                       select cult;
            if (list != null && list.Count() > 0)
                return;

            long id = AWAPI_Common.library.MiscLibrary.CreateUniqueId();
            awSiteCulture siteLang = new awSiteCulture();

            siteLang.siteCultureId = id;
            siteLang.siteId = siteId;
            siteLang.cultureCode = cultureCode;

            _context.awSiteCultures.InsertOnSubmit(siteLang);
            _context.SubmitChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="cultureCode"></param>
        public void RemoveFromSite(long siteId, string cultureCode)
        {
            var list = from c in _context.awSiteCultures
                       where c.siteId.Equals(siteId) &&
                            c.cultureCode.ToLower().Equals(cultureCode.ToLower())
                       select c;
            if (list == null || list.Count() == 0)
                return;

            _context.awSiteCultures.DeleteAllOnSubmit(list);
            _context.SubmitChanges();
            return;
        }

        #endregion

        #region CULTURE VALUE
        /// <summary>
        /// Returns a specific row field's value
        /// </summary>
        /// <returns></returns>
        public string GetValue(string cultureCode, Int64 resourceRowId, string resourceTable, string resourceField)
        {
            var value = from l in _context.awCultureValues
                        where l.cultureCode.Equals(cultureCode) &&
                            l.resourceRowId.Equals(resourceRowId) &&
                            l.resourceField.ToLower().Equals(resourceField.ToLower())
                        select l.resourceValue;
            if (value == null || value.Count() == 0)
                return null;

            return value.ToList<string>()[0];
        }

        /// <summary>
        /// Returns a specific row field's value
        /// </summary>
        /// <returns></returns>
        public string GetValue(IList<awCultureValue> listSource, string cultureCode, Int64 resourceRowId, string resourceField)
        {
            if (listSource == null || listSource.Count == 0)
                return null;

            var value = from l in listSource
                        where l.cultureCode.Equals(cultureCode) &&
                            l.resourceRowId.Equals(resourceRowId) &&
                            l.resourceField.ToLower().Equals(resourceField.ToLower())
                        select l.resourceValue;
            if (value == null || value.Count() == 0)
                return null;

            return value.ToList<string>()[0];
        }

        /// <summary>
        /// Retures a row's fields values (title, description, etc...)
        /// </summary>
        /// <returns></returns>
        public System.Collections.Generic.IList<awCultureValue> GetValueList(string cultureCode, Int64 resourceRowId)
        {
            var value = from l in _context.awCultureValues
                        where l.cultureCode.Equals(cultureCode) &&
                            l.resourceRowId.Equals(resourceRowId)
                        select l;

            if (value == null || value.Count() == 0)
                return null;

            return value.ToList<awCultureValue>();
        }

        /// <summary>
        /// Returns all the values for a table
        /// </summary>
        /// <returns></returns>
        public System.Collections.Generic.IList<awCultureValue> GetValueList(string cultureCode, string resourceTable)
        {
            var value = from l in _context.awCultureValues
                        where l.cultureCode.Equals(cultureCode) &&
                            l.resourceTable.ToLower().Equals(resourceTable.ToLower())
                        select l;

            if (value == null || value.Count() == 0)
                return null;

            return value.ToList<awCultureValue>();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="cultureCode"></param>
        /// <param name="resourceRowId"></param>
        /// <param name="resourceTable"></param>
        /// <param name="resourceField"></param>
        /// <param name="resourceValue"></param>
        /// <returns></returns>
        public long UpdateValue(string cultureCode, Int64 resourceRowId, string resourceTable, string resourceField, string resourceValue)
        {
            awCultureValue value = new awCultureValue();

            var existing = from l in _context.awCultureValues
                           where l.cultureCode.Equals(cultureCode) &&
                               l.resourceRowId.Equals(resourceRowId) &&
                               l.resourceField.ToLower().Equals(resourceField.ToLower())
                           select l;
            if (existing != null && existing.Count() > 0)
            {
                value = existing.FirstOrDefault<awCultureValue>();
                value.resourceValue = resourceValue;
            }
            else
            {
                value.cultureValueId = AWAPI_Common.library.MiscLibrary.CreateUniqueId();

                value.cultureCode = cultureCode;
                value.resourceRowId = resourceRowId;
                value.resourceTable = resourceTable.ToLower();
                value.resourceField = resourceField.ToLower();
                value.resourceValue = resourceValue;
                _context.awCultureValues.InsertOnSubmit(value);
            }
            _context.SubmitChanges();

            return value.cultureValueId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceRowId"></param>
        public void DeleteValueByRowId(Int64 resourceRowId)
        {
            var list = from l in _context.awCultureValues
                       where l.resourceRowId.Equals(resourceRowId)
                       select l;
            if (list == null || list.Count() == 0)
                return;

            _context.awCultureValues.DeleteAllOnSubmit(list);
            _context.SubmitChanges();
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cultureCode"></param>
        public void DeleteValueByCultureCode(string cultureCode)
        {
            var list = from l in _context.awCultureValues
                       where l.cultureCode.ToLower().Equals(cultureCode.ToLower())
                       select l;
            if (list == null || list.Count() == 0)
                return;

            _context.awCultureValues.DeleteAllOnSubmit(list);
            _context.SubmitChanges();
            return;
        }



        #endregion


    }
}
