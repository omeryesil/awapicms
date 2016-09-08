using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AWAPI_Data.Data;


namespace AWAPI_BusinessLibrary.library
{
    /// <summary>
    /// </summary>
    public class ConfigurationLibrary
    {
        AWAPI_Data.Data.ConfigurationContextDataContext _context = new ConfigurationContextDataContext();


        public static long CurrentConfigurationId
        {
            get
            {
                if (System.Configuration.ConfigurationManager.AppSettings["AWAPI_ConfigurationId"] != null)
                    return Convert.ToInt64(System.Configuration.ConfigurationManager.AppSettings["AWAPI_ConfigurationId"]);
                return 0;
            }

        }
        
        private static awConfiguration _config = null;
        public static awConfiguration Config
        {
            get
            {
                ConfigurationLibrary lib = new ConfigurationLibrary();
                if (_config == null && CurrentConfigurationId > 0)
                        _config = lib.Get(CurrentConfigurationId);
                if (_config == null)
                    return new awConfiguration();
                return _config;
            }

            set
            {
                _config = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public System.Collections.Generic.IEnumerable<awConfiguration> GetList()
        {
            var list = from l in _context.awConfigurations
                       orderby l.title
                       select l;
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configId"></param>
        /// <returns></returns>
        public awConfiguration Get(Int64 configId)
        {
            var list = from l in _context.awConfigurations
                       where l.configurationId.Equals(configId)
                       orderby l.title
                       select l;
            return list.FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        public awConfiguration Get(string alias)
        {
            var list = from l in _context.awConfigurations
                       where l.alias.Equals(alias)
                       orderby l.title
                       select l;
            return list.FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configurationId"></param>
        /// <param name="alias"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="fileServiceUrl"></param>
        /// <param name="contentServiceUrl"></param>
        /// <param name="blogServiceUrl"></param>
        /// <param name="pollServiceUrl"></param>
        /// <param name="automatedTaskServiceUrl"></param>
        /// <param name="twitterServiceUrl"></param>
        /// <param name="weatherServiceUrl"></param>
        /// <param name="adminBaseUrl"></param>
        /// <param name="filePath"></param>
        /// <param name="fileMimeXMLPath"></param>
        /// <param name="fileSaveOnAmazonS3"></param>
        /// <param name="fileAmazonS3BucketName"></param>
        /// <param name="fileAmazonS3AccessKey"></param>
        /// <param name="fileAmazonS3SecreyKey"></param>
        /// <param name="smtpServer"></param>
        public long Save(long configurationId, string alias, string title, string description,
                    string fileServiceUrl, string contentServiceUrl, string blogServiceUrl, string pollServiceUrl,
                    string automatedTaskServiceUrl, string twitterServiceUrl, string weatherServiceUrl, string adminBaseUrl,
                    string fileDirectory, string fileMimeXMLPath, bool fileSaveOnAmazonS3, string fileAmazonS3BucketName,
                    string fileAmazonS3AccessKey, string fileAmazonS3SecreyKey, string smtpServer)
        {
            alias = alias.ToLower().Trim();
            bool addNew = configurationId == 0 ? true : false;
            awConfiguration config = Get(alias);

            if (config != null && config.configurationId != configurationId)
                throw new Exception("Alias already exists.");

            if (addNew)
            {
                config = new awConfiguration();
                config.configurationId = AWAPI_Common.library.MiscLibrary.CreateUniqueId();
            }
            else
                config = Get(configurationId);

            config.alias = alias;
            config.title = title.Trim();
            config.description = description.Trim();

            config.adminBaseUrl = adminBaseUrl.Trim();
            config.automatedTaskServiceUrl = automatedTaskServiceUrl.Trim();
            config.blogServiceUrl = blogServiceUrl.Trim();
            config.contentServiceUrl = contentServiceUrl.Trim();
            config.fileAmazonS3AccessKey = fileAmazonS3AccessKey.Trim();
            config.fileAmazonS3BucketName = fileAmazonS3BucketName.Trim();
            config.fileAmazonS3SecreyKey = fileAmazonS3SecreyKey.Trim();
            config.fileMimeXMLPath = fileMimeXMLPath.Trim();
            config.fileDirectory = fileDirectory.Trim();
            config.fileSaveOnAmazonS3 = fileSaveOnAmazonS3;
            config.fileServiceUrl = fileServiceUrl.Trim();
            config.pollServiceUrl = pollServiceUrl.Trim();
            config.smtpServer = smtpServer.Trim();
            config.twitterServiceUrl = twitterServiceUrl.Trim();
            config.weatherServiceUrl = weatherServiceUrl.Trim();

            config.createDate = DateTime.Now;

            if (addNew)
                _context.awConfigurations.InsertOnSubmit(config);

            _context.SubmitChanges();

            //If the updated configuration is the configuration in the web.config, then reload the Config
            if (!addNew && config.configurationId == CurrentConfigurationId)
                Config = Get(config.configurationId);

            return config.configurationId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="configurationId"></param>
        public void Delete(long configurationId)
        {
            var list = from l in _context.awConfigurations
                       where l.configurationId.Equals(configurationId)
                       select l;
            if (list == null || list.Count() == 0)
                return;

            _context.awConfigurations.DeleteAllOnSubmit(list);
            _context.SubmitChanges();
            return;
        }



    }
}
