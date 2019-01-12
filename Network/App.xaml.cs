using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Network
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            HandleGlobalExceptions();
            this.ShutdownMode = ShutdownMode.OnMainWindowClose;
            (this.MainWindow = new NetworkWindow()).Show();
            //new DebugWindow1().Show();
        }

        private void HandleGlobalExceptions()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        }

        public static void ProcessException(string From, Exception e, bool ShowMessageBox)
        {
            var msg = $"Source: {From}{Environment.NewLine}Message: {e.Message}{Environment.NewLine}StackTrace:{Environment.NewLine}{e.StackTrace}";
            if (ShowMessageBox) MessageBox.Show(msg, "App Exception");
            Trace.TraceError(msg);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exp = e.ExceptionObject as Exception;
            ProcessException("AppDomain.UnhandledException", exp, false);
        }

        private static void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            ProcessException("App.DispatcherUnhandledException", e.Exception, false);
            e.Handled = false;
        }

        private static void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            ProcessException("TaskScheduler.UnobservedTaskException", e.Exception, false);
        }
    }
}
