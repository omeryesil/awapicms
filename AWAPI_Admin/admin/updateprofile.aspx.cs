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
    public partial class PageUpdateProfile : App_Code.AdminBasePage
    {
        AWAPI_BusinessLibrary.library.UserLibrary _userLib = new AWAPI_BusinessLibrary.library.UserLibrary();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                PopulateUser();

            RegisterCustomScript();
        }

        void RegisterCustomScript()
        {
            System.Text.StringBuilder script = new System.Text.StringBuilder();

            script.Append("$(document).ready(function(){ \n");
            script.Append("    $(\"a[rel='selectfile']\").colorbox({width:'960', height:'600', iframe:true}); \n");
            script.Append("}); \n");

            if (script.Length > 0)
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "", script.ToString(), true);
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

            _password.Attributes.Remove("value");
            _confirmPassword.Attributes.Remove("value");

            _description.Text = "";
            _imageUrl.Text = "";
            _image.ImageUrl = "";

            _gender.SelectedIndex = 0;
            _birthday.Text = "";
            _tel.Text = "";
            _tel2.Text = "";
            _address.Text = "";
            _city.Text = "";
            _province.Text = "";
            _postalCode.Text = "";
            _country.Text = "";

        }

        void PopulateUser()
        {
            ResetControls();

            AWAPI_Data.CustomEntities.UserExtended user = _userLib.Get(App_Code.SessionInfo.CurrentUser.userId);

            if (user == null)
                return;

            _userName.Text = user.username;
            _email.Text = user.email.Trim();
            _userId.Text = user.userId.ToString();
            _firstName.Text = user.firstName.Trim();
            _lastName.Text = user.lastName.Trim();
            _password.Text = user.password.Trim();
            _confirmPassword.Text = user.password.Trim();

            //_password.Attributes.Add("value", user.password.Trim());
            //_confirmPassword.Attributes.Add("value", user.password.Trim());
            
            _description.Text = user.description;
            _imageUrl.Text = user.imageurl;
            _image.ImageUrl = AWAPI_BusinessLibrary.library.FileLibrary.GetUrl(user.imageurl, "150x150"); // user.imageurl + "&size=150x150";

            if (user.gender != null && _gender.Items.FindByValue(user.gender) != null) _gender.SelectedValue = user.gender;
            if (user.birthday != null) _birthday.Text = user.birthday.Value.ToString("MM/dd/yyyy");
            _tel.Text = user.tel;
            _tel2.Text = user.tel2;
            _address.Text = user.address;
            _city.Text = user.city;
            _province.Text = user.state;
            _postalCode.Text = user.postalcode;
            _country.Text = user.country;
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
            AWAPI_Data.CustomEntities.UserExtended usr = _userLib.Get(App_Code.SessionInfo.CurrentUser.userId);
            usr.username = _userName.Text;
            usr.firstName = _firstName.Text;
            usr.lastName = _lastName.Text;
            usr.email = _email.Text;
            usr.imageurl = _imageUrl.Text;
            usr.link = "";
            usr.password =  _password.Text.Trim();
            usr.description = _description.Text;
            usr.gender = _gender.SelectedValue;
            usr.birthday = AWAPI_Common.library.MiscLibrary.ConvertStringToDate(_birthday.Text);
            usr.tel = _tel.Text;
            usr.tel2 = _tel2.Text;
            usr.address = _address.Text;
            usr.city = _city.Text;
            usr.state = _province.Text;
            usr.postalcode = _postalCode.Text;
            usr.country = _country.Text;

            _userLib.Update(usr.userId, usr.username,
                            usr.firstName, usr.lastName, usr.email, usr.password,
                            usr.description, usr.isEnabled, usr.isSuperAdmin, usr.link, usr.imageurl,
                            usr.gender, usr.birthday, usr.tel, usr.tel2, usr.address, usr.city, usr.state, usr.postalcode, usr.country);


            _image.ImageUrl = AWAPI_BusinessLibrary.library.FileLibrary.GetUrl(usr.imageurl, "150x150"); // user.imageurl + "&size=150x150";                 

            App_Code.SessionInfo.CurrentUser = _userLib.Get(usr.userId);


            AdminMaster.WriteMessage(AWAPI.Admin.AdminMaster.MessageType.INFO, "Profile has been updated.");
        }

    }
}
