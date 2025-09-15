using Application.DTOs;
using MediatR;


namespace Application.Queries.Products
{
    public class GetProductsQuery: IRequest<List<ProductResponseDto>> {
    
        public GetProductsQuery() { }

    }
}
