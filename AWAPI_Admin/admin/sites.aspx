<%@ Page Language="C#" MasterPageFile="~/admin/admin.Master" AutoEventWireup="true"
    CodeBehind="sites.aspx.cs" Inherits="AWAPI.Admin.PageSites" Title="AWAPI - Manage Sites" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlaceHolder" runat="server">
    <div id='left-column'>
        <div class='title'>
            <div style="float: left">
                <h3>
                    Site List
                </h3>
            </div>
            <div style="float: right">
            </div>
        </div>
        <asp:UpdatePanel ID="UpdatePanel1" runat='server'>
            <ContentTemplate>
                <div class="nav">
                    <asp:GridView ID="gwSiteList" runat="server" AutoGenerateColumns="False" BorderStyle="None"
                        BorderWidth="0px" CellPadding="2" CellSpacing="2" ShowHeader="False" Width="100%"
                        GridLines="None" OnRowCommand="gwSiteList_RowCommand" OnRowDataBound="gwSiteList_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="title">
                                <ItemTemplate>
                                    <asp:LinkButton ID='lbtnSelect' runat='server' Text='<%# Bind("title") %>' CommandName="edit_site"
                                        CommandArgument='<%# Bind("siteId") %>' />
                                </ItemTemplate>
                                <ItemStyle Width="100%" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id='center-column'>
        <asp:UpdatePanel runat='server'>
            <ContentTemplate>
                <div class="top-bar">
                    <h1>
                        Manage Site</h1>
                    <div class="breadcrumbs">
                        <asp:Literal ID='Literal1' runat='server'></asp:Literal></div>
                </div>
                <div class="select-bar">
                    <ul>
                        <li>
                            <asp:ImageButton ID="btnNewSite_" runat="server" ImageUrl="img/button/new-icon.png"
                                AlternateText="New Site" ToolTip="New Site" CausesValidation="false" OnClick="btnNewSite_Click" />
                            <br />
                            <asp:Label ID='lblNewSite_' runat='server' Text='New' />
                        </li>
                        <li class="seperator"></li>
                        <li>
                            <asp:ImageButton ID="btnSaveSite_" runat="server" ImageUrl="img/button/save-icon.png"
                                AlternateText="Save Site" ToolTip="Save Site" ValidationGroup="saveSite" OnClick="btnSaveSite_Click" />
                            <br />
                            <asp:Label ID='lblSaveSite_' runat='server' Text='Save' />
                        </li>
                        <li>
                            <asp:ImageButton ID="btnDeleteSite_" runat="server" ImageUrl="img/button/delete-icon.png"
                                AlternateText="Delete Site" ToolTip="Delete Site" OnClientClick="return alert('Sites cannot be deleted in this version. We are working on it');" /><br />
                            <asp:Label ID='lblDeleteSite_' runat='server' Text='Delete' />
                        </li>
                        <li>
                            <asp:HyperLink ID="_tagsLink" rel="managetags" runat="server" Text="Manage Tags"
                                ImageUrl="img/button/tag-icon.gif" NavigateUrl="~/admin/frames/managetags.aspx"
                                ToolTip="Manage Tags" />
                            <br />
                            <asp:Label ID='_tagsLinkText' runat='server' Text='Tags' />
                        </li>
                        <li>
                            <asp:HyperLink ID="_environmentLink" rel="manageenvironments" runat="server" Text="Manage Environments"
                                ImageUrl="img/button/network-connection-icon.png" NavigateUrl="~/admin/frames/manageenvironments.aspx"
                                ToolTip="Manage Environments" />
                            <br />
                            <asp:Label ID='_environmentsText' runat='server' Text='Environments' />
                        </li>
                        <li>
                            <asp:HyperLink ID="_culturesLink" rel="managecultures" runat="server" Text="Manage Site Cultures"
                                ImageUrl="img/button/culture-icon.png" NavigateUrl="~/admin/frames/managecultures.aspx"
                                ToolTip="Manage Site Cultures" />
                            <br />
                            <asp:Label ID='_culturesLinkText' runat='server' Text='Cultures' />
                        </li>
                        <li>
                            <asp:HyperLink ID="hplEmailTemplate_" rel="manageemailtemplates" runat="server" Text="Manage Email Templates"
                                ImageUrl="img/button/mail-icon.png" NavigateUrl="~/admin/frames/manageemailtemplates.aspx"
                                ToolTip="Manage Email Templates" />
                            <br />
                            <asp:Label ID='lblEmailTemplate_' runat='server' Text='E-Templates' />
                        </li>
                    </ul>
                </div>
                <table cellspacing="1" cellpadding="1">
                    <tbody>
                        <tr>
                            <td class="td_first">
                                Id:
                            </td>
                            <td>
                                <asp:Literal ID="_siteId" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="td_first">
                                Enabled:
                            </td>
                            <td>
                                <asp:CheckBox ID="_enabled" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="td_first">
                                Alias:
                            </td>
                            <td>
                                <asp:TextBox ID="_alias" runat="server" CssClass="textLong" ValidationGroup="saveSite" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="_alias"
                                    ErrorMessage="required" ValidationGroup="saveSite" Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID='reAlias' runat='server' ControlToValidate="_alias"
                                    ErrorMessage="You can't use special characters" ValidationGroup="saveSite" Display="Dynamic"
                                    ValidationExpression="^([0-9a-zA-Z\&amp;\-\.\'+'\'_']*|)" />
                            </td>
                        </tr>
                        <tr>
                            <td class="td_first">
                                Title:
                            </td>
                            <td>
                                <asp:TextBox ID="_title" runat="server" CssClass="textLong" ValidationGroup="saveSite" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="_title"
                                    ErrorMessage="required" ValidationGroup="saveSite" Display="Dynamic"></asp:RequiredFieldValidator>
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
                            <td class="td_first" nowrap="nowrap">
                                Default Culture:
                            </td>
                            <td>
                                <asp:DropDownList ID='_cultureCode' CssClass="textMedium" runat='server' />
                            </td>
                        </tr>
                        <tr>
                            <td class="td_first">
                                Image URL:
                            </td>
                            <td>
                                <asp:TextBox ID="_imageUrl" runat="server" CssClass="textLong" ValidationGroup="saveSite" />
                            </td>
                        </tr>
                        <tr>
                            <td class="td_first">
                                Site Url:
                            </td>
                            <td>
                                <asp:TextBox ID="_link" runat="server" CssClass="textLong" ValidationGroup="saveSite" />
                                <i>http://siteurl.com</i>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_first">
                                Maximum Blogs:
                            </td>
                            <td>
                                <asp:TextBox ID="_maxBlogs" runat="server" CssClass="textShort">10</asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_first">
                                Maximum Users:
                            </td>
                            <td>
                                <asp:TextBox ID="_maxUsers" runat="server" CssClass="textShort">0</asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_first" nowrap="nowrap">
                                Maximum Content:
                            </td>
                            <td>
                                <asp:TextBox ID="_maxContent" runat="server" CssClass="textShort">0</asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_first" nowrap="nowrap">
                                AccessKey:
                            </td>
                            <td>
                                <asp:TextBox ID="_accessKey" runat="server" CssClass="textLong"></asp:TextBox>
                                <asp:ImageButton ID='ibtnNewGuId' runat="server" ImageUrl="img/button/1616/barcode-icon.png"
                                    ToolTip="New Access Key" OnClick="ibtnNewGuId_Click" />
                            </td>
                        </tr>                        
                        <tr>
                            <td class="td_first" nowrap="nowrap">
                                User Confirmation Email Template:
                            </td>
                            <td>
                                <asp:DropDownList ID="_userConfirmationEmailTemplate" runat='server' />
                            </td>
                        </tr>                        
                        <tr>
                            <td class="td_first" nowrap="nowrap">
                                User Reset Password Email Template:
                            </td>
                            <td>
                                <asp:DropDownList ID="_userResetPasswordEmailTemplate" runat='server' />
                            </td>
                        </tr>                        
                        <tr>
                            <td class="td_first" nowrap="nowrap">
                                Granted Domains:
                            </td>
                            <td>
                                <asp:TextBox ID="_grantedIPs" runat="server" CssClass="textLong" TextMode="MultiLine"></asp:TextBox>
                                <i>Comma seperated domains: awapi.com,microsoft.com</i>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_first" nowrap="nowrap">
                                Banned Domains:
                            </td>
                            <td>
                                <asp:TextBox ID="_bannedIPs" runat="server" CssClass="textLong" TextMode="MultiLine"></asp:TextBox>
                                <i>Comma seperated domains: awapi.com,microsoft.com</i>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_first" nowrap="nowrap">
                                Site Twitter Usernanme:
                            </td>
                            <td>
                                <asp:TextBox ID="_twitterUsername" runat="server" CssClass="textMedium" />
                                <span class="table_td_second_section">Twitter Password:</span>
                                <asp:TextBox ID="_twitterPassword" runat="server" TextMode="Password" CssClass="textMedium" />
                            </td>
                        </tr>
                        <tr>
                            <td class="td_first" nowrap="nowrap">
                                AmazonS3 Bucket Name:
                            </td>
                            <td>
                                <asp:TextBox ID="_fileAmazonS3BucketName" runat="server" CssClass="textMedium" /><i>If not set default confugiration value will be used</i>
                            </td>
                        </tr>                        
                    </tbody>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
