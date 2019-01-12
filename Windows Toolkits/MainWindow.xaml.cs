using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Windows.ApplicationModel;
using Windows.UI.StartScreen;

namespace Windows_Toolkits
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            try
            {
                var v = Package.Current.Id.Version;
                Title += $" v{v.Major}.{v.Minor}.{v.Build}.{v.Revision}";
            }
            catch { }
#if DEBUG
            AddCommand(ModifierKeys.Control, Key.B, (sender, e) =>
            {
                this.Title.Count(); //Capture this to lambda
                Debugger.Break();
            });
            AddCommand(ModifierKeys.Control, Key.T, (sender, e) =>
            {
                this.Title.Count(); //Capture this to lambda
                // Test code...

            });
#endif
        }

        private void AddCommand(ModifierKeys modifierKeys, Key key, ExecutedRoutedEventHandler handler)
        {
            RoutedCommand Cmd = new RoutedCommand();
            Cmd.InputGestures.Add(new KeyGesture(key, modifierKeys));
            CommandBindings.Add(new CommandBinding(Cmd, handler));
        }
        
        private static ToolApp GetDataContextAsToolApp(object sender)
        {
            return sender.GetType().GetProperty("DataContext").GetValue(sender) as ToolApp;
        }

        private void BuildinApps_ListView_Item_Run(object sender, RoutedEventArgs e)
        {
            GetDataContextAsToolApp(sender).Run();
        }

        private void BuildinApps_ListView_Item_CopyProtocol(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(GetDataContextAsToolApp(sender).Protocol);
        }

        private void BuildinApps_ListView_Item_OpenLocation(object sender, RoutedEventArgs e)
        {
            GetDataContextAsToolApp(sender).ExplorerLocation();
        }

    }
}
