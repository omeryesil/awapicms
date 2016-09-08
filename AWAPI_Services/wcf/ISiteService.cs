using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;

namespace AWAPI_Services.wcf
{
    // NOTE: If you change the interface name "ISiteService" here, you must also update the reference to "ISiteService" in Web.config.
    [ServiceContract]
    public interface ISiteService
    {

        [OperationContract]
        AWAPI_Data.Data.awSite Get(long siteId);

        [OperationContract]
        System.Collections.Generic.IEnumerable<AWAPI_Data.Data.awSite> GetList(string where, string sortField);

        [OperationContract]
        void Delete(long siteId);

        [OperationContract]
        long Add(long ownerUserId, string alias,
                        string title, string description,
                        bool isEnabled, string link,
                        string imageurl,
                        int maxBlogs, int maxUsers, int maxContent, string defaultLanguage,
                        DateTime? pubDate);

        [OperationContract]
        bool Update(long siteId, long ownerUserId, string alias, string title, string description,
                   bool isEnabled, string link, string imageurl,
                   int maxBlogs, int maxUsers, int maxContent, string defaultLanguage,
                    DateTime? pubDate);

    }
}
