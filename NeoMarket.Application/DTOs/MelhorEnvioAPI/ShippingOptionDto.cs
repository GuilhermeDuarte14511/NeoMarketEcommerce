using System.Text.Json.Serialization;

namespace NeoMarket.Application.DTOs.MelhorEnvioAPI
{
    public class ShippingOptionDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("price")]
        public string Price { get; set; }

        [JsonPropertyName("custom_price")]
        public string CustomPrice { get; set; }

        [JsonPropertyName("error")]
        public string Error { get; set; }

        [JsonPropertyName("delivery_time")]
        public int? DeliveryTime { get; set; }

        [JsonPropertyName("delivery_range")]
        public DeliveryRange DeliveryRange { get; set; }

        [JsonPropertyName("custom_delivery_time")]
        public int? CustomDeliveryTime { get; set; }

        [JsonPropertyName("custom_delivery_range")]
        public DeliveryRange CustomDeliveryRange { get; set; }

        [JsonPropertyName("company")]
        public Company Company { get; set; }

        [JsonPropertyName("services")]
        public string Services { get; set; }

        [JsonPropertyName("discount")]
        public string Discount { get; set; }

        [JsonPropertyName("packages")]
        public List<Package> Packages { get; set; }

        [JsonPropertyName("additional_services")]
        public AdditionalServices AdditionalServices { get; set; }
    }

    public class Company
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("picture")]
        public string Picture { get; set; }
    }

    public class DeliveryRange
    {
        [JsonPropertyName("min")]
        public int Min { get; set; }

        [JsonPropertyName("max")]
        public int Max { get; set; }
    }

    public class Package
    {
        [JsonPropertyName("price")]
        public string Price { get; set; }

        [JsonPropertyName("discount")]
        public string Discount { get; set; }

        [JsonPropertyName("format")]
        public string Format { get; set; }

        [JsonPropertyName("dimensions")]
        public Dimensions Dimensions { get; set; }

        [JsonPropertyName("weight")]
        public string Weight { get; set; }

        [JsonPropertyName("insurance_value")]
        public string InsuranceValue { get; set; }

        [JsonPropertyName("products")]
        public List<Product> Products { get; set; }
    }

    public class Dimensions
    {
        [JsonPropertyName("height")]
        public int Height { get; set; }

        [JsonPropertyName("width")]
        public int Width { get; set; }

        [JsonPropertyName("length")]
        public int Length { get; set; }
    }

    public class Product
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }
    }

    public class AdditionalServices
    {
        [JsonPropertyName("receipt")]
        public bool Receipt { get; set; }

        [JsonPropertyName("own_hand")]
        public bool OwnHand { get; set; }

        [JsonPropertyName("collect")]
        public bool Collect { get; set; }
    }
}
