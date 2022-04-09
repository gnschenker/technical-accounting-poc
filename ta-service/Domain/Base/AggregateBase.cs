using System.Collections.Generic;

namespace TechnicalAccounting.Domain
{
  public abstract class AggregateBase<TID, TState> : IAggregate<TID>
        where TState : IState<TID>
    {
        private readonly IList<object> _uncommitedEvents = new List<object>();
        protected TState State;

        protected AggregateBase(TState state)
        {
            State = state;
        }

        protected void AddUncommitedEvent(object e)
        {
            _uncommitedEvents.Add(e);
        }

        protected void Apply(object e)
        {
            _uncommitedEvents.Add(e);
            State.Modify(e);
        }

        IEnumerable<object> IAggregate<TID>.GetUncommittedEvents()
        {
            return _uncommitedEvents;
        }

        void IAggregate<TID>.ClearUncommittedEvents()
        {
            _uncommitedEvents.Clear();
        }

        TID IAggregate<TID>.Id { get { return State.Id; } }
        int IAggregate<TID>.Version { get { return State.Version; } }
    }
}