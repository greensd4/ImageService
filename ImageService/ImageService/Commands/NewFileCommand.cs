using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    /// <summary>
    /// New File Command implements ICommand.
    /// </summary>
    public class NewFileCommand : ICommand
    {
        #region Members, Constructor
        private IImageServiceModal m_modal;

        /// <summary>
        /// Constructors.
        /// </summary>
        /// <param name="modal">Service Modal</param>
        public NewFileCommand(IImageServiceModal modal)
        {
            m_modal = modal;            // Storing the Modal
        }
        #endregion
        #region Methods

        public string Execute(string[] args, out bool result)
        {
            // The String Will Return the New Path if result = true, and will return the error message
           
             return this.m_modal.AddFile(args[0], out result);
            
        }
        #endregion

    }
}
