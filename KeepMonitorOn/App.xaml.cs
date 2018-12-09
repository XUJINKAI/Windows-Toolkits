using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Forms = System.Windows.Forms;
using XJK.SysX;

namespace KeepMonitorOn
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private bool _state;
        public bool State
        {
            get => _state; set
            {
                _state = value;
                Power.KeepSystemMonitorOn(value);
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

        public Forms.NotifyIcon TrayIcon;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Exit += App_Exit;
            ShutdownMode = ShutdownMode.OnExplicitShutdown;
            TrayIcon = new Forms.NotifyIcon()
            {
                ContextMenu = new Forms.ContextMenu()
            };
            TrayIcon.ContextMenu.MenuItems.Add("Check State", (sender, arg) =>
            {
                Cmd.New("cmd", "/k powercfg /requests")
                .RunAs(XJK.SysX.CommandHelper.Privilege.Admin)
                .Excute();
            });
            TrayIcon.ContextMenu.MenuItems.Add("-");
            TrayIcon.ContextMenu.MenuItems.Add("Exit", (sender, arg) =>
            {
                App.Current.Shutdown();
            });
            TrayIcon.MouseClick += TrayIcon_MouseClick;
            State = true;
            TrayIcon.Visible = true;
            TrayIcon.ShowBalloonTip(1000, "Keep On", "Keep Monitor Always On.", Forms.ToolTipIcon.Info);
        }

        private void TrayIcon_MouseClick(object sender, Forms.MouseEventArgs e)
        {
            if(e.Button == Forms.MouseButtons.Left)
            {
                State = !State;
            }
        }
        
        private void App_Exit(object sender, ExitEventArgs e)
        {
            TrayIcon.Visible = false;
            State = false;
        }
    }
}
