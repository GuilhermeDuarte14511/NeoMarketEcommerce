﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using NeoMarket.Application.Interfaces;
using NeoMarket.Domain.Entities;
using NeoMarket.Application.DTOs.Cart;
using NeoMarket.Application.Services;
using System.Text.Json;

namespace NeoMarket.Controllers
{
    [Route("[controller]")]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IProductService _productService;
        private readonly IMelhorEnvioService _melhorEnvioService;

        public CartController(ICartService cartService, IProductService productService, IMelhorEnvioService melhorEnvioService)
        {
            _cartService = cartService;
            _productService = productService;
            _melhorEnvioService = melhorEnvioService;
        }


        [HttpPost("AddToCart")]
        public IActionResult AddToCart(int productId, int quantity = 1)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            int itemCount = 0;

            if (userId.HasValue)
            {
                var cart = _cartService.CreateOrGetCart(userId.Value);
                bool success = _cartService.AddItemToCart(cart.Id, productId, quantity);

                if (success)
                {
                    itemCount = _cartService.GetCartItemCount(cart.Id);
                    HttpContext.Session.SetInt32("CartItemCount", itemCount);
                }
            }
            else
            {
                // Usuário não autenticado -> Salvar no Cookie
                var cartItemsJson = Request.Cookies["GuestCartItems"] ?? "[]";
                var cartItems = JsonSerializer.Deserialize<List<CartItem>>(cartItemsJson) ?? new List<CartItem>();

                var existingItem = cartItems.FirstOrDefault(item => item.ProductId == productId);
                if (existingItem != null)
                {
                    existingItem.Quantity += quantity;
                }
                else
                {
                    cartItems.Add(new CartItem { ProductId = productId, Quantity = quantity });
                }

                itemCount = cartItems.Sum(item => item.Quantity);

                // Salvar os itens no cookie
                Response.Cookies.Append("GuestCartItems", JsonSerializer.Serialize(cartItems), new CookieOptions { Expires = DateTimeOffset.Now.AddDays(7) });

                // Atualizar a contagem no cookie
                Response.Cookies.Append("CartItemCount", itemCount.ToString(), new CookieOptions { Expires = DateTimeOffset.Now.AddDays(7) });
            }

            return Json(new { success = true, itemCount });
        }


        [HttpGet("Details")]
        public IActionResult Details()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            var cartItems = new List<CartItemDto>();
            int itemCount = 0;

            if (userId.HasValue)
            {
                // Usuário autenticado: Buscar o carrinho do banco
                var cart = _cartService.GetCartByUserId(userId.Value);
                if (cart != null && cart.Items.Any())
                {
                    cartItems = cart.Items.Select(item => new CartItemDto
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity
                    }).ToList();

                    itemCount = cartItems.Sum(item => item.Quantity);
                }
            }
            else
            {
                // Usuário anônimo: Buscar os itens do carrinho do cookie
                var guestCartItemsJson = Request.Cookies["GuestCartItems"] ?? "[]";
                cartItems = JsonSerializer.Deserialize<List<CartItemDto>>(guestCartItemsJson) ?? new List<CartItemDto>();

                itemCount = cartItems.Sum(item => item.Quantity);
            }

            if (userId.HasValue)
            {
                HttpContext.Session.SetInt32("CartItemCount", itemCount);
            }
            else
            {
                Response.Cookies.Append("CartItemCount", itemCount.ToString(), new CookieOptions { Expires = DateTimeOffset.Now.AddDays(7) });
            }

            var productDetails = cartItems.Select(item =>
            {
                var product = _productService.GetProductById(item.ProductId);
                return new CartProductDto
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    ImageUrl = product.ImageUrl,
                    Price = product.Price,
                    Quantity = item.Quantity
                };
            }).ToList();

            return View("CartDetails", productDetails);
        }



        [HttpGet("/Cart/CalculateShipping")]
        public async Task<IActionResult> CalculateShipping(int productId, string cep)

        {
            var product = _productService.GetProductById(productId);

            if (product == null)
            {
                return BadRequest("Produto não encontrado.");
            }

            var shippingOptions = await _melhorEnvioService.CalculateShipping(
                cep,
                product.Weight,
                product.Width,
                product.Height,
                product.Length,
                product.Price
            );

            return Json(shippingOptions);
        }

        [HttpPost("RemoveFromCart")]
        public IActionResult RemoveFromCart(int productId)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            int itemCount = 0;

            if (userId.HasValue)
            {
                // Usuário autenticado: remover do banco
                var cart = _cartService.GetCartByUserId(userId.Value);
                if (cart != null)
                {
                    bool success = _cartService.RemoveItemFromCart(cart.Id, productId);
                    if (success)
                    {
                        itemCount = _cartService.GetCartItemCount(cart.Id);
                        HttpContext.Session.SetInt32("CartItemCount", itemCount);
                    }
                }
            }
            else
            {
                // Usuário anônimo: remover do cookie
                var cartItemsJson = Request.Cookies["GuestCartItems"] ?? "[]";
                var cartItems = JsonSerializer.Deserialize<List<CartItem>>(cartItemsJson) ?? new List<CartItem>();

                var itemToRemove = cartItems.FirstOrDefault(item => item.ProductId == productId);
                if (itemToRemove != null)
                {
                    cartItems.Remove(itemToRemove);
                }

                itemCount = cartItems.Sum(item => item.Quantity);

                // Atualizar o cookie com os itens restantes
                Response.Cookies.Append("GuestCartItems", JsonSerializer.Serialize(cartItems), new CookieOptions { Expires = DateTimeOffset.Now.AddDays(7) });

                // Atualizar o contador do carrinho no cookie
                Response.Cookies.Append("CartItemCount", itemCount.ToString(), new CookieOptions { Expires = DateTimeOffset.Now.AddDays(7) });
            }

            return Json(new { success = true, itemCount });
        }

    }
}
