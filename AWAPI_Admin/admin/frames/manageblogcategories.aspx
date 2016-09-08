<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="manageblogcategories.aspx.cs"
    Inherits="AWAPI.Admin.frames.manageblogcategories" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AWAPI - Manage Blog Categories</title>
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
    <div id="main" style="min-height: 460px;">
        <div id="header">
            <h3>
                Manage Blog Categories</h3>
        </div>
        <div id="middle">
            <div id='left-column' style="width: 270px;">
                <asp:UpdatePanel ID="UpdatePanel2" runat='server'>
                    <ContentTemplate>
                        <asp:GridView ID='_list' runat='server' AutoGenerateColumns="False" BorderStyle="None"
                            CellPadding="3" CellSpacing="3" GridLines="None" OnRowCommand="_list_RowCommand">
                            <Columns>
                                <asp:TemplateField HeaderText="Title" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="customFieldTitleInList" runat='server' Text='<%# Bind("title") %>'
                                            CommandName="editcat" CommandArgument='<%# Bind("blogcategoryid") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="100%" Wrap="True" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="isEnabled" HeaderText="Enabled" />
                            </Columns>
                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div id='center-column' style="width: 450px;">
                <asp:UpdatePanel ID="UpdatePanel1" runat='server'>
                    <ContentTemplate>
                        <div id='top-bar'>
                            <asp:ImageButton ID="btnNew" runat="server" ImageUrl="../img/button/new-icon.png"
                                AlternateText="Add New Category" ToolTip="Add New Category" CausesValidation="False"
                                OnClick="btnNew_Click" />
                            <asp:ImageButton ID="btnSave" runat="server" ImageUrl="../img/button/save-icon.png"
                                AlternateText="Save" ToolTip="Save Category" ValidationGroup="save_" OnClick="btnSave_Click" />
                            <asp:ImageButton ID="btnDelete" runat="server" AlternateText="Delete" ImageUrl="../img/button/delete-icon.png"
                                ToolTip="Delete Category" OnClientClick="return confirm('Are you sure you want to delete the category?');"
                                Visible="False" ValidationGroup="save_" OnClick="btnDelete_Click" />
                            <asp:Literal ID="_msg" runat='server' />
                        </div>
                        <asp:Panel ID='pnlContent' runat='server'>
                            <table align="right" cellpadding="3" cellspacing="4" class="style1">
                                <tr>
                                    <td align="right" valign="top">
                                        Id
                                    </td>
                                    <td width="100%">
                                        <asp:Literal ID="_id" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" nowrap="nowrap">
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
                                            Display="Dynamic" ErrorMessage="required" ValidationGroup="save_"></asp:RequiredFieldValidator>
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
                            </table>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
