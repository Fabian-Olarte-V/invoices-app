using System;

namespace Domain.AggregateModels.Product
{
    public interface IProductFinder
    {
        Task<List<Product>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
