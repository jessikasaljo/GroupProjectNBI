using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.StoreCommands.AddStore
{
    public class AddStoreCommandHandler : IRequestHandler<AddStoreCommand, Store>
    {
        private readonly ApplicationDbContext _context;

        public AddStoreCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Store> Handle(AddStoreCommand request, CancellationToken cancellationToken)
        {
            var store = new Store { Location = request.Location };
            _context.Stores.Add(store);
            await _context.SaveChangesAsync(cancellationToken);
            return store;
        }
    }

}
