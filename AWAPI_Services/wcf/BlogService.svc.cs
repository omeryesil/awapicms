using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AWAPI_Services.wcf
{
    // NOTE: If you change the class name "BlogService" here, you must also update the reference to "BlogService" in Web.config.
    [System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Required)]
    public class BlogService : IBlogService
    {
        AWAPI_BusinessLibrary.library.BlogLibrary _blogLibrary = new AWAPI_BusinessLibrary.library.BlogLibrary();

        #region BLOG POST COMMENT

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="siteId"></param>
        /// <param name="postId"></param>
        /// <param name="userName"></param>
        /// <param name="email"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public bool AddBlogComment(string accessKey, long siteId, long postId, string userName, string email, string firstName, string lastName, string title, string description)
        {
            try
            {
                return SecureAddBlogComment(accessKey, siteId, postId, userName, email, firstName, lastName, title, description);
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
        public bool SecureAddBlogComment(string accessKey, long siteId, long postId, string userName, string email, string firstName, string lastName, string title, string description)
        {

            if (siteId < 1)
                throw new Exception("SiteId not assigned");
            if (postId < 1)
                throw new Exception("PostId not assigned");


            long commentId = _blogLibrary.AddBlogPostComment(postId, null, title, description,
                                            email, userName, firstName, lastName,
                                            Convert.ToInt32(AWAPI_BusinessLibrary.library.BlogLibrary.Comment_Status.Pending));
            if (commentId <= 0)
                return false;


            return false;

        }
        #endregion

    }
}
