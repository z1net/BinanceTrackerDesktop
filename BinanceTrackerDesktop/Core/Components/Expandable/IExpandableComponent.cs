﻿using System.Collections.Generic;

namespace BinanceTrackerDesktop.Core.Components.Expandable
{
    public interface IExpandableComponent<TAddGetRemove, TSearchArgument>
    {
        IReadOnlyCollection<TAddGetRemove> AllComponents { get; }



        void AddComponent(TAddGetRemove value);

        void AddComponents(IEnumerable<TAddGetRemove> values);

        void RemoveComponent(TAddGetRemove value);

        TAddGetRemove GetComponentAt(TSearchArgument value);
    }
}
