using Application.DTOs;
using FluentValidation;
using MediatR;


namespace Application.Queries.Invoices
{
    public class GetInvoiceByNumberQuery: IRequest<InvoiceResponseDto>
    {
        public int InvoiceNumber { get; set; }

        public GetInvoiceByNumberQuery (int invoiceNumber)
        {
            InvoiceNumber = invoiceNumber;
        }
    }

    public class GetInvoiceByNumberQueryValidator : AbstractValidator<GetInvoiceByNumberQuery>
    {
        public GetInvoiceByNumberQueryValidator()
        {
            RuleFor(x => x.InvoiceNumber)
                .NotEmpty()
                .WithMessage("El número de factura es requerido")
                .GreaterThan(0)
                .WithMessage("El número de factura debe ser mayor que 0");
        }
    }
}
