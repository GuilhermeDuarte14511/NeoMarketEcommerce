using NeoMarket.Domain.Entities;

namespace NeoMarket.Application.Interfaces
{
    public interface ICartService
    {
        Cart GetCartByUserId(int userId);
        Cart CreateOrGetCart(int userId);
        bool AddItemToCart(int cartId, int productId, int quantity);
        bool RemoveItemFromCart(int cartId, int productId);
        bool ClearCart(int cartId);
        int GetCartItemCount(int cartId);
    }
}
