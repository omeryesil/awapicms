using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

namespace AWAPI.Admin
{
    public partial class PageUsers : App_Code.AdminBasePage
    {
        AWAPI_BusinessLibrary.library.UserLibrary _userLib = new AWAPI_BusinessLibrary.library.UserLibrary();
        const string SITE_OWNER = " <i>(site owner)</li>";

        public PageUsers()
        {
            this.ModuleName = "user";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateUsers();

                PopulateCurrentUsersSites();
                ResetControls();

                if (Request["userid"] != null)
                    PopulateUser(Convert.ToInt64(Request["userid"]));
            }

            RegisterCustomScript();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            //if (App_Code.SessionInfo.CurrentSite == null)
            //{
            //    AdminMaster.WriteIframeMessage( AWAPI.Admin.AdminMaster.MessageType.ERROR, "You need to select a poll before managing roleGroups. <br/>" +
            //                "Please click <a href='javascript:void(0);' onclick=\"$.fn.colorbox({href:'/admin/frames/selectsite.aspx', open:true, width:'470px', height:'450px', iframe:true});\">here</a> to select a poll.");
            //    btnSaveUser.Visible = false;
            //    return; 
            //}
            //btnSaveUser.Visible = true;

        }

        void RegisterCustomScript()
        {
            System.Text.StringBuilder script = new System.Text.StringBuilder();

            script.Append("$(document).ready(function(){ \n");
            script.Append("    $(\"a[rel='selectfile']\").colorbox({width:'960', height:'600', iframe:true}); \n");
            script.Append("    $(\"a[rel='assignUserRoles']\").colorbox({width:'960', height:'600', iframe:true}); \n");
            script.Append("}); \n");

            if (script.Length > 0)
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "", script.ToString(), true);
        }

        void PopulateUsers()
        {
            string where = "";
            if (txbSearchUser.Text.Trim() != "")
            {
                where = " firstName LIKE '%" + txbSearchUser.Text + "%' OR lastName LIKE '%" + txbSearchUser.Text + "%' OR " +
                        " email LIKE '%" + txbSearchUser.Text + "%' ";
            }

            System.Collections.Generic.IList<AWAPI_Data.CustomEntities.UserExtended> list
                            = _userLib.GetList(App_Code.SessionInfo.CurrentSite.siteId, where, "");
            gwUserList.DataSource = list;
            gwUserList.DataBind();
        }

        void PopulateCurrentUsersSites()
        {
            AWAPI_BusinessLibrary.library.SiteLibrary siteLib = new AWAPI_BusinessLibrary.library.SiteLibrary();
            _siteList.Items.Clear();

            _siteList.DataTextField = "title";
            _siteList.DataValueField = "siteid";

            _siteList.DataSource = siteLib.GetUserSiteList(App_Code.SessionInfo.CurrentUser.userId);
            _siteList.DataBind();
        }

        protected void gwUserList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            long userID = Convert.ToInt64(e.CommandArgument.ToString());
            switch (e.CommandName.ToLower())
            {
                case "selectuser":
                    PopulateUser(userID);
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="show"></param>
        void ShowHideControls(bool show)
        {
            ShowHideControl(btnDeleteUser_, show);
            ShowHideControl(lblDeleteUser_, show);

            ShowHideControl(hplPopupAssignRole_, show);
            ShowHideControl(lblPopupAssignRole_, show);

            if (show)
                hplPopupAssignRole_.NavigateUrl = "~/admin/frames/assignuserroles.aspx?userid=" + _userId.Text;

            ShowHideForSuperAdmin();
        }

        void ShowHideForSuperAdmin()
        {
            bool show = true;

            //If current isn't super admin, and the user is super admin
            //then the current user cannot save/delete/update the superadmin
            if (!App_Code.SessionInfo.CurrentUser.isSuperAdmin &&
                _isSuperAdmin.Checked)
            {
                show = false;
                AdminMaster.WriteMessage(AWAPI.Admin.AdminMaster.MessageType.WARNING, "You cannot update SuperAdmin.");
            }

            ShowHideControl(btnSaveUser_, show);
            ShowHideControl(lblSaveUser_, show);

            if (!String.IsNullOrEmpty(_userId.Text))
            {
                ShowHideControl(btnDeleteUser_, show);
                ShowHideControl(lblDeleteUser_, show);
                ShowHideControl(hplPopupAssignRole_, show);
                ShowHideControl(lblPopupAssignRole_, show);
            }
        }


        void ResetControls()
        {
            _userId.Text = "";
            _userName.Text = "";
            _email.Text = "";
            _firstName.Text = "";
            _lastName.Text = "";
            _password.Text = "";
            _confirmPassword.Text = "";
            _description.Text = "";
            _imageUrl.Text = "";
            _image.ImageUrl = "";
            _isSuperAdmin.Checked = false;
            _enabled.Checked = false;

            _gender.SelectedIndex = 0;
            _birthday.Text = "";
            _tel.Text = "";
            _tel2.Text = "";
            _address.Text = "";
            _city.Text = "";
            _province.Text = "";
            _postalCode.Text = "";
            _country.Text = "";

            _isSuperAdmin.Enabled = App_Code.SessionInfo.CurrentUser.isSuperAdmin;

            for (int n = 0; n < _siteList.Items.Count; n++)
                _siteList.Items[n].Selected = false;

            ListItem li = _siteList.Items.FindByValue(App_Code.SessionInfo.CurrentSite.siteId.ToString());
            if (li != null)
                li.Selected = true;

            ShowHideControls(false);
        }

        void PopulateUser(long userId)
        {
            ResetControls();

            AWAPI_Data.CustomEntities.UserExtended user = new AWAPI_Data.CustomEntities.UserExtended();
            user = _userLib.Get(userId);

            if (user == null)
                return;

            _userName.Text = user.username;
            _email.Text = user.email.Trim();
            _userId.Text = user.userId.ToString();
            _firstName.Text = user.firstName.Trim();
            _lastName.Text = user.lastName.Trim();
            _password.Text = user.password.Trim();
            _confirmPassword.Text = user.password.Trim();
            _description.Text = user.description;
            _imageUrl.Text = user.imageurl;
            _image.ImageUrl = AWAPI_BusinessLibrary.library.FileLibrary.GetUrl(user.imageurl, "150x150"); // user.imageurl + "&size=150x150";
            _enabled.Checked = user.isEnabled;
            _isSuperAdmin.Checked = user.isSuperAdmin;

            if (user.gender != null && _gender.Items.FindByValue(user.gender) != null) _gender.SelectedValue = user.gender;
            if (user.birthday != null) _birthday.Text = user.birthday.Value.ToString("MM/dd/yyyy");
            _tel.Text = user.tel;
            _tel2.Text = user.tel2;
            _address.Text = user.address;
            _city.Text = user.city;
            _province.Text = user.state;
            _postalCode.Text = user.postalcode;
            _country.Text = user.country;

            //check tag's sites:
            AWAPI_BusinessLibrary.library.SiteLibrary siteLib = new AWAPI_BusinessLibrary.library.SiteLibrary();
            var usersSites = siteLib.GetUserSiteList(userId);
            if (usersSites != null)
            {
                for (int n = 0; n < _siteList.Items.Count; n++)
                {
                    _siteList.Items[n].Selected = false;
                    _siteList.Items[n].Enabled = true;
                    //_siteList.Items[n].Text.Replace(SITE_OWNER, "");

                    long siteId = Convert.ToInt64(_siteList.Items[n].Value);
                    var sites = from s in usersSites
                                where s.siteId.Equals(siteId)
                                select s;
                    if (sites != null && sites.Count() > 0)
                    {
                        _siteList.Items[n].Selected = true;
                        AWAPI_Data.Data.awSite site = new AWAPI_Data.Data.awSite();
                        site = sites.FirstOrDefault<AWAPI_Data.Data.awSite>();
                        if (site.userId == userId)
                        {
                            _siteList.Items[n].Enabled = false;
                            //_siteList.Items[n].Text += SITE_OWNER;
                        }
                    }
                }
            }

            ShowHideControls(true);

        }



        protected void btnNewUser_Click(object sender, EventArgs e)
        {
            ResetControls();
        }

        protected void btnDeleteUser_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                _userLib.Delete(Convert.ToInt64(_userId.Text), App_Code.SessionInfo.CurrentUser.userId);
                PopulateUsers();
                ResetControls();

                AdminMaster.WriteMessage(AWAPI.Admin.AdminMaster.MessageType.INFO, "User has been deleted successfully.");
            }
            catch (Exception ex)
            {
                AdminMaster.WriteMessage(AWAPI.Admin.AdminMaster.MessageType.ERROR, ex.Message);
            }
        }


        protected void btnSaveUser_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Save();
            }
            catch (Exception ex)
            {
                AdminMaster.WriteMessage(AWAPI.Admin.AdminMaster.MessageType.ERROR, ex.Message);
            }
        }

        void Save()
        {
            AWAPI_Data.CustomEntities.UserExtended usr = new AWAPI_Data.CustomEntities.UserExtended();

            long userId = 0;
            if (_userId.Text.Trim().Length > 0)
                userId = Convert.ToInt64(_userId.Text);


            //if this is a new tag, then password must be entered.
            if (userId == 0 && _password.Text.Trim() == "")
            {
                AdminMaster.WriteMessage(AWAPI.Admin.AdminMaster.MessageType.ERROR, "Password is required in order to create a tag.");
                return;
            }


            //at least one poll must be checked
            bool siteAdded = false;
            foreach (ListItem li in _siteList.Items)
            {
                if (li.Selected)
                {
                    siteAdded = true;
                    break;
                }
            }
            if (!siteAdded)
            {
                AdminMaster.WriteMessage(AWAPI.Admin.AdminMaster.MessageType.ERROR, "At least one site must be selected.");
                return;
            }

            usr.username = _userName.Text;
            usr.firstName = _firstName.Text;
            usr.lastName = _lastName.Text;
            usr.email = _email.Text;
            usr.imageurl = _imageUrl.Text;
            usr.link = "";
            usr.password = _password.Text;

            usr.description = _description.Text;
            usr.isEnabled = _enabled.Checked;
            usr.isSuperAdmin = _isSuperAdmin.Checked;

            usr.gender = _gender.SelectedValue;
            usr.birthday = AWAPI_Common.library.MiscLibrary.ConvertStringToDate(_birthday.Text);
            usr.tel2 = _tel.Text;
            usr.tel2 = _tel2.Text;
            usr.address = _address.Text;
            usr.city = _city.Text;
            usr.state = _province.Text;
            usr.postalcode = _postalCode.Text;
            usr.country = _country.Text;


            if (userId == 0)
            {
                userId = _userLib.Add(usr.username, usr.firstName, usr.lastName,
                                            usr.email, usr.password, usr.description,
                                            usr.isEnabled, usr.isSuperAdmin, usr.link, usr.imageurl,
                                            usr.gender, usr.birthday, usr.tel, usr.tel2, usr.address, usr.city, usr.state,
                                            usr.postalcode, usr.country);
                _userId.Text = userId.ToString();
            }
            else
            {
                _userLib.Update(userId, usr.username,
                                usr.firstName, usr.lastName, usr.email, usr.password,
                                usr.description, usr.isEnabled, usr.isSuperAdmin, usr.link, usr.imageurl,
                                usr.gender, usr.birthday, usr.tel, usr.tel2, usr.address, usr.city, usr.state, usr.postalcode, usr.country);
            }

            //add tag to the poll
            foreach (ListItem li in _siteList.Items)
            {
                long siteId = Convert.ToInt64(li.Value);
                //delete if 
                if (!li.Selected)
                    _userLib.DeleteUserFromSite(siteId, userId);
                else
                {
                    AWAPI_Data.Data.awSiteUser su = _userLib.GetUserFromSite(siteId, userId);
                    if (su == null)
                    {
                        _userLib.AddUserToSite(siteId, userId, true);
                    }
                }
            }
            _image.ImageUrl = AWAPI_BusinessLibrary.library.FileLibrary.GetUrl(usr.imageurl, "150x150");

            ShowHideControls(true);
            PopulateUsers();

            AdminMaster.WriteMessage(AWAPI.Admin.AdminMaster.MessageType.INFO, "User has been saved.");
        }

        protected void lbtnFilter_Click(object sender, EventArgs e)
        {
            PopulateUsers();
        }

        protected void lbtnClear_Click(object sender, EventArgs e)
        {
            txbSearchUser.Text = "";
            PopulateUsers();
        }

        protected void gwUserList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gwUserList.PageIndex = e.NewPageIndex;
            PopulateUsers();
        }

        protected void gwUserList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lbtn = (LinkButton)e.Row.FindControl("lbtnSelect");
                if (lbtn != null)
                {
                    bool enabled = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "isEnabled"));
                    if (!enabled)
                        lbtn.ForeColor = System.Drawing.Color.Gray;
                }
            }
        }

        protected void btnRefresh_Click(object sender, ImageClickEventArgs e)
        {
            PopulateUsers();
        }
    }
}
