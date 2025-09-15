using Domain.AggregateModels.Client;
using Application.DTOs;
using MediatR;
using Domain.Exceptions;


namespace Application.Queries.Clients
{
    public class GetClientsQueryHandler: IRequestHandler<GetClientsQuery, List<ClientResponseDto>>
    {
        private readonly IClientFinder _clientsFinder;

        public GetClientsQueryHandler(IClientFinder clientsFinder)
        {
            _clientsFinder = clientsFinder;
        }

        public async Task<List<ClientResponseDto>> Handle(GetClientsQuery request, CancellationToken cancellationToken)
        {
            var clientList = await _clientsFinder.GetAllAsync(cancellationToken);
            if (clientList == null) {
                throw new EntityNotFoundException("Cliente");
            }

            return clientList.Select(i => i.ToClientResponseDto()).ToList();
        }
    }
}
