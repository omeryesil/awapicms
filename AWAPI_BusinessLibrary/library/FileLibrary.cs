using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using AWAPI_Common.library;
using AWAPI_Data.Data;
using Microsoft.Win32;

namespace AWAPI_BusinessLibrary.library
{
    public class FileLibrary
    {
        FileContextDataContext _context = new FileContextDataContext();

        VideoFileLibrary _videoLib = new VideoFileLibrary();

        public struct FileInfo
        {
            public long Id;
            public long SiteId;
            public string FileName;
            public string Path;
            /// <summary>
            /// Only for files
            /// </summary>
            public string PathWithSize;
            public string Extension;
            public string ContentType;
            public bool isOnLocal;
        }

        #region FILES -------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public awFile Get(long id)
        {
            var files = _context.GetTable<awFile>().
                       Where(f => f.fileId.Equals(id));

            if (files == null || files.Count() == 0)
                return null;
            return files.First<awFile>();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="size"></param>
        /// <param name="pushSize"></param>
        /// <returns></returns>
        public FileInfo GetFileInfo(long id, string size, bool pushSize)
        {
            FileInfo fi = new FileInfo();
            fi.Id = id;
            fi.SiteId = 0;
            fi.Path = "";
            fi.PathWithSize = "";
            fi.Extension = "";
            fi.ContentType = "image/jpeg";
            fi.isOnLocal = false;

            //GetFileInfo image
            awFile file = _context.awFiles.FirstOrDefault(i => i.fileId.Equals(fi.Id));
            if (file == null)
                return fi;

            fi.SiteId = file.siteId;
            fi.isOnLocal = file.isOnLocal;
            fi.Path = file.path;
            fi.FileName = String.IsNullOrEmpty(file.title) ?
                    GetFileNameOnly(fi.Path) :
                    file.title.Replace(" ", "_").Replace("'", "") + "." + fi.Extension;


            if (fi.isOnLocal && !File.Exists(fi.Path))
            {
                //fi.Path = AWAPI_FilePath + "\\ImageNotAvailable.jpg";
                //string source = HttpContext.Current.Server.MapPath("~/ImageNotAvailable.jpg");
                fi.Path = HttpContext.Current.Server.MapPath("~/img/imageNotAvailable.jpg");
                //if (!File.Exists(fi.Path))
                //  File.Copy(source, fi.Path);
            }

            fi.Extension = GetFileExension(fi.Path);
            fi.PathWithSize = fi.Path;

            //example without pushSize  : c:\temp\34234234_300x100.jpg
            //example with pushSize     : c:\temp\34234234_300x100_cropped.jpg
            if (!string.IsNullOrEmpty(size))
            {
                if (!pushSize)
                    fi.PathWithSize = fi.Path.Replace("." + fi.Extension, "") +
                                        "_" + size + "." + fi.Extension;
                else
                    fi.PathWithSize = fi.Path.Replace("." + fi.Extension, "") +
                                        "_" + size + "_fixed." + fi.Extension;
            }

            //get the fileContent type
            if (file.contentType != null && file.contentType != "")
                fi.ContentType = file.contentType;
            else
            {
                fi.ContentType = GetContentType(AWAPI_BusinessLibrary.library.ConfigurationLibrary.Config.fileMimeXMLPath, fi.Extension);
            }

            return fi;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public System.Collections.Generic.IList<awFile> GetListByGroupId(long groupId, string keywordSearch)
        {
            bool search = false;
            if (!String.IsNullOrEmpty(keywordSearch))
            {
                search = true;
                keywordSearch = keywordSearch.ToLower().Trim();
            }

            var files = from fl in _context.awFileInGroups
                        where fl.fileGroupId.Equals(groupId)
                         && (
                            !search ||
                            search && (fl.awFile.title.ToLower().IndexOf(keywordSearch) >= 0)
                             )
                        orderby fl.awFile.lastBuildDate descending
                        select fl.awFile;

            if (files == null)
                return null;
            return files.ToList();
        }

        /// <summary>
        /// Returns group list associted with file
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public System.Collections.Generic.IList<awFileGroup> GetFileGroupListByFileId(long fileId)
        {
            var groups = from fg in _context.awFileInGroups
                         where fg.fileId.Equals(fileId)
                         orderby fg.awFileGroup.title
                         select fg.awFileGroup;
            if (groups == null)
                return null;
            return groups.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public System.Collections.Generic.IList<awFile> GetList(long siteId, string keywordSearch)
        {
            bool search = false;
            if (!String.IsNullOrEmpty(keywordSearch))
            {
                search = true;
                keywordSearch = keywordSearch.ToLower().Trim();
            }

            var files = from fl in _context.awFiles
                        where fl.siteId.Equals(siteId)
                         && (
                            !search ||
                            search && (fl.title.ToLower().IndexOf(keywordSearch) >= 0)
                             )
                        orderby fl.lastBuildDate descending
                        select fl;

            if (files == null)
                return null;
            return files.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public List<AWAPI_Data.CustomEntities.FileContentType> GetContentTypesByGroupId(long groupId)
        {
            var contentTypes = from f in _context.awFiles
                               join fg in _context.awFileInGroups on f.fileId equals fg.fileId
                               where fg.fileGroupId.Equals(groupId)
                               group f by f.contentType into g
                               orderby g.Key
                               select new AWAPI_Data.CustomEntities.FileContentType
                               {
                                   ContentType = g.Key
                               };
            if (contentTypes == null || contentTypes.ToList().Count == 0)
                return null;
            return contentTypes.ToList();
        }


        /// <summary>
        /// Add file with URL address
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="groupIds"></param>
        /// <param name="userId"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="fileUrl"></param>
        /// <param name="isEnabled"></param>
        /// <returns></returns>
        public long Add(long siteId, long[] groupIds, long userId, string title, string description,
                    string fileUrl, string thumbUrl, bool isEnabled)
        {
            return Add(siteId, 0, groupIds, userId, title, description, fileUrl, thumbUrl, isEnabled);
        }


        /// <summary>
        /// Add file with URL address
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="groupIds"></param>
        /// <param name="userId"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="fileUrl"></param>
        /// <param name="isEnabled"></param>
        /// <returns></returns>
        public long Add(long siteId, long fileId, long[] groupIds, long userId, string title, string description,
                    string fileUrl, string thumbUrl, bool isEnabled)
        {
            //Check if all the file groups exist---------------
            var groupList = from g in _context.awFileGroups
                            where groupIds.Contains(g.fileGroupId)
                            select g;

            if (groupList == null || groupList.Count() < groupIds.Length)
                throw new Exception("One or more file groups do not exist");

            //Save the file
            awFile file = new awFile();
            file.fileId = fileId > 0 ? fileId : MiscLibrary.CreateUniqueId();

            string resourceFileName = GetFileNameOnly(fileUrl);
            string extension = GetFileExension(resourceFileName);

            //SAVE THE FILE FIRST 
            try
            {
                file.contentType = GetContentType(AWAPI_BusinessLibrary.library.ConfigurationLibrary.Config.fileMimeXMLPath, extension);
                file.isOnLocal = false; //this also can be used for external urls (not only amazon)
                file.contentSize = 0;
                file.path = fileUrl.ToLower();
                file.siteId = siteId;
                file.title = String.IsNullOrEmpty(title) ? resourceFileName : title;
                file.description = description;
                file.link = fileUrl.ToLower();
                file.thumbnail = thumbUrl.ToLower();
                file.pubDate = DateTime.Now;
                file.lastBuildDate = DateTime.Now;
                file.isEnabled = isEnabled;
                file.userId = userId;
                file.createDate = DateTime.Now;

                _context.awFiles.InsertOnSubmit(file);
                _context.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //add file to groups
            AddFileToGroups(file.fileId, groupIds);
            return file.fileId;
        }





        /// <summary>
        /// Uploads and saves a file 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="groupIds"></param>
        /// <param name="userId"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="fileFullPath"></param>
        /// <param name="fileContent"></param>
        /// <param name="isEnabled"></param>
        /// <param name="size">if the file is an image and the size is set then the image will be resized</param>
        /// <returns></returns>
        public long Upload(long siteId, long[] groupIds, long userId, string title, string description,
                    string fileFullPath, System.IO.Stream fileStream, bool isEnabled, string size)
        {

            //throw error in case of validation problem
            ValidateToUpdateFile(siteId, userId, groupIds);

            //SAVE THE FILE ------------------------------------------------------
            awFile file = new awFile();
            file.fileId = MiscLibrary.CreateUniqueId();

            string resourceFileName = GetFileNameOnly(fileFullPath);
            string extension = GetFileExension(resourceFileName);
            bool isVideoFile = _videoLib.isVideoFile(extension);
            int contentLength = (int)fileStream.Length;
            string filePath = "";
            bool saveOnLocal = isVideoFile || !FileAmazonS3.SaveOnAmazonS3();
            SAVED_FILE savedFileInfo = new SAVED_FILE();

            //SAVE THE FILE FIRST 
            try
            {
                file.contentType = GetContentType(AWAPI_BusinessLibrary.library.ConfigurationLibrary.Config.fileMimeXMLPath, extension);

                filePath = CreateFilePath(saveOnLocal, siteId, file.fileId, extension);
                savedFileInfo = SaveFile(siteId, saveOnLocal, file.contentType, filePath, fileStream, size.Trim());

                file.isOnLocal = saveOnLocal;
                file.contentSize = contentLength;
                file.path = savedFileInfo.filePath.ToLower();
                file.siteId = siteId;
                file.title = String.IsNullOrEmpty(title) ? resourceFileName : title;
                file.description = description;
                file.link = GetUrl(file.fileId, savedFileInfo.filePath, "").ToLower();
                file.pubDate = DateTime.Now;
                file.lastBuildDate = DateTime.Now;
                file.isEnabled = isEnabled;
                file.userId = userId;
                file.createDate = DateTime.Now;

                _context.awFiles.InsertOnSubmit(file);
                _context.SubmitChanges();
            }
            catch (Exception ex)
            {
                DeleteFromStorage(siteId, saveOnLocal, filePath);
                throw ex;
            }

            //add file to groups
            AddFileToGroups(file.fileId, groupIds);

            //IF FILE IS A VIDEO FILE THEN CONVERT IT
            if (isVideoFile)
                IfVideoConvertToFLV(file.fileId);

            return file.fileId;
        }


        /// <summary>
        /// Updates awFile
        /// also, if there is a relation between file and awContestEntry, and the 
        /// file's original isEnabled=false, and new isEnabled= true, then enabled the awFile record
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="groupIds"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="fileFullPath"></param>
        /// <param name="fileContent"></param>
        /// <param name="isEnabled"></param>
        /// <param name="fileStream"></parparam>
        /// <param name="size"></param>
        public void Update(long fileId, long[] groupIds, string title, string description, string fileFullPath,
                        System.IO.Stream fileStream, bool? isEnabled, string size)
        {
            //GET THE FILE 
            fileFullPath = fileFullPath.ToLower();
            awFile fl = _context.awFiles.FirstOrDefault(f => f.fileId.Equals(fileId));
            if (fl == null)
                throw new Exception("File not found in the database.");


            //throw error in case of validation problem
            ValidateToUpdateFile(fl.siteId, fl.userId, groupIds);

            bool uploadFile = (fileStream != null && fileStream.Length > 0) ? true : false;
            string resourceFileName = GetFileNameOnly(fileFullPath);
            string newFilesExtension = GetFileExension(fileFullPath);
            int contentSize = uploadFile ? (int)fileStream.Length : 0;
            bool isVideoFile = _videoLib.isVideoFile(newFilesExtension);
            string pathToSave = isVideoFile ?
                            CreateFilePath(true, fl.siteId, fl.fileId, newFilesExtension) :
                            CreateFilePath(!FileAmazonS3.SaveOnAmazonS3(), fl.siteId, fileId, newFilesExtension);
            string contentType = GetContentType(AWAPI_BusinessLibrary.library.ConfigurationLibrary.Config.fileMimeXMLPath, newFilesExtension); ;

            fl.title = String.IsNullOrEmpty(title) ? resourceFileName : title;
            fl.description = description;

            if (uploadFile)
            {
                //delete the current file 
                DeleteFromStorage(fl.siteId, fl.isOnLocal, fl.path);

                if (!String.IsNullOrEmpty(fl.thumbnail))
                    DeleteFromStorage(fl.siteId, fl.isOnLocal, fl.thumbnail);

                //delete the thumbs
                if (fl.isOnLocal)
                    DeleteThumbs(fl.fileId, fl.path);

                //set the path
                //if video file, first it will saved to the local, 
                //then will be saved to the right location (amazon or local) after the conversion
                fl.isOnLocal = isVideoFile ? true : !FileAmazonS3.SaveOnAmazonS3();
                fl.path = pathToSave;
                fl.link = GetUrl(fl.fileId, pathToSave, "");
                fl.thumbnail = "";
                fl.contentType = contentType;
                fl.contentSize = contentSize;
            }
            fl.lastBuildDate = DateTime.Now;

            //if tehere is a connection with contest,
            bool enableContestEntry = !fl.isEnabled && (isEnabled!=null && isEnabled.Value);

            fl.isEnabled = isEnabled == null ? fl.isEnabled : isEnabled.Value;
            _context.SubmitChanges();

            //add file to groups
            if (groupIds != null)
                AddFileToGroups(fileId, groupIds);

            //If new file is uploaded update the new file...
            if (uploadFile)
            {

                if (!isVideoFile)
                    SaveFile(fl.siteId, !FileAmazonS3.SaveOnAmazonS3(), contentType, pathToSave, fileStream, size);
                else
                {
                    //if video, then save on the local first
                    SaveFile(fl.siteId, true, contentType, pathToSave, fileStream, "");
                    IfVideoConvertToFLV(fileId);
                }
            }

            if (enableContestEntry)
            {
                ContestLibrary lib = new ContestLibrary();
                lib.UpdateContestEntriesStatusesByFileId (fileId, true);
            }
        }


        /// <summary>
        /// Checks if the group ids exist in the site
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="groupIds"></param>
        /// <returns></returns>
        bool GroupIdsExist(long siteId, long[] groupIds)
        {
            //Check if all the file groups exist---------------
            var groupList = from g in _context.awFileGroups
                            where groupIds.Contains(g.fileGroupId) && g.siteId.Equals(siteId)
                            select g;

            if (groupList == null || groupList.Count() < groupIds.Length)
                return false;

            return true;
        }

        public string CreateFilePath(bool isOnLocal, long siteId, long fileId, string extension)
        {
            string filePath = "";
            //if it is a video don't save it to amazon now
            if (!isOnLocal)
            {
                filePath = new FileAmazonS3(siteId).BaseURL + String.Format("{0}/{1}.{2}", siteId, fileId, extension);
            }
            else
            {
                string directory = String.Format("{0}{1}\\", AWAPI_BusinessLibrary.library.ConfigurationLibrary.Config.fileDirectory, siteId);
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                filePath = directory + fileId + "." + extension;
            }
            return filePath.ToLower();
        }

        /// <summary>
        /// If file is video, then convert it to FLV, 
        /// delete the original file, update the database 
        /// with new flv's attributes
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns>Returns the file's full path</returns>
        void IfVideoConvertToFLV(long fileId)
        {
            try
            {

                //Get the file from DB
                awFile file = (from l in _context.awFiles
                               where l.fileId.Equals(fileId)
                               select l).FirstOrDefault();
                if (file == null)
                    return;

                string extension = GetFileExension(file.path);
                string path = file.path;

                //Only Video Files, (flv won't be converted)
                if (!_videoLib.isVideoFile(extension))
                    return;

                //CONVERT and GET THE NEW FILES LOCATION
                OutputPackage videoPackage = _videoLib.ConvertToFLV(path);

                //GET THE NEW FILES INFO
                file.contentSize = videoPackage.Length;
                file.contentType = GetContentType(AWAPI_BusinessLibrary.library.ConfigurationLibrary.Config.fileMimeXMLPath, "flv");

                SAVED_FILE savedFile = new SAVED_FILE();
                if (FileAmazonS3.SaveOnAmazonS3())
                {
                    //load the flv file into byte array, and save it to amazon s3
                    FileStream fileStream = new FileStream(videoPackage.Path, System.IO.FileMode.Open);

                    //SAVE THE VIDEO FILE
                    string keyName = String.Format("{0}/{1}.flv", file.siteId, file.fileId);

                    savedFile = SaveFile(file.siteId, false, file.contentType, keyName, fileStream, "");
                    System.IO.File.Delete(videoPackage.Path);
                    file.path = savedFile.filePath;

                    //SAVE THE THUMB
                    if (videoPackage.PreviewImage != null && !String.IsNullOrEmpty(videoPackage.PreviewImagePath) &&
                        System.IO.File.Exists(videoPackage.PreviewImagePath))
                    {
                        string thumbFile = String.Format("{0}/{1}.jpg", file.siteId, file.fileId);
                        FileStream videoPreviewStream = new FileStream(videoPackage.PreviewImagePath, System.IO.FileMode.Open);
                        savedFile = SaveFile(file.siteId, false, "image/jpeg", thumbFile, videoPreviewStream, "");
                        file.thumbnail = savedFile.filePath;

                        if (System.IO.File.Exists(videoPackage.PreviewImagePath))
                            System.IO.File.Delete(videoPackage.PreviewImagePath);
                    }
                    file.isOnLocal = false;
                }
                else
                {
                    file.isOnLocal = true;
                    file.path = videoPackage.Path;
                    file.thumbnail = videoPackage.PreviewImagePath;
                }

                _context.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        /// <summary>
        /// Images' different sizes are saved as fileId_34x50.jpg... 
        ///             //Amazon does not support image resizing
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="fileAndPath"></param>
        public void DeleteThumbs(long fileId, string fileAndPath)
        {
            string pathOnly = fileAndPath.Replace(GetFileNameOnly(fileAndPath), "");

            if (!System.IO.Directory.Exists(pathOnly))
                return;
            string[] files = System.IO.Directory.GetFiles(pathOnly);
            foreach (string file in files)
            {
                string tmp = String.Format("{0}{1}", pathOnly, fileId);
                if (file.ToLower().IndexOf(tmp.ToLower()) >= 0)
                    System.IO.File.Delete(file);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public void AddFileToGroups(long fileId, long[] groupIds)
        {
            //delete file group association
            var tmpList = from fg in _context.awFileInGroups
                          where fg.fileId.Equals(fileId)
                          select fg;
            _context.awFileInGroups.DeleteAllOnSubmit(tmpList);

            foreach (long groupId in groupIds)
            {
                long fileInGroupId = MiscLibrary.CreateUniqueId();

                awFileInGroup file = new awFileInGroup();
                file.fileInGroupId = fileInGroupId;
                file.fileGroupId = groupId;
                file.fileId = fileId;
                file.createDate = DateTime.Now;

                _context.awFileInGroups.InsertOnSubmit(file);
            }
            _context.SubmitChanges();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileId"></param>
        public void Delete(long fileId)
        {
            if (fileId <= 0)
                return;

            new AWAPI_BusinessLibrary.library.BlogLibrary().RemoveFileBlogPostRelationByFileId(fileId);

            //Delete contest entry relation
            new AWAPI_BusinessLibrary.library.ContestLibrary().DeleteContestEntriesByFileId(fileId);

            //delete filegroup relation
            DeleteFileGroupRelationByFileId(fileId);

            var files = from f in _context.awFiles
                        where f.fileId.Equals(fileId)
                        select f;

            if (files == null || files.Count() == 0)
                return;

            awFile file = files.First();

            _context.awFiles.DeleteOnSubmit(file);
            _context.SubmitChanges();


            if (file.path != null)
            {
                DeleteFromStorage(file.siteId, file.isOnLocal, file.path);
                if (!String.IsNullOrEmpty(file.thumbnail))
                    DeleteFromStorage(file.siteId, file.isOnLocal, file.thumbnail);

                //delete the thumbs
                if (file.isOnLocal)
                    DeleteThumbs(file.fileId, file.path);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">userId to be deleted</param>
        /// <param name="currentUserId">Who is deleting the records, will own the files</param>
        public void RemoveFileUserRelation(long userId, long currentUserId)
        {
            var contestEntries = from l in _context.awFiles
                                 where l.userId.Equals(userId)
                                 select l;
            foreach (awFile entry in contestEntries)
                entry.userId = currentUserId;


            _context.SubmitChanges();
        }


        #endregion FILES

        #region FILE GROUPS
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public awFileGroup GetFileGroup(long id)
        {
            var grp = _context.GetTable<awFileGroup>().
                        Where(f => f.fileGroupId.Equals(id));

            if (grp == null || grp.ToList().Count() == 0)
                return null;
            return grp.First();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public awFileGroup GetFileGroup(long siteId, string groupTitle)
        {
            var grp = _context.GetTable<awFileGroup>().
                        Where(
                            f => f.title.ToLower().Equals(groupTitle) && f.siteId.Equals(siteId));

            if (grp == null || grp.ToList().Count() == 0)
                return null;
            return grp.First();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public IEnumerable<awFileGroup> GetFileGroupListBySiteId(long siteId)
        {
            var files = _context.GetTable<awFileGroup>()
                        .Where(f => f.siteId.Equals(siteId))
                        .OrderBy(f => f.title);
            return files;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="pubDate"></param>
        /// <returns></returns>
        public long AddFileGroup(long siteId, long userId,
                        string title, string description, DateTime? pubDate)
        {
            long id = AWAPI_Common.library.MiscLibrary.CreateUniqueId();
            awFileGroup flGrp = new awFileGroup();

            flGrp.fileGroupId = id;
            flGrp.siteId = siteId;
            flGrp.title = title;
            flGrp.description = description;
            flGrp.pubDate = pubDate == null ? DateTime.Now : pubDate;
            flGrp.lastBuildDate = DateTime.Now;
            flGrp.userId = userId;
            flGrp.createDate = DateTime.Now;

            _context.awFileGroups.InsertOnSubmit(flGrp);
            _context.SubmitChanges();

            return id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool UpdateFileGroup(long fileGroupId, string title, string description)
        {
            awFileGroup grp = _context.awFileGroups.FirstOrDefault(st => st.fileGroupId.Equals(fileGroupId));

            if (grp == null)
                return false;

            grp.title = title;
            grp.description = description;
            grp.lastBuildDate = DateTime.Now;

            _context.SubmitChanges();
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileGroupId"></param>
        public void DeleteFileGroup(long fileGroupId)
        {
            if (fileGroupId <= 0)
                return;

            //delete group - file relations
            DeleteFileGroupRelationByGroupId(fileGroupId);

            //delete group
            var groups = from g in _context.awFileGroups
                         where g.fileGroupId.Equals(fileGroupId)
                         select g;

            _context.awFileGroups.DeleteAllOnSubmit(groups);
            _context.SubmitChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileGroupId"></param>
        public void DeleteFileGroupRelationByGroupId(long fileGroupId)
        {
            if (fileGroupId <= 0)
                return;

            var groups = from g in _context.awFileInGroups
                         where g.fileGroupId.Equals(fileGroupId)
                         select g;

            _context.awFileInGroups.DeleteAllOnSubmit(groups);
            _context.SubmitChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileGroupId"></param>
        public void DeleteFileGroupRelationByFileId(long fileId)
        {
            if (fileId <= 0)
                return;

            var groups = from g in _context.awFileInGroups
                         where g.fileId.Equals(fileId)
                         select g;

            _context.awFileInGroups.DeleteAllOnSubmit(groups);
            _context.SubmitChanges();
        }
        #endregion

        #region HELPER METHODS

        /// <summary>
        /// Validates entries to upload/add/update file
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="userId"></param>
        /// <param name="groupIds"></param>
        /// <returns></returns>
        bool ValidateToUpdateFile(long siteId, long? userId, long[] groupIds)
        {
            //GET/CHECK THE SITE 
            awSite site = new SiteLibrary().Get(siteId);
            if (site == null || !site.isEnabled)
                throw new Exception("Site does not exists or disabled.");

            if (userId == null || userId.Value == 0)
                throw new Exception("User does not exists or disabled.");

            //GET/CHECK USER
            AWAPI_Data.CustomEntities.UserExtended user = new UserLibrary().Get(userId.Value);
            if (user == null || !user.isEnabled)
                throw new Exception("User does not exists or disabled.");


            //IF GROUP ID(s) IS SET CHECK IF THE GROUP DOES EXIST ----------------
            if (groupIds != null && groupIds.Length > 0)
                if (GroupIdsExist(siteId, groupIds) == false)
                    throw new Exception("One or more file groups do not exist");

            return true;
        }



        //public static string CreateFileDirectory(string root1, string root2, string fileID, string fileNameAndPath)
        //{
        //    int directoryLevel = 0;
        //    string rtn = "";

        //    string resourcePath = AWAPI_FilePath;

        //    if (fileID.Length < directoryLevel)
        //        directoryLevel = fileID.Length;

        //    string subFolder = "";
        //    for (int i = 0; i < directoryLevel; i++)
        //        subFolder += fileID[i] + "\\";

        //    if (!resourcePath.EndsWith("\\"))
        //        resourcePath += "\\";

        //    if (root1.Trim().Length > 0)
        //        resourcePath += root1 + "\\";

        //    if (root2.Trim().Length > 0)
        //        resourcePath += root2 + "\\";

        //    rtn = resourcePath + subFolder;

        //    if (!Directory.Exists(rtn))
        //        Directory.CreateDirectory(rtn);

        //    return rtn;
        //}

        /// <summary>
        /// Returns file's extension
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetFileExension(string fileName)
        {
            string[] parts = fileName.Split('.');
            if (parts.Length > 1)
                return parts[parts.Length - 1];
            return "";
        }


        /// <summary>
        /// Returns filename part from path/url
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>
        public static string GetFileNameOnly(string strPath)
        {
            if (strPath.Trim().Length == 0)
                return "";

            string[] arr = strPath.Split('\\');

            if (arr.Length == 0)
                return "";

            return arr[arr.Length - 1];
        }

        #region SAVE FILE
        public struct SAVED_FILE
        {
            public string filePath;
            public int fileSize;
        }



        public static byte[] StringToByteArray(string content)
        {
            byte[] bytes;
            try
            {
                bytes = Convert.FromBase64String(content);
            }
            catch
            {
                bytes = System.Text.ASCIIEncoding.Unicode.GetBytes(content);
            }
            return bytes;
        }



        #endregion
        #endregion




        #region MIME Functions
        /// <summary>
        /// Returns MIME from the registery...
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static string GetContentTypeFromRegistry(string extension)
        {
            string strRtn = "";
            string ext = extension.ToLower();

            if (!ext.StartsWith("."))
                ext = "." + ext;

            RegistryKey rk = Registry.ClassesRoot.OpenSubKey(ext);

            if (rk != null)
            {
                try
                {
                    if (rk.GetValue("Content Type") != null)
                    {
                        strRtn = rk.GetValue("Content Type").ToString();
                    }
                }
                catch (Exception ex)
                {
                    Console.Write("ERROR: GetContentTypeFromRegistry, " + ex.Message);
                }
                finally
                { rk.Close(); }
            }

            return strRtn;
        }

        /// <summary>
        /// Inserts content types into the specified XML file
        /// </summary>
        /// <param name="XMLPath"></param>
        /// <returns></returns>
        public static string FillXMLWithContentTypes(string XMLPath)
        {
            System.IO.TextWriter tr = new System.IO.StreamWriter(XMLPath);

            StringBuilder sb = new StringBuilder("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            sb.Append("<MIME>");

            RegistryKey key = Registry.ClassesRoot.OpenSubKey("MIME\\Database\\Content Type");

            foreach (string keyName in key.GetSubKeyNames())
            {
                RegistryKey temp = key.OpenSubKey(keyName);
                string ext = "";
                if (temp.GetValue("Extension") != null)
                    ext = temp.GetValue("Extension").ToString();

                if (ext.Trim().Length == 0)
                    continue;
                sb.Append("<content>");
                sb.Append("<type>" + keyName + "</type>");
                sb.Append("<extension>" + ext + "</extension>");
                sb.Append("</content>");
            }
            sb.Append("</MIME>");

            try
            {
                tr.Write(sb.ToString());
            }
            catch (Exception ex)
            {
                Console.Write("ERROR: GetContentTypeFromRegistry, " + ex.Message);
            }
            finally
            {
                tr.Close();
            }
            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetContentType(string filePath)
        {
            string extension = GetFileExension(filePath);
            return GetContentType(AWAPI_BusinessLibrary.library.ConfigurationLibrary.Config.fileMimeXMLPath, extension);
        }


        /// <summary>
        /// Returns ContentType from the XML file
        /// </summary>
        /// <param name="XMLPath"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static string GetContentType(string XMLPath, string extension)
        {
            DataSet ds = new DataSet();

            string ext = extension.ToLower();

            if (!ext.StartsWith("."))
                ext = "." + ext;

            try
            {
                string xmlFullPath = HttpContext.Current.Server.MapPath(XMLPath);
                if (!System.IO.File.Exists(xmlFullPath))
                    return "";
                ds.ReadXml(xmlFullPath);

                foreach (DataRow dr in ds.Tables["content"].Rows)
                    if (dr["extension"] != null && dr["extension"].ToString().ToLower() == ext && dr["type"] != null)
                        return dr["type"].ToString().ToLower();
            }
            catch (Exception)
            {

            }
            return "";
        }
        #endregion MIME Functions
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="filePath"></param>
        ///// <param name="fileContent"></param>
        //public static SAVED_FILE SaveFile(bool saveToAmazonS3, string contentType, string filePath, System.IO.Stream fileStream)
        //{
        //    //byte[] bytes = StringToByteArray(fileStream);
        //    return SaveFile(saveToAmazonS3, contentType, filePath, bytes);
        //}

        /// <summary>
        /// Saves file to local or Amazon S3
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="saveToLocal"></param>
        /// <param name="contentType"></param>
        /// <param name="filePath"></param>
        /// <param name="fileStream"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static SAVED_FILE SaveFile(long siteId, bool saveToLocal, string contentType, string filePath, System.IO.Stream fileStream, string size)
        {
            SAVED_FILE file = new SAVED_FILE();
            System.IO.Stream contentStream = new System.IO.MemoryStream();
            filePath = filePath.ToLower();
            file.fileSize = (int)fileStream.Length;

            try
            {
                Byte[] buffer;

                if (!String.IsNullOrEmpty(size) && contentType.ToLower().IndexOf("image")>=0)
                {
                    buffer = AWAPI_Common.library.ImageLibrary.ResizeImage("", fileStream, false, 0, 0, size, false);
                    fileStream = new MemoryStream(buffer);
                }
                if (saveToLocal)
                {
                    int length = 1024;
                    int bytesRead = 0;
                    buffer = new Byte[length];
                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        do
                        {
                            bytesRead = fileStream.Read(buffer, 0, length);
                            fs.Write(buffer, 0, bytesRead);
                        }
                        while (bytesRead == length);
                    }
                    if (!System.IO.File.Exists(filePath))
                        throw new Exception("File could not be saved.");

                    file.filePath = filePath;
                    //fileStream.Dispose();
                }
                else
                {
                    FileAmazonS3 s3 = new FileAmazonS3(siteId);
                    file.filePath = s3.Upload(filePath, fileStream, contentType);
                }
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
            return file;
        }

        /// <summary>
        /// Delete file from amazon or local
        /// </summary>
        /// <param name="isOnLocal"></param>
        /// <param name="filePath"></param>
        public void DeleteFromStorage(long siteId, bool isOnLocal, string filePath)
        {
            if (!isOnLocal)
            {
                new FileAmazonS3(siteId).Delete(filePath);
            }
            else
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
        }

        #region COMMON FOR AMAZON AND LOCAL

        public static string GetUrl(string url, string size)
        {
            url = url.ToLower();
            if ((url.IndexOf("http://") == 0 || url.IndexOf("https://") == 0) && url.IndexOf("/handler/") < 0)
                return url;


            string serviceUrl = AWAPI_BusinessLibrary.library.ConfigurationLibrary.Config.fileServiceUrl;
            if (size.Trim() != "")
                url += "&size=" + size;

            return url;
        }

        /// <summary>
        /// Returns full url to call a file (or image with size)
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="serviceUrl"></param>
        /// <param name="path"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string GetUrl(long fileId, string path, string size)
        {
            path = path.ToLower();
            if ((path.IndexOf("http://") == 0 || path.IndexOf("https://") == 0) && path.IndexOf("/handler/") < 0)
                return path;

            string serviceUrl = AWAPI_BusinessLibrary.library.ConfigurationLibrary.Config.fileServiceUrl;
            string url = String.Format("{0}?id={1}",
                            serviceUrl, fileId);
            if (size.Trim() != "")
                url += "&size=" + size;

            return url;
        }

        #endregion
    }
}
