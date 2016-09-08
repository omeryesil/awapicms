using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.IO;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Imaging;
using AWAPI_BusinessLibrary.library;
using System.ServiceModel.Syndication;
using System.Xml;

using System.Linq;
using System.Xml.Linq;
using System.Net;

namespace AWAPI_Services.handler
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class filehandler : IHttpHandler
    {
        //TODO: Should be defined in a db table (max be for each flGrp)
        public const int MAXIMUM_IMAGE_SIZE_TO_SAVE = 1000;
        FileLibrary _fileLib = new FileLibrary();
        HttpContext _context = null;

        #region PROPERTIES
        /// <summary>
        /// </summary>
        string Method
        {
            get
            {
                if (HttpContext.Current.Request["method"] != null)
                    return HttpContext.Current.Request["method"].ToString().ToLower();
                return "get";
            }
        }


        /// <summary>
        /// </summary>
        string FileId
        {
            get
            {
                if (HttpContext.Current.Request["id"] != null)
                    return HttpContext.Current.Request["id"].ToString();
                return "";
            }
        }


        /// <summary>
        /// Size for files. Format: WidthxHeight; 200x300
        /// </summary>
        string ImageWidthxHeight
        {
            get
            {
                if (HttpContext.Current.Request["size"] != null)
                    return HttpContext.Current.Request["size"];
                return "";
            }
        }

        /// <summary>
        /// if Size is not null then this is taken into consideration.
        /// if true then the original image will be resized and returned without saving.
        /// </summary>
        bool ImageDoNotSave
        {
            get
            {
                if (HttpContext.Current.Request["dontsave"] != null)
                    return Boolean.Parse(HttpContext.Current.Request["dontsave"]);
                return false;
            }
        }

        /// <summary>
        /// . if Size is not null then this is taken into consideration.
        /// 
        /// . if true then the resized image will be cropped... 
        /// . for example if the size parameter is 100x100, but if the resized image size is 100x120, 
        /// then the resized image will be cropped to  100x100
        /// </summary>
        bool CropResized
        {
            get
            {
                if (HttpContext.Current.Request["crop"] != null)
                    return Boolean.Parse(HttpContext.Current.Request["crop"]);
                return false;
            }
        }

        #endregion

        /// <summary>
        /// Processes the refferrer request
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            _context = context;
            switch (Method.ToLower())
            {
                case "upload":
                    Upload();
                    break;

                case "getlist": //returns list of the files
                    GetList();
                    break;

                case "getvideopreview": //returns video preview file
                    GetVideoPreview();
                    break;

                default: //Get File
                    Get();
                    break;
            }
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        #region GET COMMAND
        /// <summary>
        /// Returns file
        /// </summary>
        /// <param name="context"></param>
        void Get()
        {
            string size = ImageWidthxHeight;
            string fileId = FileId;

            long resourceId = 0;
            if (String.IsNullOrEmpty(fileId))
            {
                _context.Response.ContentType = "text/plain";
                _context.Response.Write("");
                return;
            }

            // get params from the file name first (size is set like size=100x200)
            Match m = Regex.Match(fileId, @"\([^\)]+\)");
            if (m != null && string.IsNullOrEmpty(m.Value) == false)
            {
                fileId = fileId.Replace(m.Value, "");
                string p = m.Value.Replace("(", "").Replace(")", "");
                if (p.Contains("x"))
                    size = p;
            }

            fileId = Regex.Replace(fileId, @"[^0-9]", "");

            if (fileId.Length > 0)
                resourceId = Int64.Parse(fileId);

            if (resourceId > 0)
                PopulateContext(_context, resourceId, size, ImageDoNotSave, CropResized);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static bool IsImageSizeRegisteredToSave(string size)
        {
            string[] ar = size.Split('x');
            int width = Convert.ToInt32(ar[0]);
            int height = Convert.ToInt32(ar[1]);

            if (width > MAXIMUM_IMAGE_SIZE_TO_SAVE ||
                height > MAXIMUM_IMAGE_SIZE_TO_SAVE)
                return false;

            return true;
        }

        /// <summary>
        /// Populates the context
        /// </summary>
        /// <param name="context">Context value</param>
        /// <param name="id">ImageGallery Id</param>
        /// <param name="size">Size</param>
        /// <param name="doNotSave">If true, origiginal file will be resized and returned, 
        /// and it won't be saved 
        /// </param>
        public void PopulateContext(HttpContext context, long imageId, string size, bool doNotSave, bool cropResized)
        {
            FileStream fileStream = null;

            try
            {
                context.Response.Clear();

                FileLibrary.FileInfo fi = _fileLib.GetFileInfo(imageId, size, cropResized);

                if (String.IsNullOrEmpty(fi.Path))
                    return;

                if (!fi.isOnLocal && (string.IsNullOrEmpty(size) || fi.ContentType.IndexOf("image") < 0))
                {
                    context.Response.Redirect(fi.Path);
                    return;
                }

                byte[] bytes;
                //check if the file exists
                context.Response.ContentType = fi.ContentType;
                context.Response.AddHeader("Content-Disposition",
                                            "inline; filename=" + fi.FileName);

                //If the file is not saved on local, and it is an image(checked above)
                //then resize the image, but don't save it
                if (!fi.isOnLocal)
                {
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(fi.Path);
                    HttpWebResponse res = (HttpWebResponse)(req).GetResponse(); 
                    Stream sr = res.GetResponseStream();
                    MemoryStream mem = new MemoryStream();
                    BinaryWriter bw = new BinaryWriter(mem);
                    BinaryReader br = new BinaryReader(sr);
                    bw.Write(br.ReadBytes(Convert.ToInt32(res.ContentLength)));

                    bytes = AWAPI_Common.library.ImageLibrary.ResizeImage("", mem, false, 0, 0, size, cropResized);
                    res.Close();
                }
                else if (doNotSave && string.IsNullOrEmpty(size) == false)
                    bytes = AWAPI_Common.library.ImageLibrary.ResizeImage(fi.Path, size, cropResized);
                else if (string.IsNullOrEmpty(size) == false && !File.Exists(fi.PathWithSize))
                {
                    bytes = AWAPI_Common.library.ImageLibrary.ResizeImage(fi.Path, size, cropResized);
                    if (IsImageSizeRegisteredToSave(size))
                    {
                        System.IO.Stream stream = new System.IO.MemoryStream(bytes);
                        AWAPI_BusinessLibrary.library.FileLibrary.SaveFile(fi.SiteId, true, fi.ContentType, fi.PathWithSize, stream, "");
                    }
                }
                else
                {
                    fileStream = new FileStream(fi.PathWithSize, FileMode.Open, FileAccess.Read, System.IO.FileShare.Read);
                    bytes = new byte[fileStream.Length];
                    //fileStream.Read(bytes, 0, (int)(fileStream.Length - 1));

                    bytes = ReadFile(fi.PathWithSize);

                    //fileStream.Close();
                }

                //write the image to the context
                context.Response.OutputStream.Write(bytes, 0, bytes.Length);
                //context.Response.Flush();
            }
            catch (Exception ex)
            {
                throw ex;

            }
            finally
            {
                if (fileStream != null)
                    fileStream.Close();

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static byte[] ReadFile(string filePath)
        {
            byte[] buffer;
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            try
            {
                int length = (int)fileStream.Length;  // get file length
                buffer = new byte[length];            // create buffer
                int count;                            // actual number of bytes read
                int sum = 0;                          // total number of bytes read

                // read until Read method returns 0 (end of the stream has been reached)
                while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                    sum += count;  // sum is a buffer offset for next reading
            }
            finally
            {
                fileStream.Close();
            }
            return buffer;
        }
        #endregion

        #region GETPREVIEW COMMAND
        void GetVideoPreview()
        {
            string fileId = FileId;
            long resourceId = 0;

            if (String.IsNullOrEmpty(fileId))
            {
                _context.Response.ContentType = "text/plain";
                _context.Response.Write("");
                return;
            }

            fileId = Regex.Replace(fileId, @"[^0-9]", "");

            if (fileId.Length > 0)
                resourceId = Int64.Parse(fileId);

            if (resourceId > 0)
                PopulateVideoPreview(_context, resourceId);
        }

        public void PopulateVideoPreview(HttpContext context, long fileId)
        {
            try
            {
                context.Response.Clear();

                FileLibrary lib = new FileLibrary();
                VideoFileLibrary videoLib = new VideoFileLibrary();

                AWAPI_Data.Data.awFile file = lib.Get(fileId);
                string extension = FileLibrary.GetFileExension(file.path);

                if (file == null || !videoLib.isVideoFile(extension))
                    return;

                if (!file.isOnLocal)
                {
                    context.Response.Redirect(file.thumbnail);
                    return;
                }

                if (!System.IO.File.Exists(file.thumbnail))
                    return;

                context.Response.ContentType = "image/jpeg";
                context.Response.AddHeader("Content-Disposition", "inline; filename=" + file.title + "_preview.jpg");

                byte[] bytes = File.ReadAllBytes(file.thumbnail);

                context.Response.OutputStream.Write(bytes, 0, bytes.Length);
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
        #endregion 

        #region UPLOAD FILE
        void Upload()
        {
            string callBackPage = _context.Request["callBackPage"] == null ? _context.Request.UrlReferrer.ToString() : _context.Request["callBackPage"];

            try
            {
                string accessKey = _context.Request["accessKey"] == null ? "" : _context.Request["accessKey"];
                long siteId = _context.Request["siteId"] == null ? 0 : Convert.ToInt64(_context.Request["siteId"]);
                long userId = _context.Request["userId"] == null ? 0 : Convert.ToInt64(_context.Request["userId"]);
                string groupIdsInStr = _context.Request["groupIds"] == null ? "" : _context.Request["groupIds"];
                string fileTitle = _context.Request["title"] == null ? "" : _context.Request["title"];
                string fileDescription = _context.Request["description"] == null ? "" : _context.Request["description"];
                string size = _context.Request["size"] == null ? "" : _context.Request["size"];


                //GET THE POSTED FILE
                HttpPostedFile postedFile = _context.Request.Files[0];

                //byte[] bytes = new byte[postedFile.InputStream.Length];
                //postedFile.InputStream.Read(bytes, 0, (int)postedFile.InputStream.Length);
                //string fileContent = Convert.ToBase64String(bytes);
                string fileName = postedFile.FileName;

                //get groupids into long array
                string[] ids = groupIdsInStr.Trim().Split('|');
                ArrayList tmpGroupIds = new ArrayList();
                foreach (string groupId in ids)
                    if (groupId.Trim() != "")
                        tmpGroupIds.Add(Convert.ToInt64(groupId));

                long[] groupIds = null;
                if (tmpGroupIds.Count > 0)
                    groupIds = (long[])tmpGroupIds.ToArray(typeof(long));

                long fileId = SecureUpload(accessKey, siteId, userId, groupIds, fileTitle, fileDescription, postedFile.InputStream, fileName, size);

                _context.Response.Redirect(callBackPage + "?status=ok&description=" + fileId.ToString(), false);
            }
            catch (Exception ex)
            {
                _context.Response.Redirect(callBackPage + "?status=error&description=" + _context.Server.HtmlEncode(ex.Message), false);
            }
        }

        [AWAPI_Services.SecurityAspect(SecurityAspect.SecurityType.AccessKey)]
        long SecureUpload(string accessKey, long siteId, long userId, long[] groupIds,
                        string fileTitle, string fileDescription, System.IO.Stream fileStream, string fileName, string size)
        {
            long fileId = _fileLib.Upload(siteId, groupIds, userId, fileTitle, fileDescription, fileName, fileStream, false, size);
            return fileId;

        }
        #endregion

        #region GET LIST METHOD
        /// <summary>
        /// Returns list of the files...
        /// </summary>
        void GetList()
        {
            long siteId = _context.Request["siteId"] == null ? 0 : Convert.ToInt64(_context.Request["siteId"]);
            long userid = _context.Request["userid"] == null ? 0 : Convert.ToInt64(_context.Request["userid"]);
            long fileGroupid = _context.Request["fileGroupid"] == null ? 0 : Convert.ToInt64(_context.Request["fileGroupid"]);
            string contentType = _context.Request["contenttype"] == null ? "" : _context.Request["contenttype"];
            string returnType = _context.Request["type"] == null ? "rss" : _context.Request["type"];


            //Chheck if the site exists and enabled
            AWAPI_Data.Data.awSite site = new AWAPI_BusinessLibrary.library.SiteLibrary().Get(siteId);
            if (site == null || !site.isEnabled)
                return;

            //SET THE FEED HEADER ----------------------------------
            string link = String.IsNullOrEmpty(site.link) ? "http://awapi.com" : site.link;
            SyndicationFeed  feed = new SyndicationFeed(site.title + " - Blog Feed", "", new Uri(link));
            feed.Authors.Add(new SyndicationPerson("omer@awapi.com"));
            feed.AttributeExtensions.Add(new XmlQualifiedName("site"), site.title);
            feed.AttributeExtensions.Add(new XmlQualifiedName("sitelink"), site.link);
            feed.AttributeExtensions.Add(new XmlQualifiedName("servertime"), DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));

            if (userid > 0) feed.AttributeExtensions.Add(new XmlQualifiedName("userid"), userid.ToString());
            if (fileGroupid > 0) feed.AttributeExtensions.Add(new XmlQualifiedName("filegroupid"), fileGroupid.ToString());
            if (contentType != "") feed.AttributeExtensions.Add(new XmlQualifiedName("contenttype"), contentType);

            //GET FILE LIST 
            IList<AWAPI_Data.Data.awFile> fileList = _fileLib.GetList(siteId, "");
            if (fileList != null && fileList.Count > 0)
            { 
                //get the list if the groupid and userid are set
                var list2 = from l in fileList
                            where l.isEnabled && 
                                (userid == 0 || userid>0 && l.userId.Equals(userid)) && 
                                (fileGroupid == 0 || 
                                 fileGroupid>0 && l.awFileInGroups.FirstOrDefault(a=>a.fileGroupId.Equals(fileGroupid)) != null)
                            select l;

                if (list2!= null && list2.Count()>0)
                {

                    //fill the syndication feed
                    List<SyndicationItem> items = new List<SyndicationItem>();
                    fileList = list2.ToArray();
                    foreach (AWAPI_Data.Data.awFile file in fileList)
                    {
                        Uri uri = null;

                        SyndicationItem item = new SyndicationItem(
                                            file.title,
                                            file.description,
                                            uri,
                                            file.fileId.ToString(),
                                            file.createDate);

                        item.ElementExtensions.Add("contenttype", null, file.contentType);
                        if (!String.IsNullOrEmpty(file.thumbnail))
                            item.ElementExtensions.Add("videopreview", null, file.thumbnail);
                        
                        item.ElementExtensions.Add("userid", null, file.userId);
                        item.ElementExtensions.Add("username", null, file.awUser_File.username);
                        item.ElementExtensions.Add("userfirstname", null, file.awUser_File.firstName);
                        item.ElementExtensions.Add("userlastname", null, file.awUser_File.lastName);


                        items.Add(item);
                    }
                    feed.Items = items;
                }
            }


            string output = "";
            XmlWriter writer = XmlWriter.Create(_context.Response.Output);
            if (feed != null)
            {
                switch (returnType)
                {
                    case "atom":
                        _context.Response.ContentType = "application/atom+xml";
                        feed.Description = new TextSyndicationContent("AWAPI Content in Atom 1.0 Feed Format");
                        Atom10FeedFormatter atom = new Atom10FeedFormatter(feed);
                        atom.WriteTo(writer);
                        break;
                    case "json":
                        _context.Response.ContentType = "application/json";
                        Rss20FeedFormatter rssFeed = new Rss20FeedFormatter(feed);
                        if (rssFeed != null)
                        {
                            output = Newtonsoft.Json.JsonConvert.SerializeObject(rssFeed);
                            //JavaScriptSerializer ser = new JavaScriptSerializer();
                            //output = ser.Serialize(rssFeed);
                        }
                        break;
                    default:    //rss
                        feed.Description = new TextSyndicationContent("AWAPI Content in RSS 2.0 Feed Format");
                        _context.Response.ContentType = "application/rss+xml";
                        Rss20FeedFormatter rss = new Rss20FeedFormatter(feed);
                        rss.WriteTo(writer);
                        break;
                }
            }

            if (output != "")
                _context.Response.Write(output);

            writer.Close();        
        }
        #endregion





    }
}
