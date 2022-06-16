﻿using Binance.Net.Objects.Models.Spot;
using BinanceTrackerDesktop.Localizations.Data;
using BinanceTrackerDesktop.Notifications.Popup.Builder;
using BinanceTrackerDesktop.User.Client;
using BinanceTrackerDesktop.Views.Tracker.Menu.Base;
using CryptoExchange.Net.Objects;
using System.Text;

namespace BinanceTrackerDesktop.Views.Tracker.Menu.Items;

public sealed class TrackerMenuAPI : TrackerMenuBase
{
    public async override void OnClick()
    {
        WebCallResult<BinanceAPIKeyPermissions> result = await UserClient.BinanceClient.SpotApi.Account.GetAPIKeyPermissionsAsync();
        BinanceAPIKeyPermissions permissions = result.Data;

        new PopupBuilder()
            .WithTitle(LocalizationData.Read().ApplicationName)
            .WithMessage(new StringBuilder()
                             .Append($"{nameof(permissions.IpRestrict)} = {permissions.IpRestrict}, ")
                             .Append($"{nameof(permissions.EnableFutures)} = {permissions.EnableFutures}, ")
                             .Append($"{nameof(permissions.EnableWithdrawals)} = {permissions.EnableWithdrawals}, ")
                             .Append($"{nameof(permissions.EnableMargin)} = {permissions.EnableMargin}, ")
                             .Append($"{nameof(permissions.EnableVanillaOptions)} = {permissions.EnableVanillaOptions}, ")
                             .Append($"{nameof(permissions.EnableSpotAndMarginTrading)} = {permissions.EnableSpotAndMarginTrading}, ")
                             .Append($"{nameof(permissions.EnableInternalTransfer)} = {permissions.EnableInternalTransfer}, ")
                             .Append($"{nameof(permissions.EnableReading)} = {permissions.EnableReading}, ")
                             .Append($"{nameof(permissions.PermitsUniversalTransfer)} = {permissions.PermitsUniversalTransfer}, ")
                             .Append($"{nameof(permissions.TradingAuthorityExpirationTime)} = {permissions.TradingAuthorityExpirationTime}"))
            .BuildToMessageBox();
    }



    protected override ToolStripMenuItem InitializeToolStripMenuItem()
    {
        return new ToolStripMenuItem(LocalizationData.Read().API);
    }
}
