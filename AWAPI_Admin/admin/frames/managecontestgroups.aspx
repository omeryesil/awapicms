<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="managecontestgroups.aspx.cs"
    Inherits="AWAPI.Admin.frames.managecontestgroups" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
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
    <div id="main" style="min-height: 370px;">
        <div id="header">
            <h3>
                Manage Contest Groups</h3>
        </div>
        <div id="middle" style="height: 310px;">
            <div id='left-column' style="width: 170px;">
                <asp:UpdatePanel runat='server'>
                    <ContentTemplate>
                        <asp:GridView ID='_list' runat='server' AutoGenerateColumns="False" BorderStyle="None"
                            CellPadding="3" CellSpacing="3" GridLines="None" OnRowCommand="_list_RowCommand">
                            <Columns>
                                <asp:TemplateField HeaderText="Title" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbtnGroupTitle" runat='server' Text='<%# Bind("title") %>' CommandName="editContestGroup"
                                            CommandArgument='<%# Bind("ContestGroupid") %>'></asp:LinkButton>
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
                            <asp:ImageButton ID="btnNewContestGroup_" runat="server" ImageUrl="../img/button/new-icon.png"
                                AlternateText="New Group" ToolTip="New Group" OnClick="btnNewContestGroup_Click"
                                CausesValidation="false" />
                            <asp:ImageButton ID="btnSaveContestGroup_" runat="server" ImageUrl="../img/button/save-icon.png"
                                AlternateText="Save" ToolTip="Save Group" OnClick="btnSaveContestGroup_Click"
                                ValidationGroup="_saveGroup" />
                            <asp:ImageButton ID="btnDeleteContestGroup_" runat="server" ImageUrl="../img/button/delete-icon.png"
                                ToolTip="Delete File Group" AlternateText="Delete File Group" OnClientClick="return confirm('Are you sure you want to delete the file group?');"
                                OnClick="btnDeleteContestGroup_Click" />
                            <asp:Literal ID='_msg' runat='server' />
                        </div>
                        <table cellspacing="1" cellpadding="1">
                            <tbody>
                                <tr>
                                    <td class="td_first">
                                        Id:
                                    </td>
                                    <td>
                                        <asp:Literal ID="_groupId" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_first">
                                        Enabled:
                                    </td>
                                    <td>
                                        <asp:CheckBox ID='_isEnabled' runat='server' />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_first">
                                        Title:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="_title" runat="server" CssClass="textLong" ValidationGroup="saveGroup" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="_title"
                                            ErrorMessage="required" ValidationGroup="_saveGroup"></asp:RequiredFieldValidator>
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
                                        Number Of Winners:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="_numberOfWinners" runat="server" CssClass="textShort" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="td_first">
                                        Publish Start Date:
                                    </td>
                                    <td nowrap="nowrap">
                                        <asp:TextBox ID="_publishStartDate" runat="server" CssClass="textMedium"></asp:TextBox>
                                        End Date:
                                        <asp:TextBox ID="_publishEndDate" runat="server" CssClass="textMedium"></asp:TextBox>
                                        <ajax:CalendarExtender ID="CalendarExtender1" runat="server" PopupPosition="TopRight"
                                            TargetControlID="_publishStartDate" Format="MM/dd/yyyy" />
                                        <ajax:CalendarExtender ID="CalendarExtender2" runat="server" PopupPosition="TopRight"
                                            TargetControlID="_publishEndDate" Format="MM/dd/yyyy" />
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
