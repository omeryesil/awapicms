using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;

namespace AWAPI.Admin.frames
{
    public partial class manageshareit : AWAPI.App_Code.AdminBasePage
    {
        AWAPI_BusinessLibrary.library.ShareItLibrary _shareItLib = new AWAPI_BusinessLibrary.library.ShareItLibrary();

        string Link
        {
            get
            {
                if (Request["Link"] == null)
                    return "";
                return Server.UrlDecode(Request["Link"].ToString());
            }

        }

        public manageshareit()
        {
            //ModuleName = "manage";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _msg.Text = "";

            if (!IsPostBack)
            {
                ResetControls();
                PopulateList();
                PopulateUserList();
            }
        }


        void ResetControls()
        {
            _id.Text = "";
            _description.Text = "";
            _title.Text = "";
            _link.Text = Link;
            foreach (ListItem li in _userList.Items)
                li.Selected = false;

            ShowHildeControls(false);
        }


        void ShowHildeControls(bool show)
        {
            ShowHideControl(btnDeleteShareIt_, show);
        }

        void PopulateList()
        {
            _list.DataSource = _shareItLib.GetList(App_Code.SessionInfo.CurrentSite.siteId, App_Code.SessionInfo.CurrentUser.userId);
            _list.DataBind();
        }

        void PopulateUserList()
        {
            _userList.Items.Clear();
            System.Collections.Generic.IList<AWAPI_Data.CustomEntities.UserExtended> list = new AWAPI_BusinessLibrary.library.UserLibrary().
                                                                GetList(App_Code.SessionInfo.CurrentSite.siteId, "isEnabled=1", "firstname, lastname");
            if (list == null)
                return;
            foreach (AWAPI_Data.CustomEntities.UserExtended usr in list)
                if (usr.userId != App_Code.SessionInfo.CurrentUser.userId)
                    _userList.Items.Add(new ListItem(usr.firstName + " " + usr.lastName + " - " + usr.email , 
                                        usr.userId.ToString()));
        }

        void PopulateSelectedUserList(long shareItId)
        {
            foreach (ListItem li in _userList.Items)
                li.Selected = false;

            IList<AWAPI_Data.Data.awUser_ShareIt> list = _shareItLib.GetSharedWithUserList(shareItId);

            if (list == null)
                return;

            foreach (AWAPI_Data.Data.awUser_ShareIt usr in list)
            {
                ListItem li = _userList.Items.FindByValue(usr.userId.ToString());
                if (li != null)
                    li.Selected = true;
            }
        }


        void PopulateShareIt(long shareItId)
        {
            ResetControls();

            AWAPI_Data.Data.awShareIt shareIt = _shareItLib.Get(shareItId);
            if (shareIt != null)
            {
                _id.Text = shareIt.shareItId.ToString();
                _title.Text = shareIt.title;
                _description.Text = shareIt.description;
                _link.Text = shareIt.link;
                _shareWithEveryone.Checked = shareIt.shareWithEveryone;

                PopulateSelectedUserList(shareIt.shareItId);

                ShowHildeControls(true);

            }
        }



        protected void _list_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int64 id = Convert.ToInt64(e.CommandArgument.ToString());
            switch (e.CommandName.ToLower())
            {
                case "editshareit":
                    PopulateShareIt(id);
                    break;

            }
        }

        protected void btnNew_Click(object sender, ImageClickEventArgs e)
        {
            ResetControls();
        }


        void DeleteShareIt(long shareItId)
        {
            _shareItLib.Delete(shareItId);
            ResetControls();
            PopulateList();

            App_Code.Misc.WriteMessage(_msg, "Record has been deleted.");
        }

        protected void btnSaveShareIt_Click(object sender, ImageClickEventArgs e)
        {
            long shareItId = 0;


            ArrayList userIds = new ArrayList();
            foreach (ListItem li in _userList.Items)
                if (li.Selected)
                    userIds.Add(Convert.ToInt64(li.Value));

            if (userIds.Count == 0)
            {
                App_Code.Misc.WriteMessage(AWAPI.App_Code.Misc.MessageType.ERROR, _msg, "At least one user must be selected.");
                return;
            }

            if (_id.Text.Trim().Length == 0) //add new one
            {
                shareItId = _shareItLib.Add(App_Code.SessionInfo.CurrentSite.siteId,
                                        App_Code.SessionInfo.CurrentUser.userId,
                                        _title.Text, _description.Text, _link.Text, _shareWithEveryone.Checked, (long[])userIds.ToArray(typeof(long)));

            }
            else //update the current one
            {
                shareItId = Convert.ToInt64(_id.Text);
                _shareItLib.Update(shareItId, _title.Text, _description.Text, _link.Text, _shareWithEveryone.Checked, (long[])userIds.ToArray(typeof(long)));
            }

            PopulateList();
            PopulateShareIt(shareItId);

            App_Code.Misc.WriteMessage(_msg, "Record has been saved.");
        }

        protected void btnDeleteShareIt_Click(object sender, ImageClickEventArgs e)
        {
            if (_id.Text.Trim().Length > 0)
                DeleteShareIt(Convert.ToInt64(_id.Text));

        }

        protected void btnNewShareIt_Click(object sender, EventArgs e)
        {
            ResetControls();
        }



    }
}
