using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyApp.Application.Queries;
using MyApp.Application.Response;
using MyApp.Core.Entitties;
using MyApp.Core.Inrerfaces;

namespace MyApp.Application.Handlers;

public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQuery, IEnumerable<ProductResponse>>
{
    private readonly IAsyncBaseRepository<Product, int> _baseRepository;
    private readonly ILogger<GetAllProductQueryHandler> _logger;

    public GetAllProductQueryHandler(
        IAsyncBaseRepository<Product, int> baseRepository,
        ILogger<GetAllProductQueryHandler> logger)
    {
        _baseRepository = baseRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<ProductResponse>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Handling GetAllProductQuery");

            var productsQuery = await _baseRepository.GetAllAsync();

            // Wrap the query execution in a try-catch to handle potential DB errors
            List<Product> products;
            try
            {
                products = await productsQuery.ToListAsync(cancellationToken);
            }
            catch (Exception ex) when (ex is DbUpdateException || ex is InvalidOperationException)
            {
                _logger.LogError(ex, "Database error occurred while fetching products");
                throw; // Let the global exception handler deal with it
            }

            if (!products.Any())
            {
                _logger.LogInformation("No products found");
                return Array.Empty<ProductResponse>();
            }

            _logger.LogInformation("Successfully retrieved {Count} products", products.Count);

            return products.Select(pr => new ProductResponse
            {
                Id = pr.Id,
                Name = pr.Name,
                CreatedBy = pr.CreatedBy,
                CreatedDate = pr.CreatedDate.GetValueOrDefault(),
                Description = pr.Description,
                LastModifiedBy = pr.LastModifiedBy,
                LastModifiedDate = pr.LastModifiedDate.GetValueOrDefault(),
                Price = pr.Price
            }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while processing GetAllProductQuery");
            throw; // Let the global exception handler deal with it
        }

    }
}
