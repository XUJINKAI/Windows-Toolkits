using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using XJK.SysX;
using System.Windows.Forms;
using System.Threading;

namespace KeepMonitorOn
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private bool _state;
        public bool State
        {
            get => _state; set
            {
                _state = value;
                Power.KeepSystemMonitorOn(value);
                UpdateContextMenu();
                if (value)
                {
                    TrayIcon.Icon = KeepMonitorOn.Properties.Resources.IconKeep;
                    TrayIcon.Text = "(ON) Prevent monitor off and system sleep.";
                }
                else
                {
                    TrayIcon.Icon = KeepMonitorOn.Properties.Resources.IconOff;
                    TrayIcon.Text = "(OFF) Prevent monitor off and system sleep.";
                }
            }
        }

        public NotifyIcon TrayIcon;

        private void UpdateContextMenu()
        {
            var menu = new ContextMenu();
            menu.MenuItems.Add("Check State (powercfg /requests)", (sender, e) =>
            {
                Cmd.New("cmd", "/k powercfg /requests")
                .RunAs(XJK.SysX.CommandHelper.Privilege.Admin)
                .Excute();
            });
            menu.MenuItems.Add("-");
            menu.MenuItems.Add("Power Suspend (Wheel Click)", (sender, e) => PowerSuspend());
            menu.MenuItems.Add("-");
            menu.MenuItems.Add(new MenuItem("Enable", (sender, e) =>
            {
                State = !State;
            })
            { Checked = State, DefaultItem = true });
            menu.MenuItems.Add("Exit", (sender, e) =>
            {
                Shutdown();
            });
            TrayIcon.ContextMenu = menu;
        }

        private void PowerSuspend()
        {
            State = false;
            Thread.Sleep(200);
            Power.Suspend();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Exit += App_Exit;
            ShutdownMode = ShutdownMode.OnExplicitShutdown;
            TrayIcon = new NotifyIcon();
            TrayIcon.MouseClick += TrayIcon_MouseClick;
            State = true;
            TrayIcon.Visible = true;
            //TrayIcon.ShowBalloonTip(1000, "Keep On", "Keep Monitor Always On.", Forms.ToolTipIcon.Info);
        }

        private void TrayIcon_MouseClick(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    State = !State;
                    break;
                case MouseButtons.Middle:
                    PowerSuspend();
                    break;
            }
        }
        
        private void App_Exit(object sender, ExitEventArgs e)
        {
            TrayIcon.Visible = false;
            State = false;
        }
    }
}
