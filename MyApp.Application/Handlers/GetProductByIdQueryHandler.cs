using MediatR;
using Microsoft.Extensions.Logging;
using MyApp.Application.Exceptions;
using MyApp.Application.Queries;
using MyApp.Application.Response;
using MyApp.Core.Entitties;
using MyApp.Core.Inrerfaces;

namespace MyApp.Application.Handlers;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductResponse>
{
    private readonly IAsyncBaseRepository<Product, int> _baseRepository;
    private readonly ILogger<GetProductByIdQueryHandler> _logger;

    public GetProductByIdQueryHandler(IAsyncBaseRepository<Product, int> baseRepository, ILogger<GetProductByIdQueryHandler> logger)
    {
        _baseRepository = baseRepository;
        _logger = logger;
    }

    public async Task<ProductResponse> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetProductByIdQuery for Id: {Id}", request.Id);

        Product product = await _baseRepository.GetByIdAsync(request.Id);
        if (product == null)
        {
            _logger.LogWarning("Product with Id {Id} not found", request.Id);
            throw new NotFoundException($"Product with Id {request.Id} not found.", request.Id);
        }

        _logger.LogInformation("Product with Id {Id} found", request.Id);

        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            CreatedBy = product.CreatedBy,
            CreatedDate = product.CreatedDate.GetValueOrDefault(),
            LastModifiedBy = product.LastModifiedBy,
            LastModifiedDate = product.LastModifiedDate.GetValueOrDefault()
        };
    }
}

