using System;

namespace Domain.AggregateModels.Client
{
    public interface IClientFinder
    {
        Task<List<Client>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
