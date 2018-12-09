using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using XJK.SysX.Hooks;

namespace InputSpy
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            this.ShutdownMode = ShutdownMode.OnMainWindowClose;
            InputSpyWindow inputSpyWindow = new InputSpyWindow(new KeyboardLLHook(), new MouseLLHook());
            this.MainWindow = inputSpyWindow;
            inputSpyWindow.Show();
            inputSpyWindow.StartSpy();
            base.OnStartup(e);
        }
    }
}
