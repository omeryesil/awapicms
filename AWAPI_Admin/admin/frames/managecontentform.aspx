<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="managecontentform.aspx.cs"
    Inherits="AWAPI.Admin.frames.managecontentform" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AWAPI - Manage Content Forms</title>
    <link href="../css/iframe.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID='scriptManager1' runat='server'>
    </asp:ScriptManager>
    <div id="main" style="min-height: 520px;">
        <div id="header">
            <h3>
                <asp:Label ID="lblTitle" runat='server' Text="Manage Content Form" /></h3>
        </div>
        <div id='middle'>
            <asp:UpdatePanel ID="UpdatePanel2" runat='server'>
                <ContentTemplate>
                    <div id='center-column'>
                        <div id='top-bar'>
                            <asp:ImageButton ID="btnSaveContentForm_" runat="server" ImageUrl="../img/button/save-icon.png"
                                AlternateText="Save" ToolTip="Save Form" ValidationGroup="saveForm" OnClick="btnSaveContentForm__Click" />
                            <asp:ImageButton ID="btnDeleteContentForm_" runat="server" AlternateText="Delete Form"
                                ImageUrl="../img/button/delete-icon.png" OnClientClick="return confirm('Are you sure you want to delete the form?');"
                                Visible="False" ValidationGroup="saveForm" OnClick="btnDeleteContentForm__Click1" />
                            <asp:ImageButton ID="btnSyncFormFields_" runat="server" AlternateText="Sync the fields"
                                ImageUrl="../img/button/share-icon.png" Visible="False" OnClick="btnSyncFormFields_Click" />
                            <asp:Literal ID="_msg" runat='server' />
                        </div>
                        <table width="100%">
                            <tr>
                                <td valign="top">
                                    <table align="right" cellpadding="3" cellspacing="4" class="style1">
                                        <tr>
                                            <td align="right" valign="top">
                                                ContentFormId
                                            </td>
                                            <td width="100%">
                                                <asp:Literal ID="_contentFormId" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                Enabled:
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="_isEnabled" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                Title:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="_title" runat="server" CssClass="textLong"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="_title"
                                                    Display="Dynamic" ErrorMessage="required" ValidationGroup="saveForm"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                Description:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="_description" runat="server" CssClass="textDescription" TextMode="MultiLine"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" nowrap="nowrap">
                                                Apply To Sub Contents:
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="_applyToSubContent" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" nowrap="nowrap">
                                                Can Create New:
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="_canCreateNew" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" nowrap="nowrap">
                                                Can Update:
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="_canUpdate" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" nowrap="nowrap">
                                                Can Delete:
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="_canDelete" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td valign="top">
                                    <asp:GridView ID="_fieldList" runat='server' GridLines="None" AutoGenerateColumns="false"
                                        Width="100%" OnRowDataBound="_fieldList_RowDataBound" OnRowCommand="_fieldList_RowCommand"
                                        CellPadding="1" CellSpacing="2">
                                        <HeaderStyle VerticalAlign="Top" HorizontalAlign="Left" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Visible" ItemStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ibtnVisible" CommandName="changevisiblestatus" CommandArgument='<%# Bind("contentFormFieldSettingId") %>'
                                                        runat='server' ImageUrl='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "isVisible"))==true?"~/admin/img/button/1212/ok-icon.png":"~/admin/img/button/1212/ok-disabled-icon.png" %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="50px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Enabled" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ibtnEnabled" CommandName="changeenablestatus" CommandArgument='<%# Bind("contentFormFieldSettingId") %>'
                                                        runat='server' ImageUrl='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "isEnabled"))==true?"~/admin/img/button/1212/ok-icon.png":"~/admin/img/button/1212/ok-disabled-icon.png" %>' />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="title" HeaderText="Field" ItemStyle-Width="50%">
                                                <ItemStyle Width="50%" />
                                            </asp:BoundField>
                                            <asp:TemplateField ItemStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ibtnUp" runat='server' ImageUrl="~/admin/img/button/1616/Up-icon.png"
                                                        ToolTip="Move Up" CommandName="moveup" CommandArgument='<%# Bind("contentFormFieldSettingId")  %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="50px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Width="50px">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ibtnDown" runat='server' ImageUrl="~/admin/img/button/1616/down-icon.png"
                                                        ToolTip="Move Down" CommandName="movedown" CommandArgument='<%# Bind("contentFormFieldSettingId")  %>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="50px" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    </form>
</body>
</html>
