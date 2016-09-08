using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AWAPI_Data.CustomEntities
{
    public class ContestEntryExtended
    {
        private long _contestEntryId;
        private long _contestId;
        private System.Nullable<long> _userId;
        private bool _isEnabled;
        private string _cultureCode;
        private string _email;
        private string _firstName;
        private string _lastName;
        private string _title;
        private string _description;
        private System.Nullable<long> _fileId;
        private string _fileUrl;
        private string _fileThumbUrl;
        private string _fileContentType;
        private bool _fileEnabled;
        private string _tel;
        private string _telType;
        private string _address;
        private string _city;
        private string _province;
        private string _postalCode;
        private string _country;
        private System.DateTime _createDate;
        private int _daysPassed;

        public long contestEntryId
        {
            get
            {
                return _contestEntryId;
            }
            set
            {
                _contestEntryId = value;
            }
        }

        public long contestId
        {
            get
            {
                return _contestId;
            }
            set
            {
                _contestId = value;
            }
        }

        public System.Nullable<long> userId
        {
            get
            {
                return _userId;
            }
            set
            {
                _userId = value;
            }
        }

        public bool isEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                _isEnabled = value;
            }
        }


        
        public string cultureCode
        {
            get
            {
                return _cultureCode;
            }
            set
            {
                _cultureCode = value;
            }
        }

        public string email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
            }
        }

        public string firstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                _firstName = value;
            }
        }

        public string lastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                _lastName = value;
            }
        }

        public string title
        {
            get
            {
                return _title;
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
                return _description;
            }
            set
            {
                _description = value;
            }
        }

        public System.Nullable<long> fileId
        {
            get
            {
                return _fileId;
            }
            set
            {
                _fileId = value;
            }
        }

        public string fileUrl
        {
            get
            {
                return _fileUrl;
            }
            set
            {
                _fileUrl = value;
            }
        }

        public string fileContentType
        {
            get
            {
                return _fileContentType;
            }
            set
            {
                _fileContentType = value;
            }
        }


        public string fileThumbUrl
        {
            get
            {
                return _fileThumbUrl;
            }
            set
            {
                _fileThumbUrl = value;
            }
        }

        public bool fileEnabled
        {
            get
            {
                return _fileEnabled;
            }
            set
            {
                _fileEnabled = value;
            }
        }

        public string tel
        {
            get
            {
                return _tel;
            }
            set
            {
                _tel = value;
            }
        }
        public string telType
        {
            get
            {
                return _telType;
            }
            set
            {
                _telType = value;
            }
        }

        public string address
        {
            get
            {
                return _address;
            }
            set
            {
                _address = value;
            }
        }

        public string city
        {
            get
            {
                return _city;
            }
            set
            {
                _city = value;
            }
        }
        public string province
        {
            get
            {
                return _province;
            }
            set
            {
                _province = value;
            }
        }
        public string postalCode
        {
            get
            {
                return _postalCode;
            }
            set
            {
                _postalCode = value;
            }
        }
        public string country
        {
            get
            {
                return _country;
            }
            set
            {
                _country = value;
            }
        }
        public System.DateTime createDate
        {
            get
            {
                return _createDate;
            }
            set
            {
                _createDate = value;
            }
        }

        public int daysPassed
        {
            get {
                return _daysPassed;
            }
            set
            {
                _daysPassed = value;
            }
        }


    }
}
