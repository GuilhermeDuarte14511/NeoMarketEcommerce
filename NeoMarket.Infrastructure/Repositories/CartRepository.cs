using NeoMarket.Domain.Entities;
using NeoMarket.Domain.Interfaces;
using NeoMarket.Infrastructure.Data;
using System;
using System.Linq;

namespace NeoMarket.Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _context;

        public CartRepository(AppDbContext context)
        {
            _context = context;
        }

        public Cart GetById(int cartId)
        {
            return _context.Carts
                .Where(c => c.Id == cartId)
                .Select(c => new Cart
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    Items = c.Items.Select(i => new CartItem
                    {
                        Id = i.Id,
                        CartId = i.CartId,
                        ProductId = i.ProductId,
                        Quantity = i.Quantity,
                        CreatedAt = i.CreatedAt
                    }).ToList(),
                    CreatedAt = c.CreatedAt
                })
                .FirstOrDefault();
        }

        public Cart GetCartByUserId(int userId)
        {
            return _context.Carts
                .Where(c => c.UserId == userId)
                .Select(c => new Cart
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    Items = c.Items.Select(i => new CartItem
                    {
                        Id = i.Id,
                        CartId = i.CartId,
                        ProductId = i.ProductId,
                        Quantity = i.Quantity,
                        CreatedAt = i.CreatedAt
                    }).ToList(),
                    CreatedAt = c.CreatedAt
                })
                .FirstOrDefault();
        }

        public Cart Create(Cart cart)
        {
            _context.Carts.Add(cart);
            _context.SaveChanges();
            return cart;
        }

        public bool AddCartItem(CartItem cartItem)
        {
            _context.CartItems.Add(cartItem);
            return _context.SaveChanges() > 0;
        }

        public bool UpdateCartItem(CartItem cartItem)
        {
            var existingItem = _context.CartItems.Find(cartItem.Id);
            if (existingItem == null) return false;

            existingItem.Quantity = cartItem.Quantity;
            _context.CartItems.Update(existingItem);
            return _context.SaveChanges() > 0;
        }

        public bool RemoveCartItem(CartItem cartItem)
        {
            _context.CartItems.Remove(cartItem);
            return _context.SaveChanges() > 0;
        }

        public bool ClearCartItems(int cartId)
        {
            var items = _context.CartItems.Where(i => i.CartId == cartId).ToList();
            if (!items.Any()) return false;

            _context.CartItems.RemoveRange(items);
            return _context.SaveChanges() > 0;
        }

        public CartItem GetCartItem(int cartId, int productId)
        {
            return _context.CartItems.FirstOrDefault(i => i.CartId == cartId && i.ProductId == productId);
        }

        public int GetCartItemCount(int cartId)
        {
            return _context.CartItems.Where(i => i.CartId == cartId).Sum(i => i.Quantity);
        }
    }
}
