using Domain.Models;
using MediatR;

namespace Application.Commands.StoreCommands.AddStore
{
    public class AddStoreCommand : IRequest<Store>
    {
        public string Location { get; set; }
        public AddStoreCommand(string location)
        {
            Location = location;
        }
    }

}
