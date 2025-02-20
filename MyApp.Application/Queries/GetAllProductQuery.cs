using MediatR;
using MyApp.Application.Response;

namespace MyApp.Application.Queries;

public class GetAllProductQuery : IRequest<IEnumerable<ProductResponse>>
{
}


