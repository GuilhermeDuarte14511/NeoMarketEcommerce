using Microsoft.AspNetCore.Http;
using NeoMarket.Application.Interfaces;
using NeoMarket.Domain.Entities;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace NeoMarket.Middlewares
{
    public class CartMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public CartMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
        {
            _next = next;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Invoke(HttpContext context)
        {
            int cartItemCount = 0;
            var userId = context.Session.GetInt32("UserId");

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var cartService = scope.ServiceProvider.GetRequiredService<ICartService>();

                if (userId.HasValue)
                {
                    // Usuário logado: busca o carrinho no banco
                    var cart = cartService.GetCartByUserId(userId.Value);
                    cartItemCount = cart?.Items.Sum(item => item.Quantity) ?? 0;

                    // Salvar na sessão
                    context.Session.SetInt32("CartItemCount", cartItemCount);
                }
                else
                {
                    // Usuário não autenticado: busca os itens do carrinho nos cookies
                    var cookieCart = context.Request.Cookies["GuestCartItems"];
                    var guestCartItems = !string.IsNullOrEmpty(cookieCart)
                        ? JsonSerializer.Deserialize<List<CartItem>>(cookieCart) ?? new List<CartItem>()
                        : new List<CartItem>();

                    cartItemCount = guestCartItems.Sum(item => item.Quantity);

                    // Salvar no cookie
                    context.Response.Cookies.Append("CartItemCount", cartItemCount.ToString(), new CookieOptions { Expires = DateTimeOffset.Now.AddDays(7) });
                }
            }

            await _next(context);
        }
    }
}
