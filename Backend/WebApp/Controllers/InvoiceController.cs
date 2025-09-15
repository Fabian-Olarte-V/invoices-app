using Application.Commands.Invoices;
using Application.Queries.Invoices;
using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoicesController : Controller
    {
        private readonly IMediator _mediator;

        public InvoicesController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet("{number}")]
        public async Task<ActionResult<InvoiceResponseDto>> GetByNumber(int number)
        {
            var result = await _mediator.Send(new GetInvoiceByNumberQuery(number));
            return Ok(result);
        }

        [HttpGet("by-client/{clientId}")]
        public async Task<ActionResult<InvoiceResponseDto>> GetByClientId(int clientId)
        {
            var result = await _mediator.Send(new GetInvoiceByClientIdQuery(clientId));
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<InvoiceResponseDto>> Create(CreateInvoiceCommand request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }
    }
}
