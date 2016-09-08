using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using AWAPI_Data.Data;
using AWAPI_Common.library;


namespace AWAPI_BusinessLibrary.library
{
    public class RoleLibrary
    {
        SiteContextDataContext _context = new SiteContextDataContext();

        public enum Module
        {
            blog,
            blogpost,
            blogpostcomment,
            content,
            contentForm,
            contest,
            file,
            filegroup,
            poll,
            user,
            managerole,
            assignrole,
            datatransfer
        }

        #region ROLES
        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public awRole Get(long roleId)
        {

            var role = _context.GetTable<awRole>()
                            .Where(st => st.roleId.Equals(roleId));

            if (role != null && role.ToList().Count() > 0)
                return role.First<awRole>();
            else
                return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public System.Collections.Generic.IList<awRole> GetList()
        {
            var list = from rg in _context.awRoles
                       orderby rg.title
                       select rg;

            if (list == null)
                return null;
            return list.ToList();
        }
        #endregion


        #region ROLE MEMBERS
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public awRole GetUserRole(long roleId, long siteId, long userId)
        {
            var userRole = from r in _context.awRoles
                           join rm in _context.awRoleMembers on r.roleId equals rm.roleId
                           where rm.siteId.Equals(siteId) && rm.userId.Equals(userId) && rm.roleId.Equals(roleId)
                           select r;

            if (userRole == null)
                return null;

            return userRole.FirstOrDefault<awRole>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public System.Collections.Generic.IList<awRole> GetUserRoleList(long userId, long siteId)
        {
            var userRoles = from rg in _context.awRoles
                            join rgm in _context.awRoleMembers on rg.roleId equals rgm.roleId
                            where rgm.userId.Equals(userId) && rgm.siteId.Equals(siteId)
                            select rg;

            if (userRoles == null || userRoles.ToList().Count == 0)
                return null;

            return userRoles.ToList<awRole>();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="siteId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public awRoleMember UserHasRole(long roleId, long siteId, long userId)
        {

            var rm = from r in _context.awRoleMembers
                     where r.userId.Equals(userId) &&
                         r.siteId.Equals(siteId) &&
                         r.roleId.Equals(roleId)
                     select r;
            if (rm == null || rm.Count() == 0)
                return null;
            return rm.FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="siteId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public long AssignRoleToUser(long roleId, long siteId, long userId)
        {
            awRoleMember rmExist = UserHasRole(roleId, siteId, userId);

            if (rmExist != null)
                return rmExist.roleMemberId;

            awRoleMember rm = new awRoleMember();
            rm.roleMemberId = AWAPI_Common.library.MiscLibrary.CreateUniqueId();
            rm.siteId = siteId;
            rm.userId = userId;
            rm.roleId = roleId;

            _context.awRoleMembers.InsertOnSubmit(rm);
            _context.SubmitChanges();

            return rm.roleMemberId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="siteId"></param>
        /// <param name="userId"></param>
        public void RemoveRoleFromUser(long roleId, long siteId, long userId)
        {
            awRoleMember rmExist = UserHasRole(roleId, siteId, userId);

            if (rmExist == null)
                return;
            _context.awRoleMembers.DeleteOnSubmit(rmExist);
            _context.SubmitChanges();
        }

        #endregion

    }
}
