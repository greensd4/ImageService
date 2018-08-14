using Communication.Client;
using Communication.Infrastructure;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace WebApp.Models
{
    public class PhotosModel
    {
        #region Properties
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Photos")]
        public List<PhotosInfo> Photos { get; set; }
        private List<string> ThumbnailsPath { get; set; }
        public string ClickedPhotoName { get; set; }
        public string ClickedPhotoPath { get; set; }
        public string ClickedPhotoThumbPath { get; set; } 
        #endregion
        #region Members & Events
        private static IISClient client;
        public delegate void Refresh();
        public event Refresh NotifyRefresh;
        #endregion
        public PhotosModel()
        {
            try
            {
                client = ISClient.ClientServiceIns;
            } 
            catch (Exception e)
            {
                client = null;
            }
            Photos = new List<PhotosInfo>();
            ThumbnailsPath = new List<string>();
        }
        public void AddPhotosToList()
        {
            ConfigModel config = ConfigModel.GetInstance;
            string output = config.OutputDir;
            if (!Directory.Exists(output) || output == null)
                return;
            string pathToThumbnailsDir = Path.Combine(output, "Thumbnails");
            List<string> files = Directory.GetFiles(pathToThumbnailsDir, "*", SearchOption.AllDirectories).ToList<string>();
            Photos = new List<PhotosInfo>();
            ThumbnailsPath = new List<string>();
            //Check whether the file is already in list, and if it does delete it.
            foreach (string file in files)
            {
                Debug.WriteLine(file);
                if (ThumbnailsPath.Contains(file))
                    files.Remove(file);
            }
            //Add only new files to lists.
            foreach (string file in files)
            {
                ThumbnailsPath.Add(file);
                string thumbPath = file;
                string path = file.Replace("Thumbnails\\", "");
                if (!File.Exists(path))
                    continue;
                FileInfo info = new FileInfo(path);
                string name = info.Name;
                DateTime dateTime = GetDateTime(path);
                string date = dateTime.ToString("d");
                Photos.Add(new PhotosInfo { Name = name, Path = path, ThumbPath = thumbPath, Date = date });
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
                    return dt;
                }
            }
            catch (Exception e)
            {
                e.ToString();
                return File.GetCreationTime(path);
            }
        }
        public void GetMessageFromClient(object sender, string crea)
        {
            CommandRecievedEventArgs command = CommandRecievedEventArgs.FromJson(crea);
            if (command.CommandID == (int)CommandEnum.RemovePhoto)
            {
                string thumbPath = command.Args[0];
                if (ThumbnailsPath.Contains(thumbPath))
                {
                    ThumbnailsPath.Remove(thumbPath);
                    foreach(PhotosInfo p in Photos)
                    {
                        if(p.ThumbPath.Equals(thumbPath))
                            Photos.Remove(p);
                    }
                }
                NotifyRefresh?.Invoke();
            }
        }
        public void SendCommandToService(CommandRecievedEventArgs command)
        {
            client.Write(command.ToJson());
        }
    }
    public class PhotosInfo
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Path")]
        public string Path { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "ThumbPath")]
        public string ThumbPath { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Date")]
        public string Date { get; set; }
    }
}