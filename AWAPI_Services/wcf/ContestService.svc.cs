using System.ServiceModel.Activation;
using AWAPI_BusinessLibrary.library;
using AWAPI_Data.CustomEntities;
using System.ServiceModel.Channels;
using System.Runtime.Serialization;
using System.ServiceModel;



namespace AWAPI_Services.wcf
{
    // NOTE: If you change the class name "ContestService" here, you must also update the reference to "ContestService" in Web.config.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class ContestService : IContestService
    {
        ContestLibrary _contestLib = new ContestLibrary();

        [AWAPI_Services.SecurityAspect(SecurityAspect.SecurityType.AccessKey)]
        public ContestExtended Get(string accessKey, long siteId, long contestId)
        {
            return SecureGet(accessKey, siteId, contestId);
        }

        public ContestExtended SecureGet(string accessKey, long siteId, long contestId)
        {
            try
            {
                ContestExtended contest = _contestLib.GetContest(contestId, true);
                return contest;
            }
            catch (System.Exception ex)
            {
                AwapiFaultException awEx = new AwapiFaultException();
                awEx.ErrorMessage = ex.Message;
                if (ex.InnerException != null)
                    awEx.ErrorMessage = ex.InnerException.Message;
                throw new FaultException<AwapiFaultException>(awEx, new FaultReason(awEx.ErrorMessage));
            }
        }


        [AWAPI_Services.SecurityAspect(SecurityAspect.SecurityType.AccessKey)]
        public System.Collections.Generic.IList<ContestExtended> GetList(string accessKey, long siteId)
        {
            System.Collections.Generic.IList<ContestExtended> list = _contestLib.GetContestList(siteId, true);
            return list;
        }


        #region ADD ENTRY
        public void AddEntry(string accessKey, long siteId,
                        long contestId, long? userId, bool isEnabled, string cultureCode, string email, string firstName, string lastName,
                         string title, string description, long? fileId, string fileUrl,
                         string tel, string telType, string address, string city, string province,
                         string postalCode, string country)
        {
            try
            {
                SecureAddEntry(accessKey, siteId,
                            contestId, userId, isEnabled, cultureCode, email, firstName, lastName,
                             title, description, fileId, fileUrl,
                             tel, telType, address, city, province,
                             postalCode, country);
            }
            catch (System.Exception ex)
            {
                AwapiFaultException awEx = new AwapiFaultException();
                awEx.ErrorMessage = ex.Message;
                if (ex.InnerException != null)
                    awEx.ErrorMessage = ex.InnerException.Message;
                throw new FaultException<AwapiFaultException>(awEx, new FaultReason(awEx.ErrorMessage));
            }
        }

        [AWAPI_Services.SecurityAspect(SecurityAspect.SecurityType.AccessKey)]
        public void SecureAddEntry(string accessKey, long siteId,
                                long contestId, long? userId, bool isEnabled, string cultureCode, string email, string firstName, string lastName,
                                 string title, string description, long? fileId, string fileUrl,
                                 string tel, string telType, string address, string city, string province,
                                 string postalCode, string country)
        {
            _contestLib.AddContestEntry(contestId, userId, isEnabled, cultureCode, email, firstName, lastName,
                                 title, description, fileId, fileUrl, tel, telType, address, city, province,
                                 postalCode, country);


        }
        #endregion


        #region GET ENTRY
        public AWAPI_Data.CustomEntities.ContestEntryExtended GetEntry(string accessKey, long siteId, long contestEntryId)
        {
            try
            {
                return SecureGetEntry(accessKey, siteId, contestEntryId);
            }
            catch (System.Exception ex)
            {
                AwapiFaultException awEx = new AwapiFaultException();
                awEx.ErrorMessage = ex.Message;
                if (ex.InnerException != null)
                    awEx.ErrorMessage = ex.InnerException.Message;
                throw new FaultException<AwapiFaultException>(awEx, new FaultReason(awEx.ErrorMessage));
            }
        }

        [AWAPI_Services.SecurityAspect(SecurityAspect.SecurityType.AccessKey)]
        public AWAPI_Data.CustomEntities.ContestEntryExtended SecureGetEntry(string accessKey, long siteId, long contestEntryId)
        {
            return _contestLib.GetContestEntry(contestEntryId, true);
        }
        #endregion


        #region GET ENTRIES
        public System.Collections.Generic.IList<AWAPI_Data.CustomEntities.ContestEntryExtended> GetEntryList(string accessKey, long siteId,
                                long contestId, long? userId, System.DateTime? dt)
        {
            try
            {
                return SecureGetEntryList(accessKey, siteId, contestId, userId, dt);
            }
            catch (System.Exception ex)
            {
                AwapiFaultException awEx = new AwapiFaultException();
                awEx.ErrorMessage = ex.Message;
                if (ex.InnerException != null)
                    awEx.ErrorMessage = ex.InnerException.Message;
                throw new FaultException<AwapiFaultException>(awEx, new FaultReason(awEx.ErrorMessage));
            }
        }

        [AWAPI_Services.SecurityAspect(SecurityAspect.SecurityType.AccessKey)]
        public System.Collections.Generic.IList<AWAPI_Data.CustomEntities.ContestEntryExtended> SecureGetEntryList(string accessKey, long siteId,
                                long contestId, long? userId, System.DateTime? dt)
        {
            return _contestLib.GetContestEntryList(contestId, userId, "", dt);
        }
        #endregion


        #region GET ENTRY DAILY SUMMARY ENTRY
        public System.Collections.Generic.IList<AWAPI_Data.CustomEntities.ContestEntryDailyReport> GetEntryDailySummary(string accessKey, long siteId,
                                long contestId, long? userId)
        {
            try
            {
                return SecureGetEntryDailySummary(accessKey, siteId, contestId, userId);
            }
            catch (System.Exception ex)
            {
                AwapiFaultException awEx = new AwapiFaultException();
                awEx.ErrorMessage = ex.Message;
                if (ex.InnerException != null)
                    awEx.ErrorMessage = ex.InnerException.Message;
                throw new FaultException<AwapiFaultException>(awEx, new FaultReason(awEx.ErrorMessage));
            }
        }

        [AWAPI_Services.SecurityAspect(SecurityAspect.SecurityType.AccessKey)]
        public System.Collections.Generic.IList<AWAPI_Data.CustomEntities.ContestEntryDailyReport> SecureGetEntryDailySummary(string accessKey, long siteId,
                                long contestId, long? userId)
        {
            return _contestLib.GetContestEntryDailyReport(contestId, userId);


        }
        #endregion
    }
}
