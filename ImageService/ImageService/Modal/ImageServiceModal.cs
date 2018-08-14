//using ImageService.Infrastructure;
using Communication.Infrastructure;
using ImageService.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ImageService.Modal
{
    public class ImageServiceModal : IImageServiceModal
    {
        #region Members, Properties, Constructor
        public string OutputFolder
        {
            get { return m_OutputFolder; }
            set { m_OutputFolder = value; }
        }
        public int ThumbnailSize
        {
            get { return m_thumbnailSize; }
            set { m_thumbnailSize = value; }
        }
        private ILoggingService logging;
        private string m_OutputFolder;            // The Output Folder
        private int m_thumbnailSize;              // The Size Of The Thumbnail Size

        public ImageServiceModal(ILoggingService logging)
        {
            this.logging = logging;
        }
        #endregion
        #region Methods
        /// <summary>
        /// Handling new file in directory
        /// </summary>
        /// <param name="path"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public string AddFile(string path, out bool result)
        {
            try
            {

                if (File.Exists(path))
                {
                    DateTime creation = this.GetDateTime(path);
                    string thumbnailPath = this.OutputFolder + "\\Thumbnails";
                    string year = creation.Year.ToString();
                    string month = GetMonthName(creation.Month.ToString());
                    string name = Path.GetFileName(path);

                    //creates the outputDir and ThumbnailsDir if not exist.
                    DirectoryInfo dir = Directory.CreateDirectory(this.OutputFolder);
                    dir.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
                    Directory.CreateDirectory(thumbnailPath);

                    //Create the directory for the year
                    Directory.CreateDirectory(this.OutputFolder + "\\" + year);
                    Directory.CreateDirectory(thumbnailPath + "\\" + year);

                    //Create the directory for the month
                    string loc = this.OutputFolder + "\\" + year + "\\" + month;
                    DirectoryInfo locationToCopy = Directory.CreateDirectory(loc);
                    //Create the thumbnails directory for the month
                    string thumLoc = thumbnailPath + "\\" + year + "\\" + month;
                    DirectoryInfo locationToCopyThumbnail = Directory.CreateDirectory(thumLoc);

                    //move the file to new direcory.
                    string dstFile = System.IO.Path.Combine(loc, name);
                    if (File.Exists(dstFile))
                    {
                        logging.Log("renaming file  " + name, MessageTypeEnum.WARNING);
                        name = RenameFile(loc, name);
                        string oldName = Path.GetFileName(path);
                        string oldPath = path;
                        path = path.Replace(oldName, name);
                        File.Move(oldPath, path);
                        dstFile = Path.Combine(loc, name);
                    }
                    //File.Create(dstFile);
                    File.Move(path, dstFile);
                    logging.Log("Added file " + name, MessageTypeEnum.INFO);
                    //Save the thumbnail image.
                    string dstThum = System.IO.Path.Combine(thumLoc, name);
                    Image thumbImage;
                    try
                    {
                        thumbImage = Image.FromStream(new MemoryStream(File.ReadAllBytes(dstFile))); 
                    } catch(ArgumentException a)
                    {
                        File.Delete(dstFile);
                        logging.Log("Argument exception at " + dstFile + "Could not create thumbnail!", MessageTypeEnum.FAIL);
                        result = false;
                        return "Image does not exist!";

                    }
                    thumbImage = thumbImage.GetThumbnailImage(this.m_thumbnailSize,
                        this.m_thumbnailSize, () => false, IntPtr.Zero);
                    thumbImage.Save(dstThum);
                    thumbImage.Dispose();
                    result = true;
                    return dstFile;
                }
                else
                {

                    logging.Log("Could not add file ", MessageTypeEnum.FAIL);
                    result = false;
                    return "Image does not exist!";
                }
            }
            catch(Exception e)
            {
                result = false;
                return e.ToString();
            }

        }

        /// <summary>
        /// GetDateTime
        /// trying to get the date when the pic was taken, otherwise returns the creation date.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public DateTime GetDateTime(string path)
        {
            Regex r = new Regex(":");
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                using (Image myImage = Image.FromStream(fs, false, false))
                {
                    PropertyItem propItem = myImage.GetPropertyItem(36867);
                    string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                    DateTime dt = DateTime.Parse(dateTaken);
                    myImage.Dispose();
                    logging.Log("got date from file: " + path + " Date is " + dt.ToString(), MessageTypeEnum.INFO);
                    return dt;
                }
            }
            catch (Exception e)
            {
                logging.Log("Could not take date from file: " + path, MessageTypeEnum.FAIL);
                e.ToString();
                return File.GetCreationTime(path);

            }
        }
        /// <summary>
        /// The function converts month represented in number to it's name.
        /// </summary>
        /// <param name="MonthNum">number represents the month</param>
        /// <returns>string of the name of the month</returns>
        public string GetMonthName(string MonthNum)
        {
           if(MonthNum.Equals("1")) { return "January"; }
           if(MonthNum.Equals("2")) { return "February"; }
           if(MonthNum.Equals("3")) { return "March"; }
           if(MonthNum.Equals("4")) { return "April"; }
           if(MonthNum.Equals("5")) { return "May"; }
           if(MonthNum.Equals("6")) { return "June"; }
           if(MonthNum.Equals("7")) { return "July"; }
           if(MonthNum.Equals("8")) { return "August"; }
           if(MonthNum.Equals("9")) { return "September"; }
           if(MonthNum.Equals("10")) { return "October"; }
           if(MonthNum.Equals("11")) { return "November"; }
           if(MonthNum.Equals("12")) { return "December"; }
           return MonthNum;
        }
        /// <summary>
        /// Rename file,
        /// renames the file name.
        /// </summary>
        /// <param name="loc">destination location</param>
        /// <param name="oldName">old name</param>
        /// <returns>new name</returns>
        public string RenameFile(string loc, string oldName)
        {
            string path = Path.Combine(loc, oldName);
            string extension = Path.GetExtension(path);
            string nameWithoutExt = Path.GetFileNameWithoutExtension(path);
            string newName = oldName;
            int i = 0;
            while (File.Exists(path))
            {
                newName = nameWithoutExt + "(" + i.ToString() + ")" + extension;
                path = Path.Combine(loc, newName);
                i++;
            }

            return newName;

        }
    }
    #endregion

}
