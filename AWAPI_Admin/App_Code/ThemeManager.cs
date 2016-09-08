using System.Collections.Generic;
using System.IO;

namespace AWAPI.App_Code
{

    public class Theme
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public Theme(string name)
        {
            Name = name;
        }
    }


    public class ThemeManager
    {
        #region Theme-Related Method
        public static List<Theme> GetThemes()
        {
            DirectoryInfo dInfo = new DirectoryInfo(System.Web.HttpContext.Current.Server.MapPath("~/App_Themes"));
            DirectoryInfo[] diList = dInfo.GetDirectories();
            List<Theme> list = new List<Theme>();
            foreach (DirectoryInfo di in diList)
            {
                if (di.Name.Contains(".svn"))
                    continue;
                Theme temp = new Theme(di.Name);
                list.Add(temp);
            }
            return list;
        }
        #endregion
    }
}