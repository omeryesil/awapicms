using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using AWAPI_Data.Data;
using AWAPI_BusinessLibrary.library;
using System.ServiceModel.Activation;

namespace AWAPI_Services.wcf
{
    // NOTE: If you change the class name "FileService" here, you must also update the reference to "FileService" in Web.config.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class FileService : IFileService
    {

        #region UPLOAD FILE
        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="userId"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="fileFullPath"></param>
        /// <param name="fileContent"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public long Upload(string accessKey, long siteId, long[] groupIds, long userId, string title, string description, string fileFullPath, string fileContent, string size)
        {
            try
            {
                return SecureUpload(accessKey, siteId, groupIds, userId, title, description, fileFullPath, fileContent, size);
            }
            catch (Exception ex)
            {
                AwapiFaultException awEx = new AwapiFaultException();
                awEx.ErrorMessage = ex.Message;
                if (ex.InnerException != null)
                    awEx.ErrorMessage = ex.InnerException.Message;
                throw new FaultException<AwapiFaultException>(awEx, new FaultReason(awEx.ErrorMessage));
            }
        }

        [AWAPI_Services.SecurityAspect(SecurityAspect.SecurityType.AccessKey)]
        public long SecureUpload(string accessKey, long siteId, long[] groupIds, long userId, string title, string description, string fileFullPath, string fileContent, string size)
        {
            FileLibrary fl = new FileLibrary();
            System.IO.Stream fileStream = new System.IO.MemoryStream(Convert.FromBase64String(fileContent));

            return fl.Upload(siteId, groupIds, userId, title, description, fileFullPath, fileStream, false, size);
        }
        #endregion

        #region ADD FILE
        /// <summary>
        /// Adds a new file with URL address.
        /// The difference between Upload and Add is: 
        ///     the Upload methods uploads a file to 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="userId"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="fileFullPath"></param>
        /// <param name="fileContent"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public long Add(string accessKey, long siteId, long fileId, long[] groupIds, long userId, string title, string description, string fileUrl, string thumbUrl)
        {
            try
            {
                return SecureAdd(accessKey, siteId, fileId, groupIds, userId, title, description, fileUrl, thumbUrl);
            }
            catch (Exception ex)
            {
                AwapiFaultException awEx = new AwapiFaultException();
                awEx.ErrorMessage = ex.Message;
                if (ex.InnerException != null)
                    awEx.ErrorMessage = ex.InnerException.Message;
                throw new FaultException<AwapiFaultException>(awEx, new FaultReason(awEx.ErrorMessage));
            }
        }

        [AWAPI_Services.SecurityAspect(SecurityAspect.SecurityType.AccessKey)]
        public long SecureAdd(string accessKey, long siteId, long fileId, long[] groupIds, long userId, string title, string description, string fileUrl, string thumbUrl)
        {
            FileLibrary fl = new FileLibrary();
            return fl.Add(siteId, fileId, groupIds, userId, title, description, fileUrl, thumbUrl, false);
        }
        #endregion

        #region UPDATE FILE
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public void Update(string accessKey, long siteId, long fileId, long[] groupIds, string title, string description, string fileFullPath, string fileContent, string size)
        {
            try
            {
                SecureUpdate(accessKey, siteId, fileId, groupIds, title, description, fileFullPath, fileContent, size);
            }
            catch (Exception ex)
            {
                AwapiFaultException awEx = new AwapiFaultException();
                awEx.ErrorMessage = ex.Message;
                if (ex.InnerException != null)
                    awEx.ErrorMessage = ex.InnerException.Message;
                throw new FaultException<AwapiFaultException>(awEx, new FaultReason(awEx.ErrorMessage));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [AWAPI_Services.SecurityAspect(SecurityAspect.SecurityType.AccessKey)]
        public void SecureUpdate(string accessKey, long siteId, long fileId, long[] groupIds, string title, string description, string fileFullPath, string fileContent, string size)
        {
            FileLibrary fl = new FileLibrary();
            System.IO.Stream fileStream = new System.IO.MemoryStream(Convert.FromBase64String(fileContent));

            fl.Update(fileId, groupIds, title, description, fileFullPath, fileStream, null, size);

        }
        #endregion

        #region GET FILE
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AWAPI_Data.Data.awFile Get(string accessKey, long siteId, long id)
        {
            try
            {
                return SecureGet(accessKey, siteId, id);
            }
            catch (Exception ex)
            {
                AwapiFaultException awEx = new AwapiFaultException();
                awEx.ErrorMessage = ex.Message;
                if (ex.InnerException != null)
                    awEx.ErrorMessage = ex.InnerException.Message;
                throw new FaultException<AwapiFaultException>(awEx, new FaultReason(awEx.ErrorMessage));
            }
        }

        [AWAPI_Services.SecurityAspect(SecurityAspect.SecurityType.AccessKey)]
        public AWAPI_Data.Data.awFile SecureGet(string accessKey, long siteId, long id)
        {
            return new FileLibrary().Get(id);
        }
        #endregion

        #region GETLISTBYGROUPID
        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public System.Collections.Generic.IEnumerable<awFile> GetListByGroupId(string accessKey, long siteId, long groupId)
        {
            try
            {
                return SecureGetListByGroupId(accessKey, siteId, groupId);
            }
            catch (Exception ex)
            {
                AwapiFaultException awEx = new AwapiFaultException();
                awEx.ErrorMessage = ex.Message;
                if (ex.InnerException != null)
                    awEx.ErrorMessage = ex.InnerException.Message;
                throw new FaultException<AwapiFaultException>(awEx, new FaultReason(awEx.ErrorMessage));
            }
        }

        public System.Collections.Generic.IEnumerable<awFile> SecureGetListByGroupId(string accessKey, long siteId, long groupId)
        {
            return new FileLibrary().GetListByGroupId(groupId, "");
        }
        #endregion

        #region DELETE
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileId"></param>
        public void Delete(string accessKey, long siteId, long fileId)
        {
            try
            {
                SecureDelete(accessKey, siteId, fileId);
            }
            catch (Exception ex)
            {
                AwapiFaultException awEx = new AwapiFaultException();
                awEx.ErrorMessage = ex.Message;
                if (ex.InnerException != null)
                    awEx.ErrorMessage = ex.InnerException.Message;
                throw new FaultException<AwapiFaultException>(awEx, new FaultReason(awEx.ErrorMessage));
            }
        }

        [AWAPI_Services.SecurityAspect(SecurityAspect.SecurityType.AccessKey)]
        public void SecureDelete(string accessKey, long siteId, long fileId)
        {
            new FileLibrary().Delete(fileId);
        }
        #endregion

        #region UTILITIES
        /// <summary>
        /// Returns file's extension
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GetContentType(string fileName)
        {
            string contentType = FileLibrary.GetContentType(fileName);
            return contentType;
        }

        /// <summary>
        /// Returns unique id,,,
        /// this is used to Add new file...
        /// </summary>
        /// <returns></returns>
        public long CreateUniqueId()
        {
            return AWAPI_Common.library.MiscLibrary.CreateUniqueId();        
        }


        #endregion 


    }
}
