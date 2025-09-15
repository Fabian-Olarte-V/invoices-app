using Domain.AggregateModels.Invoice;


namespace Application.DTOs
{    
    public class InvoiceItemRequestDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }


    public class CreateInvoiceRequestDto
    {
        public int ClientId { get; set; }
        public int InvoiceNumber { get; set; }
        public List<InvoiceItemRequestDto> Items { get; set; } = new();
    }


    public class InvoiceResponseDto
    {
        public int Id { get; set; }
        public int InvoiceNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal Total { get; set; }
    }


    public static class InvoiceExtensions
    {
        public static InvoiceResponseDto ToResponseDto(this Invoice invoice)
        {
            return new InvoiceResponseDto
            {
                Id = invoice.Id,
                InvoiceNumber = invoice.InvoiceNumber,
                CreatedDate = invoice.DateTime,
                Total = invoice.Total
            };
        }
    }
}
