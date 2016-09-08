using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AWAPI_Services.wcf
{
    // NOTE: If you change the interface name "IBlogService" here, you must also update the reference to "IBlogService" in Web.config.
    [ServiceContract]
    public interface IBlogService
    {
        [OperationContract]
        [FaultContract(typeof(AwapiFaultException))]
        bool AddBlogComment(string accessKey, long siteId, long postId, string userName, string email, string firstName, string lastName, string title, string description);

    }
}
