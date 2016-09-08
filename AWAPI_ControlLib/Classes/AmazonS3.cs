using System;
using Amazon.S3.Model;

namespace AWAPI_ControlLib.Classes
{
    public class AmazonS3
    {
        public static string BaseURL
        {
            get
            {
                return String.Format("http://{0}.s3.amazonaws.com/", AWAPI_File_AmazonS3_BucketName);
            }
        }

        #region APP CONFIG
        public static string AWAPI_File_AmazonS3_BucketName
        {
            get
            {
                if (System.Configuration.ConfigurationManager.AppSettings["AWAPI_File_AmazonS3_BucketName"] != null)
                    return System.Configuration.ConfigurationManager.AppSettings["AWAPI_File_AmazonS3_BucketName"];
                else
                    return "";
            }
        }

        public static string AWAPI_File_AmazonS3_AccessKey
        {
            get
            {
                if (System.Configuration.ConfigurationManager.AppSettings["AWAPI_File_AmazonS3_AccessKey"] != null)
                    return System.Configuration.ConfigurationManager.AppSettings["AWAPI_File_AmazonS3_AccessKey"];
                else
                    return "";
            }
        }

        public static string AWAPI_File_AmazonS3_SecretKey
        {
            get
            {
                if (System.Configuration.ConfigurationManager.AppSettings["AWAPI_File_AmazonS3_SecretKey"] != null)
                    return System.Configuration.ConfigurationManager.AppSettings["AWAPI_File_AmazonS3_SecretKey"];
                else
                    return "";
            }
        }
        #endregion

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

            Amazon.S3.AmazonS3Client client = new Amazon.S3.AmazonS3Client(AWAPI_File_AmazonS3_AccessKey, AWAPI_File_AmazonS3_SecretKey, config);
            //System.IO.MemoryStream stream = new System.IO.MemoryStream(content);

            try
            {
                PutObjectRequest request = new PutObjectRequest();
                request.WithInputStream(fileStream);
                request.WithBucketName(AWAPI_File_AmazonS3_BucketName);
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

        //public void SetACL(string fileKey, bool anonymouseReadAccess)
        //{
        //    SetACLRequest aclRequest = new SetACLRequest();
        //    aclRequest.Key = fileKey;
        //    aclRequest.BucketName = AWAPI_File_AmazonS3_BucketName;

        //    S3AccessControlList aclList = new S3AccessControlList();

        //    Owner owner = new Owner();
        //    owner.Id = "oyesil";
        //    owner.DisplayName = "";
        //    aclList.Owner = owner;

        //    if (anonymouseReadAccess)
        //    {
        //        S3Grantee grantPublicRead = new S3Grantee();
        //        grantPublicRead.URI = " http://acs.amazonaws.com/groups/global/AllUsers";
        //        aclList.AddGrant(grantPublicRead, S3Permission.READ);
        //    }

        //    //Authenticated user read access 
        //    S3Grantee grantAuthenticatedRead = new S3Grantee();
        //    grantAuthenticatedRead.URI = " http://acs.amazonaws.com/groups/global/AuthenticatedUsers";
        //    aclList.AddGrant(grantAuthenticatedRead, S3Permission.READ);

        //    aclRequest.ACL = aclList;


        //    Amazon.S3.AmazonS3Client client = new Amazon.S3.AmazonS3Client(AWAPI_File_AmazonS3_AccessKey, AWAPI_File_AmazonS3_SecretKey);
        //    SetACLResponse aclResponse = client.SetACL(aclRequest);

        //    client.Dispose();
        //}

        /// <summary>
        /// Deletes file from s3
        /// </summary>
        /// <returns></returns>
        public bool Delete(string fileUrl)
        {
            try
            {
                string key = GetKeyNameFromUrl(fileUrl);
                Amazon.S3.AmazonS3Client client = new Amazon.S3.AmazonS3Client(AWAPI_File_AmazonS3_AccessKey, AWAPI_File_AmazonS3_SecretKey);
                Amazon.S3.Model.DeleteObjectRequest req = new Amazon.S3.Model.DeleteObjectRequest();
                req.BucketName = AWAPI_File_AmazonS3_BucketName;
                req.Key = key;

                client.DeleteObject(req);
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Amazon.S3.Model.GetObjectResponse Get(string key)
        {
            try
            {
                Amazon.S3.AmazonS3Client client = new Amazon.S3.AmazonS3Client(AWAPI_File_AmazonS3_AccessKey, AWAPI_File_AmazonS3_SecretKey);
                Amazon.S3.Model.GetObjectRequest req = new Amazon.S3.Model.GetObjectRequest();
                req.BucketName = AWAPI_File_AmazonS3_BucketName;
                req.Key = key;
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
        public bool Exists(string fileUrl)
        {
            string key = GetKeyNameFromUrl(fileUrl);

            Amazon.S3.Model.GetObjectResponse res = Get(key);
            if (res == null)
                return false;
            return true;
        }

        #region HELPERS
        public string GetKeyNameFromUrl(string fileUrl)
        {
            return fileUrl.Replace(BaseURL, "");
        }
        #endregion

    }
}
