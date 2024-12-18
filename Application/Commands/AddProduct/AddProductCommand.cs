using Application.Helpers;
using Domain;
using MediatR;

namespace Application.Commands.AddAuthor
{
    public class AddProductCommand : IRequest<OperationResult<string>>
    {
        public Product newProduct { get; set; }
        public AddProductCommand(Product productToAdd)
        {
            newProduct = productToAdd;
        }
    }
}
