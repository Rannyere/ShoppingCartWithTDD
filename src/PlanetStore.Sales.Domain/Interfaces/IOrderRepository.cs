using System;
using PlanetStore.Core.Data;

namespace PlanetStore.Sales.Domain.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        void Add(Order order);
    }
}
