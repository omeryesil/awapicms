using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace AWAPI_Common.library
{
    public class ImageLibrary
    {

        public const int MAX_IMAGE_SIZE_TO_SAVE = 800;


        /// <summary>
        /// Resizes an image
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="size">WidthxHeight
        /// example: 100x150
        /// </param>
        /// <returns></returns>
        public static byte[] ResizeImage(string filename, string size)
        {
            return ResizeImage(filename, false, 0, 0, size, false);
        }

        /// <summary>
        /// if pushSize is true then then image will be resized and larger side (width or height) will be cropped
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="size"></param>
        /// <param name="crop"></param>
        /// <returns></returns>
        public static byte[] ResizeImage(string filename, string size, bool pushSize)
        {
            return ResizeImage(filename, false, 0, 0, size, pushSize);
        }

        public static byte[] ResizeImage(string filename, bool crop, int x, int y, string size, bool pushSize)
        { 
            return ResizeImage(filename, null, crop, x, y, size, pushSize);
        
        }


        /// <summary>
        /// Resizes an image
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static byte[] ResizeImage(string filename, System.IO.Stream stream, bool crop, int x, int y, string size, bool pushSize)
        {
            const int WIDTH = 0;
            const int HEIGHT = 1;
            double imgRatio = 1;

            Bitmap bmp = null;
            Graphics gs = null;
            System.Drawing.Image img = null;
            System.IO.MemoryStream ms = null;
            byte[] bytes = null;

            try
            {
                string[] ar = size.Split('x');
                int sizeW = Convert.ToInt32(ar[WIDTH]);
                int sizeH = Convert.ToInt32(ar[HEIGHT]);

                int newWidth = sizeW;
                int newHeight = sizeH;

                bool ratioByWidth = true;
                if (stream != null && stream.Length>0)
                    img = System.Drawing.Image.FromStream(stream);
                else
                    img = System.Drawing.Image.FromFile(filename);

                #region DONT CROP
                if (!crop)
                {
                        if (!pushSize)
                        {
                            if (newHeight > newWidth)
                                ratioByWidth = true;
                            else
                                ratioByWidth = false;
                        }
                        if (ratioByWidth)
                            imgRatio = (double)img.Width / (double)newWidth;
                        else
                            imgRatio = (double)img.Height / (double)newHeight;
                    //}

                    newWidth = (int)((double)img.Width / (double)imgRatio);
                    newHeight = (int)((double)img.Height / (double)imgRatio);
                    if (!pushSize && (newWidth > img.Width || newHeight > img.Height))
                    {
                        newWidth = img.Width;
                        newHeight = img.Height;
                    }

                    if (pushSize)
                    {
                        imgRatio = 1;
                        if (newWidth < sizeW)
                            imgRatio = (double)sizeW / (double)newWidth;
                        else
                            if (newHeight < sizeH)
                                imgRatio = (double)sizeH / (double)newHeight;

                        if (imgRatio != 1)
                        {
                            newWidth = (int)((double)newWidth * (double)imgRatio);
                            newHeight = (int)((double)newHeight * (double)imgRatio);
                        }
                    }
                }
                #endregion

                bmp = new Bitmap(newWidth, newHeight);
                gs = Graphics.FromImage(bmp);
                gs.SmoothingMode = SmoothingMode.AntiAlias;
                gs.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gs.PixelOffsetMode = PixelOffsetMode.HighQuality;

                if (!crop)
                    gs.DrawImage(img, x, y, newWidth, newHeight);
                else
                    gs.DrawImage(img, new Rectangle(0, 0, newWidth, newHeight), x, y, newWidth, newHeight, GraphicsUnit.Pixel);

                //ms = new System.IO.MemoryStream();

                //bmp.Save(ms, img.RawFormat);
                //ms.Seek(0, System.IO.SeekOrigin.Begin);

                //bytes = new byte[ms.Length];
                //ms.Read(bytes, 0, bytes.Length);
                ms = ImageToStream(img, bmp, ref bytes);

                if (pushSize)
                {
                    int w = Convert.ToInt32(ar[WIDTH]);
                    int h = Convert.ToInt32(ar[HEIGHT]);
                    bmp = new Bitmap(w, h);
                    gs = Graphics.FromImage(bmp);
                    img = Image.FromStream(ms);
                    gs.DrawImage(img, new Rectangle(0, 0, w, h), x, y, w, h, GraphicsUnit.Pixel);
                    ms = ImageToStream(img, bmp, ref bytes);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (ms != null)
                {
                    ms.Close();
                    ms.Dispose();
                }

                if (gs != null)
                    gs.Dispose();

                if (img != null)
                    img.Dispose();

                if (bmp != null)
                    bmp.Dispose();
            }
            return bytes;
        }

        public static System.IO.MemoryStream ImageToStream(System.Drawing.Image img, Bitmap bmp, ref byte[] bytes)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            bmp.Save(ms, img.RawFormat);
            ms.Seek(0, System.IO.SeekOrigin.Begin);

            bytes = new byte[ms.Length];
            ms.Read(bytes, 0, bytes.Length);
            return ms;
        }



        /// <summary>
        /// Returns image's dimension in width x height format.
        /// </summary>
        /// <param name="imgStream"></param>
        /// <returns></returns>
        public static string GetImageDimension(System.IO.Stream imgStream)
        {
            string dimension = "";

            try
            {
                System.Drawing.Image img = System.Drawing.Image.FromStream(imgStream);
                dimension = img.PhysicalDimension.Width.ToString() + "x" +
                     img.PhysicalDimension.Height.ToString();

            }
            catch (Exception ex)
            {

            }
            return dimension;

        }
    }
}
