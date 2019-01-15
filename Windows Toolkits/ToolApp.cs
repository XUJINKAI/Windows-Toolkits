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
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string Protocol { get; set; }
        public string CodeName { get; set; }

        public string FilePath => GetAppPath(CodeName);
        
        private static readonly string AppPackageFolder = Path.GetDirectoryName(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
        public static string GetAppPath(string Name) => Path.Combine(AppPackageFolder, $@"{Name}\{Name}.exe");

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
