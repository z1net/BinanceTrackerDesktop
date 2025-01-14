﻿using BinanceTrackerDesktop.Core.ComponentControl.LabelControl;
using BinanceTrackerDesktop.Core.Components.ButtonControl;
using BinanceTrackerDesktop.Core.Components.ButtonControl.Extension;
using BinanceTrackerDesktop.Core.Components.Safely;
using BinanceTrackerDesktop.Core.User.Control;
using BinanceTrackerDesktop.Core.User.Data;
using BinanceTrackerDesktop.Core.User.Data.Builder;
using BinanceTrackerDesktop.Core.User.Data.Extension;
using BinanceTrackerDesktop.Core.User.Data.Save;
using BinanceTrackerDesktop.Core.User.Data.Save.Binary;
using BinanceTrackerDesktop.Core.User.Status.Result;

namespace BinanceTrackerDesktop.Core.Forms.Tracker.UI.Balance
{
    public class BinanceTrackerUserBalanceControlUI
    {
        private ISafelyComponentControl formSafelyCloseControl;

        private readonly IUserStatus userStatus;

        private readonly ButtonComponentControl[] formButtonControls;

        private readonly LabelComponentControl[] formTextControls;

        private bool isBalancesHiden;



        public BinanceTrackerUserBalanceControlUI(ISafelyComponentControl formSafelyCloseControl, IUserStatus userStatus, ButtonComponentControl[] formButtonControls, LabelComponentControl[] formTextControls)
        {
            if (formSafelyCloseControl == null)
                throw new ArgumentNullException(nameof(formSafelyCloseControl));

            if (userStatus == null)
                throw new ArgumentNullException(nameof(userStatus));

            if (formButtonControls == null)
                throw new ArgumentNullException(nameof(formButtonControls));

            if (formButtonControls.Any() == false)
                throw new InvalidOperationException();

            if (formTextControls == null)
                throw new ArgumentNullException(nameof(formButtonControls));

            if (formTextControls.Any() == false)
                throw new InvalidOperationException();

            this.formSafelyCloseControl = formSafelyCloseControl;
            this.userStatus = userStatus;
            this.formButtonControls = formButtonControls;
            this.formTextControls = formTextControls;

            formTextControls[0].SetDefaultTextColor(Color.Black);
            formTextControls[1].SetDefaultTextColor(Color.Gray);

            setTextsInitializing();
            initializeAsync();

            this.formSafelyCloseControl.RegisterListener(onCloseCallbackAsync);
            this.formButtonControls[0].EventsContainer.ClickEventListener.OnTriggerEventHandler += onRefreshTotalBalanceButtonClicked;
            this.formTextControls[0].EventsContainer.ClickEventListener.OnTriggerEventHandler += onTextClicked;
            this.formTextControls[1].EventsContainer.ClickEventListener.OnTriggerEventHandler += onTextClicked;
        }



        private async void initializeAsync()
        {
            UserData data = new BinaryUserDataSaveSystem().Read();
            isBalancesHiden = data.IsBalancesHiden ?? default(bool);

            if (isBalancesHiden)
            {
                setTextsHiden();
            }
            else
            {
                await refreshBalancesFixedAsync();
            }

            await Task.CompletedTask;
        }

        private async Task refreshBalancesFixedAsync(bool lockButton = true)
        {
            if (isBalancesHiden == false)
            {
                setTextsInitializing();

                if (lockButton)
                {
                    await refreshBalancesSyncAsync(() => formButtonControls[0].LockButton(), () => formButtonControls[0].UnlockButton());
                }
                else
                {
                    await refreshBalancesSyncAsync();
                }
            }

            await Task.CompletedTask;
        }

        private async Task refreshBalanceAsync()
        {
            IUserStatusResult totalBalanceResult = await userStatus.CalculateUserTotalBalanceAsync();
            formTextControls[0].SetText(userStatus.Format((decimal)totalBalanceResult.Value));

            await Task.CompletedTask;
        }

        private async Task refreshBalanceLossesAsync()
        {
            UserData data = new BinaryUserDataSaveSystem().Read();
            
            IUserStatusResult balanceTotalResult = await userStatus.CalculateUserTotalBalanceAsync();
            IUserStatusResult balanceLossesResult = await userStatus.CalculateUserBalanceLossesAsync();

            formTextControls[1].SetText(userStatus.Format((decimal)balanceLossesResult.Value), getColorFromBalanceLosses((decimal)balanceTotalResult.Value, data.BestBalance));

            await Task.CompletedTask;
        }

        private async Task refreshBalancesSyncAsync(Action onStartedCallback = null, Action onCompletedCallback = null)
        {
            onStartedCallback?.Invoke();

            await refreshBalanceAsync();
            await refreshBalanceLossesAsync();

            onCompletedCallback?.Invoke();

            await Task.CompletedTask;
        }

        private void setTextsHiden()
        {
            for (int i = 0; i < formTextControls.Length; i++)
            {
                formTextControls[i].SetText(BinanceTrackerBalanceTextValues.Hiden);
                formTextControls[i].SetForegroundColor(Color.Black);
            }
        }

        private void setTextsInitializing()
        {
            for (int i = 0; i < formTextControls.Length; i++)
            {
                formTextControls[i].SetText(BinanceTrackerBalanceTextValues.Initializing, formTextControls[i].GetDefaultTextColor());
            }
        }

        private Color getColorFromBalanceLosses(decimal totalBalance, decimal bestBalance)
        {
            if (bestBalance == decimal.Zero)
                return Color.Gray;
            else if (bestBalance > totalBalance)
                return Color.Red;
            else if (bestBalance < totalBalance)
                return Color.Green;
            else
                return Color.Gray;
        }



        private async void onRefreshTotalBalanceButtonClicked(EventArgs e)
        {
            await refreshBalancesFixedAsync();
        }

        private async void onTextClicked(EventArgs e)
        {
            isBalancesHiden = !isBalancesHiden;
            if (isBalancesHiden)
            {
                setTextsHiden();
            }
            else
            {
                await refreshBalancesFixedAsync();
            }

            new UserDataBuilder()
                .ReadExistingUserDataAndCacheAll(new BinaryUserDataSaveSystem())
                .AddBalancesStateBasedOnData(isBalancesHiden)
                .GetLastUsedSaveSystem(out IUserDataSaveSystem saveSystem)
                .Build()
                .WriteUserData(saveSystem);
        }

        private async Task onCloseCallbackAsync()
        {
            formButtonControls[0].EventsContainer.ClickEventListener.OnTriggerEventHandler -= onRefreshTotalBalanceButtonClicked;
            formTextControls[0].EventsContainer.ClickEventListener.OnTriggerEventHandler -= onTextClicked;
            formTextControls[1].EventsContainer.ClickEventListener.OnTriggerEventHandler -= onTextClicked;

            await Task.CompletedTask;
        }
    }

    public class BinanceTrackerBalanceTextValues
    {
        public const string Initializing = "-----";

        public const string Hiden = "*****";
    }
}