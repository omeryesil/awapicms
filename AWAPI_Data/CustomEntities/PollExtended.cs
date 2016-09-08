using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AWAPI_Data.Data;

namespace AWAPI_Data.CustomEntities
{
    public class PollExtended
    {
        private long _pollId;
        private long _siteId;
        private long _userId;
        private string _title;
        private string _description;    //Question
        private string _answeredQuestion;
        private string _siteCultureCode;
        private bool _isEnabled;
        private bool _isPublic;
        private bool _isMultipleChoice;
        private bool _availableToVote;
        private System.Nullable<System.DateTime> _pubDate;
        private System.Nullable<System.DateTime> _pubEndDate;
        private System.Nullable<System.DateTime> _lastBuildDate;
        private System.DateTime _createDate;
        private IList<PollChoiceExtended> _pollChoices;
        //private awSite_Poll _awSite;

        public long pollId
        {
            get { return _pollId; }
            set { _pollId = value; }
        }


        public long siteId
        {
            get { return _siteId; }
            set { _siteId = value; }
        }


        public long userId
        {
            get { return _userId; }
            set { _userId = value; }
        }


        public string title
        {
            get { return _title; }
            set { _title = value; }
        }


        public string description
        {
            get { return _description; }
            set { _description = value; }
        }


        public string answeredQuestion
        {
            get { return _answeredQuestion; }
            set { _answeredQuestion = value; }
        }
                
        public string siteCultureCode
        {
            get { return _siteCultureCode; }
            set { _siteCultureCode = value; }
        }



        public bool isEnabled
        {
            get { return _isEnabled; }
            set { _isEnabled = value; }
        }


        public bool isPublic
        {
            get { return _isPublic; }
            set { _isPublic = value; }
        }


        public bool isMultipleChoice
        {
            get { return _isMultipleChoice; }
            set { _isMultipleChoice = value; }
        }

        /// <summary>
        /// if poll is published (now >pubDate) and the poll hasn't been expired (now is smaller than the expiry date, or pubEndDate null)
        /// </summary>
        public bool availableToVote
        {
            get { return _availableToVote; }
            set { _availableToVote = value; }
        }

        
        public System.Nullable<System.DateTime> pubDate
        {
            get { return _pubDate; }
            set { _pubDate = value; }
        }


        public System.Nullable<System.DateTime> pubEndDate
        {
            get { return _pubEndDate; }
            set { _pubEndDate = value; }
        }


        public System.Nullable<System.DateTime> lastBuildDate
        {
            get { return _lastBuildDate; }
            set { _lastBuildDate = value; }
        }


        public System.DateTime createDate
        {
            get { return _createDate; }
            set { _createDate = value; }
        }


        public IList<PollChoiceExtended> pollChoices
        {
            get { return _pollChoices; }
            set { _pollChoices = value; }
        }

        //public awSite_Poll awSite
        //{
        //    get { return _awSite; }
        //    set { _awSite = value; }
        //}
    }
}
