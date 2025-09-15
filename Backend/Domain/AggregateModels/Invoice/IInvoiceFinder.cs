using System;

namespace Domain.AggregateModels.Invoice
{
    public interface IInvoiceFinder
    {
        Task<List<Invoice>> GetByClientIdAsync(int clientId, CancellationToken cancellationToken = default);

        Task<Invoice> GetByNumberAsync(int invoiceNumber, CancellationToken cancellationToken = default);
    }
}
