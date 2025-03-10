using NeoMarket.Application.DTOs.MelhorEnvioAPI;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NeoMarket.Application.Interfaces
{
    public interface IMelhorEnvioService
    {
        Task<IEnumerable<ShippingOptionDto>> CalculateShipping(
            string destinationCep,
            decimal weight,
            decimal width,
            decimal height,
            decimal length,
            decimal price
        );
    }
}
