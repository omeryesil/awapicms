using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.IO;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Imaging;
using AWAPI_BusinessLibrary.library;

using System.Linq;
using System.Xml.Linq;

namespace AWAPI_Services.handler
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class automatedtask : IHttpHandler
    {
        //TODO: Should be defined in a db table (max be for each flGrp)
        public const int MAXIMUM_IMAGE_SIZE_TO_SAVE = 1000;

        FileLibrary _fileLib = new FileLibrary();

        #region PROPERTIES
        /// <summary>
        /// </summary>
        string TaskId
        {
            get
            {
                if (HttpContext.Current.Request["taskid"] != null)
                    return HttpContext.Current.Request["taskid"].ToString();
                return "";
            }
        }

        #endregion

        /// <summary>
        /// Processes the refferrer request
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            bool redirectAfterRun = false;
            context.Response.ContentType = "text/html";

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append("<html>\n");
            sb.Append("<body>\n");


            if (String.IsNullOrEmpty(TaskId))
            {
                context.Response.Write("TaskId is empty...");
                return;
            }
            else
            {
                AutomatedTaskLibrary taskLib = new AutomatedTaskLibrary();
                AWAPI_Data.Data.awAutomatedTask task = taskLib.Get(new Guid(TaskId));

                if (task == null)
                    sb.AppendFormat("The task couldn't be found or it has already been completed.<br/>TaskId:{0}", TaskId);
                else
                {
                    redirectAfterRun = !String.IsNullOrEmpty(task.resultRedirectUrl);

                    try
                    {
                        if (taskLib.RunTask(task))
                        {
                            if (redirectAfterRun)
                            {
                                string url = taskLib.CreateRedirectUrl(task.resultRedirectUrl, true, task.description);
                                context.Response.Redirect(url, false);
                                return;
                            }

                            sb.AppendFormat("<h3>{0}</h3>\n", task.title);
                            sb.AppendFormat("<p>{0}</p>\n", task.completedMessage);
                        }
                        else
                        {
                            if (redirectAfterRun)
                            {
                                string url = taskLib.CreateRedirectUrl(task.resultRedirectUrl, false, "The task could not run. TaskId:" +  TaskId.ToString());
                                context.Response.Redirect(url, false);
                                return;
                            }
                            sb.AppendFormat("The task could not run.<br/>TaskId:{0}", TaskId);
                        }
                    }
                    catch (Exception ex)
                    {
                        if (redirectAfterRun)
                        {
                            string url = taskLib.CreateRedirectUrl(task.resultRedirectUrl, false, "An error has occured");
                            context.Response.Redirect(url, false);
                            return;
                        }
                        sb.AppendFormat("An error has occured while running the task. <br/>TaskID:{0}<br/>Error:{1}\n", TaskId, ex.Message);
                    }
                }
            }
            sb.Append("</body>\n");
            sb.Append("</html>\n");

            context.Response.Write(sb.ToString());
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }


    }
}

