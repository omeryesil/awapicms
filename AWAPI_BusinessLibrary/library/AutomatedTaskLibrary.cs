using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AWAPI_Data.Data;
using Microsoft.CSharp;
using System.Reflection;
using System.CodeDom.Compiler;

namespace AWAPI_BusinessLibrary.library
{
    /// <summary>
    /// 
    /// </summary>
    public class AutomatedTaskLibrary
    {
        AWAPI_Data.Data.SiteContextDataContext _context = new AWAPI_Data.Data.SiteContextDataContext();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public awAutomatedTask Get(long taskId)
        {
            return (from l in _context.awAutomatedTasks where l.automatedTaskId.Equals(taskId) select l).FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guidToRun"></param>
        /// <returns></returns>
        public awAutomatedTask Get(Guid guid)
        {
            return (from l in _context.awAutomatedTasks where l.guidToRun.Equals(guid) select l).FirstOrDefault();
        }

        public void CompleteTask(long taskGroupId)
        {
            var list = from l in _context.awAutomatedTasks
                       where l.automatedTaskGroupId.Equals(taskGroupId)
                       select l;

            if (list == null)
                return;

            foreach (awAutomatedTask task in list)
            {
                if (task.deleteAfterComplete)
                    _context.awAutomatedTasks.DeleteOnSubmit(task);
                else
                {
                    task.isCompleted = true;
                    task.lastBuildDate = DateTime.Now;
                }
            }
            _context.SubmitChanges();
        }


        /// <summary>
        /// Returns url in a specified format 
        /// 
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="success"></param>
        /// <param name="description">Can be an error code, or userid, blogid, postid, etc...</param>
        /// <returns>example: http://awapi.com/registerationconfirm.aspx?status=ok&description=234545456345345345345   (userid)</returns>
        public string CreateRedirectUrl(string baseUrl, bool success, string description)
        {
            string url = String.Format("{0}?status={1}&description={2}",
                baseUrl, success ? "ok" : "error", description);

            return url;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="guid"></param>
        /// <param name="title">Approve Post Comment</param>
        /// <param name="description"></param>
        /// <param name="deleteAfterComplete"></param>
        /// <param name="completedMessage">Comment has been approved</param>
        /// <param name="namespaceAndClass">AWAPI_BusinessLibrary.library.BlogLibrary</param>
        /// <param name="methodName">UpdateBlogPostCommentStatus</param>
        /// <param name="methodParameters">int64:5635821884568557888|int:1</param>
        /// <param name="resultRedirectUrl">http://awapi.com/registerationconfirm.aspx.   The page will be resurect to 
        ///   resultRedirectUrl + "?status=ok/error&description={description}"</param>
        public void Add(long siteId, long automatedTaskGroupId, Guid guid,
                        string title, string description, bool deleteAfterComplete,
                        string completedMessage, string namespaceAndClass, string methodName, string methodParameters,
                        string resultRedirectUrl)
        {
            awAutomatedTask task = new awAutomatedTask();
            task.automatedTaskId = AWAPI_Common.library.MiscLibrary.CreateUniqueId();
            task.siteId = siteId;
            task.automatedTaskGroupId = automatedTaskGroupId;
            task.guidToRun = guid;
            task.title = title;
            task.description = description;
            task.deleteAfterComplete = deleteAfterComplete;
            task.completedMessage = completedMessage;
            task.namespaceAndClass = namespaceAndClass;
            task.methodName = methodName;
            task.methodParameters = methodParameters;
            task.resultRedirectUrl = resultRedirectUrl;
            task.lastBuildDate = DateTime.Now;
            task.createDate = DateTime.Now;

            _context.awAutomatedTasks.InsertOnSubmit(task);

            _context.SubmitChanges();

        }

        public void UpdateErrorMessage(Guid guid, string errorMessage)
        {
            awAutomatedTask task = (from l in _context.awAutomatedTasks
                                    where l.guidToRun.Equals(guid)
                                    select l).FirstOrDefault();

            if (task == null)
                return;

            task.errorMessage = errorMessage;
            task.lastBuildDate = DateTime.Now;

            _context.SubmitChanges();
        }

        public bool RunTask(long taskId)
        {
            awAutomatedTask task = Get(taskId);
            return RunTask(task);
        }

        public bool RunTask(Guid guid)
        {
            awAutomatedTask task = Get(guid);
            return RunTask(task);
        }

        public bool RunTask(awAutomatedTask task)
        {
            try
            {
                Assembly asm = Assembly.GetExecutingAssembly();
                Type type = asm.GetType(task.namespaceAndClass);
                object obj = Activator.CreateInstance(type);
                MethodInfo method = type.GetMethod(task.methodName);

                if (String.IsNullOrEmpty(task.methodParameters))
                {
                    method.Invoke(obj, null);
                }
                else
                {
                    string[] prms = task.methodParameters.Split('|');
                    object[] objParams = new object[prms.Length];
                    int n = 0;
                    foreach (string prm in prms)
                    {
                        if (prm.ToLower().IndexOf("int:") == 0) objParams[n] = Convert.ToInt32(prm.Replace("int:", ""));
                        else if (prm.ToLower().IndexOf("int64:") == 0) objParams[n] = Convert.ToInt64(prm.Replace("int64:", ""));
                        if (prm.ToLower().IndexOf("string:") == 0) objParams[n] = prm.Replace("string:", "");
                        if (prm.ToLower().IndexOf("bool:") == 0) objParams[n] = Convert.ToBoolean(prm.Replace("bool:", ""));
                        if (prm.ToLower().IndexOf("double:") == 0) objParams[n] = Convert.ToDouble(prm.Replace("double:", ""));

                        n++;
                    }

                    method.Invoke(obj, objParams);
                    CompleteTask(task.automatedTaskGroupId);
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message +
                            ex.InnerException == null ? "" : "\n" + ex.InnerException.Message;

                UpdateErrorMessage(task.guidToRun, error);
                return false;
            }

            return true;

        }



    }
}
