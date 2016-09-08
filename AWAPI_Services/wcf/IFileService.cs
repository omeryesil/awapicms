using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using AWAPI_Data.Data;

namespace AWAPI_Services.wcf
{
    // NOTE: If you change the interface name "IFileService" here, you must also update the reference to "IFileService" in Web.config.
    [ServiceContract]
    public interface IFileService
    {
        [OperationContract]
        [FaultContract(typeof(AwapiFaultException))]
        long Upload(string accessKey, long siteId, long[] groupIds, long userId, string title, string description, string fileFullPath, string fileContent, string size);

        [OperationContract]
        [FaultContract(typeof(AwapiFaultException))]
        long Add(string accessKey, long siteId, long fileId, long[] groupIds, long userId, string title, string description, string fileUrl, string thumbUrl);

        [OperationContract]
        [FaultContract(typeof(AwapiFaultException))]
        void Update(string accessKey, long siteId, long fileId, long[] groupIds, string title, string description, string fileFullPath, string fileContent, string size);

        [OperationContract]
        [FaultContract(typeof(AwapiFaultException))]
        AWAPI_Data.Data.awFile Get(string accessKey, long siteId, long id);

        [OperationContract]
        [FaultContract(typeof(AwapiFaultException))]
        System.Collections.Generic.IEnumerable<awFile> GetListByGroupId(string accessKey, long siteId, long groupId);

        [OperationContract]
        [FaultContract(typeof(AwapiFaultException))]
        void Delete(string accessKey, long siteId, long fileId);

        [OperationContract]
        string GetContentType(string fileName);

        [OperationContract]
        long CreateUniqueId();

    }
}
