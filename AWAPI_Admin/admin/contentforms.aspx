<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.Master" AutoEventWireup="true"
    CodeBehind="contentforms.aspx.cs" Inherits="AWAPI.admin.PageContentForms" %>

<%@ Register Assembly="Moxiecode.TinyMCE" Namespace="Moxiecode.TinyMCE.Web" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="controls/TinyMCEScript.ascx" TagName="TinyMCEScript" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="includes/tiny_mce/tiny_mce.js"></script>

    <style type="text/css">
        .contentForm
        {
            /* height: 630px; */
            width: 100%;
            /*overflow: auto;*/
            border: 1px solid #dcdcdc;
            background-color: #eee;
            padding: 10px;
        }
        .style1
        {
            width: 100%;
        }
    </style>

    <script language="javascript" type="text/javascript">
        function setImageFromIFrame(url, targetControlId) {
            //document.getElementById(targetControlId).style.display = '';
            //document.getElementById(targetControlId).src = url + "&size=150x150";
            document.getElementById(targetControlId).value = url;
            $.fn.colorbox.close();
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlaceHolder" runat="server">
    <div id='left-column'>
        <div class='title'>
            <div style="float: left">
                <h3>
                    Form List
                </h3>
            </div>
            <div style="float: right">
            </div>
        </div>
        <div class="nav">
            <asp:UpdatePanel runat='server'>
                <ContentTemplate>
                    <asp:GridView ID="_formList" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        CellPadding="2" CellSpacing="2" GridLines="None" PageSize="15" ShowHeader="True"
                        Style="width: 100%" OnRowCommand="_formList_RowCommand">
                        <Columns>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100%" ItemStyle-Wrap="true">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtnSelect" runat="server" CommandArgument='<%# Eval ("contentformid") %>'
                                        CommandName="selectform" Text='<%# Eval("title") %>' />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <div id='center-column'>
        <asp:UpdatePanel ID="UpdatePanel1" runat='server'>
            <ContentTemplate>
                <div class="top-bar">
                    <h1>
                        <asp:Literal ID="_formTitle" runat='server' Text="ContentForms" />
                    </h1>
                    <asp:Panel ID="_hiddenFormFeatures" runat='server' Visible="false">
                        <asp:Literal ID="_formFeature_formId" runat='server' />
                        <asp:Literal ID="_formFeature_contentId" runat='server' />
                        <asp:CheckBox ID="_formFeature_applyToSub" runat='server' />
                        <asp:CheckBox ID="_formFeature_canCreateNew" runat='server' />
                        <asp:CheckBox ID="_formFeature_canUpdate" runat='server' />
                        <asp:CheckBox ID="_formFeature_canDelete" runat='server' />
                    </asp:Panel>
                    <div class="breadcrumbs">
                    </div>
                </div>
                <div class="select-bar">
                    <ul>
                        <li>
                            <asp:ImageButton ID="btnNewContentForm_" runat="server" ImageUrl="img/button/new-icon.png"
                                AlternateText="New ContentForm" CausesValidation="False" ToolTip="New ContentForm"
                                OnClick="btnNewContentForm__Click" />
                            <br />
                            <asp:Label ID='lblNewContentForm_' runat='server' Text='New' />
                        </li>
                        <li>
                            <asp:ImageButton ID="btnSaveContentForm_" runat="server" ImageUrl="img/button/save-icon.png"
                                AlternateText="Save ContentForm" ValidationGroup="saveContentForm" ToolTip="Save ContentForm"
                                OnClick="btnSaveContentForm__Click" /><br />
                            <asp:Label ID='lblSaveContentForm_' runat='server' Text='Save' />
                        </li>
                        <li>
                            <asp:ImageButton ID="btnDeleteContentForm_" runat="server" AlternateText="Delete ContentForm"
                                ImageUrl="img/button/delete-icon.png" ToolTip="Delete ContentForm" OnClientClick="return confirm('Are you sure you want to delete the content?');"
                                ValidationGroup="saveContentForm" OnClick="btnDeleteContentForm__Click" /><br />
                            <asp:Label ID='lblDeleteContentForm_' runat='server' Text='Delete' />
                        </li>
                    </ul>
                </div>
                <div>
                    <div class="contentForm">
                        <table>
                            <tr>
                                <td class="td_first">
                                    <asp:Literal ID='_contentIdtitle' runat='server' Text="Content Id :" />
                                </td>
                                <td>
                                    <asp:Label ID="_contentId" runat='server'></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <asp:GridView ID="_contentFieldList" runat='server' GridLines="None" AutoGenerateColumns="false"
                            Width="95%" OnRowDataBound="_contentFieldList_RowDataBound" CellPadding="2" CellSpacing="2">
                            <Columns>
                                <asp:TemplateField ItemStyle-Wrap="false" ItemStyle-CssClass="td_first">
                                    <ItemTemplate>
                                        <asp:Panel runat='server' Visible="false">
                                            <asp:Literal ID="_contentCustomFieldId" runat='server' Text='<%# Bind("contentCustomFieldId") %>' />
                                            <asp:Literal ID="_fieldType" runat='server' Text='<%# Bind("fieldType") %>' />
                                            <asp:CheckBox ID="_isContentCustomField" runat='server' Checked='<%# Bind("isContentCustomField") %>' />
                                        </asp:Panel>
                                        <asp:Literal ID='_title' runat='server' Text='<%# Bind("title") %>' />
                                        :
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="100%">
                                    <ItemTemplate>
                                        <asp:CheckBox ID='_checkBox' runat='server' Checked='<%# DataBinder.Eval(Container.DataItem, "fieldValue").ToString().ToLower()=="true"?true:false %>' />
                                        <asp:TextBox ID='_editBox' CssClass="textMedium" runat='server' Text='<%# Bind("fieldValue") %>' />
                                        <ajax:CalendarExtender ID="_dateTimeExtender" runat="server" PopupPosition="TopRight"
                                            TargetControlID="_editBox" Format="MM/dd/yyyy" />
                                        <asp:HyperLink ID="_file" runat='server' rel="selectfile" NavigateUrl='~/admin/frames/selectfile.aspx?type=image&callback=setImageFromIFrame'
                                            ImageUrl="~/admin/img/button/1616/Folder-Open-icon.png" ToolTip="Select a file" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <asp:Panel ID="pnlContentList" runat='server'>
                    <div style="margin-top: 10px; border-top: 1px solid #dcdcdc;">
                        <asp:GridView ID="_contentList" runat='server' Visible="False" AutoGenerateColumns="False"
                            AllowPaging="true" PageSize="15" GridLines="None" CellPadding="2" CellSpacing="2"
                            OnRowCommand="_contentList_RowCommand" Width="550px" HeaderStyle-CssClass="GridViewHeaderStyle"
                            OnPageIndexChanging="_contentList_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Content Alias">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="_contentAlias" runat="server" Text='<%# Bind("alias") %>' CommandName="editcontent"
                                            CommandArgument='<%# Bind("contentid") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="title" HeaderText="Title" />
                                <asp:BoundField DataField="isEnabled" HeaderText="enabled" ItemStyle-Width="60" />
                                <asp:BoundField DataField="pubDate" DataFormatString="{0:MM/dd/yy HH:mm}" HeaderText="PublishDate"
                                    ItemStyle-Width="90" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
