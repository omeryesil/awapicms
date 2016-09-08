using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;

namespace AWAPI_Services.wcf
{
    // NOTE: If you change the interface name "IUserService" here, you must also update the reference to "IUserService" in Web.config.
    [ServiceContract]
    public interface IUserService
    {
        [FaultContract(typeof(AwapiFaultException))]
        [OperationContract]
        AWAPI_Data.CustomEntities.UserExtended Get(string accessKey, long siteId, long userId);

        [FaultContract(typeof(AwapiFaultException))]
        [OperationContract]
        AWAPI_Data.CustomEntities.UserExtended Login(string accessKey, long siteId, string email, string password);

        [FaultContract(typeof(AwapiFaultException))]
        [OperationContract]
        System.Collections.Generic.IEnumerable<AWAPI_Data.CustomEntities.UserExtended> GetList(string accessKey, long siteId, string where, string sortField);

        [FaultContract(typeof(AwapiFaultException))]
        [OperationContract]
        long Add(string accessKey, long siteId, string username, string firstName, string lastName,
                       string email, string password, string description, string link, string imageurl,
                       string gender, DateTime? birthday, string tel, string tel2, string address, string city,
                        string state, string postalCode, string country, string redirectUrlAfterConfirmationTask);

        [FaultContract(typeof(AwapiFaultException))]
        [OperationContract]
        bool Update(string accessKey, long siteId, long id, string username, string firstName, string lastName,
                        string email, string password, string description,
                        string link, string imageurl, string gender, DateTime? birthday, string tel, string tel2,
                        string address, string city, string state, string postalCode, string country);

        [FaultContract(typeof(AwapiFaultException))]
        [OperationContract]
        void ResetPassword(string accessKey, long siteId, string email, string redirectLink);
    }
}
