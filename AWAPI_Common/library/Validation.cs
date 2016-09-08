using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AWAPI_Common.library
{
    public class Validation
    {
        public const string PATTERN_EMAIL =
                    @"^(([^<>()[\]\\.,;:\s@\""]+"
                  + @"(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@"
                  + @"((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}"
                  + @"\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+"
                  + @"[a-zA-Z]{2,}))$";

        public static bool IsNumeric(object source)
        {
            bool isNum;
            double retNum;
            isNum = Double.TryParse(Convert.ToString(source), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }

        public static bool IsEmail(string email)
        {
            Regex reg = new Regex(PATTERN_EMAIL);
            return reg.IsMatch(email);
        }


    }
}
