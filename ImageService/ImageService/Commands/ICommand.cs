using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    /// <summary>
    /// Interface for commands.
    /// </summary>
    public interface ICommand
    {
        #region Methods

        /// <summary>
        /// Execute - executes the command.
        /// </summary>
        /// <param name="args"> arguments for the command</param>
        /// <param name="result"> boolean result </param>
        /// <returns></returns>
        string Execute(string[] args, out bool result);
        #endregion
    }
}
