using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using AWAPI_Data.Data;
using AWAPI_Common.library;
using AWAPI_Data.CustomEntities;


namespace AWAPI_BusinessLibrary.library
{
    public class UserLibrary
    {
        SiteContextDataContext _context = new SiteContextDataContext();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public AWAPI_Data.CustomEntities.UserExtended Get(long userId)
        {

            var user = from l in _context.awUsers
                       where l.userId.Equals(userId)
                       select new UserExtended
                       {
                           userId = l.userId,
                           username = l.username,
                           email = l.email,
                           password = l.password,
                           firstName = l.firstName,
                           lastName = l.lastName,
                           isSuperAdmin = l.isSuperAdmin,
                           isEnabled = l.isEnabled,
                           imageurl = l.imageurl,
                           link = l.link,
                           description = l.description,
                           gender = l.gender,
                           birthday = l.birthday,
                           tel = l.tel,
                           tel2 = l.tel2,
                           address = l.address,
                           fax = l.fax,
                           city = l.city,
                           state = l.state,
                           postalcode = l.postalcode,
                           lastBuildDate = l.lastBuildDate,
                           createDate = l.createDate,
                       };

            if (user != null && user.ToList().Count() > 0)
                return user.First<UserExtended>();
            else
                return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public UserExtended Login(string emailOrUsername, string password)
        {
            string encodedPass = new AWAPI_Common.library.SecurityLibrary().EncodeString(password);

            var user = from l in _context.awUsers
                       where (l.email.Equals(emailOrUsername) || l.username.Equals(emailOrUsername)) &&
                           l.password.Equals(encodedPass) &&
                           l.isEnabled
                       select new UserExtended
                       {
                           userId = l.userId,
                           username = l.username,
                           email = l.email,
                           password = l.password,
                           firstName = l.firstName,
                           lastName = l.lastName,
                           isSuperAdmin = l.isSuperAdmin,
                           isEnabled = l.isEnabled,
                           imageurl = l.imageurl,
                           link = l.link,
                           description = l.description,
                           gender = l.gender,
                           birthday = l.birthday,
                           tel = l.tel,
                           tel2 = l.tel2,
                           address = l.address,
                           fax = l.fax,
                           city = l.city,
                           state = l.state,
                           postalcode = l.postalcode,
                           lastBuildDate = l.lastBuildDate,
                           createDate = l.createDate,
                       };

            //_context.GetTable<awUser>()
            // .Where(st => (st.email.Equals(email) || st.username.Equals(userName)) &&
            //         st.password.Equals(encodedPass));

            if (user == null)
                return null;

            return user.FirstOrDefault();
        }

        public System.Collections.Generic.IEnumerable<UserExtended> GetList(string where, string sortField)
        {
            return GetList(0, where, sortField);
        }


        /// <summary>
        ///  Returns the current site owner and the roleGroups who are registered under this site.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public System.Collections.Generic.IList<UserExtended> GetList(long siteId, string where, string sortField)
        {
            //StringBuilder sb = new StringBuilder(" SELECT * FROM awUser ");

            //if (where.Trim().Length > 0)
            //    sb.Append(" WHERE " + where);

            //if (sortField.Trim().Length > 0)
            //    sb.Append(" ORDER BY " + sortField);
            //else
            //    sb.Append(" ORDER BY lastName, firstName, email");

            StringBuilder sb = new StringBuilder(" SELECT * FROM awUser ");
            sb.Append(" WHERE awUser.userId IN (SELECT userId FROM awSiteUser WHERE siteId='" + siteId + "') ");

            if (where.Trim().Length > 0) sb.Append(" AND (" + where + " ) ");

            if (sortField.Trim().Length > 0)
                sb.Append(" ORDER BY " + sortField);
            else
                sb.Append(" ORDER BY lastName, firstName, email");


            var users = from l in _context.ExecuteQuery<awUser>(sb.ToString())
                        select new UserExtended
                        {
                            userId = l.userId,
                            username = l.username,
                            email = l.email,
                            password = l.password,
                            firstName = l.firstName,
                            lastName = l.lastName,
                            isSuperAdmin = l.isSuperAdmin,
                            isEnabled = l.isEnabled,
                            imageurl = l.imageurl,
                            link = l.link,
                            description = l.description,
                            gender = l.gender,
                            birthday = l.birthday,
                            tel = l.tel,
                            tel2 = l.tel2,
                            address = l.address,
                            fax = l.fax,
                            city = l.city,
                            state = l.state,
                            postalcode = l.postalcode,
                            lastBuildDate = l.lastBuildDate,
                            createDate = l.createDate,
                        };

            if (users == null)
                return null;
            return users.ToList();
        }

        //TODO: Add functionality to delete the files and groups, and may be the sites.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId">UserId to be deleted</param>
        /// <param name="currentUserId">Who is deleting the user</param>
        public void Delete(long userId, long currentUserId)
        {
            if (userId <= 0)
                return;

            //Delete shareit 
            new ShareItLibrary().DeleteUserFromShareIt(userId);

            //Deletes users under the contest
            new ContestLibrary().DeleteContestEntriesByUserId(userId);


            //Remove File User relation
            new FileLibrary().RemoveFileUserRelation(userId, currentUserId);


            //Delete Site Users First
            DeleteSiteUsersByUserId(userId);

            //Delete User roles
            var rights = from r in _context.awRoleMembers
                         where r.userId.Equals(userId)
                         select r;
            _context.awRoleMembers.DeleteAllOnSubmit(rights);

            //Then delete the User
            var users = from f in _context.awUsers
                        where f.userId.Equals(userId)
                        select f;

            _context.awUsers.DeleteAllOnSubmit(users);
            _context.SubmitChanges();
        }

        public bool ValidateUserInfo(long userId, string userName, string email, string password)
        {
            //Check username
            var usernameExist = from u in _context.awUsers
                                where u.username.ToLower().Equals(userName.ToLower())
                                    && u.userId != userId
                                select u;
            if (usernameExist != null && usernameExist.Count() > 0)
                throw new Exception(ErrorLibrary.ErrorMessage(ErrorLibrary.USER.USERNAME_IN_USE));


            //check email 
            var emailExists = from u in _context.awUsers
                              where u.email.ToLower().Equals(email.ToLower())
                                  && u.userId != userId
                              select u;
            if (emailExists != null && emailExists.Count() > 0)
                throw new Exception(ErrorLibrary.ErrorMessage(ErrorLibrary.USER.EMAIL_IN_USE));


            //check password
            if (password != null)
                if (password.Trim().Length < 6 || password.Trim().Length > 30)
                    throw new Exception(ErrorLibrary.ErrorMessage(ErrorLibrary.USER.WRONG_PASSWORD_FORMAT));

            return true;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="description"></param>
        /// <param name="isEnabled"></param>
        /// <param name="link"></param>
        /// <param name="imageurl"></param>
        /// <returns></returns>
        public long Add(string userName, string firstName, string lastName,
                        string email, string password, string description,
                        bool isEnabled, bool isSuperAdmin, string link,
                        string imageurl,
                        string gender, DateTime? birthday, string tel, string tel2,
                        string address, string city, string state, string postalCode,
                        string country)
        {
            //if username is not set, then create a unique one...
            if (userName.Trim() == "")
                userName = AWAPI_Common.library.MiscLibrary.CreateUniqueId().ToString();

            //throws error if username or email already exists
            ValidateUserInfo(0, userName, email, password);

            //insert the user 
            long id = AWAPI_Common.library.MiscLibrary.CreateUniqueId();
            awUser usr = new awUser();

            usr.userId = id;
            usr.username = userName;
            usr.firstName = firstName;
            usr.lastName = lastName;
            usr.email = email;
            usr.password = new AWAPI_Common.library.SecurityLibrary().EncodeString(password);
            usr.description = description;
            usr.isEnabled = isEnabled;
            usr.isSuperAdmin = isSuperAdmin;
            usr.link = link;
            usr.imageurl = imageurl;

            usr.birthday = birthday;
            usr.gender = gender;
            usr.tel = tel;
            usr.tel2 = tel2;
            usr.address = address;
            usr.city = city;
            usr.state = state;
            usr.postalcode = postalCode;
            usr.country = country;

            usr.lastBuildDate = DateTime.Now;
            usr.createDate = DateTime.Now;

            _context.awUsers.InsertOnSubmit(usr);
            _context.SubmitChanges();

            return id;
        }

        /// <summary>
        /// Updates user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="description"></param>
        /// <param name="isEnabled"></param>
        /// <param name="isSuperAdmin"></param>
        /// <param name="link"></param>
        /// <param name="imageurl"></param>
        /// <param name="gender"></param>
        /// <param name="birthday"></param>
        /// <param name="tel"></param>
        /// <param name="tel2"></param>
        /// <param name="address"></param>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="postalCode"></param>
        /// <param name="country"></param>
        /// <returns></returns>
        public bool Update(long userId, string userName, string firstName, string lastName,
                        string email, string password, string description,
                        bool? isEnabled, bool? isSuperAdmin, string link,
                        string imageurl,
                        string gender, DateTime? birthday, string tel, string tel2,
                        string address, string city, string state, string postalCode,
                        string country)
        {


            //throws error if username or email already exists for another user
            bool updatePassword = password.Trim().Length > 0;
            ValidateUserInfo(userId, userName, email, updatePassword ? password : null);

            awUser usr = _context.awUsers.FirstOrDefault(st => st.userId.Equals(userId));

            if (usr == null)
                return false;

            usr.username = userName;
            usr.firstName = firstName;
            usr.lastName = lastName;
            usr.email = email;
            if (updatePassword)
                usr.password = new AWAPI_Common.library.SecurityLibrary().EncodeString(password);

            usr.description = description;
            usr.isEnabled = isEnabled == null ? usr.isEnabled : isEnabled.Value;
            usr.isSuperAdmin = isSuperAdmin == null ? usr.isSuperAdmin : isSuperAdmin.Value; ;
            usr.link = link;
            usr.imageurl = imageurl;

            usr.birthday = birthday;
            usr.gender = gender;
            usr.tel = tel;
            usr.tel2 = tel2;
            usr.address = address;
            usr.city = city;
            usr.state = state;
            usr.country = country;
            usr.postalcode = postalCode;

            usr.lastBuildDate = DateTime.Now;

            _context.SubmitChanges();

            return true;
        }


        /// <summary>
        /// Updates user's password
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="passworc"></param>
        /// <returns></returns>
        public bool UpdateStatus(long userId, bool isEnabled)
        {
            awUser usr = _context.awUsers.FirstOrDefault(st => st.userId.Equals(userId));

            if (usr == null)
                return false;

            usr.isEnabled = isEnabled;
            usr.lastBuildDate = DateTime.Now;

            _context.SubmitChanges();
            return true;
        }


        /// <summary>
        /// Updates user's password
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="passworc"></param>
        /// <returns></returns>
        public bool UpdatePassword(long userId, string password)
        {
            if (password.Trim().Length < 6 || password.Trim().Length > 30)
                throw new Exception(ErrorLibrary.ErrorMessage(ErrorLibrary.USER.WRONG_PASSWORD_FORMAT));

            awUser usr = _context.awUsers.FirstOrDefault(st => st.userId.Equals(userId));

            if (usr == null)
                return false;

            usr.password = new AWAPI_Common.library.SecurityLibrary().EncodeString(password);
            usr.lastBuildDate = DateTime.Now;

            _context.SubmitChanges();

            return true;
        }

        public void ResetPassword(long siteId, string email, string redirectLink)
        {
            awSite site = new SiteLibrary().Get(siteId);
            if (site == null || !site.isEnabled)
                throw new Exception(ErrorLibrary.ErrorMessage(ErrorLibrary.SITE.DOES_NOT_EXIST));

            if (site.userResetPasswordEmailTemplateId == null )
                throw new Exception(ErrorLibrary.ErrorMessage(ErrorLibrary.SITE.RESET_PASSWORD_TEMPLATE_DOES_NOT_EXIST));

            //GET THE USER
            awUser user = (from l in _context.awUsers
                       where l.email.Equals(email) && l.isEnabled != false
                       select l).FirstOrDefault();

            if (user == null)
                throw new Exception (ErrorLibrary.ErrorMessage(ErrorLibrary.USER.DOES_NOT_EXIST));


            //CREATE NEW PASSWORD 
            const int PASSWORD_LENGTH = 6;
            string _allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            Random randNum = new Random();
            char[] chars = new char[PASSWORD_LENGTH];
            int allowedCharCount = _allowedChars.Length;

            for (int i = 0; i < PASSWORD_LENGTH; i++)
                chars[i] = _allowedChars[(int)((_allowedChars.Length) * randNum.NextDouble())];
            string password = new string(chars);


            //FIRST SEND EMAIL BEFORE RESETING THE PASSWORD
            AWAPI_BusinessLibrary.library.EmailTemplateLib emailLib = new EmailTemplateLib();
            awEmailTemplate template = emailLib.Get(site.userResetPasswordEmailTemplateId.Value);
            if (template == null)
                throw new Exception(ErrorLibrary.ErrorMessage(ErrorLibrary.SITE.RESET_PASSWORD_TEMPLATE_DOES_NOT_EXIST));

            emailLib.Send(site.userResetPasswordEmailTemplateId.Value, email,
                            "firstname|" + user.firstName,
                            "lastname|" + user.lastName,
                            "password|" + password,
                            "link|" + redirectLink,
                            "date|" + DateTime.Now.ToString());
            
            //UPDATE PASSWORD
            UpdatePassword(user.userId, password);

        }


        #region SITE USERS
        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public awSiteUser GetUserFromSite(long siteId, long userId)
        {
            awSiteUser siteUser = _context.awSiteUsers.FirstOrDefault(su => su.siteId.Equals(siteId) && su.userId.Equals(userId));
            return siteUser;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        public void AddUserToSite(long siteId, long userId, bool isEnabled)
        {
            awSiteUser usr = new awSiteUser();

            //check if it is already exist, if it is update the current one
            var users = from f in _context.awSiteUsers
                        where f.userId.Equals(userId) && f.siteId.Equals(siteId)
                        select f;
            if (users != null && users.ToList().Count > 0)
            {
                UpdateUserInSite(siteId, userId, isEnabled);
                return;
            }

            usr.siteUserId = AWAPI_Common.library.MiscLibrary.CreateUniqueId();
            usr.siteId = siteId;
            usr.userId = userId;
            usr.isEnabled = isEnabled;
            usr.joinDate = DateTime.Now;
            usr.lastBuildDate = DateTime.Now;
            usr.createDate = DateTime.Now;

            _context.awSiteUsers.InsertOnSubmit(usr);
            _context.SubmitChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="userId"></param>
        /// <param name="isEnabled"></param>
        public void UpdateUserInSite(long siteId, long userId, bool isEnabled)
        {
            awSiteUser usr = _context.awSiteUsers.
                        FirstOrDefault(st => st.siteId.Equals(siteId) && st.userId.Equals(userId));

            if (usr == null)
                return;

            usr.isEnabled = isEnabled;
            usr.lastBuildDate = DateTime.Now;

            _context.SubmitChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="userId"></param>
        public void DeleteUserFromSite(long siteId, long userId)
        {
            if (userId <= 0)
                return;

            var users = from f in _context.awSiteUsers
                        where f.userId.Equals(userId) && f.siteId.Equals(siteId)
                        select f;

            _context.awSiteUsers.DeleteAllOnSubmit(users);
            _context.SubmitChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        public void DeleteSiteUsersByUserId(long userId)
        {
            if (userId <= 0)
                return;

            var users = from f in _context.awSiteUsers
                        where f.userId.Equals(userId)
                        select f;

            _context.awSiteUsers.DeleteAllOnSubmit(users);
            _context.SubmitChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        public void DeleteSiteUsersBySiteId(long siteId)
        {
            if (siteId <= 0)
                return;

            var users = from f in _context.awSiteUsers
                        where f.siteId.Equals(siteId)
                        select f;

            _context.awSiteUsers.DeleteAllOnSubmit(users);
            _context.SubmitChanges();
        }

        #endregion

    }
}
