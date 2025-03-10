using Microsoft.Extensions.Configuration;
using NeoMarket.Application.DTOs.MelhorEnvioAPI;
using NeoMarket.Application.Interfaces;
using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;

public class MelhorEnvioService : IMelhorEnvioService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public MelhorEnvioService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<IEnumerable<ShippingOptionDto>> CalculateShipping(
    string destinationCep,
    decimal weight,
    decimal width,
    decimal height,
    decimal length,
    decimal price)
    {
        // Carregar as configurações do MelhorEnvio
        var melhorEnvioConfig = _configuration.GetSection("MelhorEnvio");

        string baseUrl = melhorEnvioConfig["BaseUrl"];
        string bearerToken = melhorEnvioConfig["BearerToken"];

        // Definir o cabeçalho de autorização com o Bearer Token
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

        // Definir o corpo da requisição
        var request = new
        {
            from = new { postal_code = "06864040" },
            to = new { postal_code = destinationCep },
            products = new[] {
            new {
                id = "1",
                width = width,
                height = height,
                length = length,
                weight = weight,
                insurance_value = price,
                quantity = 1
            }
        },
            options = new { receipt = false, own_hand = false },
            services = "1,2,3,4,17,18"
        };

        _httpClient.BaseAddress = new Uri(baseUrl);
        var response = await _httpClient.PostAsync("/api/v2/me/shipment/calculate",
            new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"));

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Falha ao calcular o frete com o Melhor Envio. Erro: {errorContent}");
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        var shippingOptions = JsonSerializer.Deserialize<List<ShippingOptionDto>>(responseContent);

        return shippingOptions;
    }
}