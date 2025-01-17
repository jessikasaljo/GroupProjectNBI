using Application.Helpers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.ProductDetailCommands.DeleteProductDetail
{
    public class DeleteDetailInformationCommand : IRequest<OperationResult<string>>
    {
        public int detailInformationId { get; set; }
        
        public DeleteDetailInformationCommand(int _detailInformationId)
        {
            detailInformationId = _detailInformationId;
        }
    }
}
