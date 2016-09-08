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

namespace  AWAPI.Admin.frames
{
    public partial class managecontentform : AWAPI.App_Code.AdminBasePage
    {
        AWAPI_BusinessLibrary.library.ContentLibrary _contentLib = new AWAPI_BusinessLibrary.library.ContentLibrary();
        AWAPI_BusinessLibrary.library.ContentCustomFieldLibrary _customLib = new AWAPI_BusinessLibrary.library.ContentCustomFieldLibrary();
        AWAPI_BusinessLibrary.library.ContentFormLibrary _formLib = new AWAPI_BusinessLibrary.library.ContentFormLibrary();

        Int64 ContentId
        {
            get
            {
                if (Request["contentid"] == null)
                    return 0;
                return Convert.ToInt64(Request["contentid"]);
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _msg.Text = "";

            if (!IsPostBack && ContentId>0)
                PopulateForm();
        }


        void PopulateForm()
        {
            ResetControls();

            //Get Content Info
            AWAPI_Data.Data.awContent content = _contentLib.Get(ContentId);
            if (content == null)
                return;

            lblTitle.Text = "Manage Content Form - " + content.alias;

            //Get Form Info
            AWAPI_Data.Data.awContentForm form = _formLib.GetByContentId(ContentId, false);
            if (form == null)
                return;

            _contentFormId.Text = form.contentFormId.ToString();
            _title.Text = form.title;
            _description.Text = form.description;
            _isEnabled.Checked = form.isEnabled;
            _applyToSubContent.Checked = form.applyToSub;
            _canCreateNew.Checked = form.canCreateNew;
            _canUpdate.Checked = form.canUpdate;
            _canDelete.Checked = form.canDelete;
            ShowHildeControls(true);

            PopulateFields(form.contentFormId);
        }

        void PopulateFields(Int64 contentFormId)
        {
            _fieldList.DataSource = _formLib.GetFieldList(contentFormId);
            _fieldList.DataBind();
        }


        void ResetControls()
        {
            _contentFormId.Text = "";
            _title.Text = "";
            _description.Text = "";
            _isEnabled.Checked = false;
            _applyToSubContent.Checked = false;
            _canCreateNew.Checked = false;
            _canUpdate.Checked = true;
            _canDelete.Checked = false;

            _fieldList.DataSource = null;
            _fieldList.DataBind();

            ShowHildeControls(false);
        }

        void ShowHildeControls(bool show)
        {
            btnDeleteContentForm_.Visible = show;
            btnSyncFormFields_.Visible = show;
        }

        void DeleteContentForm()
        {
            _formLib.Delete(Convert.ToInt64(_contentFormId.Text));
            App_Code.Misc.WriteMessage(App_Code.Misc.MessageType.INFO, _msg, "Form has been deleted.");
            
            ResetControls();
        }

        protected void btnSaveContentForm__Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                SaveForm();
                App_Code.Misc.WriteMessage(App_Code.Misc.MessageType.INFO, _msg, "Form has been saved.");
            }
            catch (Exception ex)
            {
                App_Code.Misc.WriteMessage(App_Code.Misc.MessageType.ERROR, _msg, ex.Message);
            }
        }

        void SaveForm()
        {
            long id = 0;
            AWAPI_Data.Data.awContentForm form = new AWAPI_Data.Data.awContentForm();
            form.siteId = App_Code.SessionInfo.CurrentSite.siteId;
            form.contentId = ContentId;
            form.userId = App_Code.SessionInfo.CurrentUser.userId;
            form.title = _title.Text;
            form.description = _description.Text;
            form.isEnabled = _isEnabled.Checked;
            form.applyToSub = _applyToSubContent.Checked;
            form.canCreateNew = _canCreateNew.Checked;
            form.canUpdate = _canUpdate.Checked;
            form.canDelete = _canDelete.Checked;

            if (_contentFormId.Text == "")
                id = _formLib.Add(form.siteId, form.contentId, form.title, form.description, form.isEnabled, form.applyToSub,
                                form.canCreateNew, form.canUpdate, form.canDelete, form.userId.Value);


            else
            {
                id = Convert.ToInt64(_contentFormId.Text);
                _formLib.Update(id, form.title, form.description, form.isEnabled, form.applyToSub,
                                form.canCreateNew, form.canUpdate, form.canDelete, form.userId.Value);

            }
            _contentFormId.Text = id.ToString();
            ShowHildeControls(true);

            PopulateFields(id);
        }

        protected void btnDeleteContentForm__Click1(object sender, ImageClickEventArgs e)
        {
            DeleteContentForm();
        }

        protected void _fieldList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton up = (ImageButton)e.Row.FindControl("ibtnUp");
                ImageButton down = (ImageButton)e.Row.FindControl("ibtnDown");

                if (e.Row.RowIndex == 0)
                    up.Visible = false;

                if (e.Row.RowIndex == _fieldList.Rows.Count - 1)
                    down.Visible = false;
            }
                
        }

        protected void _fieldList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow row = (GridViewRow)((Control)e.CommandSource).Parent.Parent;

            switch (e.CommandName.ToLower())
            { 
                case "moveup":
                    _formLib.MoveFieldUp (Convert.ToInt64(_contentFormId.Text), Convert.ToInt64(e.CommandArgument.ToString()));
                    PopulateFields(Convert.ToInt64(_contentFormId.Text));
                    break;

                case "movedown":
                    _formLib.MoveFieldDown(Convert.ToInt64(_contentFormId.Text), Convert.ToInt64(e.CommandArgument.ToString()));
                    PopulateFields(Convert.ToInt64(_contentFormId.Text));
                    break;

                case "changeenablestatus":
                    ImageButton enabledButton = (ImageButton)row.FindControl("ibtnEnabled");
                    bool isEnabled = !(enabledButton.ImageUrl.ToLower().IndexOf("disabled") >0 ? false : true);
                    _formLib.UpdateFieldEnableStatus(Convert.ToInt64(e.CommandArgument.ToString()), isEnabled); 
                    enabledButton.ImageUrl = isEnabled ? "~/admin/img/button/1212/ok-icon.png" : "~/admin/img/button/1212/ok-disabled-icon.png";
                    break;

                case "changevisiblestatus":
                    ImageButton visibleButton = (ImageButton)row.FindControl("ibtnVisible");
                    bool isVisible = !(visibleButton.ImageUrl.ToLower().IndexOf("disabled") > 0 ? false : true);
                    _formLib.UpdateFieldVisibleStatus(Convert.ToInt64(e.CommandArgument.ToString()), isVisible);
                    visibleButton.ImageUrl = isVisible ? "~/admin/img/button/1212/ok-icon.png" : "~/admin/img/button/1212/ok-disabled-icon.png";
                    break;
            
            }
        }

        protected void btnSyncFormFields_Click(object sender, ImageClickEventArgs e)
        {
            _formLib.AddFieldsToForm(ContentId, Convert.ToInt64(_contentFormId.Text), true);
            PopulateFields(Convert.ToInt64(_contentFormId.Text));

            App_Code.Misc.WriteMessage(App_Code.Misc.MessageType.INFO, _msg, "Fields have been synced.");
        }


    }
}
