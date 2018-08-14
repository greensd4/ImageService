using Communication.Infrastructure;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller.Handlers
{
    /// <summary>
    /// Interface for Directories Handlers.
    /// </summary>
    public interface IDirectoryHandler
    {
        #region Methods
        /// <summary>
        /// The Event That Notifies that the Directory is being closed
        /// </summary>
        event EventHandler<DirectoryCloseEventArgs> DirectoryClose;
        /// <summary>
        /// The Function Recieves the directory to Handle
        /// </summary>
        /// <param name="dirPath">directory's path</param>
        void StartHandleDirectory(string dirPath);
        /// <summary>
        ///  The Event that will be activated upon new Command
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">command args</param>
        void OnCommandRecieved(object sender, CommandRecievedEventArgs e);     
        /// <summary>
        /// Close the handler. Disables the FileWatcher
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="dirArgs">args</param>
        void CloseHandler(object sender, DirectoryCloseEventArgs dirArgs);
        #endregion
    }
}
