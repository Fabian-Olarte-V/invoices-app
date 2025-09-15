using System;

namespace Domain.AggregateModels.Invoice
{
    public class Invoice
    {
        public int Id { get; set; }
        public int InvoiceNumber { get; set; }
        public int ClientId { get; set; }
        public int ItemsCount { get; set; }
        public DateTime DateTime { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }


        public Invoice() { }

        public Invoice(int invoiceNumber, int clientId, List<InvoiceDetail> items)
        {
            decimal subtotalPrice = items.Sum(i => i.ProductUnitPrice * i.ProductQuantity);
            decimal tax = subtotalPrice * 0.19m;
            decimal total = subtotalPrice + tax;


            InvoiceNumber = invoiceNumber;
            ClientId = clientId;
            ItemsCount = items.Sum(i => i.ProductQuantity);
            DateTime = DateTime.Now;
            Subtotal = subtotalPrice;
            Tax = tax;
            Total = total;
        }

        public Invoice(int id, int invoiceNumber, int clientId, int itemsCount, DateTime dateTime, decimal subtotal, decimal tax, decimal total)
        {
            Id = id;
            InvoiceNumber = invoiceNumber;
            ClientId = clientId;
            ItemsCount = itemsCount;
            DateTime = dateTime;
            Subtotal = subtotal;
            Tax = tax;
            Total = total;
        }
    }
}
