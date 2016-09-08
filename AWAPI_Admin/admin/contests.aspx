<%@ Page Title="AWAPI - Contests" Language="C#" MasterPageFile="~/admin/admin.Master"
    AutoEventWireup="true" CodeBehind="contests.aspx.cs" Inherits="AWAPI.Admin.PageContests" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlaceHolder" runat="server">
    <div id='left-column'>
        <div class='title'>
            <div style="float: left">
                <h3>
                    Contest List
                </h3>
            </div>
            <div style="float: right">
                <asp:ImageButton ID='btnRefresh' runat='server' AlternateText="Refresh List" ImageUrl="img/bg-refresh.png"
                    OnClick="btnRefresh_Click" ToolTip="Refresh Group List" />
            </div>
        </div>
        <asp:UpdatePanel ID="UpdatePanel1" runat='server'>
            <ContentTemplate>
                <div class="nav">
                    Contest Group:<br />
                    <asp:DropDownList ID="_searchGroupList" runat="server" CssClass="textMedium" AutoPostBack="True"
                        OnSelectedIndexChanged="_searchGroupList_SelectedIndexChanged" />
                    &nbsp;<asp:HyperLink ID="hplPopupContestGroup_" rel="managecontestgroups" runat="server"
                        ToolTip="Manage Contest Groups" ImageUrl="img/button/1616/folder-icon.png" NavigateUrl="~/admin/frames/managecontestgroups.aspx"></asp:HyperLink>
                    <asp:HyperLink ID="hplPopupContestGroupEntry_" rel="contestentries" runat="server"
                        Text="Group Contest Entries" ImageUrl="img/button/1616/user-group-icon.png" NavigateUrl="~/admin/frames/contestentries.aspx"
                        ToolTip="Contest Entries" />
                    <hr />
                    <asp:GridView ID="gwList" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        CellPadding="2" CellSpacing="2" GridLines="None" OnPageIndexChanging="gwList_PageIndexChanging"
                        OnRowCommand="gwList_RowCommand" PageSize="15" ShowHeader="True" Style="width: 100%"
                        OnRowDataBound="gwList_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Contest Title"
                                ItemStyle-Width="100%" ItemStyle-Wrap="true">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtnSelect" runat="server" CommandArgument='<%# Eval ("contestid") %>'
                                        CommandName="selectcontest" Text='<%# Eval("title") %>' />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle Wrap="True" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="pubDate" DataFormatString="{0:MM/dd/yy}" HeaderText="Start" />
                            <asp:BoundField DataField="pubEndDate" DataFormatString="{0:MM/dd/yy}" HeaderText="End" />
                        </Columns>
                    </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id='center-column'>
        <asp:UpdatePanel ID="UpdatePanel2" runat='server'>
            <ContentTemplate>
                <div class="top-bar">
                    <h1>
                        Manage Contest</h1>
                </div>
                <div class="select-bar">
                    <ul>
                        <li>
                            <asp:ImageButton ID="btnNewContest_" runat="server" ImageUrl="img/button/new-icon.png"
                                AlternateText="New Contest" ToolTip="New Contest" CausesValidation="false" OnClick="btnNewContest_Click" />
                            <br />
                            <asp:Label ID='lblNewContest_' runat='server' Text='New' />
                        </li>
                        <li class="seperator"></li>
                        <li>
                            <asp:ImageButton ID="btnSaveContest_" runat="server" ImageUrl="img/button/save-icon.png"
                                AlternateText="Save Contest" ToolTip="Save Contest" ValidationGroup="save_" OnClick="btnSaveContest_Click" />
                            <br />
                            <asp:Label ID='lblSaveContest_' runat='server' Text='Save' />
                        </li>
                        <li>
                            <asp:HyperLink ID="hplPopupContestEntry_" rel="contestentries" runat="server" Text="Contest Entries"
                                ImageUrl="img/button/user-group-icon.png" NavigateUrl="~/admin/frames/contestentries.aspx"
                                ToolTip="Contest Entries" />
                            <br />
                            <asp:Label ID='lblPopupContestEntry_' runat='server' Text='Entries' />
                        </li>
                        <li>
                            <asp:HyperLink ID="hplPopupContestUsers_" rel="contestusers" runat="server" Text="Contest Users"
                                ImageUrl="img/button/contest-users-icon.png" NavigateUrl="~/admin/frames/contestusers.aspx"
                                ToolTip="Contest Users" />
                            <br />
                            <asp:Label ID='lblPopupContestUsers_' runat='server' Text='Entries' />
                        </li>
                        <li>
                            <asp:ImageButton ID="btnDeleteContest_" runat="server" ImageUrl="img/button/delete-icon.png"
                                AlternateText="Delete Contest" ToolTip="Delete Contest" OnClick="btnDeleteContest_Click"
                                OnClientClick="return confirm('Are you sure you want to delete the contest?');" /><br />
                            <asp:Label ID='lblDeleteContest_' runat='server' Text='Delete' />
                        </li>
                    </ul>
                </div>
                <table cellspacing="2" cellpadding="2">
                    <tbody>
                        <tr>
                            <td class="td_first" nowrap="nowrap">
                                Id:
                            </td>
                            <td>
                                <asp:Literal ID="_id" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="td_first" nowrap="nowrap">
                                Ttitle:
                            </td>
                            <td>
                                <asp:TextBox ID="_title" runat="server" CssClass="textLong" ValidationGroup="save_" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="_title"
                                    ErrorMessage="required" ValidationGroup="save_" Display="Dynamic"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_first" nowrap="nowrap">
                                Description:
                            </td>
                            <td>
                                <asp:TextBox ID="_description" runat="server" CssClass="textDescription" TextMode="MultiLine" />
                            </td>
                        </tr>
                        <tr>
                            <td class="td_first" nowrap="nowrap">
                                Enabled:
                            </td>
                            <td nowrap="nowrap">
                                <asp:CheckBox ID='_isEnabled' runat='server' />
                            </td>
                        </tr>
                        <tr>
                            <td class="td_first" nowrap="nowrap">
                                Maximum Number of Entries:
                            </td>
                            <td nowrap="nowrap">
                                <asp:TextBox ID="_maxEntry" runat='server' CssClass="textShort" />
                            </td>
                        </tr>
                        <tr>
                            <td class="td_first" nowrap="nowrap">
                                Maximum Entry Per User:
                            </td>
                            <td nowrap="nowrap">
                                <asp:TextBox ID="_entryPerUser" runat='server' CssClass="textShort" />&nbsp;in&nbsp;
                                <asp:TextBox ID="_entryPerUserPeriodValue" runat='server' CssClass="textShort" />
                                <asp:DropDownList ID='_entryPerUserPeriodType' CssClass="textMedium" runat='server'>
                                    <asp:ListItem Text="None" Selected="True" Value="" />
                                    <asp:ListItem Text="days" Value="d" />
                                    <asp:ListItem Text="weeks" Value="w" />
                                    <asp:ListItem Text="months" Value="m" />
                                </asp:DropDownList>
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
                        <tr>
                            <td class="td_first" nowrap="nowrap">
                                Number of Winners:
                            </td>
                            <td nowrap="nowrap">
                                <asp:TextBox ID="_numberOfWinners" runat='server' Text="1" CssClass="textShort" />
                            </td>
                        </tr>
                        <tr>
                            <td class="td_first">
                                Contest Group:
                            </td>
                            <td>
                                <asp:CheckBoxList ID="_contestGroupList" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="td_first">
                                Send Email To Moderator:
                            </td>
                            <td>
                                <asp:DropDownList ID="_sendEmailToModeratorTemplate" runat='server' />
                                <i>Email will be sent after a new contest entry</i>
                                <br />
                                <asp:TextBox ID="_sendEmailToModeratorRecipes" TextMode="MultiLine" Rows="2" runat="server"
                                    CssClass="textLong" Width="100%" />
                                <i>comma seperated</i>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_first">
                                Send Email After Submission:
                            </td>
                            <td>
                                <asp:DropDownList ID="_sendEmailAfterSubmissionTemplate" runat='server' />
                                <i>Email will be sent to the contender after the submission</i>
                            </td>
                        </tr>                        
                        <tr>
                            <td class="td_first">
                                Send Email After Approve Entry:
                            </td>
                            <td>
                                <asp:DropDownList ID="_sendEmailAfterApproveEntryEmailTemplate" runat='server' />
                                <i>Email will be sent to the contender</i>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_first">
                                Send Email After Delete Entry:
                            </td>
                            <td>
                                <asp:DropDownList ID="_sendEmailAfterDeleteEntryEmailTemplate" runat='server' />
                                <i>Email will be sent to the contender</i>
                                <br />
                                <i>Awapilable Template Fields:<br />
                                    <ul>
                                        <li>{firstname} :Name of the commenter</li>
                                        <li>{lastname} : Commenter's email address</li>
                                        <li>{email} : Comment</li>
                                        <li>{link} : Link to moderate the comment through the admin module</li>
                                        <li>{date} : Comment date</li>
                                    </ul>
                                </i>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
