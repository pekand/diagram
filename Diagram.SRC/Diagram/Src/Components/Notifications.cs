using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

/*
 
*/

namespace Diagram
{
    class Notifications
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
