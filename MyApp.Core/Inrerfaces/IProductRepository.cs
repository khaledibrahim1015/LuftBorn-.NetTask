using MyApp.Core.Entitties;
using MyApp.Domain.Specifications;

namespace MyApp.Core.Inrerfaces;

public interface IProductRepository : IAsyncBaseRepository<Product, int>
{
    Task<IQueryable<Product>> GetProductByNameAsync(string name);
    Task<Pagination<Product>> GetProductPaginationAsync(PaginatedSpecsParams paginatedSpecsParams);
}
