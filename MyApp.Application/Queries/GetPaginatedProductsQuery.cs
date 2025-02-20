using MediatR;
using MyApp.Application.Response;
using MyApp.Domain.Specifications;

namespace MyApp.Application.Queries;

public class GetPaginatedProductsQuery : IRequest<Pagination<ProductResponse>>
{
    public PaginatedSpecsParams _paginatedSpecsParams { get; set; }
    public GetPaginatedProductsQuery(PaginatedSpecsParams paginatedSpecsParams)
    {
        _paginatedSpecsParams = paginatedSpecsParams;
    }
}
