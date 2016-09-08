using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AWAPI_Data.CustomEntities
{
    public class UserExtended
    {

        private long _userId;
        private string _username;
        private string _email;
        private string _password;
        private string _firstName;
        private string _lastName;
        private bool _isSuperAdmin;
        private bool _isEnabled;
        private string _imageurl;
        private string _link;
        private string _description;
        private string _gender;
        private System.Nullable<System.DateTime> _birthday;
        private string _tel;
        private string _tel2;
        private string _address;
        private string _fax;
        private string _city;
        private string _state;
        private string _postalcode;
        private string _country;
        private System.Nullable<System.DateTime> _lastBuildDate;
        private System.DateTime _createDate;

        public long userId
        {
            get
            { return this._userId; }
            set { _userId = value; }
        }

        public string username
        {
            get { return this._username; }
            set { _username = value; }
        }

        public string email
        {
            get { return this._email; }
            set { _email = value; }
        }

        public string password
        {
            get
            { return this._password; }
            set { _password = value; }
        }

        public string firstName
        {
            get { return this._firstName; }
            set { _firstName = value; }
        }

        public string lastName
        {
            get { return this._lastName; }
            set { _lastName = value; }
        }

        public bool isSuperAdmin
        {
            get { return this._isSuperAdmin; }
            set { _isSuperAdmin = value; }
        }

        public bool isEnabled
        {
            get { return this._isEnabled; }
            set { _isEnabled = value; }
        }

        public string imageurl
        {
            get { return this._imageurl; }
            set { _imageurl = value; }
        }

        public string link
        {
            get { return this._link; }
            set { _link = value; }
        }

        public string description
        {
            get { return this._description; }
            set { _description = value; }
        }

        public string gender
        {
            get { return this._gender; }
            set { _gender = value; }
        }

        public System.Nullable<System.DateTime> birthday
        {
            get { return this._birthday; }
            set { _birthday = value; }
        }

        public string tel
        {
            get { return this._tel; }
            set { _tel = value; }
        }

        public string tel2
        {
            get { return this._tel2; }
            set { _tel2 = value; }
        }

        public string address
        {
            get { return this._address; }
            set { _address = value; }
        }

        public string fax
        {
            get { return this._fax; }
            set { _fax = value; }
        }

        public string city
        {
            get { return this._city; }
            set { _city = value; }
        }

        public string state
        {
            get { return this._state; }
            set { _state = value; }
        }

        public string postalcode
        {
            get { return this._postalcode; }
            set { _postalcode = value; }
        }

        public string country
        {
            get { return this._country; }
            set { _country = value; }
        }

        public System.Nullable<System.DateTime> lastBuildDate
        {
            get { return this._lastBuildDate; }
            set { _lastBuildDate = value; }
        }

        public System.DateTime createDate
        {
            get { return this._createDate; }
            set { _createDate = value; }
        }


    }
}
