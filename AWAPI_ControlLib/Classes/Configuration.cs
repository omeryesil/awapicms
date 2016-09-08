using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace AWAPI_ControlLib.Classes
{
    public class Configuration
    {
        #region Service Urls
        public static string ContentServiceUrl
        {
            get
            {
                if (ConfigurationSettings.AppSettings["AWAPI_Content_Service_URL"] != null)
                    return ConfigurationSettings.AppSettings["AWAPI_Content_Service_URL"];
                return "";
            }
        }
        public static string BlogServiceUrl
        {
            get
            {
                if (ConfigurationSettings.AppSettings["AWAPI_Blog_Service_URL"] != null)
                    return ConfigurationSettings.AppSettings["AWAPI_Blog_Service_URL"];
                return "";
            }
        }

        public static string TwitterServiceUrl
        {
            get
            {
                if (ConfigurationSettings.AppSettings["AWAPI_Twitter_Service_URL"] != null)
                    return ConfigurationSettings.AppSettings["AWAPI_Twitter_Service_URL"];
                return "";
            }
        }

        public static string FileServiceUrl
        {
            get
            {
                if (ConfigurationSettings.AppSettings["AWAPI_File_Service_URL"] != null)
                    return ConfigurationSettings.AppSettings["AWAPI_File_Service_URL"];
                return "";
            }
        }

        public static string PollServiceUrl
        {
            get
            {
                if (ConfigurationSettings.AppSettings["AWAPI_Poll_Service_URL"] != null)
                    return ConfigurationSettings.AppSettings["AWAPI_Poll_Service_URL"];
                return "";
            }
        }

        public static string ContestServiceUrl
        {
            get
            {
                if (ConfigurationSettings.AppSettings["AWAPI_Contest_Service_URL"] != null)
                    return ConfigurationSettings.AppSettings["AWAPI_Contest_Service_URL"];
                return "";
            }
        }


        #endregion


        public static long SiteId
        {
            get
            {
                if (ConfigurationSettings.AppSettings["AWAPI_SiteId"] != null)
                    return Convert.ToInt64(ConfigurationSettings.AppSettings["AWAPI_SiteId"]);
                return -1;
            }
        }

        public static string AccessKey
        {
            get
            {
                if (ConfigurationSettings.AppSettings["AWAPI_Site_AccessKey"] != null)
                    return ConfigurationSettings.AppSettings["AWAPI_Site_AccessKey"];
                return "";
            }
        }

        /// <summary>
        /// If 1 or true, then the CacheDuration for the controls will be set to empty,, so, no cache...
        /// </summary>
        public static bool ForceToDisableCache
        {
            get
            {
                if (ConfigurationSettings.AppSettings["AWAPI_ForceToDisableCache"] != null)
                {
                    string value = ConfigurationSettings.AppSettings["AWAPI_ForceToDisableCache"].ToLower().Trim();
                    if (value == "true" || value == "1")
                        return true;
                }
                return false;
            }
        }


    }
}
