using Application.DTOs;
using Domain.AggregateModels.Invoice;
using MediatR;


namespace Application.Commands.Invoices
{
    public class CreateInvoiceCommandHandler: IRequestHandler<CreateInvoiceCommand, InvoiceResponseDto>
    {
        private readonly IInvoiceWriter _invoiceWriter;

        public CreateInvoiceCommandHandler (IInvoiceWriter invoiceWriter)
        {
            _invoiceWriter = invoiceWriter;
        }

        public async Task<InvoiceResponseDto> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
        {
            List<InvoiceDetail> invoiceDetails = request.InvoiceData.Items
                .Select(i => new InvoiceDetail(i.ProductId, i.Quantity, i.UnitPrice))
                .ToList();


            var invoice = new Invoice(request.InvoiceData.InvoiceNumber, request.InvoiceData.ClientId, invoiceDetails);
            
            var result = await _invoiceWriter.CreateAsync(invoice, invoiceDetails, cancellationToken);
            if (result == null) return null;

            return result.ToResponseDto();
        }
    }
}
