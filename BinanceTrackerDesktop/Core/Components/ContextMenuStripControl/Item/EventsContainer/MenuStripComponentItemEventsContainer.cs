﻿using BinanceTrackerDesktop.Core.Triggers.Events.Listener;

namespace BinanceTrackerDesktop.Core.Components.ContextMenuStripControl.Item.EventsContainer
{
    public sealed class MenuStripComponentItemEventsContainer
    {
        public readonly EventListener OnClick;



        public MenuStripComponentItemEventsContainer()
        {
            OnClick = new EventListener();
        }
    }
}
