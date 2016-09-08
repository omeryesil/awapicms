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
    public partial class managecustomfields : AWAPI.App_Code.AdminBasePage
    {
        AWAPI_BusinessLibrary.library.ContentLibrary _contentLib = new AWAPI_BusinessLibrary.library.ContentLibrary();
        AWAPI_BusinessLibrary.library.ContentCustomFieldLibrary _customLib = new AWAPI_BusinessLibrary.library.ContentCustomFieldLibrary();

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

            if (!IsPostBack)
                PopulateFields();
        }

        void PopulateFields()
        {
            _customFieldList.DataSource = null;
            System.Collections.Generic.IList<AWAPI_Data.CustomEntities.ContentCustomField> tmpList = _customLib.GetFieldList(ContentId, false);

            if (tmpList != null)
            {
                var fieldList = from f in tmpList
                                where f.fieldContentId.Equals(ContentId)
                                select f;


                _customFieldList.DataSource = fieldList;
            }
            _customFieldList.DataBind();
        }

        void ResetControls()
        {
            _customFieldId.Text = "";
            _title.Text = "";
            _description.Text = "";
            _minimumValue.Text = "";
            _maximumValue.Text = "";
            _defaultValue.Text = "";
            _regularExpression.Text = "";

            ShowHildeControls(false);
        }

        void ShowHildeControls(bool show)
        {
            btnDeleteField.Visible = show;
        }

        void PopulateField(Int64 fieldId)
        {

            AWAPI_Data.Data.awContentCustomField cstField = _customLib.GetField(fieldId);

            _customFieldId.Text = cstField.customFieldId.ToString();
            _title.Text = cstField.title;
            _description.Text = cstField.description;
            _isEnabled.Checked = cstField.isEnabled;
            _applyToChildContent.Checked = cstField.applyToSubContents;
            _fieldType.SelectedValue = cstField.fieldType;
            _maximumLength.Text = cstField.maximumLength.ToString();
            _minimumValue.Text = cstField.minimumValue;
            _maximumValue.Text = cstField.maximumValue;
            _defaultValue.Text = cstField.defaultValue;
            _regularExpression.Text = cstField.regularExpression;
            _sortOrder.Text = cstField.sortOrder.ToString();

            ShowHildeControls(true);
        }

        protected void _customFieldList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int64 fieldId = Convert.ToInt64(e.CommandArgument.ToString());
            switch (e.CommandName.ToLower())
            {
                case "editfield":
                    PopulateField(fieldId);
                    break;

            }
        }

        protected void btnNewCustomField_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void btnNewCustomField_Click1(object sender, ImageClickEventArgs e)
        {
            ResetControls();
        }

        protected void btnSaveCustomField_Click(object sender, ImageClickEventArgs e)
        {
            SaveField();
            App_Code.Misc.WriteMessage(App_Code.Misc.MessageType.INFO, _msg, "Field has been saved.");
        }

        void SaveField()
        {
            AWAPI_Data.Data.awContentCustomField f = new AWAPI_Data.Data.awContentCustomField();

            //update current
            if (_customFieldId.Text != "")
                f.customFieldId = Convert.ToInt64(_customFieldId.Text);
            else
            {
                f.contentId = ContentId;
                f.userId = App_Code.SessionInfo.CurrentUser.userId;
            }


            f.title = _title.Text;
            f.description = _description.Text;
            f.isEnabled = _isEnabled.Checked;
            f.applyToSubContents = _applyToChildContent.Checked;
            f.fieldType = _fieldType.SelectedValue;
            f.maximumLength = Convert.ToInt32(_maximumLength.Text);
            f.minimumValue = _minimumValue.Text;
            f.maximumValue = _maximumValue.Text;
            f.defaultValue = _defaultValue.Text;
            f.regularExpression = _regularExpression.Text;
            f.sortOrder = Convert.ToInt32(_sortOrder.Text);

            f.customFieldId = _customLib.SaveField(f);
            _customFieldId.Text = f.customFieldId.ToString();

            ShowHildeControls(true);
            PopulateFields();


        }

        protected void btnDeleteField_Click(object sender, ImageClickEventArgs e)
        {
            DeleteField();
            App_Code.Misc.WriteMessage(App_Code.Misc.MessageType.INFO, _msg, "Field has been deleted.");
        }

        void DeleteField()
        {
            _customLib.DeleteField(Convert.ToInt64(_customFieldId.Text));
            ResetControls();
            PopulateFields();
        }



    }
}
