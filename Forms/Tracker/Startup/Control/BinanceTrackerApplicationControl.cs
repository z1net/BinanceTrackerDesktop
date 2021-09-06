﻿using BinanceTrackerDesktop.Core.Extension;
using BinanceTrackerDesktop.Core.UserData.API;
using BinanceTrackerDesktop.Core.Wallet;
using BinanceTrackerDesktop.Forms.API;
using ConsoleBinanceTracker.Core.Wallet.API;
using System;
using System.Threading.Tasks;

namespace BinanceTrackerDesktop.Forms.Tracker.Startup.Control
{
    public class BinanceTrackerApplicationControl
    {
        private readonly IFormControl formControl;

        private readonly IFormSafelyCloseControl formSafelyCloseControl;

        private readonly BinanceUserWallet wallet;



        public BinanceTrackerApplicationControl(IFormControl formControl, IFormSafelyCloseControl formSafelyCloseControl, BinanceUserWallet wallet)
        {
            if (formControl == null)
                throw new ArgumentNullException(nameof(formControl));

            if (formSafelyCloseControl == null)
                throw new ArgumentNullException(nameof(formSafelyCloseControl));

            if (wallet == null)
                throw new ArgumentNullException(nameof(wallet));

            this.formControl = formControl;
            this.formSafelyCloseControl = formSafelyCloseControl;
            this.wallet = wallet;

            this.formSafelyCloseControl.RegisterListener(onCloseCallbackAsync);
        }



        private async Task onCloseCallbackAsync()
        {
            this.formControl.Hide();

            BinanceUserWalletResult walletResult = await wallet.GetTotalBalanceAsync();
            BinanceUserData userData = await new BinanceUserDataReader().ReadDataAsync() as BinanceUserData;

            userData
                .With(s => s.Balance = walletResult.Value)
                .With(s => s.BestBalance = walletResult.Value, userData.BestBalance < walletResult.Value);

            await new BinanceUserDataWriter().WriteDataAsync(userData);
        }
    }
}
