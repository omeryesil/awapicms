using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Linq.SqlClient;

using AWAPI_Data.Data;
using AWAPI_Common.library;

namespace AWAPI_BusinessLibrary.library
{
    public class EnvironmentLibrary
    {
        SiteContextDataContext _context = new SiteContextDataContext();

        #region ENVIRONMENT ---
        /// <summary>
        /// 
        /// </summary>
        /// <param name="environmentId"></param>
        /// <returns></returns>
        public awEnvironment Get(long environmentId)
        {
            if (environmentId <= 0)
                return null;
            var Environment = _context.GetTable<awEnvironment>()
                        .Where(st => st.environmentId.Equals(environmentId));

            if (Environment == null && Environment.ToList().Count() == 0)
                return null;
            return Environment.FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public System.Collections.Generic.IList<awEnvironment> GetList(long siteId)
        {
            var list = from l in _context.awEnvironments
                       where l.siteId.Equals(siteId)
                       orderby l.title
                       select l;

            if (list == null || list.Count() == 0)
                return null;
            return list.ToList<awEnvironment>();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="environmentId"></param>
        public void Delete(long environmentId)
        {
            if (environmentId <= 0)
                return;

            //DELETE Environment  ---------------------------------------------------------------
            var Environments = from b in _context.awEnvironments
                        where b.environmentId.Equals(environmentId)
                        select b;
            _context.awEnvironments.DeleteAllOnSubmit(Environments);

            //SUBMIT CHANGES -------------------------------------------------------------
            _context.SubmitChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="userId"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="serviceUrl"></param>
        /// <param name="isEnabled"></param>
        /// <returns></returns>
        public long Add(long siteId, long userId,
                        string title, string description, string serviceUrl, string publicKey)
        {
            long id = AWAPI_Common.library.MiscLibrary.CreateUniqueId();
            awEnvironment env = new awEnvironment();

            env.environmentId = id;
            env.title = title;
            env.description = description;
            env.serviceUrl = serviceUrl;
            env.publicKey = publicKey;

            env.siteId = siteId;
            env.userId = userId;

            env.lastBuildDate = DateTime.Now;
            env.createDate = DateTime.Now;

            _context.awEnvironments.InsertOnSubmit(env);
            _context.SubmitChanges();

            return id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="environmentId"></param>
        /// <param name="userId"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="serviceUrl"></param>
        /// <param name="publicKey"></param>
        /// <param name="isEnabled"></param>
        /// <returns></returns>
        public bool Update(long environmentId, long userId,
                        string title, string description, string serviceUrl, string publicKey)
        {
            awEnvironment env = _context.awEnvironments.FirstOrDefault(st => st.environmentId.Equals(environmentId));

            if (env == null)
                return false;

            env.title = title;
            env.description = description;
            env.serviceUrl = serviceUrl;
            env.publicKey = publicKey;

            env.userId = userId;

            _context.SubmitChanges();

            return true;
        }

        #endregion

    }
}
