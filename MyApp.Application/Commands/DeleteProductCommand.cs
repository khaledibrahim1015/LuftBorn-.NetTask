using MediatR;

namespace MyApp.Application.Commands;

public class DeleteProductCommand : IRequest<Unit>
{
    public DeleteProductCommand(int id)
    {
        Id = id;
    }
    public DeleteProductCommand()
    {

    }
    public int Id { get; set; }
}