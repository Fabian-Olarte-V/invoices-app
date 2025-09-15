using Application.Queries.Clients;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : Controller
    {
        private readonly IMediator _mediator;

        public ClientsController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            var clients = await _mediator.Send(new GetClientsQuery());
            return Ok(clients);
        }
    }
}
