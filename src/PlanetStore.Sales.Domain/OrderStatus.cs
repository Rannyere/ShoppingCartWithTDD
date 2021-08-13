using System;
namespace PlanetStore.Sales.Domain
{
    public enum OrderStatus
    {
        draft = 0,
        Authorized = 1,
        Paid = 2,
        Declined = 3,
        Delivered = 4,
        Canceled = 5
    }
}
