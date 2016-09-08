using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AWAPI_Data.CustomEntities
{
    public class ContentCustomFieldValueExtended
    {
        private long _customFieldValueId;
        private long _customFieldId;
        private string _title;
        private long _contentId;
        private string _cultureCode;
        private string _fieldValue;
        private long _userId;
        private System.Nullable<System.DateTime> _lastBuildDate;
        private System.DateTime _createDate;

        public long customFieldValueId
        {
            get { return _customFieldValueId; }
            set { _customFieldValueId = value; }
        }
        public long customFieldId
        {
            get { return _customFieldId; }
            set { _customFieldId = value; }
        }

        public string title
        {
            get { return _title; }
            set { _title = value; }
        }

        public long contentId
        {
            get { return _contentId; }
            set { _contentId = value; }
        }
        public string cultureCode
        {
            get { return _cultureCode; }
            set { _cultureCode = value; }
        }

        public string fieldValue
        {
            get { return _fieldValue; }
            set { _fieldValue = value; }
        }

        public long userId
        {
            get { return _userId; }
            set { _userId = value; }
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
