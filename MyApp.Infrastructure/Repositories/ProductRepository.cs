using Microsoft.EntityFrameworkCore;
using MyApp.Core.Entitties;
using MyApp.Core.Inrerfaces;
using MyApp.Domain.Specifications;
using MyApp.Infrastructure.Data;

namespace MyApp.Infrastructure.Repositories;

public class ProductRepository : RepositoryBase<Product, int>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context)
    {
    }

    public Task<IQueryable<Product>> GetProductByNameAsync(string name)
    {
        return Task.FromResult(_context.Products.Where(p => p.Name == name).AsQueryable());
    }

    public async Task<Pagination<Product>> GetProductPaginationAsync(PaginatedSpecsParams paginatedSpecsParams)
    {
        var query = GetWhereAsync(prd => prd.Name.ToLower().Contains(paginatedSpecsParams.Search.Trim().ToLower()));
        var totalItems = await query.CountAsync();
        var products = await query
                        .OrderByDescending(x => x.Id)
                        .Skip((paginatedSpecsParams.PageIndex - 1) * paginatedSpecsParams.PageSize)
                        .Take(paginatedSpecsParams.PageSize)
                        .ToListAsync();
        return new Pagination<Product>
        {
            Data = products,
            PageIndex = paginatedSpecsParams.PageIndex,
            PageSize = paginatedSpecsParams.PageSize,
            Count = totalItems
        };
    }
}
