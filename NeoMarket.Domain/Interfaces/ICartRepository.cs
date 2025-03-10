using NeoMarket.Domain.Entities;
using System.Collections.Generic;

namespace NeoMarket.Domain.Interfaces
{
    public interface ICartRepository
    {
        Cart GetById(int cartId);
        Cart GetCartByUserId(int userId);
        Cart Create(Cart cart);
        bool AddCartItem(CartItem cartItem);
        bool UpdateCartItem(CartItem cartItem);
        bool RemoveCartItem(CartItem cartItem);
        bool ClearCartItems(int cartId);
        CartItem GetCartItem(int cartId, int productId);
        int GetCartItemCount(int cartId);
    }
}
