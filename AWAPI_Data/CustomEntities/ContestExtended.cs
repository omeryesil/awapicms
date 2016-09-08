using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AWAPI_Data.CustomEntities
{
    public class ContestExtended
    {
        private long _contestId;
        private long _siteId;
        private string _title;
        private string _description;
        private bool _isEnabled;
        private int _maxEntry;
        private int _entryPerUser;
        private int _entryPerUserPeriodValue;
        private string _entryPerUserPeriodType;
        private int _numberOfWinners;
        private System.Nullable<System.DateTime> _pubDate;
        private System.Nullable<System.DateTime> _pubEndDate;
        private System.Nullable<long> _userId;
        private System.DateTime _lastBuildDate;
        private System.DateTime _createDate;

        private long? _sendEmailToModeratorTemplateId;
        private string _sendEmailToModeratorRecipes;
        private long? _sendEmailAfterSubmissionTemplateId;
        private long? _sendEmailAfterApproveEntryTemplateId;
        private long? _sendEmailAfterDeleteEntryTemplateId;


        public long contestId
        {
            get { return this._contestId; }
            set { _contestId = value; }
        }

        public long siteId
        {
            get
            {
                return this._siteId;
            }
            set
            {
                _siteId = value;
            }
        }

        public string title
        {
            get
            {
                return this._title;
            }
            set
            {
                _title = value;
            }
        }

        public string description
        {
            get
            {
                return this._description;
            }
            set
            {
                _description = value;
            }
        }

        public bool isEnabled
        {
            get
            {
                return this._isEnabled;
            }
            set
            {
                _isEnabled = value;
            }
        }

        public int maxEntry
        {
            get
            {
                return this._maxEntry;
            }
            set
            {
                _maxEntry = value;
            }
        }

        public int entryPerUser
        {
            get
            {
                return this._entryPerUser;
            }
            set
            {
                _entryPerUser = value;
            }
        }

        public int entryPerUserPeriodValue
        {
            get
            {
                return this._entryPerUserPeriodValue;
            }
            set
            {
                _entryPerUserPeriodValue = value;
            }
        }

        public string entryPerUserPeriodType
        {
            get
            {
                return this._entryPerUserPeriodType;
            }
            set
            {
                _entryPerUserPeriodType = value;
            }
        }

        public int numberOfWinners
        {
            get
            {
                return this._numberOfWinners;
            }
            set
            {
                _numberOfWinners = value;
            }
        }

        public System.Nullable<System.DateTime> pubDate
        {
            get
            {
                return this._pubDate;
            }
            set
            {
                _pubDate = value;
            }
        }

        public System.Nullable<System.DateTime> pubEndDate
        {
            get
            {
                return this._pubEndDate;
            }
            set
            {
                _pubEndDate = value;
            }
        }

        public System.Nullable<long> userId
        {
            get
            {
                return this._userId;
            }
            set
            {
                _userId = value;
            }
        }

        public System.DateTime lastBuildDate
        {
            get
            {
                return this._lastBuildDate;
            }
            set
            {
                _lastBuildDate = value;
            }
        }

        public System.DateTime createDate
        {
            get
            {
                return this._createDate;
            }
            set
            {
                _createDate = value;
            }
        }


        public long? sendEmailToModeratorTemplateId
        {
            get { return this._sendEmailToModeratorTemplateId; }
            set { _sendEmailToModeratorTemplateId = value; }
        }

        public string sendEmailToModeratorRecipes
        {
            get { return this._sendEmailToModeratorRecipes; }
            set { _sendEmailToModeratorRecipes = value; }
        }


        public long? sendEmailAfterSubmissionTemplateId
        {
            get { return this._sendEmailAfterSubmissionTemplateId; }
            set { _sendEmailAfterSubmissionTemplateId = value; }
        }



        public long? sendEmailAfterApproveEntryTemplateId
        {
            get { return this._sendEmailAfterApproveEntryTemplateId; }
            set { _sendEmailAfterApproveEntryTemplateId = value; }
        }

        public long? sendEmailAfterDeleteEntryTemplateId
        {
            get { return this._sendEmailAfterDeleteEntryTemplateId; }
            set { _sendEmailAfterDeleteEntryTemplateId = value; }
        }
    }
}
