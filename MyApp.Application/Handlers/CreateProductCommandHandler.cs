using MediatR;
using Microsoft.Extensions.Logging;
using MyApp.Application.Commands;
using MyApp.Application.Exceptions;
using MyApp.Core.Entitties;
using MyApp.Core.Inrerfaces;

namespace MyApp.Application.Handlers;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
{
    private readonly IAsyncBaseRepository<Product, int> _baseRepository;
    private readonly ILogger<CreateProductCommandHandler> _logger;

    public CreateProductCommandHandler(IAsyncBaseRepository<Product, int> baseRepository, ILogger<CreateProductCommandHandler> logger)
    {
        _baseRepository = baseRepository;
        _logger = logger;
    }

    public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            _logger.LogError("Product name cannot be empty");
            throw new ValidationException($"Product name cannot be empty {nameof(request.Name)}");
        }

        if (request.Price <= 0)
        {
            _logger.LogError("Product price must be greater than zero");
            throw new ValidationException($"Product price must be greater than zero {nameof(request.Price)}");
        }

        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price
        };

        await _baseRepository.AddAsync(product);

        _logger.LogInformation("Product created with ID: {ProductId}", product.Id);

        return product.Id;
    }
}
