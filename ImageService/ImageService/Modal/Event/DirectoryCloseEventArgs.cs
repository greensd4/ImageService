using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Modal
{
    /// <summary>
    /// Event Args for the Directory Close.
    /// </summary>
    public class DirectoryCloseEventArgs : EventArgs
    {
        #region Properties
        public string DirectoryPath { get; set; }
        public string Message { get; set; }             // The Message That goes to the logger
        #endregion
        #region Constructor
        public DirectoryCloseEventArgs(string dirPath, string message)
        {
            DirectoryPath = dirPath;                    // Setting the Directory Name
            Message = message;                          // Storing the String
        }
        #endregion

    }
}
