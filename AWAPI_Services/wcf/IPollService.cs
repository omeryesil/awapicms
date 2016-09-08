using System.Collections.Generic;
using System.ServiceModel;
using AWAPI_Data.CustomEntities;

namespace AWAPI_Services.wcf
{
    // NOTE: If you change the interface name "IPollService" here, you must also update the reference to "IPollService" in Web.config.
    [ServiceContract]
    public interface IPollService
    {
        [OperationContract]
        PollExtended Get(string accessKey, long siteId, long pollId, string cultureCode);

        [OperationContract]
        IList<PollExtended> GetList(string accessKey, long siteId, string cultureCode, int pageSize, int pageIndex, System.DateTime? pubStartDate);

        [OperationContract]
        void AnswerPoll(string accessKey, long siteId, long pollId, long pollChoiceId);
    }
}
