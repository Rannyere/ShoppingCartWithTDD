using System;
using System.Threading.Tasks;

namespace PlanetStore.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
