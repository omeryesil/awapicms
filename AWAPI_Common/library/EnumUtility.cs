using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace AWAPI_Common.library
{
    public class EnumUtility
    {
        /// <summary>
        /// Returns description attribute of an enum's value
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static String GetEnumDescription(Enum e)
        {
            FieldInfo fieldInfo = e.GetType().GetField(e.ToString());
            DescriptionAttribute[] enumAttributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (enumAttributes.Length > 0)
                return enumAttributes[0].Description;

            return e.ToString();
        }


    }

}
