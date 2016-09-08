using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AWAPI_Data.CustomEntities
{
    public class ContentCustomField
    {
        private long _fieldContentId;
        private string _fieldContentTitle;
        private long _valueContentId;
        private string _valueTitle;
        private long _customFieldId;
        private string _title;
        private string _description;
        private string _fieldType;
        private bool _applyToSubContents;
        private int? _maximumLength;
        private string _minimumValue;
        private string _maximumValue;
        private string _defaultValue;
        private string _regularExpression;
        private int _sortOrder;
        private bool _isEnabled;
        private DateTime? _createDate;


        public long fieldContentId
        {
            get { return _fieldContentId; }
            set { _fieldContentId = value; }
        }


        public string fieldContentTitle
        {
            get { return _fieldContentTitle; }
            set { _fieldContentTitle = value; }
        }

        public long valueContentId
        {
            get { return _valueContentId; }
            set { _valueContentId = value; }
        }

        public string valueTitle
        {
            get { return _valueTitle; }
            set { _valueTitle = value; }
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

        public string description
        {
            get { return _description; }
            set { _description = value; }
        }

        public string fieldType
        {
            get { return _fieldType; }
            set { _fieldType = value; }
        }

        public bool applyToSubContents
        {
            get { return _applyToSubContents; }
            set { _applyToSubContents = value; }
        }

        public int? maximumLength
        {
            get { return _maximumLength; }
            set { _maximumLength = value; }
        }

        public string minimumValue
        {
            get { return _minimumValue; }
            set { _minimumValue = value; }
        }

        public string maximumValue
        {
            get { return _maximumValue; }
            set { _maximumValue = value; }
        }

        public string defaultValue
        {
            get { return _defaultValue; }
            set { _defaultValue = value; }
        }

        public string regularExpression
        {
            get { return _regularExpression; }
            set { _regularExpression = value; }
        }

        public int sortOrder
        {
            get { return _sortOrder; }
            set { _sortOrder = value; }
        }

        public bool isEnabled
        {
            get { return _isEnabled; }
            set { _isEnabled = value; }
        }

        public DateTime? createDate
        {
            get { return _createDate; }
            set { _createDate = value;}
        }

    }
}
