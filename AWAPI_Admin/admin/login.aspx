<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="AWAPI.Admin.PageLogin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>AWAPI - Admin Login</title>
    <style>
        body
        {
            font-size: 0.7em;
            font-family: Verdana;
        }
        img
        {
            padding: 4px;
        }
        .login
        {
            width: 450px;
            min-height: 250px;
            left: auto;
            top: auto;
            border: solid 1px #dcdcdc;
            margin: 50px 33% auto 33%;
            vertical-align: top;
        }
        .loginheader
        {
            background-color: #ffffff;
        }
        .loginTable
        {
            margin-top: 0px;
            padding: 5px;
        }
        .errorMsg
        {
            color: Red;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class='login'>
        <div class="loginheader">
            <asp:Label runat='server' ID='loginTitle' Font-Size="Large" Font-Bold="true"/>
            <asp:Image runat="server" ID='loginLogo' ImageUrl="img/logo.jpg" />
        </div>
        <asp:Panel ID='_pnlLogin' runat='server' CssClass="loginTable">
            <h3>
                Login</h3>
            <table cellpadding="3" cellspacing="3" class="loginTable">

                <script language="javascript" type="text/javascript">
                    //forefox sends the event in parameter.
                    function loginClick(e) {
                        var code;

                        if (window.event) //IE
                            code = e.keyCode;
                        else if (e.which) //netscape/firefox/opera
                            code = e.which;

                        if (code == 13)
                            __doPostBack('<%=lbtnLogin.ClientID%>', '');
                    }    
                </script>

                <tr>
                    <td nowrap="nowrap">
                        Email/Username :
                    </td>
                    <td width="100%">
                        <asp:TextBox ID="txbEmail" runat="server" Width="200px" onkeydown="loginClick(event)"
                            ValidationGroup="login"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txbEmail"
                            ErrorMessage="required" ValidationGroup="login"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td nowrap="nowrap">
                        Password :
                    </td>
                    <td>
                        <asp:TextBox ID="txbPassword" runat="server" Width="200px" TextMode="Password" onkeydown="loginClick(event)"
                            ValidationGroup="login"></asp:TextBox>
                       <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txbPassword"
                            ErrorMessage="required" ValidationGroup="login"></asp:RequiredFieldValidator>--%>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:LinkButton ID="lbtnLogin" runat="server" OnClick="lbtnLogin_Click" ValidationGroup="login">Login</asp:LinkButton>
                        <br />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID='_pnlSelectSite' runat='server' CssClass="loginTable">
            <h3>
                Select a Site to Continue</h3>
            <table cellpadding="3" cellspacing="3" class="loginTable">
                <tr>
                    <td>
                        Site
                    </td>
                    <td width="100%">
                        <asp:DropDownList ID='_siteList' CssClass="textMedium" runat='server' Width="200px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:LinkButton ID="lbtnBack" runat="server" OnClick="lbtnBack_Click">Cancel</asp:LinkButton>
                        &nbsp;
                        <asp:LinkButton ID="lbtnContinue" runat="server" OnClick="lbtnContinue_Click">Continue</asp:LinkButton>
                        <br />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <div>
            <asp:Label ID='lblMsg' runat='server' CssClass="errorMsg"></asp:Label>
        </div>
    </div>
    </form>
</body>
</html>
