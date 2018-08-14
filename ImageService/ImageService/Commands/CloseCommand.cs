using Communication.Infrastructure;
using ImageService.Commands;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    class CloseCommand : ICommand
    {
        #region Methods

        public string Execute(string[] args, out bool result)
        {
            try
            {
                string path = args[0];
                SettingsObject settings = SettingsObject.GetInstance;
                result = settings.RemoveHandler(path);
                string[] arguments = new string[1];
                arguments[0] = settings.Handlers;
                //Remove handler from app config!
                CommandRecievedEventArgs c = new CommandRecievedEventArgs((int)CommandEnum.CloseCommand, arguments, null);
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
