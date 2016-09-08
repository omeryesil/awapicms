using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;

using AWAPI_BusinessLibrary.library;

namespace AWAPI_Services.wcf
{
    // NOTE: If you change the class name "UserService" here, you must also update the reference to "UserService" in Web.config.
    [System.ServiceModel.Activation.AspNetCompatibilityRequirements(RequirementsMode = System.ServiceModel.Activation.AspNetCompatibilityRequirementsMode.Required)]
    public class UserService : IUserService
    {
        UserLibrary _userLib = new UserLibrary();

        #region GET
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public AWAPI_Data.CustomEntities.UserExtended Get(string accessKey, long siteId, long userId)
        {
            try
            {
                return SecureGet(accessKey, siteId, userId);
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
        public AWAPI_Data.CustomEntities.UserExtended SecureGet(string accessKey, long siteId, long userId)
        {
            return _userLib.Get(userId);
        }
        #endregion

        #region LOGIN

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="siteId"></param>
        /// <param name="emailOrUsername"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public AWAPI_Data.CustomEntities.UserExtended Login(string accessKey, long siteId, string emailOrUsername, string password)
        {
            try
            {
                return SecureLogin(accessKey, siteId, emailOrUsername, password);
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
        /// <param name="emailOrUsername"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [AWAPI_Services.SecurityAspect(SecurityAspect.SecurityType.AccessKey)]
        public AWAPI_Data.CustomEntities.UserExtended SecureLogin(string accessKey, long siteId, string emailOrUsername, string password)
        {
            AWAPI_Data.CustomEntities.UserExtended user = _userLib.Login(emailOrUsername, password);

            if (user == null)
                return null;


            //Super admin has right for all the sites...
            if (user.isSuperAdmin)
                return user;

            //Check if the user is in the site...
            AWAPI_Data.Data.awSiteUser siteUser = _userLib.GetUserFromSite(siteId, user.userId);
            if (siteUser == null || !siteUser.isEnabled)
                return null;

            return user;

        }
        #endregion

        #region GET LIST
        public System.Collections.Generic.IEnumerable<AWAPI_Data.CustomEntities.UserExtended> GetList(string accessKey, long siteId, string where, string sortField)
        {
            try
            {
                return SecureGetList(accessKey, siteId, where, sortField);
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
        /// <param name="where"></param>
        /// <returns></returns>
        [AWAPI_Services.SecurityAspect(SecurityAspect.SecurityType.AccessKey)]
        public System.Collections.Generic.IEnumerable<AWAPI_Data.CustomEntities.UserExtended> SecureGetList(string accessKey, long siteId, string where, string sortField)
        {
            return _userLib.GetList(siteId, where, sortField);
        }
        #endregion

        #region ADD USER
        /// <summary>
        /// . Creates a new user, and adds this user to a site
        /// . IsSuperAdmin and isEnabled fields are set as false. 
        /// . After the user creation and automated task will be created to enable the user
        /// . An email with the confirmation task will be sent for the confirmation
        /// . After the automated task completion the user will be redirected to the site 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="siteId"></param>
        /// <param name="username"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="description"></param>
        /// <param name="isEnabled"></param>
        /// <param name="link"></param>
        /// <param name="imageurl"></param>
        /// <param name="gender"></param>
        /// <param name="birthday"></param>
        /// <param name="tel"></param>
        /// <param name="tel2"></param>
        /// <param name="address"></param>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="postalCode"></param>
        /// <param name="country"></param>
        /// <param name="redirectUrlAfterConfirmationTask"></param>
        /// <returns></returns>
        public long Add(string accessKey, long siteId, string username, string firstName, string lastName,
                       string email, string password, string description, string link, string imageurl,
                       string gender, DateTime? birthday, string tel, string tel2, string address, string city,
                        string state, string postalCode, string country, string redirectUrlAfterConfirmationTask)
        {
            try
            {
                return SecureAdd(accessKey, siteId, username, firstName, lastName,
                            email, password, description, link,
                            imageurl, gender, birthday, tel, tel2, address, city,
                             state, postalCode, country, redirectUrlAfterConfirmationTask);
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
        private long SecureAdd(string accessKey, long siteId, string username, string firstName, string lastName,
                        string email, string password, string description, string link, string imageurl,
                        string gender, DateTime? birthday, string tel, string tel2, string address, string city,
                         string state, string postalCode, string country, string redirectUrlAfterConfirmationTask)
        {
            //CONFIRMATION EMAIL IS REQUIRED -------------------------------------------------------------
            AWAPI_Data.Data.awSite site = new AWAPI_BusinessLibrary.library.SiteLibrary().Get(siteId);
            if (site.userConfirmationEmailTemplateId == 0)
                throw new Exception(ErrorLibrary.ErrorMessage(ErrorLibrary.USER.USER_CONFIRMATION_EMAIL_REQURED));

            AWAPI_BusinessLibrary.library.EmailTemplateLib emailTemplateLib = new EmailTemplateLib();
            AWAPI_Data.Data.awEmailTemplate emailTemplate = emailTemplateLib.Get(site.userConfirmationEmailTemplateId);
            if (emailTemplate == null)
                throw new Exception(ErrorLibrary.ErrorMessage(ErrorLibrary.USER.USER_CONFIRMATION_EMAIL_NOT_FOUND));

            //ADD USER ------------------------------------------------------------------------------------
            long userId = _userLib.Add(username, firstName, lastName, email, password, description,
                       false, false, link, imageurl, gender, birthday, tel, tel2, address,
                       city, state, postalCode, country);

            if (userId <= 0)
                return 0;

            //ADD USER TO THE SITE ------------------------------------------------------------------------
            _userLib.AddUserToSite(siteId, userId, true);

            //ADD AN AUTOMATED CONFIRMATION TASK ----------------------------------------------------------
            AutomatedTaskLibrary taskLib = new AutomatedTaskLibrary();
            Guid enableUserTaskId = Guid.NewGuid();
            taskLib.Add(siteId, 0, enableUserTaskId,
                        "Enable User", userId.ToString(),
                        false, "",
                        "AWAPI_BusinessLibrary.library.UserLibrary", "UpdateStatus", String.Format("int64:{0}|bool:{1}", userId, (bool)true), 
                        redirectUrlAfterConfirmationTask);

            //SEND CONFIRMATION EMAIL ----------------------------------------------------------------------
            string confirmationLink = ConfigurationLibrary.Config.automatedTaskServiceUrl + "?taskid=" + enableUserTaskId.ToString();
            AWAPI_BusinessLibrary.library.EmailTemplateLib emailLib = new EmailTemplateLib();
            emailLib.Send(emailTemplate.emailTemplateId, email, 
                            "firstname|" + firstName,
                            "lastname|" + lastName,
                            "confirmationlink|" + confirmationLink,
                            "date|" + DateTime.Now.ToString());

            return userId;
        }
        #endregion

        #region UPDATE USER
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessKey"></param>
        /// <param name="siteId"></param>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="description"></param>
        /// <param name="isEnabled"></param>
        /// <param name="link"></param>
        /// <param name="imageurl"></param>
        /// <param name="gender"></param>
        /// <param name="birthday"></param>
        /// <param name="tel"></param>
        /// <param name="tel2"></param>
        /// <param name="address"></param>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="postalCode"></param>
        /// <param name="country"></param>
        /// <returns></returns>
        public bool Update(string accessKey, long siteId, long id, string username, string firstName, string lastName,
                        string email, string password, string description,
                        string link, string imageurl, string gender, DateTime? birthday, string tel, string tel2, string address, string city,
                         string state, string postalCode, string country)
        {
            try
            {
                return SecureUpdate(accessKey, siteId, id, username, firstName, lastName,
                            email, password, description, link,
                            imageurl, gender, birthday, tel, tel2, address, city,
                             state, postalCode, country);
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
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="description"></param>
        /// <param name="isEnabled"></param>
        /// <param name="link"></param>
        /// <param name="imageurl"></param>
        /// <param name="gender"></param>
        /// <param name="birthday"></param>
        /// <param name="tel"></param>
        /// <param name="tel2"></param>
        /// <param name="address"></param>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="country"></param>
        /// <returns></returns>
        [AWAPI_Services.SecurityAspect(SecurityAspect.SecurityType.AccessKey)]
        public bool SecureUpdate(string accessKey, long siteId, long id, string username, string firstName, string lastName,
                        string email, string password, string description, string link, string imageurl,
                        string gender, DateTime? birthday, string tel, string tel2, string address, string city,
                         string state, string postalCode, string country)
        {
            return _userLib.Update(id, username, firstName, lastName,
                        email, password, description, null, null, link,
                        imageurl, gender, birthday, tel, tel2, address, city, state, postalCode, country);

        }
        #endregion

        #region RESET PASSWORD
       /// <summary>
       /// 
       /// </summary>
       /// <param name="accessKey"></param>
       /// <param name="siteId"></param>
       /// <param name="email"></param>
        public void ResetPassword(string accessKey, long siteId, string email, string redirectLink)
        {
            try
            {
                SecureResetPassword(accessKey, siteId, email, redirectLink);
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
        /// <param name="accessKey"></param>
        /// <param name="siteId"></param>
        /// <param name="email"></param>
        [AWAPI_Services.SecurityAspect(SecurityAspect.SecurityType.AccessKey)]
        public void SecureResetPassword(string accessKey, long siteId, string email, string redirectLink)
        {
            _userLib.ResetPassword(siteId, email,redirectLink);

        }
        #endregion
    }
}
