using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using AWAPI_Data.Data;
using AWAPI_Data.CustomEntities;
using AWAPI_Common.library;

namespace AWAPI_BusinessLibrary.library
{
    public class ContestLibrary
    {
        ContestContextDataContext _context = new ContestContextDataContext();

        #region contest ---

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contestGroupId"></param>
        /// <returns></returns>
        public ContestExtended GetContest(long contestId, bool onlyEnabled)
        {
            DateTime now = DateTime.Now;

            if (contestId <= 0)
                return null;
            var list = from l in _context.awContests
                       where l.contestId.Equals(contestId) &&
                          (!onlyEnabled ||
                           (
                             onlyEnabled &&
                             l.awSite_Contest.isEnabled &&
                             l.isEnabled &&
                             (l.pubDate.Equals(null) || (l.pubDate != null && l.pubDate <= now)) &&
                             (l.pubEndDate.Equals(null) || (l.pubEndDate != null && l.pubEndDate > now))
                             )
                          )
                       select new ContestExtended
                       {
                           contestId = l.contestId,
                           title = l.title,
                           description = l.description,
                           isEnabled = l.isEnabled,
                           siteId = l.siteId,
                           maxEntry = l.maxEntry,
                           userId = l.userId,
                           numberOfWinners = l.numberOfWinners,
                           entryPerUser = l.entryPerUser,
                           entryPerUserPeriodType = l.entryPerUserPeriodType,
                           entryPerUserPeriodValue = l.entryPerUserPeriodValue,
                           pubDate = l.pubDate,
                           pubEndDate = l.pubEndDate,
                           lastBuildDate = l.lastBuildDate,
                           createDate = l.createDate,
                           sendEmailToModeratorTemplateId = l.sendEmailToModeratorTemplateId,
                           sendEmailToModeratorRecipes = l.sendEmailToModeratorRecipes,
                           sendEmailAfterSubmissionTemplateId = l.sendEmailAfterSubmissionTemplateId,
                           sendEmailAfterApproveEntryTemplateId = l.sendEmailAfterApproveEntryTemplateId,
                           sendEmailAfterDeleteEntryTemplateId = l.sendEmailAfterDeleteEntryTemplateId
                       };


            if (list == null && list.ToList().Count() == 0)
                return null;
            return list.FirstOrDefault();
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public System.Collections.Generic.IList<ContestExtended> GetContestList(long siteId, bool onlyEnabled)
        {
            DateTime now = DateTime.Now;
            var contests = from l in _context.awContests
                           where l.awSite_Contest.siteId.Equals(siteId) &&
                          (!onlyEnabled ||
                           (
                             onlyEnabled &&
                             l.awSite_Contest.isEnabled &&
                             l.isEnabled &&
                             (l.pubDate.Equals(null) || (l.pubDate != null && l.pubDate <= now)) &&
                             (l.pubEndDate.Equals(null) || (l.pubEndDate != null && l.pubEndDate > now))
                             )
                          )
                           orderby l.pubDate descending
                           select new ContestExtended
                           {
                               contestId = l.contestId,
                               title = l.title,
                               description = l.description,
                               isEnabled = l.isEnabled,
                               siteId = l.siteId,
                               maxEntry = l.maxEntry,
                               userId = l.userId,
                               numberOfWinners = l.numberOfWinners,
                               entryPerUser = l.entryPerUser,
                               entryPerUserPeriodType = l.entryPerUserPeriodType,
                               entryPerUserPeriodValue = l.entryPerUserPeriodValue,
                               pubDate = l.pubDate,
                               pubEndDate = l.pubEndDate,
                               lastBuildDate = l.lastBuildDate,
                               createDate = l.createDate,
                               sendEmailToModeratorTemplateId = l.sendEmailToModeratorTemplateId,
                               sendEmailToModeratorRecipes = l.sendEmailToModeratorRecipes,
                               sendEmailAfterSubmissionTemplateId = l.sendEmailAfterSubmissionTemplateId,
                               sendEmailAfterApproveEntryTemplateId = l.sendEmailAfterApproveEntryTemplateId,
                               sendEmailAfterDeleteEntryTemplateId = l.sendEmailAfterDeleteEntryTemplateId
                           };

            if (contests == null)
                return null;

            return contests.ToList<ContestExtended>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="groupId"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public System.Collections.Generic.IList<ContestExtended> GetContestListByGroupId(long siteId, long groupId, bool onlyEnabled)
        {
            DateTime now = DateTime.Now;
            var list = from l in _context.awContests
                       join gm in _context.awContestGroupMembers on l.contestId equals gm.contestId
                       where gm.contestGroupId.Equals(groupId) &&
                            l.siteId.Equals(siteId) &&
                           (!onlyEnabled ||
                           (
                             onlyEnabled &&
                             gm.awContestGroup.isEnabled &&
                             l.awSite_Contest.isEnabled &&
                             l.isEnabled &&
                             (l.pubDate.Equals(null) || (l.pubDate != null && l.pubDate <= now)) &&
                             (l.pubEndDate.Equals(null) || (l.pubEndDate != null && l.pubEndDate > now))
                             )
                          )
                       orderby l.pubDate descending
                       select new ContestExtended
                       {
                           contestId = l.contestId,
                           title = l.title,
                           description = l.description,
                           isEnabled = l.isEnabled,
                           siteId = l.siteId,
                           maxEntry = l.maxEntry,
                           userId = l.userId,
                           numberOfWinners = l.numberOfWinners,
                           entryPerUser = l.entryPerUser,
                           entryPerUserPeriodType = l.entryPerUserPeriodType,
                           entryPerUserPeriodValue = l.entryPerUserPeriodValue,
                           pubDate = l.pubDate,
                           pubEndDate = l.pubEndDate,
                           lastBuildDate = l.lastBuildDate,
                           createDate = l.createDate,
                           sendEmailToModeratorTemplateId = l.sendEmailToModeratorTemplateId,
                           sendEmailToModeratorRecipes = l.sendEmailToModeratorRecipes,
                           sendEmailAfterSubmissionTemplateId = l.sendEmailAfterSubmissionTemplateId,
                           sendEmailAfterApproveEntryTemplateId = l.sendEmailAfterApproveEntryTemplateId,
                           sendEmailAfterDeleteEntryTemplateId = l.sendEmailAfterDeleteEntryTemplateId
                       };

            if (list == null)
                return null;

            return list.ToList<ContestExtended>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string ShortenDateTime(DateTime? dt)
        {
            if (dt == null)
                return "";

            return dt.Value.ToString("yyyy-MM-dd");

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contestId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IList<AWAPI_Data.CustomEntities.ContestEntryDailyReport> GetContestEntryDailyReport(long contestId, long? userId)
        {
            AWAPI_Data.Data.ContestContextDataContext _context = new ContestContextDataContext();
            if (userId == 0)
                userId = null;

            var list = from l in _context.awContestEntries
                       where
                           (l.contestId.Equals(contestId)
                            && ((userId == null || userId != null && l.userId != null && l.userId.Value.Equals(userId)))
                            && l.isEnabled
                       )
                       &&
                        (
                            l.awFile_Contest == null || l.awFile_Contest.isEnabled == true

                        )
                       select l;

            var groupedList = from l in list
                              where (l.contestId.Equals(contestId)
                                     && (
                                     (
                                        userId == null ||
                                        userId != null && l.userId.Equals(userId)))
                                     )
                              group l by new
                              {
                                  contestId = l.contestId,
                                  contestTitle = l.title,
                                  contestPubDate = l.awContest.pubDate == null ? l.awContest.createDate : l.awContest.pubDate,
                                  contentType = l.awFile_Contest == null ? "" : l.awFile_Contest.contentType,
                                  entryDay = new DateTime(l.createDate.Year, l.createDate.Month, l.createDate.Day)
                              }
                                  into d
                                  orderby d.Key.entryDay descending, d.Key.contentType
                                  select new AWAPI_Data.CustomEntities.ContestEntryDailyReport
                                  {
                                      contestId = d.Key.contestId,
                                      contestTitle = d.Key.contestTitle,
                                      contestPubDate = d.Key.contestPubDate,
                                      contentType = d.Key.contentType,
                                      entryDay = d.Key.entryDay,
                                      firstFileId = (from l2 in list
                                                     where l2.awFile_Contest != null && new DateTime(l2.createDate.Year, l2.createDate.Month, l2.createDate.Day) == d.Key.entryDay
                                                     select l2.fileId).FirstOrDefault(),
                                      firstFileUrl = (from l2 in list
                                                      where l2.awFile_Contest != null && new DateTime(l2.createDate.Year, l2.createDate.Month, l2.createDate.Day) == d.Key.entryDay
                                                      select l2.fileUrl).FirstOrDefault(),
                                      numberOfEntries = d.Count()
                                  };

            if (groupedList == null)
                return null;

            return groupedList.ToList();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contestGroupId"></param>
        public void DeleteContest(long contestId)
        {
            if (contestId <= 0)
                return;

            //Delete the winners
            var winners = from w in _context.awContestWinners
                          where w.contestId.Equals(contestId)
                          select w;
            _context.awContestWinners.DeleteAllOnSubmit(winners);

            //delete the group relation
            var grprel = from gm in _context.awContestGroupMembers
                         where gm.contestId.Equals(contestId)
                         select gm;
            _context.awContestGroupMembers.DeleteAllOnSubmit(grprel);


            //Delete group entries
            var entries = from c in _context.awContestEntries
                          where c.contestId.Equals(contestId)
                          select c;
            _context.awContestEntries.DeleteAllOnSubmit(entries);


            //Delete contests
            var contests = from p in _context.awContests
                           where p.contestId.Equals(contestId)
                           select p;
            _context.awContests.DeleteAllOnSubmit(contests);

            //SUBMIT CHANGES -------------------------------------------------------------
            _context.SubmitChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="userId"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="isEnabled"></param>
        /// <param name="maxEntry"></param>
        /// <param name="maxEntryPerUser"></param>
        /// <param name="maxEntryPerUserPeriodValue"></param>
        /// <param name="maxEntryPerUserPeriodType"></param>
        /// <param name="numberOfWinners"></param>
        /// <param name="pubDate"></param>
        /// <param name="pubEndDate"></param>
        /// <param name="sendEmailToModeratorRecipes"></param>
        /// <param name="sendEmailToModeratorTemplateId"></param>
        /// <param name="sendEmailAfterApproveEntryTemplateId"></param>
        /// <param name="sendEmailAfterDeleteEntryTemplateId"></param>
        /// <returns></returns>
        public long AddContest(long siteId, long userId,
                        string title, string description, bool isEnabled,
                        int maxEntry, int maxEntryPerUser, int maxEntryPerUserPeriodValue, string maxEntryPerUserPeriodType,
                        int numberOfWinners, DateTime? pubDate,
                        DateTime? pubEndDate,
                        string sendEmailToModeratorRecipes, long? sendEmailToModeratorTemplateId,
                        long? sendEmailAfterSubmissionTemplateId,
                        long? sendEmailAfterApproveEntryTemplateId,
                        long? sendEmailAfterDeleteEntryTemplateId)
        {
            long id = AWAPI_Common.library.MiscLibrary.CreateUniqueId();
            awContest contest = new awContest();

            contest.contestId = id;
            contest.title = title;
            contest.description = description;
            contest.siteId = siteId;
            contest.userId = userId;
            contest.isEnabled = isEnabled;
            contest.maxEntry = maxEntry;
            contest.numberOfWinners = numberOfWinners;

            //like: one user can enter maximum 4 times (maxEntryPerUser) 
            //in every 2 (maxEntryPerUserPeriodType) weeks (maxEntryPerUserPeriodValue)
            contest.entryPerUser = maxEntryPerUser;
            contest.entryPerUserPeriodType = maxEntryPerUserPeriodType;
            contest.entryPerUserPeriodValue = maxEntryPerUserPeriodValue;

            contest.pubDate = pubDate;
            contest.pubEndDate = pubEndDate;

            contest.sendEmailToModeratorRecipes = sendEmailToModeratorRecipes;
            contest.sendEmailToModeratorTemplateId = sendEmailToModeratorTemplateId;
            contest.sendEmailAfterSubmissionTemplateId = sendEmailAfterSubmissionTemplateId;
            contest.sendEmailAfterApproveEntryTemplateId = sendEmailAfterApproveEntryTemplateId;
            contest.sendEmailAfterDeleteEntryTemplateId = sendEmailAfterDeleteEntryTemplateId;

            contest.lastBuildDate = DateTime.Now;
            contest.createDate = DateTime.Now;

            _context.awContests.InsertOnSubmit(contest);
            _context.SubmitChanges();

            return id;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="contestId"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="isEnabled"></param>
        /// <param name="maxEntry"></param>
        /// <param name="maxEntryPerUser"></param>
        /// <param name="maxEntryPerUserPeriodValue"></param>
        /// <param name="maxEntryPerUserPeriodType"></param>
        /// <param name="numberOfWinners"></param>
        /// <param name="pubDate"></param>
        /// <param name="pubEndDate"></param>
        /// <param name="sendEmailToModeratorRecipes"></param>
        /// <param name="sendEmailToModeratorTemplateId"></param>
        /// <param name="sendEmailAfterApproveEntryTemplateId"></param>
        /// <param name="sendEmailAfterDeleteEntryTemplateId"></param>
        /// <returns></returns>
        public bool UpdateContest(long contestId,
                        string title, string description, bool isEnabled,
                        int maxEntry, int maxEntryPerUser, int maxEntryPerUserPeriodValue, string maxEntryPerUserPeriodType,
                        int numberOfWinners, DateTime? pubDate,
                        DateTime? pubEndDate,
                        string sendEmailToModeratorRecipes, long? sendEmailToModeratorTemplateId,
                        long? sendEmailAfterSubmissionTemplateId,
                        long? sendEmailAfterApproveEntryTemplateId,
                        long? sendEmailAfterDeleteEntryTemplateId)
        {
            awContest contest = _context.awContests.FirstOrDefault(st => st.contestId.Equals(contestId));

            if (contest == null)
                return false;

            contest.title = title;
            contest.description = description;
            contest.isEnabled = isEnabled;
            contest.maxEntry = maxEntry;
            contest.entryPerUser = maxEntryPerUser;
            contest.entryPerUserPeriodType = maxEntryPerUserPeriodType;
            contest.entryPerUserPeriodValue = maxEntryPerUserPeriodValue;
            contest.numberOfWinners = numberOfWinners;
            contest.pubDate = pubDate;
            contest.pubEndDate = pubEndDate;

            contest.sendEmailToModeratorRecipes = sendEmailToModeratorRecipes;
            contest.sendEmailToModeratorTemplateId = sendEmailToModeratorTemplateId;
            contest.sendEmailAfterSubmissionTemplateId = sendEmailAfterSubmissionTemplateId;
            contest.sendEmailAfterApproveEntryTemplateId = sendEmailAfterApproveEntryTemplateId;
            contest.sendEmailAfterDeleteEntryTemplateId = sendEmailAfterDeleteEntryTemplateId;

            contest.lastBuildDate = DateTime.Now;

            _context.SubmitChanges();

            return true;
        }

        #endregion

        #region contest group Group ---

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contestGroupId"></param>
        /// <returns></returns>
        public awContestGroup GetContestGroup(long contestGroupId)
        {
            if (contestGroupId <= 0)
                return null;
            var cgroup = _context.GetTable<awContestGroup>()
                        .Where(st => st.contestGroupId.Equals(contestGroupId));

            if (cgroup == null && cgroup.ToList().Count() == 0)
                return null;
            return cgroup.First();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public System.Collections.Generic.IList<awContestGroup> GetContestGroupList(long siteId, string where)
        {
            StringBuilder sb = new StringBuilder(" SELECT * FROM awContestGroup ");
            StringBuilder sbWhere = new StringBuilder(" WHERE siteId=" + siteId);

            if (where.Trim().Length > 0)
                sbWhere.Append(" AND (" + where + ") ");

            sb.Append(sbWhere.ToString());
            sb.Append(" ORDER BY pubDate DESC");

            var groups = _context.ExecuteQuery<awContestGroup>(sb.ToString());
            if (groups == null)
                return null;

            return groups.ToList<awContestGroup>();
        }

        public System.Collections.Generic.IList<awContestGroup> GetContestGroupListByContestId(long contestId)
        {
            var list = from cg in _context.awContestGroups
                       join gm in _context.awContestGroupMembers on cg.contestGroupId equals gm.contestGroupId
                       where gm.contestId.Equals(contestId)
                       orderby cg.pubDate descending
                       select cg;
            if (list == null)
                return null;

            return list.ToList<awContestGroup>();

        }


        /// <summary>
        /// Contests can be managed seperately from the groups.
        /// When we delete a group, we don't need to delete the contests, or its entries.
        /// </summary>
        /// <param name="contestGroupId"></param>
        public void DeleteContestGroup(long groupId)
        {
            //Delete the group winners
            var winners = from w in _context.awContestWinners
                          where w.contestGroupId.Equals(groupId)
                          select w;
            _context.awContestWinners.DeleteAllOnSubmit(winners);

            //Delete the relationship
            var relations = from gm in _context.awContestGroupMembers
                            where gm.contestGroupId.Equals(groupId)
                            select gm;
            _context.awContestGroupMembers.DeleteAllOnSubmit(relations);

            //Delete the group
            var contests = from p in _context.awContestGroups
                           where p.contestGroupId.Equals(groupId)
                           select p;
            _context.awContestGroups.DeleteAllOnSubmit(contests);

            //SUBMIT CHANGES -------------------------------------------------------------
            _context.SubmitChanges();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="userId"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="isEnabled"></param>
        /// <param name="numberOfWinners"></param>
        /// <param name="pubDate"></param>
        /// <param name="pubEndDate"></param>
        /// <returns></returns>
        public long AddContestGroup(long siteId, long userId,
                        string title, string description, bool isEnabled,
                        int numberOfWinners, DateTime? pubDate, DateTime? pubEndDate)
        {
            long id = AWAPI_Common.library.MiscLibrary.CreateUniqueId();
            awContestGroup group = new awContestGroup();

            group.contestGroupId = id;
            group.title = title;
            group.description = description;
            group.siteId = siteId;
            group.userId = userId;
            group.isEnabled = isEnabled;
            group.numberOfWinners = numberOfWinners;

            group.pubDate = pubDate;
            group.pubEndDate = pubEndDate;

            group.lastBuildDate = DateTime.Now;
            group.createDate = DateTime.Now;

            _context.awContestGroups.InsertOnSubmit(group);
            _context.SubmitChanges();

            return id;
        }

        public void AddContestToGroups(long[] groupIds, long contestId)
        {
            //delete if doesn't exist
            var toBeDeleted = from c in _context.awContestGroupMembers
                              where !groupIds.Contains(c.contestGroupId) &&
                                    c.contestId.Equals(contestId)
                              select c;
            _context.awContestGroupMembers.DeleteAllOnSubmit(toBeDeleted);

            var existing = from c in _context.awContestGroupMembers
                           where c.contestId.Equals(contestId)
                           select c;

            IList<awContestGroupMember> tmpList = null;
            if (existing != null)
                tmpList = existing.ToList();

            //Add the new ones
            foreach (long grpId in groupIds)
            {
                bool alreadyExist = false;
                if (tmpList != null && tmpList.Count > 0)
                {
                    var exist = from t in tmpList
                                where t.contestId.Equals(contestId) &&
                                    t.contestGroupId.Equals(grpId)
                                select t;
                    if (exist != null && exist.Count() > 0)
                        alreadyExist = true;
                }
                if (alreadyExist)
                    continue;

                awContestGroupMember grpMember = new awContestGroupMember();
                grpMember.contestGroupMemberId = AWAPI_Common.library.MiscLibrary.CreateUniqueId();
                grpMember.contestGroupId = grpId;
                grpMember.contestId = contestId;

                _context.awContestGroupMembers.InsertOnSubmit(grpMember);
            }

            _context.SubmitChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contestGroupId"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="isEnabled"></param>
        /// <param name="maxEntry"></param>
        /// <param name="maxEntryPerUser"></param>
        /// <param name="maxEntryPerUserPeriodValue"></param>
        /// <param name="maxEntryPerUserPeriodType"></param>
        /// <param name="pubDate"></param>
        /// <param name="pubEndDate"></param>
        /// <returns></returns>
        public bool UpdateContestGroup(long contestGroupId, string title, string description, bool isEnabled,
                        int numberOfWinners, DateTime? pubDate, DateTime? pubEndDate)
        {
            awContestGroup group = _context.awContestGroups.FirstOrDefault(st =>
                                    st.contestGroupId.Equals(contestGroupId));

            if (group == null)
                return false;

            group.title = title;
            group.description = description;
            group.isEnabled = isEnabled;
            group.numberOfWinners = numberOfWinners;
            group.pubDate = pubDate;
            group.pubEndDate = pubEndDate;

            group.lastBuildDate = DateTime.Now;

            _context.SubmitChanges();

            return true;
        }

        #endregion


        #region group Entry
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contestEntryId"></param>
        /// <returns></returns>
        public AWAPI_Data.CustomEntities.ContestEntryExtended GetContestEntry(long contestEntryId, bool onlyEnabled)
        {
            if (contestEntryId <= 0)
                return null;
            AWAPI_Data.CustomEntities.ContestEntryExtended entry =
                (from c in _context.awContestEntries
                 where c.contestEntryId.Equals(contestEntryId) &&
                      (!onlyEnabled ||
                      onlyEnabled && c.awContest.isEnabled)
                 select new AWAPI_Data.CustomEntities.ContestEntryExtended
                 {
                     address = c.address,
                     city = c.city,
                     contestEntryId = c.contestEntryId,
                     contestId = c.contestId,
                     isEnabled = c.isEnabled,
                     country = c.country,
                     createDate = c.createDate,
                     cultureCode = c.cultureCode,
                     description = c.description,
                     email = c.email,
                     fileId = c.fileId,
                     fileUrl = c.fileUrl,
                     fileContentType = c.awFile_Contest == null || !c.awFile_Contest.isEnabled ? "" : c.awFile_Contest.contentType,
                     fileThumbUrl = c.awFile_Contest == null || !c.awFile_Contest.isEnabled ? "" : c.awFile_Contest.thumbnail,
                     fileEnabled = c.awFile_Contest == null || !c.awFile_Contest.isEnabled ? false : true,
                     firstName = c.firstName,
                     lastName = c.lastName,
                     postalCode = c.postalCode,
                     province = c.province,
                     tel = c.tel,
                     telType = c.telType,
                     title = c.title,
                     userId = c.userId,
                     daysPassed = CalculateDaysPassed(c.awContest.pubDate, c.createDate)
                 }).FirstOrDefault();

            return entry;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contestId"></param>
        /// <returns></returns>
        public System.Collections.Generic.IList<AWAPI_Data.CustomEntities.ContestEntryExtended> GetContestEntryList(long contestId)
        {
            return GetContestEntryList(contestId, null, "", null);
        }

        public int CalculateDaysPassed(DateTime? contestPublishDate, DateTime entryCreateDate)
        {
            if (contestPublishDate == null)
                return 0;

            TimeSpan difference = entryCreateDate.Subtract(contestPublishDate.Value);
            return difference.Days;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contestGroupId"></param>
        /// <returns></returns>
        public System.Collections.Generic.IList<AWAPI_Data.CustomEntities.ContestEntryExtended> GetContestEntryList(long contestId, long? userId, string keywordSearch, DateTime? dt)
        {
            string keyword = keywordSearch.ToLower().Trim();
            if (userId == 0)
                userId = null;

            var entries = from c in _context.awContestEntries
                          where c.contestId.Equals(contestId) &&
                            (keyword.Length.Equals(0) ||
                             (keyword.Length > 0 &&
                                (c.firstName.ToLower().Equals(keyword) ||
                                 c.lastName.ToLower().Equals(keyword) ||
                                 c.email.ToLower().Equals(keyword))
                             ))
                              && (
                                userId == null || userId != null && c.userId.Equals(userId)
                             )
                          orderby c.createDate descending
                          select new AWAPI_Data.CustomEntities.ContestEntryExtended
                          {
                              address = c.address,
                              city = c.city,
                              contestEntryId = c.contestEntryId,
                              contestId = c.contestId,
                              isEnabled = c.isEnabled,
                              country = c.country,
                              createDate = c.createDate,
                              cultureCode = c.cultureCode,
                              description = c.description,
                              email = c.email,
                              fileId = c.fileId,
                              fileUrl = c.fileUrl,
                              fileContentType = c.awFile_Contest == null || !c.awFile_Contest.isEnabled ? "" : c.awFile_Contest.contentType,
                              fileThumbUrl = c.awFile_Contest == null || !c.awFile_Contest.isEnabled ? "" : c.awFile_Contest.thumbnail,
                              fileEnabled = c.awFile_Contest == null || !c.awFile_Contest.isEnabled ? false : true,
                              firstName = c.firstName,
                              lastName = c.lastName,
                              postalCode = c.postalCode,
                              province = c.province,
                              tel = c.tel,
                              telType = c.telType,
                              title = c.title,
                              userId = c.userId,
                              daysPassed = CalculateDaysPassed(c.awContest.pubDate, c.createDate)
                          };


            if (entries == null || entries.Count() == 0)
                return null;

            if (dt == null)
                return entries.ToList<AWAPI_Data.CustomEntities.ContestEntryExtended>();

            DateTime dt2 = dt.Value;
            var list2 = from l in entries
                        where l.createDate >= dt2 && l.createDate < dt2.AddDays(1)
                        select l;

            return list2.ToList<AWAPI_Data.CustomEntities.ContestEntryExtended>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contestGroupId"></param>
        /// <returns></returns>
        public System.Collections.Generic.IList<AWAPI_Data.CustomEntities.ContestEntryExtended> GetContestEntryListByGroupId(long grupId, string keywordSearch, DateTime? dt)
        {
            string keyword = keywordSearch.ToLower().Trim();
            var tmpContestIds = from cgm in _context.awContestGroupMembers
                                where cgm.contestGroupId.Equals(grupId)
                                select cgm.contestId;
            if (tmpContestIds == null)
                return null;

            DateTime dtStart = dt == null ? new DateTime(2000, 1, 1) : dt.Value;
            DateTime dtEnd = dt == null ? new DateTime(2100, 1, 1) : new DateTime(dt.Value.Year, dt.Value.Month, dt.Value.Day, 23, 59, 59);

            var entries = from c in _context.awContestEntries
                          where tmpContestIds.Contains(c.contestId) &&
                            (keyword.Length.Equals(0) ||
                             (keyword.Length > 0 &&
                                (c.firstName.ToLower().Equals(keyword) ||
                                 c.lastName.ToLower().Equals(keyword) ||
                                 c.email.ToLower().Equals(keyword))
                             ))
                             &&
                             c.createDate >= dtStart && c.createDate <= dtEnd
                          orderby c.createDate descending
                          select new AWAPI_Data.CustomEntities.ContestEntryExtended
                          {
                              address = c.address,
                              city = c.city,
                              contestEntryId = c.contestEntryId,
                              contestId = c.contestId,
                              isEnabled = c.isEnabled,
                              country = c.country,
                              createDate = c.createDate,
                              cultureCode = c.cultureCode,
                              description = c.description,
                              email = c.email,
                              fileId = c.fileId,
                              fileUrl = c.fileUrl,
                              fileContentType = c.awFile_Contest == null || !c.awFile_Contest.isEnabled ? "" : c.awFile_Contest.contentType,
                              fileThumbUrl = c.awFile_Contest == null || !c.awFile_Contest.isEnabled ? "" : c.awFile_Contest.thumbnail,
                              fileEnabled = c.awFile_Contest == null || !c.awFile_Contest.isEnabled ? false : true,
                              firstName = c.firstName,
                              lastName = c.lastName,
                              postalCode = c.postalCode,
                              province = c.province,
                              tel = c.tel,
                              telType = c.telType,
                              title = c.title,
                              userId = c.userId,
                              daysPassed = CalculateDaysPassed(c.awContest.pubDate, c.createDate)
                          };
            if (entries == null || entries.Count() == 0)
                return null;

            return entries.ToList<AWAPI_Data.CustomEntities.ContestEntryExtended>();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="contestId"></param>
        /// <param name="userId"></param>
        /// <param name="isEnabled"></param>
        /// <param name="cultureCode"></param>
        /// <param name="email"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="fileId"></param>
        /// <param name="fileUrl"></param>
        /// <param name="tel"></param>
        /// <param name="telType"></param>
        /// <param name="address"></param>
        /// <param name="city"></param>
        /// <param name="province"></param>
        /// <param name="postalCode"></param>
        /// <param name="country"></param>
        /// <returns></returns>
        public long AddContestEntry(long contestId, long? userId, bool isEnabled, string cultureCode, string email, string firstName, string lastName,
                                 string title, string description, long? fileId, string fileUrl,
                                 string tel, string telType, string address, string city, string province,
                                 string postalCode, string country)
        {
            long id = AWAPI_Common.library.MiscLibrary.CreateUniqueId();
            ContestExtended contest = GetContest(contestId, true);
            if (contest == null)
                throw new Exception(ErrorLibrary.ErrorMessage(ErrorLibrary.CONTEST.DOES_NOT_EXIST));

            awContestEntry entry = new awContestEntry();

            if (String.IsNullOrEmpty(email) || !AWAPI_Common.library.Validation.IsEmail(email))
                throw new Exception(ErrorLibrary.ErrorMessage(ErrorLibrary.CONTEST.EMAIL_REQUIRED));

            if (String.IsNullOrEmpty(firstName) || String.IsNullOrEmpty(lastName))
                throw new Exception(ErrorLibrary.ErrorMessage(ErrorLibrary.CONTEST.FIRSTNAME_LASTNAME_REQUIRED));

            entry.contestEntryId = id;
            entry.contestId = contestId;
            entry.userId = userId;
            entry.isEnabled = isEnabled;
            entry.cultureCode = cultureCode;

            entry.email = email;
            entry.firstName = firstName;
            entry.lastName = lastName;

            entry.title = title;
            entry.description = description;
            entry.fileId = fileId;
            entry.fileUrl = fileUrl;

            entry.tel = tel;
            entry.telType = telType;
            entry.address = address;
            entry.city = city;
            entry.province = province;
            entry.postalCode = postalCode;
            entry.country = country;

            entry.createDate = DateTime.Now;

            _context.awContestEntries.InsertOnSubmit(entry);
            _context.SubmitChanges();


            AWAPI_BusinessLibrary.library.EmailTemplateLib lib = new EmailTemplateLib();
            //SEND EMAIL TO THE MODERATORS --------------------
            if (contest.sendEmailToModeratorTemplateId != null && !String.IsNullOrEmpty(contest.sendEmailToModeratorRecipes))
            {
                string link = "";
                if (entry.fileId != null)
                    link = ConfigurationLibrary.Config.adminBaseUrl + "admin/files.aspx?fileid=" + entry.fileId.Value.ToString();

                lib.Send(entry.awContest.sendEmailToModeratorTemplateId.Value,
                    contest.sendEmailToModeratorRecipes,
                                "firstname|" + entry.firstName,
                                "lastname|" + entry.lastName,
                                "link|" + link,
                                "date|" + DateTime.Now.ToString());

            }
            //SEND EMAIL TO THE CONTENDER --------------------
            if (contest.sendEmailAfterSubmissionTemplateId != null)
            {
                lib.Send(entry.awContest.sendEmailAfterSubmissionTemplateId.Value,
                        entry.email,
                        "firstname|" + entry.firstName,
                        "lastname|" + entry.lastName,
                        "date|" + DateTime.Now.ToString());
            }



            return id;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="contestEntryId"></param>
        /// <param name="isEnabled"></param>
        public void UpdateContestEntryStatus(long contestEntryId, bool isEnabled)
        {
            awContestEntry entry = (from l in _context.awContestEntries
                                    where l.contestEntryId.Equals(contestEntryId)
                                    select l).FirstOrDefault();
            if (entry == null)
                return;

            entry.isEnabled = isEnabled;
            _context.SubmitChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contestEntryId"></param>
        /// <param name="isEnabled"></param>
        public void UpdateContestEntriesStatusesByFileId(long fileId, bool isEnabled)
        {
            var contestEntries = from l in _context.awContestEntries
                                 where l.fileId.Equals(fileId)
                                 select l;

            if (contestEntries == null || contestEntries.Count() == 0)
                return;

            foreach (awContestEntry entry in contestEntries)
                entry.isEnabled = isEnabled;
            _context.SubmitChanges();


            //send email
            AWAPI_BusinessLibrary.library.EmailTemplateLib lib = new EmailTemplateLib();
            foreach (awContestEntry entry in contestEntries)
            {
                if (entry.awContest.sendEmailAfterApproveEntryTemplateId == null)
                    break;

                lib.Send(entry.awContest.sendEmailAfterApproveEntryTemplateId.Value,
                        entry.email,
                                "firstname|" + entry.firstName,
                                "lastname|" + entry.lastName,
                                "date|" + DateTime.Now.ToString());
            }

        }

        /// <summary>
        /// Deletes the contest entry by the fileId,
        /// If the contest entry is allready approved, won't be deleted,,,
        /// </summary>
        /// <param name="fileId"></param>
        public void DeleteContestEntriesByFileId(long fileId)
        {
            var contestEntries = from l in _context.awContestEntries
                                 where l.fileId.Equals(fileId)
                                    //&& !l.isEnabled
                                 select l;

            if (contestEntries == null || contestEntries.Count() == 0)
                return;

            _context.awContestEntries.DeleteAllOnSubmit(contestEntries);

            //SEND EMAIL TO INFORM USER THAT THE CONTEST ENTRY NOT APPROVED
            AWAPI_BusinessLibrary.library.EmailTemplateLib lib = new EmailTemplateLib();
            foreach (awContestEntry entry in contestEntries)
            {
                if (entry.awContest.sendEmailAfterDeleteEntryTemplateId == null)
                    break;

                lib.Send(entry.awContest.sendEmailAfterDeleteEntryTemplateId.Value,
                        entry.email,
                                "firstname|" + entry.firstName,
                                "lastname|" + entry.lastName,
                                "date|" + DateTime.Now.ToString());
            }

            _context.SubmitChanges();


        }


        /// <summary>
        /// Deletes contest entries by user id
        /// </summary>
        /// <param name="userId"></param>
        public void DeleteContestEntriesByUserId(long userId)
        {
            var contestEntries = from l in _context.awContestEntries
                                 where l.userId.Equals(userId)
                                 select l;
            if (contestEntries == null)
                return;

            _context.awContestEntries.DeleteAllOnSubmit(contestEntries);
            _context.SubmitChanges();


        }
        #endregion

    }
}
