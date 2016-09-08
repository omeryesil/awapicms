using System;
using Amazon.S3.Model;

namespace AWAPI_BusinessLibrary.library
{
    public class FileAmazonS3
    {
        private string _bucketName = ConfigurationLibrary.Config.fileAmazonS3BucketName;
        public string BucketName
        {
            get {
                return _bucketName;
            }
            set {
                _bucketName = value;
            }
        }


        public FileAmazonS3(string bucketName)
        {
            if (!String.IsNullOrEmpty(bucketName))
                BucketName = bucketName;
            else
                BucketName = ConfigurationLibrary.Config.fileAmazonS3BucketName;
        }


        public FileAmazonS3(long siteId)
        {

            //if the site has its own bucket name then use it, else get from he main configuration
            SiteLibrary lib = new SiteLibrary();
            AWAPI_Data.Data.awSite site = lib.Get(siteId);

            if (site != null && !String.IsNullOrEmpty(site.fileAmazonS3BucketName))
                BucketName = site.fileAmazonS3BucketName;
            else 
                BucketName = ConfigurationLibrary.Config.fileAmazonS3BucketName;

        }

        public string BaseURL
        {
            get
            {
                return String.Format("http://{0}.s3.amazonaws.com/", BucketName);
            }
        }

        //com.amazonaws.s3.AmazonS3 _amazonProxy = new AmazonS3();

        //com.amazonaws.s3.AmazonS3 AmazonProxy
        //{
        //    get {
        //        _amazonProxy.Pipeline.OutputFilters.Remove(typeof(Microsoft.Web.Services2.Security.SecurityOutputFilter));
        //        _amazonProxy.Pipeline.OutputFilters.Remove(typeof(Microsoft.Web.Services2.Referral.ReferralOutputFilter));
        //        _amazonProxy.Pipeline.OutputFilters.Remove(typeof(Microsoft.Web.Services2.Policy.PolicyEnforcementOutputFilter));

        //        /// Add our custom filter to remove the unwanted WSE soap headers.
        //        Microsoft.Web.Services2.SoapOutputFilter filt = 
        //        _amazonProxy.Pipeline.OutputFilters.Add( new   (new com.amazonaws.s3.fFiilters.HeaderOutputFilter("wsa:"));
        //        new 
        //        return _amazonProxy;
        //    }

        //}

        public static bool SaveOnAmazonS3()
        {
            return AWAPI_BusinessLibrary.library.ConfigurationLibrary.Config.fileSaveOnAmazonS3;
        }


        /// <summary>
        //
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="fileName">should be unique id: 234234546.jpg or 
        ///     /images/34234234234.pjg
        /// </param>
        /// <param name="content"></param>
        /// <param name="contentType"></param>
        /// <returns>Returns the URL address of the file...</returns>
        public string Upload(string filePath, System.IO.Stream fileStream, string contentType)
        {
            Amazon.S3.AmazonS3Config config = new Amazon.S3.AmazonS3Config().
                        WithCommunicationProtocol(Protocol.HTTP);

            string key = filePath.Replace(BaseURL, "");

            Amazon.S3.AmazonS3Client client = new Amazon.S3.AmazonS3Client(ConfigurationLibrary.Config.fileAmazonS3AccessKey,
                                                                ConfigurationLibrary.Config.fileAmazonS3SecreyKey, config);
            //System.IO.MemoryStream stream = new System.IO.MemoryStream(content);

            try
            {
                PutObjectRequest request = new PutObjectRequest();
                request.WithInputStream(fileStream);
                request.WithBucketName(BucketName);
                request.WithKey(key);
                request.WithCannedACL(S3CannedACL.PublicRead);

                S3Response response = client.PutObject(request);
                //response.Dispose();
            }
            catch (Amazon.S3.AmazonS3Exception ex)
            {
                if (ex.ErrorCode != null && (ex.ErrorCode.Equals("InvalidAccessKeyId") || ex.ErrorCode.Equals("InvalidSecurity")))
                {
                    Console.WriteLine("Please check the provided AWS Credentials.");
                    Console.WriteLine("If you haven't signed up for Amazon S3, please visit http://aws.amazon.com/s3");
                }
                else
                {
                    Console.WriteLine("An error occurred with the message '{0}' when writing an object", ex.Message);
                }
            }
            client.Dispose();

            //SetACL(fileName, true);

            string fileUrl = BaseURL + key;
            return fileUrl;


        }

        public void SetACL(string fileKey, bool anonymouseReadAccess)
        {
            SetACLRequest aclRequest = new SetACLRequest();
            aclRequest.Key = fileKey;
            aclRequest.BucketName = BucketName;

            S3AccessControlList aclList = new S3AccessControlList();

            Owner owner = new Owner();
            owner.Id = "oyesil";
            owner.DisplayName = "";
            aclList.Owner = owner; 

            if (anonymouseReadAccess)
            {
                S3Grantee grantPublicRead = new S3Grantee();
                grantPublicRead.URI = " http://acs.amazonaws.com/groups/global/AllUsers";
                aclList.AddGrant(grantPublicRead, S3Permission.READ);
            }

            //Authenticated user read access 
            S3Grantee grantAuthenticatedRead = new S3Grantee();
            grantAuthenticatedRead.URI = " http://acs.amazonaws.com/groups/global/AuthenticatedUsers";            
            aclList.AddGrant(grantAuthenticatedRead, S3Permission.READ);

            aclRequest.ACL = aclList;


            Amazon.S3.AmazonS3Client client = new Amazon.S3.AmazonS3Client(ConfigurationLibrary.Config.fileAmazonS3AccessKey,
                                                                ConfigurationLibrary.Config.fileAmazonS3SecreyKey);
            SetACLResponse aclResponse = client.SetACL(aclRequest);

            client.Dispose();
        }


        /// <summary>
        /// Deletes file from s3
        /// </summary>
        /// <returns></returns>
        public bool Delete(string fileUrl)
        {
            try
            {
                string key = fileUrl.Replace(BaseURL, "");
                string bucketName = GetBucketNameFromUrl(fileUrl);

                Amazon.S3.AmazonS3Client client = new Amazon.S3.AmazonS3Client(ConfigurationLibrary.Config.fileAmazonS3AccessKey,
                                                            ConfigurationLibrary.Config.fileAmazonS3SecreyKey);
                Amazon.S3.Model.DeleteObjectRequest req = new Amazon.S3.Model.DeleteObjectRequest();
                req.BucketName = bucketName;
                req.Key = key;

                client.DeleteObject(req);
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }

        public string GetBucketNameFromUrl(string url)
        {
            if (String.IsNullOrEmpty(url))
                return "";

            string tmp = url.ToLower().Replace("https://", "").Replace("http://", "");
            string[] parts = tmp.Split('.');
            if (parts.Length > 1)
                return parts[0];
            return "";
        }


        /// <summary>
        /// Check if the file exists or not
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public Amazon.S3.Model.GetObjectResponse Get(string fileName)
        {
            try
            {
                Amazon.S3.AmazonS3Client client = new Amazon.S3.AmazonS3Client(ConfigurationLibrary.Config.fileAmazonS3AccessKey,
                                                            ConfigurationLibrary.Config.fileAmazonS3SecreyKey);
                Amazon.S3.Model.GetObjectRequest req = new Amazon.S3.Model.GetObjectRequest();
                req.BucketName = GetBucketNameFromUrl(fileName); // ConfigurationLibrary.Config.fileAmazonS3BucketName;
                req.Key = fileName;
                Amazon.S3.Model.GetObjectResponse res = client.GetObject(req);

                return res;
            }
            catch (Exception ex)
            {

            }
            return null;
        }


        /// <summary>
        /// Check if the file exists or not
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool Exists(string fileName)
        {

            Amazon.S3.Model.GetObjectResponse res = Get(fileName);
            if (res == null)
                return false;
            return true;
        }

    }
}
