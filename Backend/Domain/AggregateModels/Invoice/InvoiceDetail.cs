using System;

namespace Domain.AggregateModels.Invoice
{
    public class InvoiceDetail
    {
        public int InvoiceId { get; set; }
        public int ProductId { get; set; }
        public int ProductQuantity { get; set; }
        public decimal ProductUnitPrice { get; set; }
        public decimal Subtotal { get; set; }
        public string? Note { get; set; }


        public InvoiceDetail() { }

        public InvoiceDetail(int productId, int productQuantity, decimal productUnitPrice)
        {
            ProductId = productId;
            ProductQuantity = productQuantity;
            ProductUnitPrice = productUnitPrice;
            Subtotal = productUnitPrice * productQuantity;
        }
    }
}
