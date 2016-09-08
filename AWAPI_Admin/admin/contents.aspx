<%@ Page Language="C#" MasterPageFile="~/admin/admin.Master" AutoEventWireup="true"
    CodeBehind="contents.aspx.cs" Inherits="AWAPI.Admin.PageContents" Title="AWAPI - Manage Contents" %>

<%@ Register Assembly="Moxiecode.TinyMCE" Namespace="Moxiecode.TinyMCE.Web" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<%@ Register Src="controls/TinyMCEScript.ascx" TagName="TinyMCEScript" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="includes/tiny_mce/tiny_mce.js"></script>

    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>

    <script language="javascript" type="text/javascript">
        function setImageFromIFrame(url) {
            document.getElementById("<%= _contentImageUrl.ClientID %>").value = url;
        
            var image = document.getElementById("<%= _contentImage.ClientID %>");
            image.style.display = '';
            
            if (url.indexOf("http://") == 0 || url.toIndexOf("https://") == 0)
                image.src = url;
            else
                image.src = url + "&size=150x150";
    
            image.value = url;
            $.fn.colorbox.close();
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlaceHolder" runat="server">
    <div id='left-column'>
        <asp:UpdatePanel runat='server'>
            <ContentTemplate>
                <div class='title'>
                    <div style="float: left">
                        <h3>
                            Content List
                        </h3>
                    </div>
                    <div style="float: right">
                        <asp:ImageButton ID="btnRefreshTree" runat="server" AlternateText="Refresh Content List"
                            ImageUrl="img/bg-refresh.png" OnClick="btnRefreshTree_Click" />
                    </div>
                </div>
                <div class="nav">
                    <asp:Literal ID="_parentContentName" runat="server" />
                    <asp:Panel ID="pnlTreeView" runat="server">
                        <asp:TreeView ID="twContent" runat="server" OnSelectedNodeChanged="twContent_SelectedNodeChanged"
                            Width="90%">
                        </asp:TreeView>
                    </asp:Panel>
                    <asp:Panel ID="pnlGridView" runat="server" Visible="false">
                        <asp:TextBox ID="txbFilter" runat="server" />
                        <asp:LinkButton ID="lbtnFilter" runat="server" Text="Filter" OnClick="lbtnFilter_Click" />
                        &nbsp;<asp:LinkButton ID="lbtnClear" runat="server" Text="Clear" OnClick="lbtnClear_Click" /><br />
                        <asp:GridView ID="gwContentList" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                            CellPadding="2" CellSpacing="2" GridLines="None" OnPageIndexChanging="gwContentList_PageIndexChanging"
                            OnRowCommand="gwContentList_RowCommand" PageSize="15" ShowHeader="True" Style="width: 100%">
                            <Columns>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Post Title" ItemStyle-Width="100%"
                                    ItemStyle-Wrap="true">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbtnSelect" runat="server" CommandArgument='<%# Eval ("contentid") %>'
                                            CommandName="selectcontent" Text='<%# Eval("alias") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle Wrap="True" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="pubDate" DataFormatString="{0:MM/dd/yy}" HeaderText="Date" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbtnSubContent" runat="server" CommandArgument='<%# Eval ("contentid") %>'
                                            CommandName="subcontent" Text="childs" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id='center-column'>
        <asp:UpdatePanel ID="UpdatePanel1" runat='server'>
            <ContentTemplate>
                <div class="top-bar">
                    <h1>
                        Contents</h1>
                    <div class="breadcrumbs">
                        <asp:LinkButton ID='_lbtnBreadCrumbReset' runat='server' Text="Content" OnClick="_lbtnBreadCrumbReset_Click" />&nbsp;&gt;&nbsp;
                        <asp:Repeater ID="_rptBreadCrumb" runat="server" OnItemCommand="_rptBreadCrumb_ItemCommand">
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtnBreadCrumb" runat="server" Text='<%# Bind("alias") %>' CommandName="selectcontent"
                                    CommandArgument='<%# Bind("contentid") %>' />
                                &gt;&nbsp;
                            </ItemTemplate>
                        </asp:Repeater>
                        <asp:Label ID='_lblBreadCrumbCurrentContent' runat='server' />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanel2" runat='server'>
            <ContentTemplate>
                <div class="select-bar">
                    <div style="float: left">
                        <ul>
                            <li>
                                <asp:ImageButton ID="btnNewContent_" runat="server" ImageUrl="img/button/new-icon.png"
                                    AlternateText="New Content" CausesValidation="False" ToolTip="New Content" OnClick="btnNewContent_Click" />
                                <br />
                                <asp:Label ID='lblNewContent_' runat='server' Text='New' />
                            </li>
                            <li>
                                <asp:ImageButton ID="btnNewContentChild_" runat="server" ImageUrl="img/button/new-icon.png"
                                    AlternateText="New Child Content" CausesValidation="False" ToolTip="New Child Content"
                                    OnClick="btnNewChild_Click" />
                                <br />
                                <asp:Label ID='lblNewContentChild' runat='server' Text='New Child' />
                            </li>
                            <li class="seperator"></li>
                            <li>
                                <asp:ImageButton ID="btnSaveContent_" runat="server" ImageUrl="img/button/save-icon.png"
                                    AlternateText="Save Content" ValidationGroup="saveContent" ToolTip="Save Content"
                                    OnClick="btnSaveContent_Click" /><br />
                                <asp:Label ID='lblSaveContent_' runat='server' Text='Save' />
                            </li>
                            <li>
                                <asp:HyperLink ID="_customFieldLink" rel="managecustomfields" runat="server" ToolTip="Manage Custom Fields"
                                    ImageUrl="img/button/applications-cascade-icon.png" NavigateUrl="~/admin/frames/managecustomfields.aspx"></asp:HyperLink><br />
                                <asp:Label ID='_customFieldLinkText' runat='server' Text='Fields' />
                            </li>
                            <li>
                                <asp:HyperLink ID="hplContentForm_" rel="managecontentform" runat="server" ToolTip="Manage Content Form"
                                    ImageUrl="img/button/document-write-icon.png" NavigateUrl="~/admin/frames/managecontentform.aspx"></asp:HyperLink><br />
                                <asp:Label ID='lblContentForm_' runat='server' Text='Form' />
                            </li>
                            <li>
                                <asp:ImageButton ID="btnDeleteContent_" runat="server" AlternateText="Delete Content"
                                    ImageUrl="img/button/delete-icon.png" ToolTip="Delete Content" OnClientClick="return confirm('Are you sure you want to delete the content?');"
                                    ValidationGroup="saveContent" OnClick="btnDeleteContent_Click" /><br />
                                <asp:Label ID='lblDeleteContent_' runat='server' Text='Delete' />
                            </li>
                        </ul>
                    </div>
                    <div style="float: right">
                        <b>
                            <asp:Label ID='_methodsTitle' runat='server' Text='XML Calls' /></b>
                        <asp:DropDownList runat='server' ID='_methodType' AutoPostBack="True" CssClass="textMedium"
                            OnSelectedIndexChanged="_methodType_SelectedIndexChanged">
                            <asp:ListItem Value="atom">Atom</asp:ListItem>
                            <asp:ListItem Value="json">JSON</asp:ListItem>
                            <asp:ListItem Value="rss" Selected="True">RSS</asp:ListItem>
                        </asp:DropDownList>
                        <asp:HyperLink ID="_methodGet" Target="_blank" Visible="false" runat="server" Text="Get" />
                        <asp:HyperLink ID="_methodGetList" Target="_blank" Visible="false" runat="server"
                            Text="GetList" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanel3" runat='server'>
            <ContentTemplate>
                <table>
                    <tr>
                        <td>
                            <table cellpadding="3" cellspacing="2" class="style1">
                                <tr>
                                    <td align="right" nowrap="nowrap" valign="top" class="requiredField">
                                        Id:
                                    </td>
                                    <td nowrap="nowrap" valign="top">
                                        <asp:Literal ID="_contentId" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" nowrap="nowrap" valign="top" class="requiredField">
                                        Alias:
                                    </td>
                                    <td nowrap="nowrap" valign="top" width="100%">
                                        <asp:TextBox ID="_alias" runat="server" CssClass="textLong" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="_alias"
                                            ErrorMessage="required" ValidationGroup="saveContent" Display="Dynamic" />
                                        <asp:RegularExpressionValidator ID='revContentName' runat='server' ControlToValidate="_alias"
                                            ErrorMessage="You can't use special characters" ValidationGroup="saveContent"
                                            Display="Dynamic" ValidationExpression="^([0-9a-zA-Z\&amp;\-\.\'+'\'_']*|)" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" nowrap="nowrap">
                                        Parent Content:
                                    </td>
                                    <td nowrap="nowrap">
                                        <asp:DropDownList ID="_contentParentList" CssClass="textMedium" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" nowrap="nowrap">
                                        Enabled:
                                    </td>
                                    <td nowrap="nowrap" colspan="3">
                                        <asp:CheckBox ID='cbStatusContent_' runat='server' />&nbsp;&nbsp;Commentable:<asp:CheckBox
                                            ID="_contentIsCommentable" runat="server" />
                                        &nbsp;&nbsp;Sort Order:
                                        <asp:TextBox ID="_contentSortOrder" runat="server" CssClass="textShort"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td nowrap="nowrap">
                                        Publish Start Date:
                                    </td>
                                    <td nowrap="nowrap">
                                        <asp:TextBox ID="_contentPublishStartDate" runat="server" CssClass="textMedium"></asp:TextBox>
                                        <span class="table_td_second_section" />End Date:<asp:TextBox ID="_contentPublishEndDate"
                                            runat="server" CssClass="textMedium"></asp:TextBox><ajax:CalendarExtender ID="CalendarExtender1"
                                                runat="server" PopupPosition="TopRight" TargetControlID="_contentPublishStartDate"
                                                Format="MM/dd/yyyy" />
                                        <ajax:CalendarExtender ID="CalendarExtender2" runat="server" PopupPosition="TopRight"
                                            TargetControlID="_contentPublishEndDate" Format="MM/dd/yyyy" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" nowrap="nowrap">
                                        Event Start Date:
                                    </td>
                                    <td nowrap="nowrap">
                                        <asp:TextBox ID="_contentEventStartDate" runat="server" CssClass="textMedium"></asp:TextBox><ajax:CalendarExtender
                                            ID="CalendarExtender3" runat="server" Format="MM/dd/yyyy" PopupPosition="TopRight"
                                            TargetControlID="_contentEventStartDate" />
                                        <span class="table_td_second_section" />End Date:<asp:TextBox ID="_contentEventEndDate"
                                            runat="server" CssClass="textMedium"></asp:TextBox><ajax:CalendarExtender ID="CalendarExtender4"
                                                runat="server" PopupPosition="TopRight" TargetControlID="_contentEventEndDate"
                                                Format="MM/dd/yyyy" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" nowrap="nowrap" valign="top">
                                        Langugage:
                                    </td>
                                    <td nowrap="nowrap" valign="top" width="100%">
                                        <asp:DropDownList ID='_culture' runat='server' AutoPostBack="True" OnSelectedIndexChanged="_culture_SelectedIndexChanged"
                                            CssClass="textMedium" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" nowrap="nowrap" valign="top">
                                        Title:
                                    </td>
                                    <td nowrap="nowrap" valign="top" width="100%">
                                        <asp:TextBox ID="_title" runat="server" CssClass="textLong" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" nowrap="nowrap">
                                        Image URL:
                                    </td>
                                    <td nowrap="nowrap">
                                        <asp:TextBox ID="_contentImageUrl" runat="server" class="textLong" Width="300px"></asp:TextBox><a
                                            rel="selectfile" href='frames/selectfile.aspx?type=image&group=Content Images&callback=setImageFromIFrame'><img
                                                class="toolBarButton" src="img/button/1616/Folder-Open-icon.png" alt="" />
                                        </a>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" nowrap="nowrap" valign="top">
                                        Tags:
                                    </td>
                                    <td nowrap="nowrap">
                                        <asp:TextBox ID="_contentTags" runat="server" ReadOnly="true" class="textMedium"
                                            Width="300px" />
                                        <asp:HyperLink runat='server' ID='_expandTags' onclick='showHideDiv("tagList");'
                                            NavigateUrl="#"><img alt="" src="img/expand_collapse_icon.png" /></asp:HyperLink><i>(Comma
                                                seperated tags)</i>
                                        <div id='tagList' class="collapsablePanelSmall" style="display: none;">
                                            <asp:CheckBoxList ID="_tagList" runat='server'>
                                            </asp:CheckBoxList>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top" align="right" style="text-align: right;">
                            <br />
                            <asp:Image ID='_contentImage' runat='server' Width="150px" />
                        </td>
                    </tr>
                </table>
                <div>
                    <cc1:TinyMCETextArea ID="_description" InstallPath="includes/tiny_mce" theme="advanced"
                        plugins="spellchecker,safari,pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,inlinepopups,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template"
                        theme_advanced_buttons1="bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,|,formatselect,fontselect,fontsizeselect,|,forecolor,backcolor,|,preview,fullscreen"
                        theme_advanced_buttons2="pastetext,pasteword,|,search,replace,|,bullist,numlist,|,outdent,indent,|,link,unlink,anchor,image,cleanup,code,|,removeformat,|,sub,sup,|,charmap,emotions,iespell,media,advhr"
                        theme_advanced_buttons3=""
                        theme_advanced_buttons4=""
                        theme_advanced_toolbar_location="top"
                        theme_advanced_toolbar_align="left" theme_advanced_path_location="bottom" ConvertFontToSpan="false"
                        runat="server" Width="100%" Rows="17" />
                </div>
                <asp:GridView ID='customFieldsList' runat='server' AutoGenerateColumns="False" Width="99%"
                    GridLines="None" OnRowDataBound="customFieldsList_RowDataBound" CellPadding="4"
                    ForeColor="#333333">
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <Columns>
                        <asp:BoundField DataField="title" HeaderText="Custom Fields" HeaderStyle-Wrap="false">
                            <HeaderStyle Wrap="False" />
                            <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" Width="150px" />
                        </asp:BoundField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID='_fieldId' runat='server' Visible="false" Text='<%# Eval("customFieldId") %>'></asp:Label><asp:TextBox
                                    ID="_fieldValue" runat='server' Text='<%# Eval("defaultValue") %>'></asp:TextBox></ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="99%" />
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#5D7B9D" CssClass="full" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#999999" />
                    <AlternatingRowStyle CssClass="bg" BackColor="White" ForeColor="#284775" />
                </asp:GridView>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnSaveContent_" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
