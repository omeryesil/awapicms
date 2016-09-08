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

namespace AWAPI.Admin.frames
{
    public partial class manageemailtemplates : AWAPI.App_Code.AdminBasePage
    {
        AWAPI_BusinessLibrary.library.EmailTemplateLib _emailTempLib = new AWAPI_BusinessLibrary.library.EmailTemplateLib();

        Int64 SiteId
        {
            get
            {
                if (Request["siteid"] == null)
                    return 0;
                return Convert.ToInt64(Request["siteid"]);
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _msg.Text = "";

            if (!IsPostBack)
                PopulateList();
        }

        void PopulateList()
        {
            _list.DataSource = _emailTempLib.GetList(App_Code.SessionInfo.CurrentSite.siteId);
            _list.DataBind();
        }

        void ResetControls()
        {
            _id.Text = "";
            _title.Text = "";
            _description.Text = "";
            _emailFrom.Text = "";
            _emailSubject.Text = "";
            _emailBody.Text = "";

            ShowHildeControls(false);
        }

        void ShowHildeControls(bool show)
        {
            btnDelete.Visible = show;
        }

        void Populate(Int64 catId)
        {

            AWAPI_Data.Data.awEmailTemplate t = _emailTempLib.Get(catId);

            _id.Text = t.emailTemplateId.ToString();
            _title.Text = t.title;
            _description.Text = t.description;
            _emailFrom.Text = t.emailFrom;
            _emailSubject.Text = t.emailSubject;
            _emailBody.Text = t.emailBody;

            ShowHildeControls(true);
        }

        protected void _list_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int64 id = Convert.ToInt64(e.CommandArgument.ToString());
            switch (e.CommandName.ToLower())
            {
                case "editemailtemplate":
                    Populate(id);
                    break;
            }
        }

        protected void btnNew_Click(object sender, ImageClickEventArgs e)
        {
            ResetControls();
        }

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            Save();
        }

        void Save()
        {

            long id = 0;
            AWAPI_Data.Data.awEmailTemplate t= new AWAPI_Data.Data.awEmailTemplate();

            try
            {
                t.title = _title.Text;
                t.description = _description.Text;
                t.emailFrom = _emailFrom.Text;
                t.emailSubject = _emailSubject.Text;
                t.emailBody = _emailBody.Text;
                t.siteId = App_Code.SessionInfo.CurrentSite.siteId;
                t.userId = App_Code.SessionInfo.CurrentUser.userId;

                //update current
                if (_id.Text == "")
                {
                    id = _emailTempLib.Add(t.siteId, t.userId.Value, t.title, t.description, t.emailFrom, t.emailSubject, t.emailBody);
                    _id.Text = id.ToString();
                }
                else
                {
                    id = Convert.ToInt64(_id.Text);
                    _emailTempLib.Update(id, t.userId.Value, t.title, t.description, t.emailFrom, t.emailSubject, t.emailBody);
                }

                ShowHildeControls(true);
                PopulateList();
                App_Code.Misc.WriteMessage(_msg, "Email template has been saved.");
            }
            catch (Exception ex)
            {
                _msg.Text = ex.Message;
                App_Code.Misc.WriteMessage(App_Code.Misc.MessageType.ERROR, _msg, ex.Message);
            }
        }

        protected void btnDelete_Click(object sender, ImageClickEventArgs e)
        {
            Delete();
            App_Code.Misc.WriteMessage(_msg, "Email template has been deleted.");
        }

        void Delete()
        {
            _emailTempLib.Delete(Convert.ToInt64(_id.Text));
            ResetControls();
            PopulateList();
        }



    }
}
