﻿using BinanceTrackerDesktop.Core.UserData.API;
using BinanceTrackerDesktop.Forms.SystemTray.API;
using BinanceTrackerDesktop.Forms.SystemTray.Tray;
using BinanceTrackerDesktop.Forms.Tracker.API;
using BinanceTrackerDesktop.Forms.Tracker.Notifications;
using BinanceTrackerDesktop.Forms.Tracker.Notifications.API;
using System;
using System.Windows.Forms;

namespace BinanceTrackerDesktop.Forms.SystemTray
{
    public partial class BinanceTrackerSystemTrayForm : Form, ISystemTrayFormControl
    {
        private readonly IFormEventListener[] formEventListeners;

        private readonly IFormControl control;

        private readonly ISystemTrayFormControl trayControl;



        public NotifyIcon Notify => this.NotifyIcon;



        public BinanceTrackerSystemTrayForm(IFormControl control)
        {
            InitializeComponent();

            if (control == null)
                throw new ArgumentNullException(nameof(control));

            trayControl = this;

            NotifyIcon.ContextMenuStrip = Tray;
            NotifyIcon.Text = TrayDataContainer.ApplicationName;
            NotifyIcon.DoubleClick += (s, e) => formEventListeners[0].TriggerEvent(s, e);

            new BinanceTrackerTray(this.control = control, this, this, new BinanceTrackerNotificationsControl(new StableNotificationsControl(NotifyIcon)), 
            formEventListeners = new IFormEventListener[]
            {
                new FormEventListener(),
                new FormEventListener(),
                new FormEventListener(),
                new FormEventListener(),
            });

            initializeContextMenuStripAndReadUserDataAsync();

            control.FormClosing += onFormClosing;
        }

        

        private async void initializeContextMenuStripAndReadUserDataAsync()
        {
            BinanceUserData binanceUserData = await new BinanceUserDataReader().ReadDataAsync() as BinanceUserData;

            this.Tray.Items.Add(new ToolStripMenuItem(TrayDataContainer.OpenApplication, default, (s, e) => formEventListeners[1].TriggerEvent(s, e)));
            this.Tray.Items.Add(new ToolStripMenuItem(binanceUserData.NotificationsEnabled == true ? TrayDataContainer.DisableNotifications : TrayDataContainer.EnableNotifications, default, (s, e) => formEventListeners[2].TriggerEvent(s, e)));
            this.Tray.Items.Add(new ToolStripMenuItem(TrayDataContainer.QuitApplication, default, (s, e) => formEventListeners[3].TriggerEvent(s, e)));
        }



        void ISystemTrayFormControl.Close()
        {
            using (NotifyIcon)
                this.NotifyIcon.Visible = false;
        }

        void ISystemTrayFormControl.ChangeMenuItemTitle(int index, string to)
        {
            if (this.Tray.Items[index] == null)
                throw new IndexOutOfRangeException(nameof(index));

            if (string.IsNullOrEmpty(to))
                throw new ArgumentNullException(nameof(to));

            this.Tray.Items[index].Text = to;
        }



        private void onFormClosing(object sender, FormClosingEventArgs e)
        {
            this.control.FormClosing -= onFormClosing;

            trayControl.Close();
        }
    }
}