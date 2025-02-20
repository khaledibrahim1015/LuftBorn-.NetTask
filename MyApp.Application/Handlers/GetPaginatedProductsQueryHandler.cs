using MediatR;
using Microsoft.Extensions.Logging;
using MyApp.Application.Queries;
using MyApp.Application.Response;
using MyApp.Core.Entitties;
using MyApp.Core.Inrerfaces;
using MyApp.Domain.Specifications;

namespace MyApp.Application.Handlers;

public class GetPaginatedProductsQueryHandler : IRequestHandler<GetPaginatedProductsQuery, Pagination<ProductResponse>>
{

    private readonly ILogger<GetPaginatedProductsQueryHandler> _logger;
    private readonly IProductRepository _productRepository;

    public GetPaginatedProductsQueryHandler(ILogger<GetPaginatedProductsQueryHandler> logger, IProductRepository productRepository)
    {
        _logger = logger;
        _productRepository = productRepository;
    }

    public async Task<Pagination<ProductResponse>> Handle(GetPaginatedProductsQuery request, CancellationToken cancellationToken)
    {
        Pagination<Product> paginatedProductslist = await _productRepository.GetProductPaginationAsync(request._paginatedSpecsParams);
        return new Pagination<ProductResponse>
        {
            Count = paginatedProductslist.Count,
            PageIndex = paginatedProductslist.PageIndex,
            PageSize = paginatedProductslist.PageSize,
            Data = paginatedProductslist.Data.Select(prd => new ProductResponse()
            {
                Id = prd.Id,
                Name = prd.Name,
                CreatedBy = prd.CreatedBy,
                CreatedDate = prd.CreatedDate.GetValueOrDefault(),
                Description = prd.Description,
                LastModifiedBy = prd.LastModifiedBy,
                LastModifiedDate = prd.LastModifiedDate.GetValueOrDefault(),
                Price = prd.Price,

            }).ToList()
        };
    }
}
