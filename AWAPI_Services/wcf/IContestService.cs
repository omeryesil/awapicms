using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using AWAPI_Data.CustomEntities;

namespace AWAPI_Services.wcf
{
    // NOTE: If you change the interface name "IContestService" here, you must also update the reference to "IContestService" in Web.config.
    [ServiceContract]
    public interface IContestService
    {
        [OperationContract]
        [FaultContract(typeof(AwapiFaultException))]
        ContestExtended Get(string accessKey, long siteId, long contestId);

        [OperationContract]
        [FaultContract(typeof(AwapiFaultException))]
        System.Collections.Generic.IList<ContestExtended> GetList(string accessKey, long siteId);

        [OperationContract]
        [FaultContract(typeof(AwapiFaultException))]
        void AddEntry(string accessKey, long siteId,
                                long contestId, long? userId, bool isEnabled, string cultureCode, string email, string firstName, string lastName,
                                 string title, string description, long? fileId, string fileUrl,
                                 string tel, string telType, string address, string city, string province,
                                 string postalCode, string country);

        [OperationContract]
        [FaultContract(typeof(AwapiFaultException))]
        AWAPI_Data.CustomEntities.ContestEntryExtended GetEntry(string accessKey, long siteId, long contestEntryId);

        [OperationContract]
        [FaultContract(typeof(AwapiFaultException))]
        System.Collections.Generic.IList<AWAPI_Data.CustomEntities.ContestEntryExtended> GetEntryList(string accessKey, long siteId,
                                long contestId, long? userId, System.DateTime? dt);

        [OperationContract]
        [FaultContract(typeof(AwapiFaultException))]
        System.Collections.Generic.IList<AWAPI_Data.CustomEntities.ContestEntryDailyReport> GetEntryDailySummary(string accessKey, long siteId,
                                long contestId, long? userId);
    }
}
