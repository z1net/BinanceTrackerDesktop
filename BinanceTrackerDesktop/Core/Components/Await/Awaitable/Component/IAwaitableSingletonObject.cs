﻿namespace BinanceTrackerDesktop.Core.Components.Await.Awaitable.Component
{
    /// <summary>
    /// For awaitable component, an a simple singleton to give a easy work with your object, without this nothing is going to be work. 
    /// <para>It set`s automatically at runtime via "Reflection".</para>
    /// </summary>
    public interface IAwaitableSingletonObject
    {
        /// <summary>
        /// Instance to the this object.
        /// </summary>
        object Instance { get; }
    }
}
