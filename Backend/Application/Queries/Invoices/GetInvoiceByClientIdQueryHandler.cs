using Application.DTOs;
using Domain.AggregateModels.Invoice;
using Domain.Exceptions;
using MediatR;


namespace Application.Queries.Invoices
{
    public class GetInvoiceByClientIdQueryHandler: IRequestHandler<GetInvoiceByClientIdQuery, List<InvoiceResponseDto>>
    {
        private readonly IInvoiceFinder _invoicesFinder;

        public GetInvoiceByClientIdQueryHandler(IInvoiceFinder invoicesFinder)
        {
            _invoicesFinder = invoicesFinder;
        }

        public async Task<List<InvoiceResponseDto>> Handle(GetInvoiceByClientIdQuery request, CancellationToken cancellationToken)
        {
            var invoices = await _invoicesFinder.GetByClientIdAsync(request.ClientId, cancellationToken);
            if (invoices == null) {
                throw new EntityNotFoundException("Factura");
            };

            return invoices.Select(i => i.ToResponseDto()).ToList();
        }
    }
}
