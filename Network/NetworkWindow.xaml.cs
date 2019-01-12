using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
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
using System.Windows.Shapes;
using Net = XJK.Network.NetLegacy;

namespace Network
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class NetworkWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public IntPtr Handle { get; private set; }
        public string Url => UrlBox.Text;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public NetworkWindow()
        {
            InitializeComponent();
#if DEBUG
            AddCommand(ModifierKeys.Alt | ModifierKeys.Shift, Key.D, (sender, e) =>
            {
                new DebugWindow().Show();
            });
#endif
        }

        private void Log(string s) { LogBox.AppendText(s); LogBox_Scroller.ScrollToEnd(); }
        private void LogLine(string s) { Log(s + Environment.NewLine); }

        private void AddCommand(ModifierKeys modifierKeys, Key key, ExecutedRoutedEventHandler handler)
        {
            RoutedCommand Cmd = new RoutedCommand();
            Cmd.InputGestures.Add(new KeyGesture(key, modifierKeys));
            CommandBindings.Add(new CommandBinding(Cmd, handler));
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            Handle = new WindowInteropHelper(this).Handle;
        }

        private void Menu_Exit(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private async void GetContent(object sender, RoutedEventArgs e)
        {
            LogLine($"Loading... <{Url}>");
            string content = await Net.GetAsync(Url, true);
            LogLine(content);
        }
    }
}
