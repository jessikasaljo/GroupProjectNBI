using Application.DTOs;
using Application.Helpers;
using Domain;
using MediatR;

namespace Application.Commands.AddAuthor
{
    public class AddProductCommand : IRequest<OperationResult<string>>
    {
        public AddProductDTO newProduct { get; set; }
        public AddProductCommand(AddProductDTO productToAdd)
        {
            newProduct = productToAdd;
        }
    }
}
