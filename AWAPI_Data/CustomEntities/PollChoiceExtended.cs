using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AWAPI_Data.Data;

namespace AWAPI_Data.CustomEntities
{
    public class PollChoiceExtended
    {
        private long _pollChoiceId;
        private long _pollId;
        private string _title;
        private string _description;
        private string _siteCultureCode;
        private int _numberOfVotes;
        private int _sortOrder;
        private System.Nullable<System.DateTime> _lastBuildDate;
        private System.DateTime _createDate;

        public long pollChoiceId
        {
            get { return _pollChoiceId; }
            set { _pollChoiceId = value; }
        }

        public long pollId
        {
            get { return _pollId; }
            set { _pollId = value; }
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

        public string siteCultureCode
        {
            get { return _siteCultureCode; }
            set { _siteCultureCode = value; }
        }

        public int numberOfVotes
        {
            get { return _numberOfVotes; }
            set { _numberOfVotes = value; }
        }

        public int sortOrder
        {
            get { return _sortOrder; }
            set { _sortOrder = value; }
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

    }
}
