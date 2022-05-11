﻿namespace BinanceTrackerDesktop.Core.Components.Await.Awaitable.Component
{
    /// <summary>
    /// For awaitable component, an a interface that giving a special method when application closing is completed.
    /// <para>It executes automatically at runtime via <see langword="Reflection"/></para>
    /// </summary>
    public interface IAwaitableComponentComplete
    {
        /// <summary>
        /// Execute`s when application closing is completed.
        /// </summary>
        void OnComplete();
    }
}
