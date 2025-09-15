using Domain.AggregateModels.Product;
using System;

namespace Application.DTOs
{
    public class ProductResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }    
        public decimal UnitPrice { get; set; }
        public byte[]? ImageBytes { get; set; }
        public string Ext { get; set; }
        public string ImageBase64 { get; set; }
    }

    public static class ProductExtension
    {
        public static ProductResponseDto ToProductResponseDto(this Product product)
        {
            return new ProductResponseDto 
            { 
                Id = product.Id, 
                Name = product.Name, 
                UnitPrice = product.UnitPrice, 
                ImageBytes = product.ImageBytes,
                Ext = product.Extension,
                ImageBase64 = product.ImageBytes is null ? string.Empty : Convert.ToBase64String(product.ImageBytes)
            };
        }
    }
}

