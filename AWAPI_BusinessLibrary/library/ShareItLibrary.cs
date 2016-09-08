using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using AWAPI_Data.Data;
using AWAPI_Common.library;

namespace AWAPI_BusinessLibrary.library
{
    public class ShareItLibrary
    {
        ShareItContextDataContext _context = new ShareItContextDataContext();

        #region ShareIt
        /// <summary>
        /// 
        /// </summary>
        /// <param name="shareItId"></param>
        /// <returns></returns>
        public awShareIt Get(long shareItId)
        {
            if (shareItId <= 0)
                return null;
            var shareIt = _context.GetTable<awShareIt>()
                        .Where(st => st.shareItId.Equals(shareItId));

            if (shareIt == null && shareIt.ToList().Count() == 0)
                return null;
            return shareIt.First();
        }

        /// <summary>
        /// Returns user's shares
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public System.Collections.Generic.IEnumerable<awShareIt> GetList(long siteId, long userId)
        {
            var list = from l in _context.awShareIts
                       where l.siteId.Equals(siteId) &&
                            l.userId.Equals(userId)
                       select l;

            if (list == null)
                return null;

            return list.ToList<awShareIt>();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="shareItId"></param>
        public void Delete(long shareItId)
        {
            if (shareItId <= 0)
                return;

            //Delete the shareItWith
            var shareItWith = from l in _context.awShareItWiths
                              where l.shareItId.Equals(shareItId)
                              select l;
            _context.awShareItWiths.DeleteAllOnSubmit(shareItWith);

            //delete the shareit
            var shareIts = from l in _context.awShareIts
                           where l.shareItId.Equals(shareItId)
                           select l;

            _context.awShareIts.DeleteAllOnSubmit(shareIts);
            _context.SubmitChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="userId"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="link"></param>
        /// <param name="shareWithEveryone"></param>
        /// <param name="shareWithUserIds"></param>
        /// <returns></returns>
        public long Add(long siteId, long userId, string title, string description, string link, bool shareWithEveryone, long[] shareWithUserIds)
        {
            awShareIt shareIt = new awShareIt();
            shareIt.shareItId = AWAPI_Common.library.MiscLibrary.CreateUniqueId();

            shareIt.siteId = siteId;
            shareIt.title = title;
            shareIt.description = description;
            shareIt.shareWithEveryone = shareWithEveryone;
            shareIt.link = link;
            shareIt.userId = userId;

            shareIt.createDate = DateTime.Now;

            _context.awShareIts.InsertOnSubmit(shareIt);
            _context.SubmitChanges();

            AddUsersToShareIt(shareIt.shareItId, shareWithUserIds);

            return shareIt.shareItId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shareItId"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public void Update(long shareItId, string title, string description, string link, bool shareWithEveryone, long[] shareWithUserIds)
        {
            awShareIt shareIt = _context.awShareIts.FirstOrDefault(st => st.shareItId.Equals(shareItId));

            if (shareIt == null)
                return;

            shareIt.title = title;
            shareIt.description = description;
            shareIt.shareWithEveryone = shareWithEveryone;
            shareIt.link = link;

            _context.SubmitChanges();

            AddUsersToShareIt(shareItId, shareWithUserIds);

        }
        #endregion

        #region ShareIt With  -------------------------------------------

        /// <summary>
        /// Removes user from shareitWith
        /// </summary>
        /// <param name="shareItId"></param>
        /// <param name="userId"></param>
        public void RemoveUserFromShareIt (long shareItId, long userId)
        {
            awShareItWith shareItWith = _context.awShareItWiths.
                                        FirstOrDefault(s => s.shareItId.Equals(shareItId) && s.userId.Equals(userId));

            if (shareItWith == null)
                return;

            _context.awShareItWiths.DeleteOnSubmit(shareItWith);
            _context.SubmitChanges();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="shareItIds"></param>
        /// <param name="blogId"></param>
        public void AddUsersToShareIt(long shareItId, long[] userIds)
        {
            //Delete the users first 
            var alreadyExist = from l in _context.awShareItWiths
                               where l.shareItId.Equals(shareItId)
                               select l;
            if (alreadyExist != null && alreadyExist.Count() > 0)
                _context.awShareItWiths.DeleteAllOnSubmit(alreadyExist);

            foreach (long uId in userIds)
            {
                awShareItWith sw = new awShareItWith();
                sw.shareItWithId = AWAPI_Common.library.MiscLibrary.CreateUniqueId();
                sw.shareItId = shareItId;
                sw.userId = uId;
                sw.createDate = DateTime.Now;
                _context.awShareItWiths.InsertOnSubmit(sw);
            }

            _context.SubmitChanges();

        }

        /// <summary>
        /// REturn list of the users who share..
        /// </summary>
        /// <param name="shareItId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IList<awUser_ShareIt> GetSharedWithUserList(long shareItId)
        {
            var list = from l in _context.awShareItWiths
                       where l.shareItId.Equals(shareItId) 
                       select l.awUser_ShareIt;
            if (list == null)
                return null;

            return list.ToList<awUser_ShareIt>();
        }

        /// <summary>
        /// REturn list of the users who share..
        /// </summary>
        /// <param name="shareItId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IList<AWAPI_Data.CustomEntities.ShareItExtended> GetUsersShareIts(long siteId, long userId)
        {
            var list = from l in _context.awShareItWiths
                       where (l.awShareIt.siteId.Equals(siteId) && l.userId.Equals(userId)) ||
                             (l.awShareIt.shareWithEveryone.Equals(true) && l.awShareIt.siteId.Equals(siteId))
                       orderby l.awShareIt.createDate descending
                       select new AWAPI_Data.CustomEntities.ShareItExtended
                       {
                           shareItId = l.awShareIt.shareItId,
                           title = l.awShareIt.title,
                           description = l.awShareIt.description,
                           link = l.awShareIt.link,
                           shareWithEveryone = l.awShareIt.shareWithEveryone,
                           userId = l.awShareIt.userId,
                           firstName = l.awShareIt.awUser_ShareIt.firstName,
                           lastName = l.awShareIt.awUser_ShareIt.lastName,
                           email = l.awShareIt.awUser_ShareIt.email,
                           userImageUrl = l.awShareIt.awUser_ShareIt.imageurl
                       };

            if (list == null)
                return null;

            return list.ToList<AWAPI_Data.CustomEntities.ShareItExtended>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        public void DeleteUserFromShareIt(long userId)
        {
            if (userId <= 0)
                return;

            //Delete the shareItWith
            var shareItWith = from l in _context.awShareItWiths
                              where l.userId.Equals(userId)
                              select l;
            _context.awShareItWiths.DeleteAllOnSubmit(shareItWith);
            _context.SubmitChanges();
        }



        #endregion

    }
}
