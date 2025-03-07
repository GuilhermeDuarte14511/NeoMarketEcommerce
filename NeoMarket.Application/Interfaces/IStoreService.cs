using NeoMarket.Application.DTOs;

namespace NeoMarket.Application.Interfaces
{
    public interface IStoreService
    {
        StoreDto GetStoreDtoBySlug(string urlSlug);
    }
}
