using MediatR;

namespace MyApp.Application.Commands
{
    public class UpdateProductCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public string Description { get; set; }

        // Parameterless constructor for deserialization
        public UpdateProductCommand() { }
        public UpdateProductCommand(int productId, string name, decimal price, string description)
        {
            Id = productId;
            Name = name;
            Price = price;
            Description = description;
        }
    }
}
