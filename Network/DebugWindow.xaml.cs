using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Network
{
    /// <summary>
    /// DebugWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DebugWindow : Window
    {
        public static Window MainWindow => App.Current.MainWindow;

        public DebugWindow()
        {
            InitializeComponent();
            new XJK.Logger.Listener(s => Dispatcher.Invoke(() =>
            {
                Log(s);
                LogBox_Scroller.ScrollToEnd();
            }))
            { Registered = true };
            AddCommand(ModifierKeys.Control, Key.B, DebuggerBreak);
            AddCommand(ModifierKeys.Control, Key.T, TestFunction);
        }

        private void Log(string s) => LogBox.AppendText(s);

        private void AddCommand(ModifierKeys modifierKeys, Key key, ExecutedRoutedEventHandler handler)
        {
            RoutedCommand Cmd = new RoutedCommand();
            Cmd.InputGestures.Add(new KeyGesture(key, modifierKeys));
            CommandBindings.Add(new CommandBinding(Cmd, handler));
        }

        private void LogBox_Clear(object sender, RoutedEventArgs e)
        {
            LogBox.Clear();
        }

        private void AppShutDown(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void DebuggerBreak(object sender, RoutedEventArgs e)
        {
            Debugger.Break();
        }

        private void ThrowException(object sender, RoutedEventArgs e)
        {
            throw new Exception("Exception from ThrowExceptionCmd by user.");
        }

        private void TestFunction(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("TestFunction");
        }

    }
}
