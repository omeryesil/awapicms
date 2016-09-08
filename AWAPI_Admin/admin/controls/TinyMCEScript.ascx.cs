using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AWAPI.admin.controls
{
    public partial class TinyMCEScript : System.Web.UI.UserControl
    {
        public string Text
        {
            get { return _control.Text; }
            set { _control.Text = value; }
        }

        override public bool Visible
        {
            get { return _control.Visible; }
            set { _control.Visible = value; }
        }

        public Unit Width
        {
            get { return _control.Width; }
            set { _control.Width = value; }
        }

        public string CssClass
        {
            get { return _control.CssClass; }
            set { _control.CssClass = value; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append("<script language='javascript'>\n");
            sb.Append("    function initTinyMCE() { \n");
            sb.Append("            try { \n");
            sb.Append("                tinyMCE.init({ \n");
            sb.Append("                    // General options \n");
            sb.Append("                    mode: \"exact\", \n");
            sb.Append("                    elements: \"" + _control.ClientID + "\", \n");
            sb.Append("                    theme: \"advanced\", \n");
            sb.Append("                    plugins: \"safari,pagebreak,layer,save,advhr,advimage,advlink,emotions,inlinepopups,insertdatetime,preview,media,searchreplace,print,contextmenu,paste,directionality,fullscreen,noneditable,visualchars,nonbreaking,xhtmlxtras,template\", \n");

            sb.Append("                    // Theme options \n");
            sb.Append("                    theme_advanced_buttons1: \"bold,italic,underline,strikethrough,|,justifyleft,justifycenter,justifyright,justifyfull,formatselect\", \n");
            sb.Append("                    theme_advanced_buttons2: \"cut,copy,paste,pastetext,pasteword,media,|,search,replace,|,bullist,numlist,|,outdent,indent,blockquote,|,undo,redo,|,link,unlink,anchor,image,cleanup,code,|,preview,|,forecolor,backcolor,|,fullscreen\", \n");
            sb.Append("                    theme_advanced_buttons3: \"\", \n");
            sb.Append("                    theme_advanced_buttons4: \"\", \n");
            sb.Append("                    theme_advanced_toolbar_location: \"top\", \n");
            sb.Append("                    theme_advanced_toolbar_align: \"left\", \n");
            sb.Append("                    theme_advanced_statusbar_location: \"bottom\", \n");
            sb.Append("                    theme_advanced_resizing: false, \n");

            sb.Append("                    forced_root_block: false, \n");
            sb.Append("                    force_p_newlines: 'false', \n");
            sb.Append("                    remove_linebreaks: false, \n");
            sb.Append("                    force_br_newlines: true,              //btw, I still get <p> tags if this is false \n");
            sb.Append("                    remove_trailing_nbsp: false, \n");
            //sb.Append("                    entity_encoding : 'numeric', \n");
            sb.Append("                    verify_html: true \n");
            sb.Append("                }); \n");
            sb.Append("           } \n");
            sb.Append("           catch (e) { \n");
            sb.Append("           } \n");
            sb.Append("       } \n");

            sb.Append("       initTinyMCE();  \n");
            //sb.Append("       tinyMCE.triggerSave(false, true); \n");
            sb.Append("</script>\n");


            _script.Text = sb.ToString();
        }
    }
}