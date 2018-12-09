using System;
using System.Collections.Generic;
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
using XJK.PInvoke;
using XJK.SysX;
using XJK.SysX.Hooks;
using XJK.SysX.WinMsg;

namespace InputSpy
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class InputSpyWindow : Window
    {
        private KeyboardLLHook KeyboardLLHook;
        private MouseLLHook MouseLLHook;
        
        public bool IsSpying { get; private set; }
        
        public InputSpyWindow(KeyboardLLHook keyboardLLHook, MouseLLHook mouseLLHook)
        {
            InitializeComponent();
            this.Closed += InputSpyWindow_Closed;
            this.KeyboardLLHook = keyboardLLHook;
            this.MouseLLHook = mouseLLHook;
        }

        private void InputSpyWindow_Closed(object sender, EventArgs e)
        {
            StopSpy();
        }

        public void StartSpy()
        {
            if (IsSpying) return;
            IsSpying = true;
            KeyboardLLHook.KeyChange += KeyboardLLHook_KeyChange;
            MouseLLHook.MouseChange += MouseLLHook_MouseChange;
        }
        
        private void MouseLLHook_MouseChange(object sender, MouseChangeEventArgs e)
        {
            if (e.MouseMoved)
            {
                MousePosition.Text = e.MousePosition.ToString();
            }
            if (e.PressType != PressType.None)
            {
                LogLine(e);
            }
        }

        private void KeyboardLLHook_KeyChange(object sender, KeyChangeEventArgs e)
        {
            LogLine(e);
        }

        public void StopSpy()
        {
            if (!IsSpying) return;
            IsSpying = false;
            KeyboardLLHook.KeyChange -= KeyboardLLHook_KeyChange;
            MouseLLHook.MouseChange -= MouseLLHook_MouseChange;
        }
        
        protected override void OnStateChanged(EventArgs e)
        {
            this.WindowState = WindowState.Normal;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var handle = Win.Find.ByWindow(this);
            XJK.TimerHelper.NewTickTock(1000, (s, _) =>
            {
                if (!Win.Topmost.Get(handle))
                {
                    Topmost = false;
                    Topmost = true;
                    this.Activate();
                }
            });
        }

        private DateTime LastLogTime;

        private void LogLine(object s)
        {
            if((DateTime.Now-LastLogTime).TotalMilliseconds > 1000)
            {
                LogBox.Text += "--------------" + Environment.NewLine;
            }
            LastLogTime = DateTime.Now;
            LogBox.Text += s.ToString() + Environment.NewLine;
            Scroller.ScrollToEnd();
        }
    }
}
