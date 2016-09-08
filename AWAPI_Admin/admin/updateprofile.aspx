<%@ Page Language="C#" MasterPageFile="~/admin/admin.Master" AutoEventWireup="true"
    CodeBehind="updateprofile.aspx.cs" Inherits="AWAPI.Admin.PageUpdateProfile" Title="AWAPI - Manage Users" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<%--    <script language="javascript" type="text/javascript">
        function setImageFromIFrame(url) {
            document.getElementById("<%= _image.ClientID %>").src = url + "&size=150x150";
            document.getElementById("<%= _imageUrl.ClientID %>").value = url;

            $.fn.colorbox.close();
        } 
    </script>--%>
    
    <script language="javascript" type="text/javascript">
        function setImageFromIFrame(url) {
            document.getElementById("<%= _imageUrl.ClientID %>").value = url;
            var image = document.getElementById("<%= _image.ClientID %>");
            image.style.display = '';

            if (url.indexOf("http://") == 0 || url.toIndexOf("https://") == 0)
                image.src = url;
            else
                image.src = url + "&size=150x150";

            image.value = url;
            $.fn.colorbox.close();
        }
    </script>    

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentPlaceHolder" runat="server">
    <div id='center-column' style="width:97%">
        <asp:UpdatePanel ID="UpdatePanel2" runat='server'>
            <ContentTemplate>
                <div class="top-bar">
                    <h1>
                        Update Profile</h1>
                </div>
                <div class="select-bar">
                    <ul>
                        <li>
                            <asp:ImageButton ID="btnSaveUser_" runat="server" ImageUrl="img/button/save-icon.png"
                                ToolTip="Save User" AlternateText="Save User" ValidationGroup="saveUser" OnClick="btnSaveUser_Click" /><br />
                            <asp:Label ID='lblSaveUser_' runat='server' Text='Save' />
                        </li>
                    </ul>
                </div>
                <table>
                    <tr>
                        <td>
                            <table cellspacing="1" cellpadding="1">
                                <tbody>
                                    <tr>
                                        <td class="td_first">
                                            Id:
                                        </td>
                                        <td>
                                            <asp:Literal ID="_userId" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_first">
                                            Username:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="_userName" runat="server" CssClass="textMedium" ValidationGroup="saveUser"
                                                Enabled="false" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_first">
                                            Email:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="_email" runat="server" CssClass="textLong" ValidationGroup="saveUser" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="_email"
                                                Display="Dynamic" ErrorMessage="required" ValidationGroup="saveUser"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="_email"
                                                Display="Dynamic" ErrorMessage="wrong email format" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                ValidationGroup="saveUser"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_first">
                                            Password:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="_password" runat="server" CssClass="textMedium" TextMode="Password"
                                                ValidationGroup="saveUser" /><asp:RegularExpressionValidator ID="RegularExpressionValidator3"
                                                    runat="server" ControlToValidate="_password" Display="Dynamic" ErrorMessage="Minimum 6, maximum 30 characters"
                                                    ValidationExpression=".{6,30}" ValidationGroup="saveUser"></asp:RegularExpressionValidator><i>Leave
                                                        emtpy if you don't want to chage</i>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_first" nowrap="nowrap">
                                            Confirm Password:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="_confirmPassword" runat="server" CssClass="textMedium" TextMode="Password"
                                                ValidationGroup="saveUser" />
                                            <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="_password"
                                                ControlToValidate="_confirmPassword" Display="Dynamic" ErrorMessage="doesn't match with password"
                                                ValidationGroup="saveUser"></asp:CompareValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_first">
                                            First Name:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="_firstName" runat="server" CssClass="textMedium" ValidationGroup="saveUser" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="_firstName"
                                                ErrorMessage="required" ValidationGroup="saveUser" Display="Dynamic"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_first">
                                            Last Name:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="_lastName" runat="server" CssClass="textMedium" ValidationGroup="saveUser" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="_lastName"
                                                Display="Dynamic" ErrorMessage="required" ValidationGroup="saveUser"></asp:RequiredFieldValidator>
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
                                            Image URL:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="_imageUrl" runat="server" CssClass="textLong" ValidationGroup="saveGroup" />
                                            <a rel="selectfile" href='frames/selectfile.aspx?type=image&callback=setImageFromIFrame&group=User Images'>
                                                <img class="toolBarButton" src="img/button/1616/Folder-Open-icon.png" />
                                            </a></a>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_first">
                                            Gender:
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="_gender" runat='server' CssClass="textMedium">
                                                <asp:ListItem Selected="true" Text="Not Specified" Value="" />
                                                <asp:ListItem Text="Female" Value="F" />
                                                <asp:ListItem Text="Male" Value="M" />
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_first">
                                            Birthday:
                                        </td>
                                        <td>
                                            <asp:TextBox ID='_birthday' runat='server' CssClass="textMedium" />
                                            <ajax:CalendarExtender ID="CalendarExtender1" runat="server" PopupPosition="TopRight"
                                                TargetControlID="_birthday" Format="MM/dd/yyyy" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_first">
                                            Tel:
                                        </td>
                                        <td>
                                            <asp:TextBox ID='_tel' runat='server' CssClass="textMedium" />&nbsp; Alternate:
                                            <asp:TextBox ID='_tel2' runat='server' CssClass="textMedium" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_first">
                                            Address:
                                        </td>
                                        <td>
                                            <asp:TextBox ID='_address' runat='server' CssClass="textLong" TextMode="MultiLine" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_first">
                                            City:
                                        </td>
                                        <td>
                                            <asp:TextBox ID='_city' runat='server' CssClass="textMedium" />&nbsp;Province:
                                            <asp:TextBox ID='_province' runat='server' CssClass="textMedium" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="td_first">
                                            Postal Code:
                                        </td>
                                        <td>
                                            <asp:TextBox ID='_postalCode' runat='server' CssClass="textMedium" />&nbsp;Country:<asp:TextBox
                                                ID='_country' runat='server' CssClass="textMedium" />
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                        <td valign="top" align="right">
                            <asp:Image ID="_image" runat='server' Width="150" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
