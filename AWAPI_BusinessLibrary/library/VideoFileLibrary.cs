using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace AWAPI_BusinessLibrary.library
{

    ///// <summary>
    ///// If it is a video file, it will be saved differently:
    ///// . flv will be saved as it is 
    ///// . other files will be enconed than they will be saved...
    ///// </summary>
    //public class VideoFileLibrary
    //{
    //    public string[] VideFile_Extensions = {".3g2", ".3gp", ".asf", ".asx", ".avi", ".flv",
    //                    ".mov", ".mp4", ".mpg", ".mpeg", ".rm", ".swf", ".vob", ".wmv" };

    //    public const string VIDEO_ENCODER = "~/utilities/ffmpeg/ffmpeg.exe";


    //    /// <summary>
    //    /// Converts video file to FLV...
    //    /// </summary>
    //    /// <param name="fileId"></param>
    //    /// <param name="sourceFilePath"></param>
    //    /// <returns></returns>
    //    public string ConvertToFLV(string sourceFilePath)
    //    {
    //        if (!File.Exists(sourceFilePath))
    //            throw new Exception("File does not exist.");

    //        string sourceFileExtension = FileLibrary.GetFileExension(sourceFilePath);
    //        if (sourceFileExtension == "flv")
    //            return sourceFilePath;

    //        string targetFilePath = sourceFilePath.Replace("." + sourceFileExtension, ".flv");
    //        //if exists, delete the target file path 
    //        if (System.IO.File.Exists(targetFilePath))
    //            System.IO.File.Delete(targetFilePath);

    //        string fileargs = String.Format(" -i \"{0}\" \"{1}\" ",
    //                                    sourceFilePath, targetFilePath);

    //        Process proc = new Process();
    //        proc.StartInfo.Arguments = fileargs;
    //        proc.StartInfo.FileName = System.Web.HttpContext.Current.Server.MapPath(VIDEO_ENCODER);
    //        proc.Start(); // start !

    //        proc.WaitForExit(240000);

    //        return targetFilePath;
    //    }




    //}




    public class VideoFileLibrary
    {
        public const string FFMPEG_PATH = "~/utilities/ffmpeg/ffmpeg.exe";


        #region Properties
        private string _ffExe;
        public string ffExe
        {
            get
            {
                return _ffExe;
            }
            set
            {
                _ffExe = value;
            }
        }

        private string _WorkingPath;
        public string WorkingPath
        {
            get
            {
                return _WorkingPath;
            }
            set
            {
                _WorkingPath = value;
            }
        }

        #endregion

        #region Constructors
        public VideoFileLibrary()
        {
            Initialize();
        }
        public VideoFileLibrary(string ffmpegExePath)
        {
            _ffExe = ffmpegExePath;
            Initialize();
        }
        #endregion

        #region Initialization
        private void Initialize()
        {
            //first make sure we have a value for the ffexe file setting  
            if (string.IsNullOrEmpty(_ffExe))
            {
                _ffExe = HttpContext.Current.Server.MapPath(FFMPEG_PATH);
            }

            //Now see if ffmpeg.exe exists  
            string workingpath = GetWorkingFile();
            if (string.IsNullOrEmpty(workingpath))
            {
                //ffmpeg doesn't exist at the location stated.  
                throw new Exception("Could not find a copy of ffmpeg.exe");
            }
            _ffExe = workingpath;

            //now see if we have a temporary place to work  
            if (string.IsNullOrEmpty(_WorkingPath))
            {
                object o = ConfigurationManager.AppSettings["ffmpeg:WorkingPath"];
                if (o != null)
                {
                    _WorkingPath = o.ToString();
                }
                else
                {
                    _WorkingPath = string.Empty;
                }
            }
        }

        public bool isVideoFile(string extension)
        {
            string[] fileExtensions = {"3g2", "3gp", "asf", "asx", "avi", "flv",
                       "mov", "mp4", "mpg", "mpeg", "rm", "swf", "vob", "wmv" };

            if (fileExtensions.Contains(extension.ToLower()))
                return true;
            return false;
        }


        private string GetWorkingFile()
        {
            //try the stated directory  
            if (File.Exists(_ffExe))
            {
                return _ffExe;
            }

            //oops, that didn't work, try the base directory  
            if (File.Exists(Path.GetFileName(_ffExe)))
            {
                return Path.GetFileName(_ffExe);
            }

            //well, now we are really unlucky, let's just return null  
            return null;
        }
        #endregion

        #region Get the File without creating a file lock
        public static System.Drawing.Image LoadImageFromFile(string fileName)
        {
            System.Drawing.Image theImage = null;
            using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                if (fileStream.Length == 0)
                    return null;

                byte[] img;
                img = new byte[fileStream.Length];
                fileStream.Read(img, 0, img.Length);
                fileStream.Close();
                theImage = System.Drawing.Image.FromStream(new MemoryStream(img));
                img = null;
            }
            GC.Collect();
            return theImage;
        }

        public static MemoryStream LoadMemoryStreamFromFile(string fileName)
        {
            MemoryStream ms = null;
            using (FileStream fileStream = new FileStream(fileName, FileMode.Open,
            FileAccess.Read))
            {
                byte[] fil;
                fil = new byte[fileStream.Length];
                fileStream.Read(fil, 0, fil.Length);
                fileStream.Close();
                ms = new MemoryStream(fil);
            }
            GC.Collect();
            return ms;
        }
        #endregion

        #region Run the process
        private string RunProcess(string Parameters)
        {
            //create a process info  
            ProcessStartInfo oInfo = new ProcessStartInfo(this._ffExe, Parameters);
            oInfo.UseShellExecute = false;
            oInfo.CreateNoWindow = true;
            oInfo.RedirectStandardOutput = true;
            oInfo.RedirectStandardError = true;

            //Create the output and streamreader to get the output  
            string output = null; StreamReader srOutput = null;

            //try the process  
            try
            {
                //run the process  
                Process proc = System.Diagnostics.Process.Start(oInfo);

                proc.WaitForExit();

                //get the output  
                srOutput = proc.StandardError;

                //now put it in a string  
                output = srOutput.ReadToEnd();

                proc.Close();
            }
            catch (Exception)
            {
                output = string.Empty;
            }
            finally
            {
                //now, if we succeded, close out the streamreader  
                if (srOutput != null)
                {
                    srOutput.Close();
                    srOutput.Dispose();
                }
            }
            return output;
        }
        #endregion

        #region GetVideoInfo
        public VideoFile GetVideoInfo(MemoryStream inputFile, string Filename)
        {
            string tempfile = Path.Combine(this.WorkingPath, System.Guid.NewGuid().ToString() + Path.GetExtension(Filename));
            FileStream fs = File.Create(tempfile);
            inputFile.WriteTo(fs);
            fs.Flush();
            fs.Close();
            GC.Collect();

            VideoFile vf = null;
            try
            {
                vf = new VideoFile(tempfile);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            GetVideoInfo(vf);

            try
            {
                File.Delete(tempfile);
            }
            catch (Exception)
            {

            }

            return vf;
        }

        public VideoFile GetVideoInfo(string inputPath)
        {
            VideoFile vf = null;
            try
            {
                vf = new VideoFile(inputPath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            GetVideoInfo(vf);
            return vf;
        }

        public void GetVideoInfo(VideoFile input)
        {
            //set up the parameters for video info  
            string Params = string.Format("-i {0}", input.Path);
            string output = RunProcess(Params);
            input.RawInfo = output;

            //get duration  
            Regex re = new Regex("[D|d]uration:.((\\d|:|\\.)*)");
            Match m = re.Match(input.RawInfo);

            if (m.Success)
            {
                string duration = m.Groups[1].Value;
                string[] timepieces = duration.Split(new char[] { ':', '.' });
                if (timepieces.Length == 4)
                {
                    input.Duration = new TimeSpan(0, Convert.ToInt16(timepieces[0]), Convert.ToInt16(timepieces[1]), Convert.ToInt16(timepieces[2]), Convert.ToInt16(timepieces[3]));
                }
            }

            //get audio bit rate  
            re = new Regex("[B|b]itrate:.((\\d|:)*)");
            m = re.Match(input.RawInfo);
            double kb = 0.0;
            if (m.Success)
            {
                Double.TryParse(m.Groups[1].Value, out kb);
            }
            input.BitRate = kb;

            //get the audio format  
            re = new Regex("[A|a]udio:.*");
            m = re.Match(input.RawInfo);
            if (m.Success)
            {
                input.AudioFormat = m.Value;
            }

            //get the video format  
            re = new Regex("[V|v]ideo:.*");
            m = re.Match(input.RawInfo);
            if (m.Success)
            {
                input.VideoFormat = m.Value;
            }

            //get the video format  
            re = new Regex("(\\d{2,3})x(\\d{2,3})");
            m = re.Match(input.RawInfo);
            if (m.Success)
            {
                int width = 0; int height = 0;
                int.TryParse(m.Groups[1].Value, out width);
                int.TryParse(m.Groups[2].Value, out height);
                input.Width = width;
                input.Height = height;
            }
            input.infoGathered = true;
        }
        #endregion

        #region Convert to FLV
        public OutputPackage ConvertToFLV(MemoryStream inputFile, string Filename)
        {
            string tempfile = Path.Combine(this.WorkingPath, System.Guid.NewGuid().ToString() + Path.GetExtension(Filename));
            FileStream fs = File.Create(tempfile);
            inputFile.WriteTo(fs);
            fs.Flush();
            fs.Close();
            GC.Collect();

            VideoFile vf = null;
            try
            {
                vf = new VideoFile(tempfile);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            OutputPackage oo = ConvertToFLV(vf);

            try
            {
                File.Delete(tempfile);
            }
            catch (Exception)
            {

            }

            return oo;
        }
        public OutputPackage ConvertToFLV(string inputPath)
        {
            VideoFile vf = null;
            //try
            //{
                vf = new VideoFile(inputPath);
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

            OutputPackage oo = ConvertToFLV(vf);
            return oo;
        }
        public OutputPackage ConvertToFLV(VideoFile input)
        {
            if (!input.infoGathered)
            {
                GetVideoInfo(input);
            }
            OutputPackage ou = new OutputPackage();

            string sourceExtension = GetFileExension(input.Path);
            string sourceWithoutExtension = input.Path.Replace("." + sourceExtension, "");

            //GET PREVIEW IMAGE ---------------------------------------
            string previewPath = sourceWithoutExtension + ".jpg";
            string processParams = string.Format(" -i \"{0}\" -f mjpeg -t 0.001 -ss 5 -y \"{1}\" ", input.Path, previewPath);
            string output = RunProcess(processParams);
            ou.RawOutput = output;

            if (File.Exists(previewPath))
            {
                ou.PreviewImage = LoadImageFromFile(previewPath);
                ou.PreviewImagePath = previewPath;
            }
            else
            { //try running again at frame 1 to get something  
                processParams = string.Format("-i \"{0}\" \"{1}\" -vcodec mjpeg -ss {2} -vframes 1 -an -f rawvideo", input.Path, previewPath, 1);
                output = RunProcess(processParams);

                ou.RawOutput = output;

                if (File.Exists(previewPath))
                {
                    ou.PreviewImage = LoadImageFromFile(previewPath);
                    ou.PreviewImagePath = previewPath;
                }
            }

            //CONVERT THE FILE --------------------------------------------------------------------------------
            if (sourceExtension == "flv")
            {
                ou.Path = input.Path;
                ou.Length = 0;

                byte[] fileBytes = File.ReadAllBytes(input.Path);
                MemoryStream mems = new MemoryStream(fileBytes);
                ou.VideoStream = mems;
                ou.Length = (int)ou.VideoStream.Length;

            }
            else
            {
                string flvPath = sourceWithoutExtension + ".flv";
                //processParams = string.Format("-i {0} -s 480x360 -y -ar 44100 -ab 64 -f flv {1}", input.Path, flvPath);
                //string videoParam = "-b 512k -f flv -s 480x270 -vcodec libx264 -vpre hq";
                //string autidoParam = "-ar 44100 -ac 2 -ab 93k -acodec aac";
                // Params = String.Format("-i \"{0}\" {1} {2} -y \"{3}\"",
                // input.Path, videoParam, autidoParam, flvPath);

                //medium quality
                processParams = string.Format("-i \"{0}\" -acodec mp3 -ab 48k -ac 1 -ar 44100 -f flv -deinterlace -nr 500 -r 25 -b 270k -me_range 25 -i_qfactor 0.71 -g 500 \"{1}\" ",
                                            input.Path, flvPath);

                ////high quality
                //processParams = string.Format("-i \"{0}\" -acodec mp3 -ab 64k -ac 2 -ar 44100 -f flv -deinterlace -nr 500 -r 25 -b 650k -me_range 25 -i_qfactor 0.71 -g 500 \"{1}\"",
                //            input.Path, flvPath);
                                    

                Process proc = new Process();
                proc.StartInfo.Arguments = processParams;
                proc.StartInfo.FileName = this._ffExe;
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;

                proc.Start(); // start !
                proc.WaitForExit();

                //output = RunProcess(processParams);

                if (File.Exists(flvPath))
                {
                    ou.Path = flvPath;
                    ou.VideoStream = LoadMemoryStreamFromFile(flvPath);
                    ou.Length = (int)ou.VideoStream.Length;

                    try
                    {
                        File.Delete(input.Path);
                    }
                    catch (Exception) { }
                }
            }
            ou.Success = true;

            return ou;
        }
        #endregion

        public static string GetFileExension(string fileName)
        {
            string[] parts = fileName.Split('.');
            if (parts.Length > 1)
                return parts[parts.Length - 1].ToLower();
            return "";
        }
    }

    public class VideoFile
    {
        #region Properties
        private string _Path;
        public string Path
        {
            get
            {
                return _Path;
            }
            set
            {
                _Path = value;
            }
        }

        public TimeSpan Duration { get; set; }
        public double BitRate { get; set; }
        public string AudioFormat { get; set; }
        public string VideoFormat { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public string RawInfo { get; set; }
        public bool infoGathered { get; set; }
        #endregion

        #region Constructors
        public VideoFile(string path)
        {
            _Path = path;
            Initialize();
        }
        #endregion

        #region Initialization
        private void Initialize()
        {
            this.infoGathered = false;
            //first make sure we have a value for the video file setting  
            if (string.IsNullOrEmpty(_Path))
            {
                throw new Exception("Could not find the location of the video file");
            }

            //Now see if the video file exists  
            if (!File.Exists(_Path))
            {
                throw new Exception("The video file " + _Path + " does not exist.");
            }
        }
        #endregion
    }

    public class OutputPackage
    {
        public MemoryStream VideoStream { get; set; }
        public int Length { get; set; }
        public System.Drawing.Image PreviewImage { get; set; }
        public string PreviewImagePath { get; set; }
        public string RawOutput { get; set; }
        public bool Success { get; set; }
        public string Path { get; set; }
    }

}