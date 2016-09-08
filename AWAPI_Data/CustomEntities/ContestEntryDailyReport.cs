using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AWAPI_Data.CustomEntities
{
    public class ContestEntryDailyReport
    {
        private long _contestId = 0;
        private string _contestTitle = "";
        private DateTime? _contestPubDate = null;
        private string _contentType = "";
        private DateTime _entryDay ;
        private long? _firstFileId = null;
        private string _firstFileUrl = "";
        private int _numberOfEntries = 0;
        private int _daysPassed = 0;

        public long contestId
        {
            get { return this._contestId; }
            set { _contestId = value; }
        }

        public string contestTitle
        {
            get
            {
                return this._contestTitle;
            }
            set
            {
                _contestTitle = value;
            }
        }

        public DateTime? contestPubDate
        {
            get
            {
                return this._contestPubDate;
            }
            set
            {
                _contestPubDate = value;
            }
        }

        public string contentType
        {
            get
            {
                return this._contentType;
            }
            set
            {
                _contentType = value;
            }
        }

        public DateTime entryDay
        {
            get
            {
                return this._entryDay;
            }
            set
            {
                _entryDay = value;
            }
        }

        public long? firstFileId
        {
            get
            {
                return this._firstFileId;
            }
            set
            {
                _firstFileId = value;
            }
        }

        public string firstFileUrl
        {
            get
            {
                return this._firstFileUrl;
            }
            set
            {
                _firstFileUrl = value;
            }
        }

        public int numberOfEntries
        {
            get
            {
                return this._numberOfEntries;
            }
            set
            {
                _numberOfEntries = value;
            }
        }


    }
}
