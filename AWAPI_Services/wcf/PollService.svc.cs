using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Channels;
using AWAPI_Data.CustomEntities;
using System.Linq;

namespace AWAPI_Services.wcf
{
    // NOTE: If you change the class name "PollService" here, you must also update the reference to "PollService" in Web.config.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class PollService : IPollService
    {
        AWAPI_BusinessLibrary.library.PollLibrary _pollLib = new AWAPI_BusinessLibrary.library.PollLibrary();
        [AWAPI_Services.SecurityAspect(SecurityAspect.SecurityType.AccessKey)]
        public PollExtended Get(string accessKey, long siteId, long pollId, string cultureCode)
        {
            PollExtended poll = _pollLib.GetPoll(pollId, cultureCode, true);
            return poll;
        }

        [AWAPI_Services.SecurityAspect(SecurityAspect.SecurityType.AccessKey)]
        public IList<PollExtended> GetList(string accessKey, long siteId, string cultureCode, int pageSize, int pageIndex, DateTime? pubStartDate)
        {
            return _pollLib.GetPollList(siteId, "", cultureCode, true, 0, pubStartDate);
        }

        [AWAPI_Services.SecurityAspect(SecurityAspect.SecurityType.AccessKey)]
        public void AnswerPoll(string accessKey, long siteId, long pollId, long pollChoiceId)
        {
            _pollLib.AnswerPoll(pollId, pollChoiceId);
        }

    }
}
