using System;

namespace TechnicalAccounting.Domain
{
    public interface IState
    {
        string Id { get; }
        int Version { get; }
        void Modify(object e);
    }
}