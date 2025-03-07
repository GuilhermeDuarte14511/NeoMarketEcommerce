using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using NeoMarket.Application.DTOs;
using NeoMarket.Application.Interfaces;
using System.Text.Json;

namespace NeoMarket.Filters
{
    public class StoreDataFilter : IActionFilter
    {
        private readonly IStoreService _storeService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StoreDataFilter(IStoreService storeService, IHttpContextAccessor httpContextAccessor)
        {
            _storeService = storeService;
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
            }
        }


        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Não precisa de lógica após a execução da ação
        }
    }
}
