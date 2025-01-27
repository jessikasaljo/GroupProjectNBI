using Application.Helpers;
using MediatR;

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
