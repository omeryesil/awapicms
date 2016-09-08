using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using AWAPI_Common.library;

namespace AWAPI_BusinessLibrary.library
{
    /// <summary>
    /// 0x1XX - Does not exist etc...
    /// 0x2XX - Disabled / not pusblished, etc...
    /// 0x3xx - Expired, etc...
    /// 0x4xx - Required, wrong format, etc...
    /// 0x5xx - Limitations
    /// 0x9xx - MIXED
    /// </summary>
    public class ErrorLibrary
    {
        /// <summary>
        /// Poll Error Messages
        /// </summary>
        public enum SITE
        {
            [Description("0x100:Site does not exist or disabled")]
            DOES_NOT_EXIST = 100,


            [Description("0x910:Reset password template does not exist")]
            RESET_PASSWORD_TEMPLATE_DOES_NOT_EXIST = 100,


        }

        /// <summary>
        /// Poll Error Messages
        /// </summary>
        public enum POLL
        {
            [Description("0x100:Poll does not exist")]
            DOES_NOT_EXIST = 100,

            [Description("0x200:Poll has not been published")]
            NOT_PUBLISHED = 200,

            [Description("0x300:Poll has been expired")]
            EXPIRED = 300
        }

        /// <summary>
        /// Poll Error Messages
        /// </summary>
        public enum CONTEST
        {
            [Description("0x100:Contest does not exist")]
            DOES_NOT_EXIST = 100,

            [Description("0x200:Contest has not been published")]
            NOT_PUBLISHED = 200,

            [Description("0x300:Contest has been expired")]
            EXPIRED = 300,

            [Description("0x400:Contest Id is required")]
            CONTESTID_REQUIRED = 400,

            [Description("0x410:Email is empty or in wrong format")]
            EMAIL_REQUIRED = 410,

            [Description("0x411:FirstName and LastName are required")]
            FIRSTNAME_LASTNAME_REQUIRED = 411,

            [Description("0x500:Maximum number of contest entries has been exceeded")]
            NUMBER_OF_CONTEST_EXCEEDED = 500
        }

        /// <summary>
        /// Poll Error Messages
        /// </summary>
        public enum USER
        {
            [Description("0x100:User does not exist")]
            DOES_NOT_EXIST = 100,

            [Description("0x400:Username is required")]
            USERNAME_REQUIRED = 400,

            [Description("0x401:Email is required")]
            EMAIL_REQUIRED = 401,

            [Description("0x402:Password is required")]
            PASSWORD_REQUIRED = 402,

            [Description("0x410:Firstname is required")]
            FIRSTNAME_REQUIRED = 410,

            [Description("0x411:Lastname is required")]
            LASTNAME_REQUIRED = 411,

            [Description("0x500:Username is already in use")]
            USERNAME_IN_USE = 500,

            [Description("0x501:Email is already in use")]
            EMAIL_IN_USE = 501,

            [Description("0x510:Password must be at least 6 characters or maximumum 30 characters")]
            WRONG_PASSWORD_FORMAT = 510,
            
            [Description("0x920:UserConfirmationEmailTemplateId is required for the site")]
            USER_CONFIRMATION_EMAIL_REQURED = 920,

            [Description("0x920:UserConfirmationEmailTemplateId not found")]
            USER_CONFIRMATION_EMAIL_NOT_FOUND = 920
        }



        public static string ErrorMessage(Enum e)
        {
            return EnumUtility.GetEnumDescription(e);
        }



    }
}
