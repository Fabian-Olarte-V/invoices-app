using Application.DTOs;
using FluentValidation;
using MediatR;


namespace Application.Commands.Invoices
{
    public class CreateInvoiceCommand: IRequest<InvoiceResponseDto>
    {
        public CreateInvoiceRequestDto InvoiceData {  get; set; }

        public CreateInvoiceCommand(CreateInvoiceRequestDto invoiceData)
        {
            InvoiceData = invoiceData;
        }
    }

    public class InvoiceItemRequestDtoValidator : AbstractValidator<InvoiceItemRequestDto>
    {
        public InvoiceItemRequestDtoValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty()
                .WithMessage("El ID del producto es requerido")
                .GreaterThan(0)
                .WithMessage("El ID del producto debe ser mayor que 0");

            RuleFor(x => x.Quantity)
                .NotEmpty()
                .WithMessage("La cantidad es requerida")
                .GreaterThan(0)
                .WithMessage("La cantidad debe ser mayor que 0");

            RuleFor(x => x.UnitPrice)
                .NotEmpty()
                .WithMessage("El precio es requerido")
                .GreaterThan(0)
                .WithMessage("La cantidad debe ser mayor que 0");
        }
    }

    public class CreateInvoiceRequestDtoValidator : AbstractValidator<CreateInvoiceRequestDto>
    {
        public CreateInvoiceRequestDtoValidator()
        {
            RuleFor(x => x.ClientId)
                .NotEmpty()
                .WithMessage("El ID del cliente es requerido")
                .GreaterThan(0)
                .WithMessage("El ID del cliente debe ser mayor que 0");

            RuleFor(x => x.InvoiceNumber)
                .NotEmpty()
                .WithMessage("El número de factura es requerido")
                .GreaterThan(0)
                .WithMessage("El número de factura debe ser mayor que 0");

            RuleFor(x => x.Items)
                .NotEmpty()
                .WithMessage("La factura debe contener al menos un item");

            RuleForEach(x => x.Items)
                .SetValidator(new InvoiceItemRequestDtoValidator());
        }
    }

    public class CreateInvoiceCommandValidator : AbstractValidator<CreateInvoiceCommand>
    {
        public CreateInvoiceCommandValidator()
        {
            RuleFor(x => x.InvoiceData)
                .NotNull()
                .WithMessage("Los datos de la factura son requeridos")
                .SetValidator(new CreateInvoiceRequestDtoValidator());
        }
    }
}
