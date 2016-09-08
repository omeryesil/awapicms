<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="manageshareit.aspx.cs"
    Inherits="AWAPI.Admin.frames.manageshareit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AWAPI - Manage Blog Tags</title>
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
    <div id="main" style="min-height: 450px;">
        <div id="header">
            <h3>
                My Shares</h3>
        </div>
        <div id="middle">
            <div id='left-column' style="width: 170px;">
                <asp:UpdatePanel runat='server'>
                    <ContentTemplate>
                        <asp:GridView ID='_list' runat='server' AutoGenerateColumns="False" BorderStyle="None"
                            CellPadding="3" CellSpacing="3" GridLines="None" OnRowCommand="_list_RowCommand">
                            <Columns>
                                <asp:TemplateField HeaderText="Title" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbtnEdit" runat='server' Text='<%# Bind("title") %>' CommandName="editshareit"
                                            CommandArgument='<%# Bind("shareitid") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="100%" Wrap="True" HorizontalAlign="Left" />
                                </asp:TemplateField>
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
                            <asp:ImageButton ID="btnNewShareIt_" runat="server" ImageUrl="../img/button/new-icon.png"
                                AlternateText="New Group" ToolTip="New Group" OnClick="btnNewShareIt_Click" CausesValidation="false" />
                            <asp:ImageButton ID="btnSaveShareIt_" runat="server" ImageUrl="../img/button/save-icon.png"
                                AlternateText="Save" ToolTip="Save File Group" OnClick="btnSaveShareIt_Click"
                                ValidationGroup="_save" />
                            <asp:ImageButton ID="btnDeleteShareIt_" runat="server" ImageUrl="../img/button/delete-icon.png"
                                ToolTip="Delete File Group" AlternateText="Delete File Group" OnClientClick="return confirm('Are you sure you want to delete the record?');"
                                OnClick="btnDeleteShareIt_Click" />
                            <asp:Literal ID="_msg" runat="server" />
                        </div>
                        <table cellspacing="1" cellpadding="1">
                            <tbody>
                                <tr>
                                    <td class="td_first">
                                        Id:
                                    </td>
                                    <td>
                                        <asp:Literal ID="_id" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_first">
                                        Share with everyone:
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="_shareWithEveryone" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_first">
                                        Title:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="_title" runat="server" CssClass="textLong" ValidationGroup="_save" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="_title"
                                            ErrorMessage="required" ValidationGroup="_save"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_first">
                                        Description:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="_description" runat="server" CssClass="textDescription" TextMode="MultiLine" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_first">
                                        Link:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="_link" runat="server" CssClass="textLong" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_first">
                                        Share With:
                                    </td>
                                    <td>
                                        <asp:CheckBoxList ID="_userList" runat="server" />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
