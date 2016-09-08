<%@ Page Language="C#" ValidateRequest="false" %>

<%@ Register TagPrefix="tinymce" Namespace="Moxiecode.TinyMCE.Web" Assembly="Moxiecode.TinyMCE" %>

<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
            info.InnerHtml = "Contents: " + Server.HtmlEncode(elm1.Value);
    }
</script>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Full featured example</title>
</head>
<body>
    <label id="info" runat="server">
    </label>
    <form method="post" action="http://tinymce.moxiecode.com/dump.php?example=true" runat="server">
    <h3>
        Full featured example</h3>
    <p>
        This page shows all available buttons and plugins that are included in the TinyMCE
        core package. There are more examples on how to use TinyMCE in the <a href="http://wiki.moxiecode.com/examples/tinymce/">
            Wiki</a>.
    </p>
    <!-- Gets replaced with TinyMCE, remember HTML in a textarea should be encoded -->
    <tinymce:TextArea ID="elm1" theme="advanced" plugins="spellchecker,safari,pagebreak,style,layer,table,save,advhr,advimage,advlink,emotions,iespell,inlinepopups,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template"
        theme_advanced_buttons1="spellchecker,save,newdocument,|,bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,|,styleselect,formatselect,fontselect,fontsizeselect"
        theme_advanced_buttons2="cut,copy,paste,pastetext,pasteword,|,search,replace,|,bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,link,unlink,anchor,image,cleanup,help,code,|,insertdate,inserttime,preview,|,forecolor,backcolor"
        theme_advanced_buttons3="tablecontrols,|,hr,removeformat,visualaid,|,sub,sup,|,charmap,emotions,iespell,media,advhr,|,print,|,ltr,rtl,|,fullscreen"
        theme_advanced_buttons4="insertlayer,moveforward,movebackward,absolute,|,styleprops,|,cite,abbr,acronym,del,ins,attribs,|,visualchars,nonbreaking,template,pagebreak"
        theme_advanced_toolbar_location="top" theme_advanced_toolbar_align="left" theme_advanced_path_location="bottom"
        theme_advanced_resizing="true" runat="server" />
    <div>
        <!-- Some integration calls -->
        <a href="javascript:void(0);" onmousedown="tinyMCE.get('elm1').show();">[Show]</a>
        <a href="javascript:void(0);" onmousedown="tinyMCE.get('elm1').hide();">[Hide]</a>
        <a href="javascript:void(0);" onmousedown="tinyMCE.get('elm1').execCommand('Bold');">
            [Bold]</a> <a href="javascript:void(0);" onmousedown="alert(tinyMCE.get('elm1').getContent());">
                [Get contents]</a> <a href="javascript:void(0);" onmousedown="alert(tinyMCE.get('elm1').selection.getContent());">
                    [Get selected HTML]</a> <a href="javascript:void(0);" onmousedown="alert(tinyMCE.get('elm1').selection.getContent({format:'text'}));">
                        [Get selected text]</a> <a href="javascript:void(0);" onmousedown="alert(tinyMCE.get('elm1').selection.getNode().nodeName);">
                            [Get selected element]</a> <a href="javascript:void(0);" onmousedown="tinyMCE.execCommand('mceInsertContent',false,'<b>Hello world!!</b>');">
                                [Insert HTML]</a> <a href="javascript:void(0);" onmousedown="tinyMCE.execCommand('mceReplaceContent',false,'<b>{$selection}</b>');">
                                    [Replace selection]</a>
    </div>
    <br />
    <input type="submit" name="save" value="Submit" runat="server" />
    <input type="reset" name="reset" value="Reset" runat="server" />
    </form>
</body>
</html>
