using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;


namespace AWAPI_Common.library
{
    public class DataLibrary
    {
        public static DataSet LINQToDataSet(System.Data.Linq.DataContext dataContext, IQueryable linQ)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = dataContext.GetCommand(linQ) as SqlCommand;
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            adp.Fill(ds);

            return ds;
        }

        public static DataTable LINQToDataTable<T>(IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();

            // column names 
            PropertyInfo[] oProps = null;

            if (varlist == null) return dtReturn;

            foreach (T rec in varlist)
            {
                // Use reflection to get property names, to create table, Only first time, others   will follow 
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                        == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }

                DataRow dr = dtReturn.NewRow();

                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                    (rec, null);
                }

                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtrs"></param>
        /// <returns></returns>
        public static DataTable DataRowsToDataTable(DataRow[] dtrs)
        {
            return DataRowsToDataTable(dtrs, 0, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtrs"></param>
        /// <param name="startRow"></param>
        /// <param name="endRow"></param>
        /// <returns></returns>
        public static DataTable DataRowsToDataTable(DataRow[] dtrs, int startRow, int endRow)
        {
            DataTable dt = new DataTable();

            if (dtrs == null || dtrs.Length == 0)
                dt = null;
            else
            {
                dt = dtrs[0].Table.Clone();
                {
                    bool startEnabled = startRow > 0 ? true : false;

                    int nCurrentRow = 0;
                    foreach (DataRow dr in dtrs)
                    {
                        if (endRow > 0 && nCurrentRow >= endRow)
                            break;

                        if (startRow > 0)
                        {
                            if (nCurrentRow >= startRow)
                                dt.ImportRow(dr);
                        }
                        else
                            dt.ImportRow(dr);

                        nCurrentRow++;
                    }
                }
            }
            return dt;
        }

        public static bool IsDataSetEmptyOrNull(DataSet ds)
        {
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return true;
            return false;
        }


    }
}
