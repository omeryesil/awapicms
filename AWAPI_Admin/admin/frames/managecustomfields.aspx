<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="managecustomfields.aspx.cs"
    Inherits="AWAPI.Admin.frames.managecustomfields" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AWAPI - Manage Custom Fields</title>
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
    <div id="main" style="min-height: 465px;">
        <div id="header">
            <h3>
                Manage Custom Fields</h3>
        </div>
        <div id="middle">
            <asp:UpdatePanel ID="UpdatePanel2" runat='server'>
                <ContentTemplate>
                    <div id='left-column' style="width: 340px;">
                        <asp:GridView ID='_customFieldList' runat='server' AutoGenerateColumns="False" BorderStyle="None"
                            CellPadding="3" CellSpacing="3" GridLines="None" OnRowCommand="_customFieldList_RowCommand">
                            <Columns>
                                <asp:TemplateField HeaderText="title">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="customFieldTitleInList" runat='server' Text='<%# Bind("title") %>'
                                            CommandName="editfield" CommandArgument='<%# Bind("customFieldId") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="100%" Wrap="True" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="sortOrder" HeaderText="Order" />
                                <asp:BoundField DataField="maximumLength" HeaderText="Max.Len." />
                                <asp:BoundField DataField="fieldType" HeaderText="Type" />
                                <asp:BoundField DataField="applyToSubContents" HeaderStyle-Wrap="false" HeaderText="Sub" />
                                <asp:BoundField DataField="isEnabled" HeaderText="Enabled" />
                            </Columns>
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:GridView>
                    </div>
                    <div id='center-column' style="width: 450px;">
                        <div id='top-bar'>
                            <asp:ImageButton ID="btnNewCustomField" runat="server" ImageUrl="../img/button/new-icon.png"
                                AlternateText="Add New Custom Field" CausesValidation="False" OnClick="btnNewCustomField_Click1" />
                            <asp:ImageButton ID="btnSaveCustomField" runat="server" ImageUrl="../img/button/save-icon.png"
                                AlternateText="Save" ValidationGroup="saveField" OnClick="btnSaveCustomField_Click" />
                            <asp:ImageButton ID="btnDeleteField" runat="server" AlternateText="Delete Field"
                                ImageUrl="../img/button/delete-icon.png" OnClientClick="return confirm('Are you sure you want to delete the field?');"
                                Visible="False" ValidationGroup="saveField" OnClick="btnDeleteField_Click" />
                            <asp:Literal ID="_msg" runat='server' />
                        </div>
                        <asp:Panel ID='pnlContent' runat='server'>
                            <table align="right" cellpadding="3" cellspacing="4" class="style1">
                                <tr>
                                    <td align="right" valign="top">
                                        Field Id
                                    </td>
                                    <td width="100%">
                                        <asp:Literal ID="_customFieldId" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        Title:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="_title" runat="server" CssClass="textLong"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="_title"
                                            Display="Dynamic" ErrorMessage="required" ValidationGroup="saveField"></asp:RequiredFieldValidator>
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
                                        Enabled:
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="_isEnabled" runat="server" />&nbsp;&nbsp; Apply To Child Contents:<asp:CheckBox
                                            ID="_applyToChildContent" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" valign="top">
                                        Data Type:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="_fieldType" CssClass="textMedium" runat="server">
                                            <asp:ListItem>Checkbox</asp:ListItem>
                                            <asp:ListItem>DateTime</asp:ListItem>
                                            <asp:ListItem>File</asp:ListItem>
                                            <asp:ListItem Selected="True">TextBox</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" valign="top">
                                        Maximum Length
                                    </td>
                                    <td>
                                        <asp:TextBox ID="_maximumLength" runat="server" CssClass="textShort">0</asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" valign="top">
                                        Minimum Value
                                    </td>
                                    <td>
                                        <asp:TextBox ID="_minimumValue" runat="server" CssClass="textShort">0</asp:TextBox>
                                        &nbsp;Maximum Value
                                        <asp:TextBox ID="_maximumValue" runat="server" CssClass="textShort">0</asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" valign="top">
                                        Default Value:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="_defaultValue" runat="server" CssClass="textLong"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" valign="top">
                                        Regular Expression
                                    </td>
                                    <td>
                                        <asp:TextBox ID="_regularExpression" runat="server" CssClass="textLong"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" valign="top">
                                        Sort Order
                                    </td>
                                    <td>
                                        <asp:TextBox ID="_sortOrder" runat="server" CssClass="textShort">1</asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    </form>
</body>
</html>
