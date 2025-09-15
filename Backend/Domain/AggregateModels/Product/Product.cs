using System;

namespace Domain.AggregateModels.Product
{
    public class Product
    {
        public int Id { get; set; }                      
        public string Name { get; set; } = default!;    
        public decimal UnitPrice { get; set; }            
        public byte[]? ImageBytes { get; set; }          
        public string? Extension { get; set; }


        public Product(int id, string name, decimal unitPrice, byte[]? imageBytes = null, string? extension = null)
        {
            Id = id;
            Name = name.Trim();
            UnitPrice = decimal.Round(unitPrice, 2);
            ImageBytes = imageBytes;
            Extension = string.IsNullOrWhiteSpace(extension) ? null : extension.Trim();
        }
    }
}
