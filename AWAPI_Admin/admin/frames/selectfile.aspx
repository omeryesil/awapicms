<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="selectfile.aspx.cs" Inherits="AWAPI.Admin.frames.selectfile" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Select a File</title>
    <link href="../css/iframe.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        li
        {
            text-align: center;
            margin: 2px;
            padding: 2px;
            overflow: hidden;
            float: left;
            width: 110px;
            height: 110px;
            border: solid 1px #aaaaaa;
        }
        li:hover
        {
            background-color: #dcdcdc;
        }
    </style>

    <script type="text/javascript" language="javascript">
        function CallBack ()
        {
            var url = document.getElementById('<%= _fileUrl.ClientID %>').value; 
            var targetControlId = '<%= TargetControlId %>';
            window.parent.<%= CallbackFunction %>(url, targetControlId); 
        }
        
        function SetFile(imageUrl, title) 
        {
            document.getElementById('<%= _fileUrl.ClientID %>').value = imageUrl ;//'<%= ImageServerLink %>' + fileId;
            document.getElementById('_selectedFileTitle').innerHTML = title; //document.getElementById('title_' + fileId).innerHTML;
        }

    </script>

</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID='scriptManager1' runat='server'>
    </asp:ScriptManager>
    <div id="main" style="min-height: 500px;">
        <div id="header">
            <h3>
                Select Image</h3>
        </div>
        <div id="middle">
            <div id='center-column' style="width: 850px;">
                <div id='selectFromExisting'>
                    <div id='top-bar'>
                        <table width="100%">
                            <tr>
                                <td>
                                    File Groups:
                                </td>
                                <td>
                                    <asp:DropDownList ID='ddlGroupList' CssClass="textMedium" runat='server' AutoPostBack="True"
                                        OnSelectedIndexChanged="ddlGroupList_SelectedIndexChanged" />
                                    <%--<asp:Literal ID='_fileGroupId' runat='server' Visible="false" />--%>
                                    <asp:TextBox ID='txbFilter' runat='server' />
                                    <asp:LinkButton ID='lbtnFilter' runat='server' Text='Filter' OnClick="lbtnFilter_Click" />
                                    &nbsp;<asp:LinkButton ID='lbtnClear' runat='server' Text='Clear' OnClick="lbtnClear_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    File Title:
                                </td>
                                <td>
                                    <strong>
                                        <asp:Label runat='server' ID='_selectedFileTitle' /></strong>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    File URL:
                                </td>
                                <td>
                                    <asp:TextBox ID='_fileUrl' runat='server' Style="width: 550px" />
                                    <a href="javascript:void(0);" onclick="CallBack();">Select</a>&nbsp;|&nbsp;<asp:LinkButton
                                        ID='lbtnUploadNew' runat='server' Text='Upload' OnClick="lbtnUploadNew_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <asp:Panel ID='pnlUploadNew' runat='server' Visible='false'>
                        <h3>
                            Upload File</h3>
                        <table>
                            <tr>
                                <td valign="top">
                                    <table cellpadding="0" cellspacing="0" class="listing form">
                                        <tbody>
                                            <tr>
                                                <td class="first">
                                                    Id:
                                                </td>
                                                <td class="last">
                                                    <asp:Literal ID="_fileId" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="first">
                                                    Title:
                                                </td>
                                                <td class="last">
                                                    <asp:TextBox ID="_fileTitle" runat="server" CssClass="textLong" ValidationGroup="saveFile" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="first">
                                                    Description:
                                                </td>
                                                <td class="last">
                                                    <asp:TextBox ID="_fileDescription" runat="server" CssClass="textDescription" TextMode="MultiLine" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="first">
                                                    Upload Image Size:
                                                </td>
                                                <td  class="last">
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
                                                <td class="first">
                                                    File:
                                                </td>
                                                <td class="last">
                                                    <asp:FileUpload ID="_fileUpload" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="first">
                                                </td>
                                                <td class="last">
                                                    <asp:Button ID='btnSaveFile_' runat='server' Text='Upload' OnClick="btnSaveFile__Click"
                                                        Style="width: 64px" ValidationGroup="saveFile" />
                                                    &nbsp;<asp:Button ID='btlnCancel' runat='server' Text='Cancel' OnClick="btlnCancel_Click" />
                                                    &nbsp;
                                                    <asp:Label ID="lblMessage" runat="server" CssClass="msgError"></asp:Label>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                                <td valign="top" align="right" style="width: 100%">
                                    <asp:Image ID='_fileImg' runat='server' Visible="false" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID='pnlExisting' runat='server' Style="width: 100%; height: 350px; overflow: auto;">
                        <h3>
                            Select File</h3>
                        <ul>
                            <asp:Repeater ID="_fileList" runat="server" OnItemCommand="_fileList_ItemCommand"
                                OnItemDataBound="_fileList_ItemDataBound">
                                <ItemTemplate>
                                    <li style="list-style: none;">
                                        <div>
                                            <asp:HyperLink ID="hplImage" runat='server' Width="100">
                                                <asp:Image ID="image" runat='server' Width="100" />
                                            </asp:HyperLink>
                                            <%--                                            <a href='javascript:void(0)' onclick='<%# "javascript:SetFile(\"" + DataBinder.Eval(Container.DataItem, "fileId") + "\");" %>'>
                                                <img style="border: none" src='<%# ImageServerLink + DataBinder.Eval(Container.DataItem, "fileId") + "&amp;size=100x100" %>' />
                                            </a>--%>
                                        </div>
                                        <div>
                                            <span id='<%# "title_" + DataBinder.Eval(Container.DataItem, "fileId") %>'>
                                                <asp:Literal ID='litTitle' runat='server' Text='<%# Bind("title") %>' />
                                            </span>
                                            <br />
                                        </div>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
