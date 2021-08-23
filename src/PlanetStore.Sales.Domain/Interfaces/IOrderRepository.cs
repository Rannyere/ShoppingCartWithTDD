using System;
namespace PlanetStore.Sales.Domain.Interfaces
{
    public interface IOrderRepository
    {
        void Add(Order order);
    }
}
