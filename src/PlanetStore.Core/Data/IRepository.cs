using System;
using PlanetStore.Core.DomainObjects;

namespace PlanetStore.Core.Data
{
    public interface IRepository<T> : IDisposable where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
