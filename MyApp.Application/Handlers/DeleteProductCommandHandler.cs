using MediatR;
using Microsoft.Extensions.Logging;
using MyApp.Application.Commands;
using MyApp.Application.Exceptions;
using MyApp.Core.Entitties;
using MyApp.Core.Inrerfaces;

namespace MyApp.Application.Handlers;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Unit>
{
    private readonly IAsyncBaseRepository<Product, int> _baseRepository;
    private readonly ILogger<DeleteProductCommandHandler> _logger;

    public DeleteProductCommandHandler(
                IAsyncBaseRepository<Product, int> baseRepository,
                ILogger<DeleteProductCommandHandler> logger)
    {
        _baseRepository = baseRepository;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var productToDelete = await _baseRepository.GetByIdAsync(request.Id);
        if (productToDelete != null)
        {
            await _baseRepository.DeleteAsync(productToDelete.Id);
            _logger.LogInformation($"Order with id: {productToDelete.Id} is successfuly deleted ! ");
            return Unit.Value;
        }
        throw new NotFoundException(typeof(Product).Name, request.Id);
    }
}
