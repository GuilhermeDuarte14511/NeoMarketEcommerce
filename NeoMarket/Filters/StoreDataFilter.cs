using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using NeoMarket.Application.DTOs;
using NeoMarket.Application.Interfaces;
using NeoMarket.Domain.Entities;
using System.Text.Json;

namespace NeoMarket.Filters
{
    public class StoreDataFilter : IActionFilter
    {
        private readonly IStoreService _storeService;
        private readonly ICartService _cartService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StoreDataFilter(IStoreService storeService, ICartService cartService, IHttpContextAccessor httpContextAccessor)
        {
            _storeService = storeService;
            _cartService = cartService;
            _httpContextAccessor = httpContextAccessor;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null) return;

            // Pega a URL slug da rota ou da sessão
            var storeUrlSlug = httpContext.Request.RouteValues["urlSlug"]?.ToString()
                                ?? httpContext.Session.GetString("StoreUrlSlug");

            if (!string.IsNullOrEmpty(storeUrlSlug))
            {
                // Verifica se os dados já estão na sessão
                var storedStoreName = httpContext.Session.GetString("StoreName");
                var storedCategories = httpContext.Session.GetString("StoreCategories");
                var storedCarouselImages = httpContext.Session.GetString("StoreCarouselImages");

                if (string.IsNullOrEmpty(storedStoreName) || string.IsNullOrEmpty(storedCategories) || string.IsNullOrEmpty(storedCarouselImages))
                {
                    var storeDto = _storeService.GetStoreDtoBySlug(storeUrlSlug);
                    if (storeDto != null)
                    {
                        // Define as informações no ViewData
                        if (context.Controller is Controller controller)
                        {
                            controller.ViewData["StoreDto"] = storeDto;
                            controller.ViewData["StoreName"] = storeDto.Name;
                            controller.ViewData["Categories"] = storeDto.Categories;
                            controller.ViewData["CarouselImages"] = storeDto.CarouselImages;
                        }

                        // Armazena os dados na sessão
                        httpContext.Session.SetString("StoreName", storeDto.Name);
                        httpContext.Session.SetString("StoreUrlSlug", storeDto.UrlSlug);

                        var categoriesJson = JsonSerializer.Serialize(storeDto.Categories);
                        httpContext.Session.SetString("StoreCategories", categoriesJson);

                        var carouselImagesJson = JsonSerializer.Serialize(storeDto.CarouselImages);
                        httpContext.Session.SetString("StoreCarouselImages", carouselImagesJson);
                    }
                }

                // Lógica do Carrinho
                int cartItemCount = 0;
                var userId = httpContext.Session.GetInt32("UserId");

                if (userId.HasValue)
                {
                    // Usuário logado: busca o carrinho do banco de dados
                    var cart = _cartService.GetCartByUserId(userId.Value);
                    cartItemCount = cart?.Items.Sum(item => item.Quantity) ?? 0;
                }
                else
                {
                    // Usuário não autenticado: busca os itens do carrinho na sessão (cookies)
                    var cookieCart = httpContext.Request.Cookies["CartItems"];
                    if (!string.IsNullOrEmpty(cookieCart))
                    {
                        var cartItems = JsonSerializer.Deserialize<List<CartItem>>(cookieCart);
                        cartItemCount = cartItems?.Sum(item => item.Quantity) ?? 0;
                    }
                }

                // Define o contador de itens do carrinho na sessão
                httpContext.Session.SetInt32("CartItemCount", cartItemCount);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Não precisa de lógica após a execução da ação
        }
    }
}
