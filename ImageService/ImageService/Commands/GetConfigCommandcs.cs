using Communication.Infrastructure;
using ImageService.Commands;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    class GetConfigCommand : ICommand
    {
        #region Methods
        public string Execute(string[] args, out bool result)
        {
            try
            {
                SettingsObject settings = SettingsObject.GetInstance;
                string[] arguments = new string[1];
                arguments[0] = settings.ToJson();
                CommandRecievedEventArgs c = new CommandRecievedEventArgs((int)CommandEnum.GetConfigCommand, arguments, null);
                result = true;
                return c.ToJson();
            }
            catch (Exception e)
            {
                result = false;
                return e.ToString();
            }

        }
        #endregion
    }
}
