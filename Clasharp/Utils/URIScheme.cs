using Clasharp.Models.Profiles;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clasharp.Utils
{
    public class URIScheme
    {
        public const string UriScheme = "clash";
        const string FriendlyName = "Clasharp URI Scheme";
        public static void RegisterUriScheme()
        {
            string applicationLocation = "C:\\Users\\me\\Desktop\\HiddifyDesktop-new\\Clasharp\\bin\\Release\\net7.0\\publish\\Clasharp.exe";
            using (var key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Classes\\" + UriScheme))
            {
                // Replace typeof(App) by the class that contains the Main method or any class located in the project that produces the exe.
                // or replace typeof(App).Assembly.Location by anything that gives the full path to the exe

                key.SetValue("", "URL:" + FriendlyName);
                key.SetValue("URL Protocol", "");

                using (var defaultIcon = key.CreateSubKey("DefaultIcon"))
                {
                    defaultIcon.SetValue("", applicationLocation + ",1");
                }

                using (var commandKey = key.CreateSubKey(@"shell\open\command"))
                {
                    commandKey.SetValue("", "\"" + applicationLocation + "\" \"%1\"");
                }
            }
        }
        public static bool CheckUriSchemeExist()
        {
            using (var key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Classes\\" + UriScheme))
            {
                if (key.GetValue("").ToString() == "URL:" + FriendlyName)
                {
                    return true;
                }              
            }
            return false;
        }
        public static bool IsUriForProgram(string uri)
        {
            if (uri.ToLower().StartsWith(UriScheme + "://"))
            {
                return true;
            }
            return false;
        }
        public static Profile ParseUri(string uri)
        {
            // Valid URI sample = clash://install-config?url=https://mysite.com/all.yml&name=profilename
           
            string url, profile_name;
            
            // Remove additional things
            var prunedUri = uri.Split("url=");
            if (prunedUri.Length < 2)
            {
                throw new Exception("Uri is invalid");
            }
            // Just keep url and profile name
            prunedUri = prunedUri[1].Split('&');
            if (prunedUri.Length < 2)
            {
                throw new Exception("Uri is invalid");
            }
            url = prunedUri[0];

            // For extract profile name from uri
            prunedUri = prunedUri[1].Split('=');
            if (prunedUri.Length < 2)
            {
                throw new Exception("Uri is invalid");
            }
            profile_name = prunedUri[1];

            Profile p = new Profile()
            {
                Filename = profile_name,
                Name = profile_name,
                RemoteUrl = url,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now,
                Type = ProfileType.Remote,
                Description = $"{profile_name} Remote profile",
            };

            return p;
        }
    }
}
