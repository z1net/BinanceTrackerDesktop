﻿using BinanceTrackerDesktop.Core.Forms.Authorization;
using BinanceTrackerDesktop.Core.User.Data.Save.Binary;
using BinanceTrackerDesktop.Core.Window.Extension;
using BinanceTrackerDesktop.Tracker.Forms;
using System.Diagnostics;

namespace BinanceTrackerDesktop.Core.Entry
{
    public sealed class BinanceTrackerEntryPoint
    {
        public BinanceTrackerEntryPoint()
        {
            if (Process.GetCurrentProcess().TryGetArleadyStartedSimilarProcess(out Process anotherProcess))
            {
                anotherProcess.SetProcessWindowToForeground();
                return;
            }

            if (new BinaryUserDataSaveSystem().Read() == null)
            {
                Application.Run(new BinanceTrackerAuthorizationForm());
            }
            else
            {
                Application.Run(new BinanceTrackerForm());
            }
        }
    }
}
