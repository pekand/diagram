using System;
using System.Timers;

namespace Diagram
{
    class Notifications //UID8876272772
    {
        private Timer timer = null;

        public void startNotificationChecking()
        {
            this.timer = new Timer(1000);
            this.timer.Elapsed += OnTimedEvent;
            this.timer.Start();
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {

        }

        public void ShowNotification(String message)
        {
            var notification = new System.Windows.Forms.NotifyIcon()
            {
                Visible = true,
                Icon = System.Drawing.SystemIcons.Information,
                BalloonTipText = message,
            };
            notification.ShowBalloonTip(60);
        }
    }
}
