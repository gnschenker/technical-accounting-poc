using System;

namespace TechnicalAccounting.Domain
{
    public interface IState<TID>
    {
        TID Id { get; }
        int Version { get; }
        void Modify(object e);
    }
}