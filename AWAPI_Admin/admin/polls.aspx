<%@ Page Title="AWAPI - Polls" Language="C#" MasterPageFile="~/admin/admin.Master"
    AutoEventWireup="true" CodeBehind="polls.aspx.cs" Inherits="AWAPI.Admin.PagePolls" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlaceHolder" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat='server'>
        <ContentTemplate>
            <div id='left-column'>
                <div class='title'>
                    <div style="float: left">
                        <h3>
                            Poll List
                        </h3>
                    </div>
                    <div style="float: right">
                    </div>
                </div>
                <div class="nav">
                    <asp:GridView ID="gwList" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        CellPadding="2" CellSpacing="2" GridLines="None" OnPageIndexChanging="gwList_PageIndexChanging"
                        OnRowCommand="gwList_RowCommand" PageSize="10" ShowHeader="True" Style="width: 100%"
                        OnRowDataBound="gwList_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Poll Title" ItemStyle-Width="100%"
                                ItemStyle-Wrap="true">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtnSelect" runat="server" CommandArgument='<%# Eval ("pollId") %>'
                                        CommandName="selectpoll" Text='<%# Eval("title") %>' />
                                    &nbsp;&nbsp;
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle Wrap="True" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="pubDate" DataFormatString="{0:MM/dd/yy}" HeaderText="Pubish Date" />
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="RSS" ItemStyle-Width="100%"
                                ItemStyle-Wrap="true">
                                <ItemTemplate>
                                    <asp:HyperLink ID='rssLink' runat='server' Target="_blank" Text='edit' ToolTip="Poll RSS"
                                        NavigateUrl='<%# AWAPI_BusinessLibrary.library.ConfigurationLibrary.Config.pollServiceUrl + "?method=getpoll&siteid=" + Eval ("siteid") + "&pollid=" + Eval ("pollid")  %>'
                                        ImageUrl="img/button/1616/rss-icon.png" />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <div id='center-column'>
                <!-- POLL -------------------------------------------------------------------- -->
                <div class="top-bar">
                    <h1>
                        Manage Poll</h1>
                </div>
                <div class="select-bar">
                    <ul>
                        <li>
                            <asp:ImageButton ID="btnNewPoll_" runat="server" ImageUrl="img/button/new-icon.png"
                                AlternateText="New Poll" ToolTip="New Poll" CausesValidation="false" OnClick="btnNewPoll__Click" />
                            <br />
                            <asp:Label ID='lblNewPoll_' runat='server' Text='New' />
                        </li>
                        <li class="seperator"></li>
                        <li>
                            <asp:ImageButton ID="btnSavePoll_" runat="server" ImageUrl="img/button/save-icon.png"
                                AlternateText="Save Poll" ToolTip="Save Poll" ValidationGroup="save_" OnClick="btnSavePoll__Click" />
                            <br />
                            <asp:Label ID='lblSavePoll_' runat='server' Text='Save' />
                        </li>
                        <li>
                            <asp:ImageButton ID="btnDeletePoll_" runat="server" ImageUrl="img/button/delete-icon.png"
                                AlternateText="Delete Poll" ToolTip="Delete Poll" OnClick="btnDeletePoll__Click"
                                OnClientClick="return confirm('Are you sure you want to delete the poll?');" /><br />
                            <asp:Label ID='lblDeletePoll_' runat='server' Text='Delete' />
                        </li>
                        <%--                        <li>
                            <asp:HyperLink ID="_choicesLink" rel="managepollchoices" runat="server" Text="Manage Choices"
                                ImageUrl="img/button/ok-icon.png" NavigateUrl="~/admin/frames/managepollchoices.aspx"
                                ToolTip="Manage Poll Choices" />
                            <br />
                            <asp:Label ID='_choicesLinkText' runat='server' Text='choices' />
                        </li>--%>
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
                                Enabled:
                            </td>
                            <td nowrap="nowrap">
                                <asp:CheckBox ID='_isEnabled' runat='server' /><span class="table_td_second_section">Multiple
                                    Choice:</span><asp:CheckBox ID="_isMultipleChoice" runat="server" />
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
                            <td class="td_first">
                                Publish Start Date:
                            </td>
                            <td nowrap="nowrap">
                                <asp:TextBox ID="_publishStartDate" runat="server" CssClass="textMedium"></asp:TextBox>
                                <ajax:CalendarExtender ID="CalendarExtender1" runat="server" PopupPosition="TopRight"
                                    TargetControlID="_publishStartDate" Format="MM/dd/yyyy" />
<%--                                <span class="table_td_second_section">End Date:</span>
                                <asp:TextBox ID="_publishEndDate" runat="server" CssClass="textMedium"></asp:TextBox>
                                <ajax:CalendarExtender ID="CalendarExtender2" runat="server" PopupPosition="TopRight"
                                    TargetControlID="_publishEndDate" Format="MM/dd/yyyy" />--%>
                                <br />
                                <i>If you set the value, poll will be available after this date</i>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" nowrap="nowrap" valign="top">
                                Langugage:
                            </td>
                            <td nowrap="nowrap" valign="top" width="100%">
                                <asp:DropDownList ID='_culture' runat='server' AutoPostBack="True" CssClass="textMedium"
                                    OnSelectedIndexChanged="_culture_SelectedIndexChanged" />
                            </td>
                        </tr>
                        <tr>
                            <td class="td_first" nowrap="nowrap">
                                Question:
                            </td>
                            <td>
                                <asp:TextBox ID="_description" runat="server" CssClass="textDescription" TextMode="MultiLine" />
                            </td>
                        </tr>
                        <tr>
                            <td class="td_first" nowrap="nowrap">
                                Answered Question:
                            </td>
                            <td>
                                <asp:TextBox ID="_answeredQuestion" runat="server" CssClass="textDescription" TextMode="MultiLine" />
                            </td>
                        </tr>
                        <tr>
                            <td class="td_first" nowrap="nowrap">
                                Total Number Of Votes:
                            </td>
                            <td>
                                <asp:Label ID="_totalNumberOfVotes" runat="server" ReadOnly="True">0</asp:Label>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <!-- CHOICES -------------------------------------------------------------------- -->
                <div>
                    <asp:Panel ID='pnlChoices' runat='server' Visible="false">
                        <div class="select-bar">
                            <ul>
                                <li>
                                    <h3>
                                        Poll
                                        <br />
                                        Choices</h3>
                                </li>
                                <li>
                                    <asp:ImageButton ID="btnNewPoll_Choice" runat="server" ImageUrl="img/button/new-icon.png"
                                        AlternateText="New Poll" ToolTip="New Choice" CausesValidation="false" OnClick="btnNewPoll_Choice_Click" />
                                    <br />
                                    <asp:Label ID='lblNewPoll_Choice' runat='server' Text='New' />
                                </li>
                                <li class="seperator"></li>
                                <li>
                                    <asp:ImageButton ID="btnSavePoll_Choice" runat="server" ImageUrl="img/button/save-icon.png"
                                        AlternateText="Save Choice" ToolTip="Save Choice" ValidationGroup="savechoice_"
                                        OnClick="btnSavePoll_Choice_Click" />
                                    <br />
                                    <asp:Label ID='lblSavePoll_Choice' runat='server' Text='Save' />
                                </li>
                                <li>
                                    <asp:ImageButton ID="btnDeletePoll_Choice" runat="server" ImageUrl="img/button/delete-icon.png"
                                        AlternateText="Delete Poll" ToolTip="Delete Poll" OnClientClick="return confirm('Are you sure you want to delete the choice?');"
                                        OnClick="btnDeletePoll_Choice_Click" /><br />
                                    <asp:Label ID='lblDeletePoll_Choice' runat='server' Text='Delete' />
                                </li>
                            </ul>
                        </div>
                        <table>
                            <tr>
                                <td valign="top">
                                    <table cellpadding="3" cellspacing="4">
                                        <tr>
                                            <td align="right" valign="top">
                                                Id
                                            </td>
                                            <td width="100%">
                                                <asp:Literal ID="_choiceId" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" valign="top">
                                                Choice:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="_choiceTitle" runat="server" CssClass="textLong" ValidationGroup="savechoice_"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="_title"
                                                    Display="Dynamic" ErrorMessage="required" ValidationGroup="savechoice_"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" valign="top">
                                                Description:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="_choiceDescription" runat="server" CssClass="textDescription" TextMode="MultiLine"
                                                    Height="40px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" valign="top">
                                                Sort Order
                                            </td>
                                            <td>
                                                <asp:TextBox ID="_choiceSortOrder" runat="server" CssClass="textShort">1</asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td valign="top">
                                    <br />
                                    <br />
                                    <asp:GridView ID='gwChoiceList' runat='server' AutoGenerateColumns="False" CellPadding="4"
                                        GridLines="None" Width="350px" ForeColor="#333333" OnRowCommand="gwChoiceListFieldList_RowCommand"
                                        OnRowDataBound="gwChoiceList_RowDataBound">
                                        <RowStyle BackColor="#EFF3FB" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="choice">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="_editChoice1" runat='server' Text='<%# Bind("title") %>' CommandName="editchoice"
                                                        CommandArgument='<%# Bind("pollChoiceId") %>'></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle Wrap="True" HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:Image ID='_choiceBar' runat='server' Visible="false" ImageUrl="img/poll_bar.jpg"
                                                        Height="13px" />
                                                </ItemTemplate>
                                                <ItemStyle Width="77px" Wrap="True" HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="numberOfVotes" HeaderText="Votes" ItemStyle-Width="40px" />
                                            <asp:BoundField DataField="sortOrder" HeaderText="Order" ItemStyle-Width="40px" />
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="_editChoice2" runat='server' Text='edit' CommandName="editchoice"
                                                        CommandArgument='<%# Bind("pollChoiceId") %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Top" BackColor="#507CD1" Font-Bold="True"
                                            ForeColor="White" />
                                        <EditRowStyle BackColor="#2461BF" />
                                        <AlternatingRowStyle BackColor="White" />
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
