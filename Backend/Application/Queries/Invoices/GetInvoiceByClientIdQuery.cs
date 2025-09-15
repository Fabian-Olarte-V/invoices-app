using Application.DTOs;
using FluentValidation;
using MediatR;


namespace Application.Queries.Invoices
{
    public class GetInvoiceByClientIdQuery: IRequest<List<InvoiceResponseDto>>
    {
        public int ClientId { get; set; }

        public GetInvoiceByClientIdQuery(int clientId)
        {
            ClientId = clientId;
        }
    }

    public class GetInvoiceByClientIdQueryValidator : AbstractValidator<GetInvoiceByClientIdQuery>
    {
        public GetInvoiceByClientIdQueryValidator()
        {
            RuleFor(x => x.ClientId)
                .NotEmpty()
                .WithMessage("El ID del cliente es requerido")
                .GreaterThan(0)
                .WithMessage("El ID del cliente debe ser mayor que 0");
        }
    }
}
