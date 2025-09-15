using Application.DTOs;
using Domain.AggregateModels.Invoice;
using Domain.Exceptions;
using MediatR;


namespace Application.Queries.Invoices
{
    public class GetInvoiceByNumberQueryHandler: IRequestHandler<GetInvoiceByNumberQuery, InvoiceResponseDto>
    {
        private readonly IInvoiceFinder _invoiceFinder;

        public GetInvoiceByNumberQueryHandler(IInvoiceFinder invoicesFinder)
        {
            _invoiceFinder = invoicesFinder;
        }


        public async Task<InvoiceResponseDto> Handle(GetInvoiceByNumberQuery request, CancellationToken cancellationToken)
        {
            var invoice = await _invoiceFinder.GetByNumberAsync(request.InvoiceNumber, cancellationToken);
            if (invoice == null)
            {
                throw new EntityNotFoundException("Factura");
            };

            return invoice.ToResponseDto();
        }
    }
}
