<%@ Page Language="C#" MasterPageFile="~/admin/admin.Master" AutoEventWireup="true"
    CodeBehind="configuration.aspx.cs" Inherits="AWAPI.Admin.PageConfiguration" Title="AWAPI - Manage Sites" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlaceHolder" runat="server">
    <div id='left-column'>
        <div class='title'>
            <div style="float: left">
                <h3>
                    Configuration List
                </h3>
            </div>
            <div style="float: right">
            </div>
        </div>
        <asp:UpdatePanel ID="UpdatePanel1" runat='server'>
            <ContentTemplate>
                <div class="nav">
                    <asp:GridView ID="_list" runat="server" AutoGenerateColumns="False" BorderStyle="None"
                        BorderWidth="0px" CellPadding="2" CellSpacing="2" ShowHeader="False" Width="100%"
                        GridLines="None" OnRowCommand="_list_RowCommand" OnRowDataBound="_list_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="title">
                                <ItemTemplate>
                                    <asp:LinkButton ID='lbtnSelect' runat='server' Text='<%# Bind("title") %>' CommandName="edit_config"
                                        CommandArgument='<%# Bind("configurationId") %>' />
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
                        Manage Configuration</h1>
                    <div class="breadcrumbs">
                    </div>
                </div>
                <div class="select-bar">
                    <ul>
                        <li>
                            <asp:ImageButton ID="btnNewConfiguration_" runat="server" ImageUrl="img/button/new-icon.png"
                                AlternateText="New Site" ToolTip="New Site" CausesValidation="false" OnClick="btnNewConfiguration_Click" />
                            <br />
                            <asp:Label ID='lblNewConfiguration_' runat='server' Text='New' />
                        </li>
                        <li class="seperator"></li>
                        <li>
                            <asp:ImageButton ID="btnSaveConfiguration_" runat="server" ImageUrl="img/button/save-icon.png"
                                AlternateText="Save Site" ToolTip="Save Site" ValidationGroup="saveConfig" OnClick="btnSaveConfiguration_Click" />
                            <br />
                            <asp:Label ID='lblSaveConfiguration_' runat='server' Text='Save' />
                        </li>
                        <li>
                            <asp:ImageButton ID="btnDeleteConfiguration_" runat="server" ImageUrl="img/button/delete-icon.png"
                                AlternateText="Delete Site" ToolTip="Delete Site" 
                                OnClientClick="return confirm('Are you sure you want to delete the configuration?');" 
                                onclick="btnDeleteConfiguration__Click" /><br />
                            <asp:Label ID='lblDeleteConfiguration_' runat='server' Text='Delete' />
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
                                <asp:Literal ID="_configurationId" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="td_first">
                                Alias:
                            </td>
                            <td>
                                <asp:TextBox ID="_alias" runat="server" CssClass="textLong" ValidationGroup="saveConfig" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="_alias"
                                    ErrorMessage="required" ValidationGroup="saveConfig" Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID='reAlias' runat='server' ControlToValidate="_alias"
                                    ErrorMessage="You can't use special characters" ValidationGroup="saveConfig"
                                    Display="Dynamic" ValidationExpression="^([0-9a-zA-Z\&amp;\-\.\'+'\'_']*|)" />
                            </td>
                        </tr>
                        <tr>
                            <td class="td_first">
                                Title:
                            </td>
                            <td>
                                <asp:TextBox ID="_title" runat="server" CssClass="textLong" ValidationGroup="saveConfig" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="_title"
                                    ErrorMessage="required" ValidationGroup="saveConfig" Display="Dynamic"></asp:RequiredFieldValidator>
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
                                File Service URL:
                            </td>
                            <td>
                                <asp:TextBox ID="_fileServiceUrl" runat="server" CssClass="textLong" ValidationGroup="saveConfig" />
                                <br /><i>http://service.xxx.com/handler/filehandler.ashx</i>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_first" nowrap="nowrap">
                                Content Service URL:
                            </td>
                            <td>
                                <asp:TextBox ID="_contentServiceUrl" runat="server" CssClass="textLong" ValidationGroup="saveConfig" />
                                <br /><i>http://service.xxx.com/handler/contenthandler.ashx</i>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_first">
                                Blog Service URL
                            </td>
                            <td>
                                <asp:TextBox ID="_blogServiceUrl" runat="server" CssClass="textLong" ValidationGroup="saveConfig" />
                                <br /><i>http://service.xxx.com/handler/bloghandler.ashx</i>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_first">
                                Poll Service URL
                            </td>
                            <td>
                                <asp:TextBox ID="_pollServiceUrl" runat="server" CssClass="textLong" ValidationGroup="saveConfig" />
                                <br /><i>http://service.xxx.com/handler/pollhandler.ashx</i>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_first">
                                Automated Task Service Url:
                            </td>
                            <td>
                                <asp:TextBox ID="_automatedTaskServiceUrl" runat="server" CssClass="textLong" ValidationGroup="saveConfig" />
                                <br /><i>http://service.xxx.com/handler/automatedtask.ashx</i>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_first">
                                Twitter Service Url:
                            </td>
                            <td>
                                <asp:TextBox ID="_twitterServiceUrl" runat="server" CssClass="textLong" ValidationGroup="saveConfig" />
                                <br /><i>http://service.xxx.com/handler/twitterhandler.ashx</i>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_first" nowrap="nowrap">
                                Weather Service Url:
                            </td>
                            <td>
                                <asp:TextBox ID="_weatherServiceUrl" runat="server" CssClass="textLong" ValidationGroup="saveConfig" />
                                <br /><i>http://service.xxx.com/handler/weatherhandler.ashx</i>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_first" nowrap="nowrap">
                                Admin Base Url:
                            </td>
                            <td>
                                <asp:TextBox ID="_adminBaseUrl" runat="server" CssClass="textLong" ValidationGroup="saveConfig" />
                                <br /><i>http://admin.xxx.com/</i>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_first" nowrap="nowrap">
                                SMTP Server:
                            </td>
                            <td>
                                <asp:TextBox ID="_smtpServer" runat="server" CssClass="textMedium" ValidationGroup="saveConfig" />
                                <br /><i>localhost</i>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_first" nowrap="nowrap">
                                File Directory:
                            </td>
                            <td>
                                <asp:TextBox ID="_fileDirectory" runat="server" CssClass="textLong" ValidationGroup="saveConfig" />
                                <br /><i>c:\resources\</i>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_first" nowrap="nowrap">
                                MIME XML Path:
                            </td>
                            <td>
                                <asp:TextBox ID="_fileMimeXMLPath" runat="server" CssClass="textLong" ValidationGroup="saveConfig" Text="~/MIMEContentTypes.xml" />
                                <br /><i>~/MIMEContentTypes.xml</i>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_first" nowrap="nowrap">
                                Save Files on Amazon S3:
                            </td>
                            <td>
                                <asp:CheckBox ID="_fileSaveOnAmazonS3" runat="server" AutoPostBack="True" 
                                    oncheckedchanged="_fileSaveOnAmazonS3_CheckedChanged" />
                                <asp:Panel ID="pnlAmaxonS3" runat='server' Visible="false">
                                    <table>
                                        <tr>
                                            <td class="td_first" nowrap="nowrap">
                                                Amazon S3 Bucket Name:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="_fileAmazonS3BucketName" runat="server" CssClass="textMedium" ValidationGroup="saveConfig" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td_first" nowrap="nowrap">
                                                Amazon S3 AccessKey:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="_fileAmazonS3AccessKey" runat="server" CssClass="textLong" ValidationGroup="saveConfig" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="td_first" nowrap="nowrap">
                                                Amazon S3 SecretKey:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="_fileAmazonS3SecreyKey" runat="server" CssClass="textLong" ValidationGroup="saveConfig" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
