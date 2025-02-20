using MediatR;
using MyApp.Application.Response;

namespace MyApp.Application.Queries;

public class GetProductByIdQuery : IRequest<ProductResponse>
{
    public int Id { get; set; }
}
