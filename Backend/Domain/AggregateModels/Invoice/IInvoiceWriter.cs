using System;

namespace Domain.AggregateModels.Invoice
{
    public interface IInvoiceWriter
    {
        Task<Invoice> CreateAsync(Invoice invoice, List<InvoiceDetail> invoiceDetails, CancellationToken cancellationToken = default);
    }
}
