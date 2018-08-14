using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller
{
    /// <summary>
    /// Interface for ImageController.
    /// </summary>
    public interface IImageController
    {
        #region Methods

        /// <summary>
        /// Executes commands that the controller recive.
        /// </summary>
        /// <param name="commandID"></param>
        /// <param name="args"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        string ExecuteCommand(int commandID, string[] args, out bool result);
        #endregion
    }
}
