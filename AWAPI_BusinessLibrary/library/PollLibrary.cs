using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.ComponentModel;

using AWAPI_Data.Data;
using AWAPI_Common.library;

namespace AWAPI_BusinessLibrary.library
{

    public class PollLibrary
    {
        PollContextDataContext _context = new PollContextDataContext();
        CultureLibrary _cultureLib = new CultureLibrary();

        #region POLL

        /// <summary>
        /// Returns poll based on the culturecode
        /// </summary>
        /// <param name="pollId"></param>
        /// <param name="cultureCode"></param>
        /// <returns></returns>
        public AWAPI_Data.CustomEntities.PollExtended GetPoll(long pollId, string cultureCode, bool onlyPublished)
        {
            if (pollId <= 0)
                return null;
            DateTime now = DateTime.Now;
            AWAPI_Data.CustomEntities.PollExtended poll = (from l in _context.awPolls
                       where l.pollId.Equals(pollId) &&
                            (!onlyPublished ||
                             (onlyPublished && l.isEnabled && l.awSite_Poll.isEnabled))
                       select new AWAPI_Data.CustomEntities.PollExtended
                       {
                           pollId = l.pollId,
                           title = l.title,
                           description = l.description,
                           answeredQuestion = l.answeredQuestion,
                           isEnabled = l.isEnabled,
                           isPublic = l.isPublic,
                           isMultipleChoice = l.isMultipleChoice,
                           siteId = l.siteId,
                           userId = l.userId,
                           pubDate = l.pubDate,
                           pubEndDate = l.pubEndDate,
                           createDate = l.createDate,
                           lastBuildDate = l.lastBuildDate,
                           availableToVote = ((l.pubDate.Equals(null) || l.pubDate <= now) &&
                                             (l.pubEndDate.Equals(null) || l.pubEndDate > now)) ? true : false,

                           pollChoices = this.GetPollChoiceList(l.pollId, cultureCode),
                           siteCultureCode = l.awSite_Poll.cultureCode

                       }).FirstOrDefault < AWAPI_Data.CustomEntities.PollExtended>();

            //if (list == null && list.ToList().Count() == 0)
             //   return null;

            //AWAPI_Data.CustomEntities.PollExtended poll = list.FirstOrDefault();
            if (poll == null)
                return null;

            if (onlyPublished)
            {
                // if not published yet.... 
                int dateBetween = MiscLibrary.IsDateBetween(DateTime.Now, poll.pubDate, poll.pubEndDate);
                if (dateBetween == 0)
                    return null;
                //throw new Exception(ErrorLibrary.ErrorMessage(ErrorLibrary.POLL.NOT_PUBLISHED));
            }

            //if site's culture code equals to cultureCode parameter
            if (String.IsNullOrEmpty(cultureCode) || poll.siteCultureCode.ToLower() == cultureCode.ToLower())
                return poll;

            poll.description = _cultureLib.GetValue(cultureCode.ToLower(), poll.pollId, "awpoll", "description");
            poll.answeredQuestion = _cultureLib.GetValue(cultureCode.ToLower(), poll.pollId, "awpoll", "answeredQuestion");
            return poll;
        }



        /// <returns></returns>
        public System.Collections.Generic.IList<AWAPI_Data.CustomEntities.PollExtended> GetPollList(long siteId, string where, string cultureCode, bool onlyEnabled)
        {
            return GetPollList(siteId, where, cultureCode, onlyEnabled, 0, null);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="where"></param>
        /// <param name="cultureCode"></param>
        /// <param name="onlyEnabled"></param>
        /// <param name="maxRecord">Number of records to be returned</param>
        /// <returns></returns>
        public System.Collections.Generic.IList<AWAPI_Data.CustomEntities.PollExtended> GetPollList(long siteId, string where, string cultureCode, bool onlyEnabled, int maxRecord, DateTime? pubStartDate)
        {
            DateTime now = DateTime.Now;
            var list = from l in _context.awPolls
                       where l.siteId.Equals(siteId) &&
                            (!onlyEnabled ||
                            (onlyEnabled && l.isEnabled && l.awSite_Poll.isEnabled &&
                             (l.pubDate.Equals(null) || l.pubDate <= now)
                             )) && 
                             (pubStartDate== null || pubStartDate != null && pubStartDate<=l.pubDate)
                       orderby l.pubDate descending, l.createDate descending
                       select new AWAPI_Data.CustomEntities.PollExtended
                       {
                           pollId = l.pollId,
                           title = l.title,
                           description = l.description,
                           answeredQuestion = l.answeredQuestion,
                           isEnabled = l.isEnabled,
                           isPublic = l.isPublic,
                           isMultipleChoice = l.isMultipleChoice,
                           siteId = l.siteId,
                           userId = l.userId,
                           pubDate = l.pubDate,
                           pubEndDate = l.pubEndDate,
                           createDate = l.createDate,
                           lastBuildDate = l.lastBuildDate,
                           availableToVote = ((l.pubDate.Equals(null) || l.pubDate <= now) &&
                                             (l.pubEndDate.Equals(null) || l.pubEndDate > now)) ? true : false,
                           pollChoices = this.GetPollChoiceList(l.pollId, cultureCode),
                           siteCultureCode = l.awSite_Poll.cultureCode,

                       };
            if (list == null || list.Count() == 0)
                return null;

            IList<AWAPI_Data.CustomEntities.PollExtended> pollList;
            if (maxRecord > 0)
                pollList = list.Take(maxRecord).ToList<AWAPI_Data.CustomEntities.PollExtended>();
            else
                pollList = list.ToList<AWAPI_Data.CustomEntities.PollExtended>();

            if (cultureCode.Trim() == "" || pollList[0].siteCultureCode.ToLower() == cultureCode.ToLower())
                return pollList;

            IList<awCultureValue> valueList = _cultureLib.GetValueList(cultureCode, "awpoll");

            foreach (AWAPI_Data.CustomEntities.PollExtended poll in pollList)
            {
                poll.description = _cultureLib.GetValue(valueList, cultureCode, poll.pollId, "description");
                poll.answeredQuestion = _cultureLib.GetValue(valueList, cultureCode, poll.pollId, "answeredQuestion");
            }

            return pollList;
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="pollId"></param>
        public void DeletePoll(long pollId)
        {
            if (pollId <= 0)
                return;

            //Delete culture values
            _cultureLib.DeleteValueByRowId(pollId);


            //Delete choices
            var choices = from c in _context.awPollChoices
                          where c.pollId.Equals(pollId)
                          select c;
            _context.awPollChoices.DeleteAllOnSubmit(choices);

            //Delete polls
            var polls = from p in _context.awPolls
                        where p.pollId.Equals(pollId)
                        select p;
            _context.awPolls.DeleteAllOnSubmit(polls);

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
        /// <param name="isPublic"></param>
        /// <param name="isMultipleChoice"></param>
        /// <param name="pubDate"></param>
        /// <param name="pubEndDate"></param>
        /// <returns></returns>
        public long AddPoll(long siteId, long userId,
                        string title, string description, string answeredQuestion, bool isEnabled,
                        bool isPublic, bool isMultipleChoice, DateTime? pubDate,
                        DateTime? pubEndDate)
        {
            long id = AWAPI_Common.library.MiscLibrary.CreateUniqueId();
            awPoll poll = new awPoll();

            poll.pollId = id;
            poll.title = title;
            poll.description = description;
            poll.answeredQuestion = answeredQuestion;
            poll.siteId = siteId;
            poll.userId = userId;
            poll.isEnabled = isEnabled;
            poll.isPublic = isPublic;
            poll.isMultipleChoice = isMultipleChoice;
            poll.pubDate = pubDate;
            poll.pubEndDate = pubEndDate;

            poll.lastBuildDate = DateTime.Now;
            poll.createDate = DateTime.Now;

            _context.awPolls.InsertOnSubmit(poll);
            _context.SubmitChanges();

            return id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pollId"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="isEnabled"></param>
        /// <param name="isPublic"></param>
        /// <param name="isMultipleChoice"></param>
        /// <param name="pubDate"></param>
        /// <param name="pubEndDate"></param>
        /// <returns></returns>
        public bool UpdatePoll(long pollId, string title, string description, string answeredQuestion, bool isEnabled,
                        bool isPublic, bool isMultipleChoice, DateTime? pubDate,
                        DateTime? pubEndDate)
        {
            awPoll poll = _context.awPolls.FirstOrDefault(st => st.pollId.Equals(pollId));

            if (poll == null)
                return false;

            poll.title = title;
            poll.description = description;
            poll.answeredQuestion = answeredQuestion;
            poll.isEnabled = isEnabled;
            poll.isPublic = isPublic;
            poll.isMultipleChoice = isMultipleChoice;
            poll.pubDate = pubDate;
            poll.pubEndDate = pubEndDate;

            poll.lastBuildDate = DateTime.Now;

            _context.SubmitChanges();

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pollId"></param>
        /// <param name="cultureCode"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public bool UpdatePollForCulture(long pollId, string cultureCode, string description, string answeredQuestion)
        {
            awPoll poll = _context.awPolls.FirstOrDefault(st => st.pollId.Equals(pollId));

            if (poll == null)
                return false;

            _cultureLib.UpdateValue(cultureCode, poll.pollId, "awpoll", "description", description);
            _cultureLib.UpdateValue(cultureCode, poll.pollId, "awpoll", "answeredQuestion", answeredQuestion);
            return true;
        }

        #endregion

        #region poll Choice
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pollChoiceId"></param>
        /// <returns></returns>
        public awPollChoice GetPollChoice(long pollChoiceId, string cultureCode)
        {
            if (pollChoiceId <= 0)
                return null;

            var list = _context.GetTable<awPollChoice>()
                        .Where(st => st.pollChoiceId.Equals(pollChoiceId));

            if (list == null && list.ToList().Count() == 0)
                return null;

            awPollChoice choice = list.FirstOrDefault();

            //if site's culture code equals to cultureCode parameter
            if (String.IsNullOrEmpty(cultureCode) || choice.awPoll.awSite_Poll.cultureCode.ToLower() == cultureCode.ToLower())
                return choice;

            choice.title = _cultureLib.GetValue(cultureCode.ToLower(), choice.pollChoiceId, "awpollchoice", "title");
            choice.description = _cultureLib.GetValue(cultureCode.ToLower(), choice.pollChoiceId, "awpollchoice", "description");

            return choice;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pollId"></param>
        /// <returns></returns>
        public System.Collections.Generic.IList<AWAPI_Data.CustomEntities.PollChoiceExtended> GetPollChoiceList(long pollId, string cultureCode)
        {
            var tmpList = from pc in _context.awPollChoices
                          where pc.pollId.Equals(pollId)
                          orderby pc.sortOrder
                          select new AWAPI_Data.CustomEntities.PollChoiceExtended
                          {
                              pollChoiceId = pc.pollChoiceId,
                              pollId = pc.pollId, 
                              title = pc.title,
                              description = pc.description,
                              numberOfVotes = pc.numberOfVotes,
                              sortOrder = pc.sortOrder,
                              createDate = pc.createDate,
                              lastBuildDate = pc.lastBuildDate,
                              siteCultureCode = pc.awPoll.awSite_Poll.cultureCode,
                          };
            if (tmpList == null || tmpList.Count() == 0)
                return null;

            IList<AWAPI_Data.CustomEntities.PollChoiceExtended> list = tmpList.ToList<AWAPI_Data.CustomEntities.PollChoiceExtended>();

            if (cultureCode.Trim() == "" || list[0].siteCultureCode.ToLower() == cultureCode.ToLower())
                return list;

            IList<awCultureValue> valueList = _cultureLib.GetValueList(cultureCode, "awpollchoice");

            foreach (AWAPI_Data.CustomEntities.PollChoiceExtended choice in list)
            {
                choice.title = _cultureLib.GetValue(valueList, cultureCode, choice.pollChoiceId, "title");
                choice.description = _cultureLib.GetValue(valueList, cultureCode, choice.pollChoiceId, "description");
            }

            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pollChoiceId"></param>
        public void DeletePollChoice(long pollChoiceId)
        {
            if (pollChoiceId <= 0)
                return;

            //delete culture
            _cultureLib.DeleteValueByRowId(pollChoiceId);

            //Delete poll choices
            var post = from p in _context.awPollChoices
                       where p.pollChoiceId.Equals(pollChoiceId)
                       select p;
            _context.awPollChoices.DeleteAllOnSubmit(post);

            _context.SubmitChanges();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pollId"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        public long AddPollChoice(long pollId, string title, string description,
                        int sortOrder)
        {
            long id = AWAPI_Common.library.MiscLibrary.CreateUniqueId();
            awPollChoice pollChoice = new awPollChoice();

            pollChoice.pollChoiceId = id;
            pollChoice.title = title;
            pollChoice.description = description;
            pollChoice.pollId = pollId;
            pollChoice.sortOrder = sortOrder;

            pollChoice.lastBuildDate = DateTime.Now;
            pollChoice.createDate = DateTime.Now;

            _context.awPollChoices.InsertOnSubmit(pollChoice);
            _context.SubmitChanges();

            return id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pollChoiceId"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        public bool UpdatePollChoice(long pollChoiceId, string title, string description, int sortOrder)
        {
            awPollChoice pollChoice = _context.awPollChoices.FirstOrDefault(st => st.pollChoiceId.Equals(pollChoiceId));

            if (pollChoice == null)
                return false;

            pollChoice.title = title;
            pollChoice.description = description;
            pollChoice.sortOrder = sortOrder;

            pollChoice.lastBuildDate = DateTime.Now;

            _context.SubmitChanges();

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pollId"></param>
        /// <param name="cultureCode"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public bool UpdatePollChoiceForCulture(long pollChoiceId, string cultureCode, string title, string description)
        {
            awPollChoice pollChoice = _context.awPollChoices.FirstOrDefault(st => st.pollChoiceId.Equals(pollChoiceId));

            if (pollChoice == null)
                return false;

            _cultureLib.UpdateValue(cultureCode, pollChoiceId, "awpollchoice", "title", title);
            _cultureLib.UpdateValue(cultureCode, pollChoiceId, "awpollchoice", "description", description);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pollId"></param>
        /// <param name="pollChoiceId"></param>
        public void AnswerPoll(long pollId, long pollChoiceId)
        {
            awPollChoice choice = (from l in _context.awPollChoices
                                   where l.pollId.Equals(pollId) && l.pollChoiceId.Equals(pollChoiceId) &&
                                         l.awPoll.isEnabled && l.awPoll.awSite_Poll.isEnabled
                                   select l).FirstOrDefault<awPollChoice>();

            if (choice == null)
                throw new Exception(ErrorLibrary.ErrorMessage(ErrorLibrary.POLL.DOES_NOT_EXIST));

            int dateBetween = MiscLibrary.IsDateBetween(DateTime.Now,
                                                    choice.awPoll.pubDate,
                                                    choice.awPoll.pubEndDate);
            switch (dateBetween)
            {
                case -1:
                    throw new Exception(ErrorLibrary.ErrorMessage(ErrorLibrary.POLL.NOT_PUBLISHED));
                    break;
                case 0:
                    throw new Exception(ErrorLibrary.ErrorMessage(ErrorLibrary.POLL.EXPIRED));
                    break;
            }

            choice.numberOfVotes += 1;
            choice.lastBuildDate = DateTime.Now;

            _context.SubmitChanges();
        }

        #endregion


    }
}
