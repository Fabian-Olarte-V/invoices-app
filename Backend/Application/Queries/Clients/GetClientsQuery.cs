using Application.DTOs;
using MediatR;

namespace Application.Queries.Clients
{
    public class GetClientsQuery : IRequest<List<ClientResponseDto>>
    {
        public GetClientsQuery() { }
    }
}
