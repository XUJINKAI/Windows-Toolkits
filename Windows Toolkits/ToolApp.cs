using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using XJK.SysX;

namespace Windows_Toolkits
{
    [Serializable]
    public class ToolApp
    {
        private static readonly string AppPackageFolder = Path.GetDirectoryName(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
        public static string GetAppPath(string AppName) => Path.Combine(AppPackageFolder, $@"{AppName}\{AppName}.exe");

        public ToolApp() { }

        public ToolApp(string name)
        {
            AppName = name;
        }

        public string AppName { get; set; }
        public string Description { get; set; }
        public string Protocol => AppName + ":";
        public string FilePath => GetAppPath(AppName);

        public void Run()
        {
            Process.Start(FilePath);
        }

        public void ExplorerLocation()
        {
            Cmd.Explorer(FilePath);
        }
    }
}
