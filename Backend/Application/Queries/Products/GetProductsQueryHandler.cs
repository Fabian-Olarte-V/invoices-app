using Application.Common.Exceptions;
using Application.DTOs;
using Domain.AggregateModels.Product;
using MediatR;

namespace Application.Queries.Products
{
    public class GetProductsQueryHandler: IRequestHandler<GetProductsQuery, List<ProductResponseDto>>
    {
        private readonly IProductFinder _productsFinder;

        public GetProductsQueryHandler (IProductFinder productsFinder)
        {
            _productsFinder = productsFinder;
        }

        public async Task<List<ProductResponseDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _productsFinder.GetAllAsync(cancellationToken);
            if (products == null) {
                throw new EntityNotFoundException("Producto");
            }

            return products.Select(p => p.ToProductResponseDto()).ToList();     
        }
    }
}
