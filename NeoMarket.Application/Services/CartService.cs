using NeoMarket.Application.Interfaces;
using NeoMarket.Domain.Entities;
using NeoMarket.Domain.Interfaces;

namespace NeoMarket.Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public Cart GetCartByUserId(int userId)
        {
            return _cartRepository.GetCartByUserId(userId);
        }

        public Cart CreateOrGetCart(int userId)
        {
            var cart = _cartRepository.GetCartByUserId(userId);
            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _cartRepository.Create(cart);
            }
            return cart;
        }

        public bool AddItemToCart(int cartId, int productId, int quantity)
        {
            var cart = _cartRepository.GetById(cartId);
            if (cart == null) return false;

            var cartItem = cart.Items.FirstOrDefault(item => item.ProductId == productId);
            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
                _cartRepository.UpdateCartItem(cartItem);
            }
            else
            {
                cartItem = new CartItem
                {
                    CartId = cartId,
                    ProductId = productId,
                    Quantity = quantity
                };
                _cartRepository.AddCartItem(cartItem);
            }
            return true;
        }

        public bool RemoveItemFromCart(int cartId, int productId)
        {
            var cartItem = _cartRepository.GetCartItem(cartId, productId);
            if (cartItem == null) return false;

            _cartRepository.RemoveCartItem(cartItem);
            return true;
        }

        public bool ClearCart(int cartId)
        {
            var cart = _cartRepository.GetById(cartId);
            if (cart == null) return false;

            _cartRepository.ClearCartItems(cartId);
            return true;
        }

        public int GetCartItemCount(int cartId)
        {
            return _cartRepository.GetCartItemCount(cartId);
        }
    }
}
