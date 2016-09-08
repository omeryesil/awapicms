using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AWAPI_Data.Data
{
    public partial class ShareItContextDataContext : System.Data.Linq.DataContext
    {
        partial void OnCreated()
        {
            this.Connection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["AWAPI_Data.Properties.Settings.AWAPIConnectionString"].ConnectionString;
        }


    }
}
