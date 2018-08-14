using Communication.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    class RemovePhotoCommand : ICommand
    {
        public string Execute(string[] args, out bool result)
        {
            if (args.Length != 2)
            {
                result = false;
                return null;
            }
            if (args[0] == null || args[1] == null)
            {
                result = false;
                return null;
            }
            string path = args[0];
            string thumbPath = args[1];
            if (!File.Exists(path) || !File.Exists(thumbPath))
            {
                result = false;
                return null;
            }
            try
            {
                File.Delete(path);
                File.Delete(thumbPath);
                result = true;
                return new CommandRecievedEventArgs((int)CommandEnum.RemovePhoto, new string[] { path, thumbPath }, null).ToJson();
            } catch (Exception e)
            {

            }
            result = false;
            return null;

        }
    }
}
