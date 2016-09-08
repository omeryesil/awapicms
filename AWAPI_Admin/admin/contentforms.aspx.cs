using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AWAPI_Data.Data;
using AWAPI_Common.values;

namespace AWAPI.admin
{
    public partial class PageContentForms : AWAPI.App_Code.AdminBasePage
    {
        #region members
        AWAPI_BusinessLibrary.library.ContentLibrary _contentLib = new AWAPI_BusinessLibrary.library.ContentLibrary();
        AWAPI_BusinessLibrary.library.ContentCustomFieldLibrary _customLib = new AWAPI_BusinessLibrary.library.ContentCustomFieldLibrary();
        AWAPI_BusinessLibrary.library.ContentFormLibrary _formLib = new AWAPI_BusinessLibrary.library.ContentFormLibrary();

        AWAPI_Data.Data.awContent _content = new awContent();


        public long ContentFormId
        {
            get
            {
                if (Request["ContentFormId"] != null)
                    return Convert.ToInt64(Request["ContentFormId"]);
                return 0;
            }
        }

        #endregion

        System.Text.StringBuilder _jScript = new System.Text.StringBuilder();

        public PageContentForms()
        {
            ModuleName = "contentform";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ResetAllControls();
                PopulateFormList();
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            RegisterCustomScript();
        }

        void RegisterCustomScript()
        {
            System.Text.StringBuilder script = new System.Text.StringBuilder();

            //if (_contentId.Text != "")
            //    script.Append("parent.location.hash = '" + _contentId.Text + "'");

            script.Append("$(document).ready(function(){ \n");
            script.Append("    $(\"a[rel='selectfile']\").colorbox({width:'960', height:'620', iframe:true}); \n");
            script.Append("}); \n");

            if (script.Length > 0)
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "", script.ToString(), true);
        }

        void PopulateFormList()
        {
            IList<AWAPI_Data.Data.awContentForm> list = _formLib.GetList(App_Code.SessionInfo.CurrentSite.siteId, true);
            _formList.DataSource = list;
            _formList.DataBind();
        }

        void ResetAllControls()
        {
            _formFeature_formId.Text = "";
            _formTitle.Text = "Content Forms";
            _formFeature_contentId.Text = "";
            _formFeature_applyToSub.Checked = false;
            _formFeature_canCreateNew.Checked = false;
            _formFeature_canUpdate.Checked = false;
            _formFeature_canDelete.Checked = false;

            pnlContentList.Visible = false;

            btnNewContentForm_.Visible = false;
            lblNewContentForm_.Visible = false;

            btnSaveContentForm_.Visible = false;
            lblSaveContentForm_.Visible = false;

            btnDeleteContentForm_.Visible = false;
            lblDeleteContentForm_.Visible = false;


            _contentList.DataSource = null;
            _contentList.DataBind();

            ResetContentControls();
        }

        void ApplyContentButtonRights(bool applyToSub)
        {
            if (applyToSub)
            {
                if (_formFeature_canCreateNew.Checked)
                {
                    btnNewContentForm_.Visible = true;
                    lblNewContentForm_.Visible = true;
                }

                if (_formFeature_canDelete.Checked)
                {
                    btnDeleteContentForm_.Visible = true;
                    lblDeleteContentForm_.Visible = true;
                }
            }
            if (_formFeature_canUpdate.Checked)
            {
                btnSaveContentForm_.Visible = true;
                lblSaveContentForm_.Visible = true;
            }
        }

        void ResetContentControls()
        {
            _content = null;
            _contentId.Text = "";
            _contentIdtitle.Visible = false;

            _contentFieldList.DataSource = null;
            _contentFieldList.DataBind();

            ShowHideContentControls(false);
        }


        void ShowHideContentControls(bool show)
        {
            //ShowHideControl(btnNewContentForm_, show);
            //ShowHideControl(lblNewContentForm_, show);

            //ShowHideControl(btnSaveContentForm_, show);
            //ShowHideControl(lblSaveContentForm_, show);

            _contentIdtitle.Visible = show;

            ShowHideControl(btnDeleteContentForm_, show);
            ShowHideControl(lblDeleteContentForm_, show);
        }

        void PopulateForm(long contentFormId)
        {
            ResetAllControls();

            awContentForm form = _formLib.Get(contentFormId, true);
            if (form == null)
                return;

            _formFeature_formId.Text = form.contentFormId.ToString();
            _formTitle.Text = form.title;
            _formFeature_contentId.Text = form.contentId.ToString();
            _formFeature_applyToSub.Checked = form.applyToSub; ;
            _formFeature_canCreateNew.Checked = form.canCreateNew;
            _formFeature_canUpdate.Checked = form.canUpdate;
            _formFeature_canDelete.Checked = form.canDelete;

            ApplyContentButtonRights(form.applyToSub);


            if (form.applyToSub)
            {
                pnlContentList.Visible = true;
                PopulateFields(form.contentFormId, 0);
                PopulateChildContentList(form.contentId);
            }
            else
            {
                PopulateFields(form.contentFormId, form.contentId);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentContentId"></param>
        void PopulateChildContentList(long parentContentId)
        {
            IList<awContent> contentList = _contentLib.GetList(App_Code.SessionInfo.CurrentSite.siteId,
                                                            "parentcontentid=" + parentContentId, "pubDate");

            _contentList.DataSource = contentList;
            _contentList.DataBind();
            _contentList.Visible = true;

        }

        /// <summary>
        /// if contentid is 0, then only get the fields, not the field values.
        /// </summary>
        /// <param name="contentFormId"></param>
        /// <param name="contentId"></param>
        void PopulateFields(long contentFormId, long contentId)
        {
            ResetContentControls();

            _content = _contentLib.Get(contentId);

            IList<AWAPI_Data.CustomEntities.ContentFormFieldSettingExtended> fieldList = _formLib.GetFieldList(contentFormId);

            if (fieldList == null || fieldList.Count == 0)
                return;

            var list2 = from l in fieldList
                        where l.isVisible
                        select l;
            if (list2 == null || list2.Count() == 0)
                return;

            System.Data.DataTable dt = AWAPI_Common.library.DataLibrary.LINQToDataTable(list2);
            dt.Columns.Add("fieldValue");

            //GET LIST OF THE VALUES
            if (contentId > 0)
            {
                IList<AWAPI_Data.CustomEntities.ContentCustomFieldValueExtended> valueList = _customLib.GetFieldValueList(contentId, "");

                for (int n = 0; n < dt.Rows.Count; n++)
                {
                    long customFieldId = dt.Rows[n]["contentCustomFieldId"] == DBNull.Value ? 0 : (long)dt.Rows[n]["contentCustomFieldId"];
                    dt.Rows[n]["fieldValue"] = GetFieldValue(customFieldId, valueList, dt.Rows[n]["staticFieldName"].ToString());
                }
                _contentId.Text = contentId.ToString();

                ShowHideContentControls(true);
            }

            _contentFieldList.DataSource = dt;

            _contentFieldList.DataBind();

        }

        protected void _formList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.ToLower())
            {
                case "selectform":
                    PopulateForm(Convert.ToInt64(e.CommandArgument.ToString()));
                    break;
            }
        }

        protected void _contentFieldList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                bool isCustomField = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "isContentCustomField"));
                string staticFieldName = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "staticFieldName"));
                long customFieldId = DataBinder.Eval(e.Row.DataItem, "contentCustomFieldId") == DBNull.Value ? 0 : Convert.ToInt64(DataBinder.Eval(e.Row.DataItem, "contentCustomFieldId"));
                bool isEnabled = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "isEnabled"));

                TextBox txb = (TextBox)e.Row.FindControl("_editBox");
                CheckBox cb = (CheckBox)e.Row.FindControl("_checkBox");
                HyperLink file = (HyperLink)e.Row.FindControl("_file");

                AjaxControlToolkit.CalendarExtender calender = (AjaxControlToolkit.CalendarExtender)e.Row.FindControl("_dateTimeExtender");

                txb.Visible = false;
                cb.Visible = false;
                file.Visible = false;
                calender.Enabled = false;

                string fieldType = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "fieldType")).Trim().ToLower();
                switch (fieldType)
                {
                    case "checkbox":
                        cb.Visible = true;
                        cb.Enabled = isEnabled;
                        break;

                    default:
                        if (fieldType == "datetime")
                            calender.Enabled = true;

                        if (fieldType == "file" || (!isCustomField && staticFieldName.ToLower().Equals("imageurl")))
                        {
                            file.Visible = true;
                            file.NavigateUrl = "~/admin/frames/selectfile.aspx?type=image&callback=setImageFromIFrame&targetcontrolid=" + txb.ClientID;
                        }
                        if (!isCustomField && staticFieldName.ToLower() == "sortorder")
                            txb.Text = "1";                               


                        txb.Visible = true;
                        txb.Enabled = isEnabled;
                        int maxLength = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "maximumLength"));
                        
                        UpdateFieldTextBoxSize(txb, maxLength);
                        break;
                }

                cb.Enabled = isEnabled;
                txb.Enabled = isEnabled;
                file.Enabled = isEnabled;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customFieldId"></param>
        /// <param name="valueList"></param>
        /// <param name="staticFieldName"></param>
        /// <returns></returns>
        string GetFieldValue(long customFieldId, IList<AWAPI_Data.CustomEntities.ContentCustomFieldValueExtended> valueList, string staticFieldName)
        {
            if (_content == null || String.IsNullOrEmpty(staticFieldName) && customFieldId <= 0)
                return "";

            string rtn = "";
            if (!String.IsNullOrEmpty(staticFieldName))
            {
                switch (staticFieldName.ToLower().Trim())
                {
                    case "alias": rtn =
                        _content.alias;
                        break;
                    case "title":
                        rtn = _content.title;
                        break;
                    case "description":
                        rtn = _content.description;
                        break;
                    case "sortorder":
                        rtn = _content.sortOrder.ToString();
                        break;
                    case "isenabled":
                        rtn = _content.isEnabled.ToString();
                        break;
                    case "iscommentable":
                        rtn = _content.isCommentable.ToString();
                        break;
                    case "link":
                        rtn = _content.link;
                        break;
                    case "imageurl":
                        rtn = _content.imageurl;
                        break;
                    case "pubdate":
                        if (_content.pubDate != null) rtn = _content.pubDate.Value.ToString("MM/dd/yyyy HH:mm");
                        break;
                    case "pubenddate":
                        if (_content.pubEndDate != null) rtn = _content.pubEndDate.Value.ToString("MM/dd/yyyy HH:mm");
                        break;
                    case "eventstartdate":
                        if (_content.eventStartDate != null) rtn = _content.eventStartDate.Value.ToString("MM/dd/yyyy HH:mm");
                        break;
                    case "eventenddate":
                        if (_content.eventEndDate != null) rtn = _content.eventEndDate.Value.ToString("MM/dd/yyyy HH:mm");
                        break;
                }
                return rtn;
            }
            else if (valueList != null && valueList.Count > 0)
            {
                string value = (from v in valueList
                                where v.customFieldId.Equals(customFieldId)
                                select v.fieldValue).FirstOrDefault<string>();
                if (!String.IsNullOrEmpty(value))
                    rtn = value;
            }

            return rtn;
        }

        /// <summary>
        /// Updates the field's textbox size in the field list gridview
        /// </summary>
        /// <param name="txb"></param>
        /// <param name="maxLength"></param>
        void UpdateFieldTextBoxSize(TextBox txb, int maxLength)
        {
            txb.MaxLength = maxLength;
            txb.TextMode = TextBoxMode.SingleLine;

            if (maxLength <= 10)
                txb.Width = new Unit(75, UnitType.Pixel);
            else if (maxLength <= 20)
                txb.Width = new Unit(130, UnitType.Pixel);
            else if (maxLength <= 50)
                txb.Width = new Unit(220, UnitType.Pixel);
            else if (maxLength <= 100)
                txb.Width = new Unit(400, UnitType.Pixel);
            else if (maxLength <= 255)
            {
                txb.Width = new Unit(100, UnitType.Percentage);
                txb.Height = new Unit(70, UnitType.Pixel);
                txb.TextMode = TextBoxMode.MultiLine;
            }
            else
            {
                txb.Width = new Unit(100, UnitType.Percentage);
                txb.Height = new Unit(150, UnitType.Pixel);
                txb.TextMode = TextBoxMode.MultiLine;
            }
        }

        protected void _contentList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName.ToLower())
            {
                case "editcontent":
                    PopulateFields(Convert.ToInt64(_formFeature_formId.Text), Convert.ToInt64(e.CommandArgument));
                    break;
            }
        }

        protected void btnNewContentForm__Click(object sender, ImageClickEventArgs e)
        {
            ResetContentControls();
            PopulateFields(Convert.ToInt64(_formFeature_formId.Text), 0);

        }

        protected void btnDeleteContentForm__Click(object sender, ImageClickEventArgs e)
        {
            _contentLib.Delete(Convert.ToInt64(_contentId.Text));
            ResetContentControls();
            PopulateFields(Convert.ToInt64(_formFeature_formId.Text), 0);
            PopulateChildContentList(Convert.ToInt64(_formFeature_contentId.Text));

            AdminMaster.WriteMessage(AWAPI.Admin.AdminMaster.MessageType.INFO, "Content has been deleted.");

        }

        protected void btnSaveContentForm__Click(object sender, ImageClickEventArgs e)
        {
            long id = _contentId.Text.Trim() == "" ? 0 : Convert.ToInt64(_contentId.Text);
            string cultureCode = App_Code.SessionInfo.CurrentSite.cultureCode;

            awContent content = new awContent();
            bool newContent = false;

            try
            {

                if (id == 0)    //Add new content 
                {
                    newContent = true;
                    id = AWAPI_Common.library.MiscLibrary.CreateUniqueId();
                }
                else    //update the content 
                {
                    content = _contentLib.Get(id);
                    if (content == null)
                    {
                        AdminMaster.WriteMessage(AWAPI.Admin.AdminMaster.MessageType.ERROR, "Content not found.");
                        return;
                    }
                }

                content.siteId = App_Code.SessionInfo.CurrentSite.siteId;
                content.userId = App_Code.SessionInfo.CurrentUser.userId;

                #region SAVE CONTENT STATIC VALUES
                //SAVE THE CONTENT, FIRST (STATIC VALUES ONLY ) ----------------------------------------------
                foreach (GridViewRow gr in _contentFieldList.Rows)
                {
                    Boolean isCustomField = ((CheckBox)gr.FindControl("_isContentCustomField")).Checked;
                    string fieldName = ((Literal)gr.FindControl("_title")).Text.ToLower().Trim();
                    string fieldType = ((Literal)gr.FindControl("_fieldType")).Text.ToLower();
                    Literal tmpContentCustomFieldId = (Literal)gr.FindControl("_contentCustomFieldId");
                    long contentCustomFieldId = (tmpContentCustomFieldId.Text == "" || tmpContentCustomFieldId.Text == "0") ?
                                                0 : Convert.ToInt64(tmpContentCustomFieldId.Text);

                    TextBox txb = (TextBox)gr.FindControl("_editBox");
                    CheckBox cb = (CheckBox)gr.FindControl("_checkBox");

                    if (!newContent && isCustomField && contentCustomFieldId > 0)
                    {
                        _customLib.UpdateFieldValue(id, contentCustomFieldId, App_Code.SessionInfo.CurrentUser.userId,
                            txb.Text, cultureCode);
                    }
                    else
                        SetContentStaticValues(fieldName, txb.Text, cb.Checked, content);

                }
                #endregion

                if (content.alias == null || content.alias.Trim() == "")
                    if (content.title.Trim() != "")
                        content.alias = AWAPI_Common.library.MiscLibrary.FormatAliasName(content.title);
                    else
                        content.alias = AWAPI_Common.library.MiscLibrary.FormatAliasName(_formTitle.Text.Trim() + "_" + id.ToString());


                #region ADD or UPDATE CONTENT
                if (newContent)
                {
                    content.parentContentId = 0;
                    if (_formFeature_applyToSub.Checked && _formFeature_contentId.Text != "")
                        content.parentContentId = Convert.ToInt64(_formFeature_contentId.Text);

                    id = _contentLib.Add(id, content.alias, content.title, content.description,
                                content.contentType, content.siteId, content.userId, content.parentContentId,
                                content.eventStartDate, content.eventEndDate,
                                content.link, content.imageurl, content.sortOrder, content.isEnabled, content.isCommentable,
                                content.pubDate, content.pubEndDate);
                    _contentId.Text = id.ToString();
                }
                else
                {
                    _contentLib.Update(id, content.alias, content.title, content.description,
                                content.contentType, content.userId, content.parentContentId,
                                content.eventStartDate, content.eventEndDate,
                                content.link, content.imageurl, content.sortOrder, content.isEnabled, content.isCommentable,
                                content.pubDate, content.pubEndDate);

                }
                #endregion

                #region SAVE CONTENT CUSTOM FIELDS
                //SAVE THE CUSTOM FIELDS 
                foreach (GridViewRow gr in _contentFieldList.Rows)
                {
                    Boolean isCustomField = ((CheckBox)gr.FindControl("_isContentCustomField")).Checked;
                    Literal tmpContentCustomFieldId = (Literal)gr.FindControl("_contentCustomFieldId");
                    long contentCustomFieldId = (tmpContentCustomFieldId.Text == "" || tmpContentCustomFieldId.Text == "0") ?
                                                0 : Convert.ToInt64(tmpContentCustomFieldId.Text);

                    if (!isCustomField || contentCustomFieldId <= 0)
                        continue;

                    string fieldType = ((Literal)gr.FindControl("_fieldType")).Text.ToLower();
                    TextBox txb = (TextBox)gr.FindControl("_editBox");
                    CheckBox cb = (CheckBox)gr.FindControl("_checkBox");

                    string value = txb.Text;
                    if (fieldType == "checkbox")
                        value = cb.Checked.ToString();

                    _customLib.UpdateFieldValue(id, contentCustomFieldId, App_Code.SessionInfo.CurrentUser.userId,
                            value, cultureCode);

                }
                #endregion


                PopulateFields(Convert.ToInt64(_formFeature_formId.Text), id);

                if (_formFeature_applyToSub.Checked)
                    PopulateChildContentList(Convert.ToInt64(_formFeature_contentId.Text));

                AdminMaster.WriteMessage(AWAPI.Admin.AdminMaster.MessageType.INFO, "Content has been saved.");
            }
            catch (Exception ex)
            {
                AdminMaster.WriteMessage(AWAPI.Admin.AdminMaster.MessageType.ERROR, ex.Message);
            }
        }

        void SetContentStaticValues(string staticFieldName, string textValue, bool booleanValue, awContent content)
        {
            switch (staticFieldName.ToLower().Trim())
            {
                case "alias":
                    content.alias = textValue;
                    break;
                case "title":
                    content.title = textValue;
                    break;
                case "description":
                    content.description = textValue;
                    break;
                case "sortorder":
                    content.sortOrder = Convert.ToInt32(textValue);
                    break;
                case "isenabled":
                    content.isEnabled = booleanValue;
                    break;
                case "iscommentable":
                    content.isCommentable = booleanValue;
                    break;
                case "link":
                    content.link = textValue;
                    break;
                case "imageurl":
                    content.imageurl = textValue;
                    break;
                case "pubdate":
                    content.pubDate = AWAPI_Common.library.MiscLibrary.ConvertStringToDate(textValue);
                    break;
                case "pubenddate":
                    content.pubEndDate = AWAPI_Common.library.MiscLibrary.ConvertStringToDate(textValue, true);
                    break;
                case "eventstartdate":
                    content.eventStartDate = AWAPI_Common.library.MiscLibrary.ConvertStringToDate(textValue);
                    break;
                case "eventenddate":
                    content.eventEndDate = AWAPI_Common.library.MiscLibrary.ConvertStringToDate(textValue, true);
                    break;
            }
        }

        protected void _contentList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            _contentList.PageIndex = e.NewPageIndex;
            PopulateChildContentList(Convert.ToInt64(_formFeature_contentId.Text));
        }
    }
}
