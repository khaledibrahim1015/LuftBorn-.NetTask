
using MediatR;
using Microsoft.Extensions.Logging;
using MyApp.Application.Commands;
using MyApp.Core.Entitties;
using MyApp.Core.Inrerfaces;

namespace MyApp.Application.Handlers;

internal class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
{
    private readonly IAsyncBaseRepository<Product, int> _baseRepository;
    private readonly ILogger<UpdateProductCommandHandler> _logger;

    public UpdateProductCommandHandler(IAsyncBaseRepository<Product, int> baseRepository, ILogger<UpdateProductCommandHandler> logger)
    {
        _baseRepository = baseRepository;
        _logger = logger;
    }

    public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling UpdateProductCommand for ProductId: {ProductId}", request.Id);

        var product = await _baseRepository.GetByIdAsync(request.Id);
        if (product == null)
        {
            _logger.LogWarning("Product with Id: {ProductId} not found", request.Id);
            return false;
        }

        product.Name = request.Name;
        product.Price = request.Price;
        product.Description = request.Description;

        await _baseRepository.UpdateAsync(product);
        _logger.LogInformation("Product with Id: {ProductId} updated successfully", request.Id);
        return true;
    }
}
