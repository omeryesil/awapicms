<%@ Page Language="C#" MasterPageFile="~/admin/admin.Master" AutoEventWireup="true"
    CodeBehind="files.aspx.cs" Inherits="AWAPI.Admin.PageFiles" Title="AWAPI - Manage Files" %>

<%@ Register Src="controls/shareIt.ascx" TagName="shareIt" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script language="javascript">

        function SelectUnselect(chk) {
            $('#<%=_fileList.ClientID %>').find("input:checkbox").each(function() {
               this.checked = chk;
            });

        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlaceHolder" runat="server">
    <asp:UpdatePanel ID='pnl1' runat='server'>
        <ContentTemplate>
            <div id='left-column'>
                <div class='title'>
                    <div style="float: left">
                        <h3>
                            Resource List
                        </h3>
                    </div>
                    <div style="float: right">
                        <asp:ImageButton ID='btnRefreshTree' runat='server' AlternateText="Refresh List"
                            ImageUrl="img/bg-refresh.png" OnClick="btnRefreshTree_Click" />
                    </div>
                </div>
                <div class="nav">
                    Group Name:<br />
                    <asp:DropDownList ID="ddlGroupList" runat="server" CssClass="textMedium" AutoPostBack="True"
                        OnSelectedIndexChanged="ddlGroupList_SelectedIndexChanged" />
                    &nbsp;<asp:HyperLink ID="hplPopupFileGroup_" rel="managefilegroups" runat="server"
                        ToolTip="Manage Resource Groups" ImageUrl="img/button/1616/folder-icon.png" NavigateUrl="~/admin/frames/managefilegroups.aspx"></asp:HyperLink>
                    <br />
                    Search:<br />
                    <asp:TextBox ID="txbFilter" runat="server" CssClass="textMedium" />
                    <asp:LinkButton ID="lbtnFilter" runat="server" OnClick="lbtnFilter_Click" Text="Filter" />
                    &nbsp;<asp:LinkButton ID="lbtnClear" runat="server" OnClick="lbtnClear_Click" Text="Clear" />
                    <hr />
                    <a href='javascript:void(0)' onclick='SelectUnselect(true);'>All</a>&nbsp;
                    <a href='javascript:void(0)' onclick='SelectUnselect(false);'>None</a>&nbsp;
                    <asp:LinkButton ID="btnDelete" runat='server' Text="Delete" 
                        OnClientClick="return confirm('All the selected files will be deleted, are you sure?');" 
                        onclick="btnDelete_Click" /><br />
                    <asp:GridView ID="_fileList" runat="server" AllowPaging="true" AutoGenerateColumns="False"
                        CellPadding="2" CellSpacing="2" GridLines="None" OnPageIndexChanging="_fileList_PageIndexChanging"
                        OnRowCommand="_fileList_RowCommand" PageSize="15" ShowHeader="False" OnRowDataBound="_fileList_RowDataBound">
                        <Columns>
                            <asp:TemplateField ItemStyle-Wrap="false">
                                <ItemTemplate>
                                    <asp:Literal ID='_fileId' runat='server' Visible='false' Text='' />
                                    <asp:CheckBox ID="_deleteFile" rel="_deleteFile" runat='server' />
                                    <asp:Image runat='server' ID='_fileIcon' ImageUrl="~/admin/img/fileassociation/1616/icon-text.png" />
                                    <asp:Image runat='server' ID='Image1' ToolTip="Saved on Amazon S3" ImageUrl="~/admin/img/button/1616/amazon-ws-icon.png"
                                        Visible='<%# !Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "isOnLocal")) %>' />
                                    <asp:Image runat='server' ID='Image2' ToolTip="Saved on Local" ImageUrl="~/admin/img/button/1616/hardrive-icon.gif"
                                        Visible='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "isOnLocal")) %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="FileName" ItemStyle-Width="100%">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtnSelect" runat="server" CommandArgument='<%# Bind("fileid") %>'
                                        CommandName="selectfile" Text='<%# DataBinder.Eval(Container.DataItem, "title")==null || DataBinder.Eval(Container.DataItem, "title").ToString()==""?"undefined":DataBinder.Eval(Container.DataItem, "title") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Size">
                                <ItemTemplate>
                                    <asp:Label ID="_lblContentSize" runat='server' Text='<%# String.Format("{0:0,0}KB", Convert.ToDouble(DataBinder.Eval(Container.DataItem, "contentsize"))/1024) %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <div id='center-column'>
                <div class="top-bar">
                    <h1>
                        Manage Resource
                    </h1>
                </div>
                <div class="select-bar">
                    <ul>
                        <li>
                            <asp:ImageButton ID="btnNewFile_" runat="server" ImageUrl="img/button/new-icon.png"
                                AlternateText="New" OnClick="btnNewFile_Click" CausesValidation="false" /><br />
                            <asp:Label ID='lblNewFile_' runat='server' Text='New File' />
                        </li>
                        <li class="seperator"></li>
                        <li>
                            <asp:ImageButton ID="btnSaveFile_" runat="server" ImageUrl="img/button/save-icon.png"
                                AlternateText="Save" OnClick="btnSaveFile_Click" ValidationGroup="saveFile" /><br />
                            <asp:Label ID='lblSaveFile_' runat='server' Text='Save' />
                        </li>
                        <li>
                            <asp:ImageButton ID="btnDeleteFile_" runat="server" AlternateText="Delete File" ImageUrl="img/button/delete-icon.png"
                                OnClick="btnDeleteFile_Click" OnClientClick="return confirm('Are you sure you want to delete the file?');" /><br />
                            <asp:Label ID='lblDeleteFile_' runat='server' Text='Delete' />
                        </li>
                        <li>
                            <uc1:shareIt ID="_shareIt" runat="server" />
                        </li>
                    </ul>
                </div>
                <table>
                    <tr>
                        <td valign="top">
                            <table cellpadding="1" cellspacing="1">
                                <tbody>
                                    <tr>
                                        <td class="td_first">
                                            Id:
                                        </td>
                                        <td>
                                            <asp:Literal ID="_fileId" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_first">
                                            Enabled:
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="_fileIsEnabled" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_first">
                                            Title:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="_fileTitle" runat="server" CssClass="textLong" ValidationGroup="saveFile" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="_fileTitle"
                                                ErrorMessage="required" ValidationGroup="saveFile"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_first">
                                            Description:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="_fileDescription" runat="server" CssClass="textDescription" TextMode="MultiLine" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_first">
                                            Resource Group:
                                        </td>
                                        <td>
                                            <div class="collapsablePanelSmall">
                                                <asp:CheckBoxList ID="_fileGroupList" runat="server" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_first">
                                            Upload Image Size:
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="_fileSize" runat='server' Enabled="true">
                                                <asp:ListItem Text="Not defined" Value="" Selected="True" />
                                                <asp:ListItem Text="400x300" Value="400x300" />
                                                <asp:ListItem Text="640x480" Value="640x480" />
                                                <asp:ListItem Text="800x600" Value="800x600" />
                                                <asp:ListItem Text="1024x768" Value="1024x768" />
                                                <asp:ListItem Text="1600x1200" Value="1024x768" />
                                                <asp:ListItem Text="1920x1200" Value="1920x1200" />
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_first">
                                            File:
                                        </td>
                                        <td>
                                            <asp:FileUpload ID="_fileUpload" runat="server" CssClass="textMedium" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_first">
                                        </td>
                                        <td>
                                            <asp:Label ID="_fileUploadInfo" runat='server' />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                        <td valign="top" align="left" style="width: 100%">
                            <asp:HyperLink ID='_fileLink' runat="server" Target="_blank" Visible="false" Text="Click to open" /><br />
                            <asp:Image ID='_fileImg' runat='server' Visible="false" Width="150" />
                            <asp:Literal ID="_filePreview" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSaveFile_" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
