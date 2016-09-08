using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AWAPI_Data.CustomEntities
{
    public class ContentFormFieldSettingExtended
    {
        private long _contentFormFieldSettingId;
        private long _contentFormId;
        private string _title;
        private string _description;
        private string _staticFieldName;
        private string _fieldType;
        private int? _maximumLength;
        private bool _isContentCustomField;
        private long? _contentCustomFieldId;
        private int _sortOrder;
        private bool _isEnabled;
        private bool _isVisible;

        public long contentFormFieldSettingId
        {
            get { return _contentFormFieldSettingId; }
            set { _contentFormFieldSettingId = value; }
        }

        public long contentFormId
        {
            get { return _contentFormId; }
            set { _contentFormId = value; }
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

        public string staticFieldName
        {
            get { return _staticFieldName; }
            set { _staticFieldName = value; }
        }


        public string fieldType 
        {
            get { return _fieldType; }
            set { _fieldType = value; }
        }

        public int? maximumLength
        {
            get { return _maximumLength; }
            set { _maximumLength = value; }
        }

        public bool isContentCustomField
        {
            get { return _isContentCustomField; }
            set { _isContentCustomField = value; }
        }

        public long? contentCustomFieldId
        {
            get { return _contentCustomFieldId; }
            set { _contentCustomFieldId = value; }
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

        public bool isVisible
        {
            get { return _isVisible; }
            set { _isVisible = value; }
        }

    }
}
