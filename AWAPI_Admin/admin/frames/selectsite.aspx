<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="selectsite.aspx.cs" Inherits="AWAPI.Admin.frames.selectsite" %>

<%@ Register Src="../controls/site_List.ascx" TagName="site_List" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Select Site</title>
    <link href="../css/iframe.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="main" style="min-height: 350px;">
        <div id="header">
            <h3>
                Select a site to continue</h3>
        </div>
        <div id="middle">
            <uc1:site_List ID="site_List1" runat="server"  OnSiteSelected="siteList_SiteSelected"  />
        </div>
    </div>
    </form>
</body>
</html>
