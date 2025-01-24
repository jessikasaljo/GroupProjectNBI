using Application.DTOs.Product;
using Application.Helpers;
using MediatR;

namespace Application.Commands.ProductDetailCommands.UpdateProductDetail
{
    public class UpdateDetailInformationCommand : IRequest<OperationResult<string>>
    {
        public int detailInformationId { get; set; }
        public ProductDetailDTO updatedProductDetail { get; set; }
        public UpdateDetailInformationCommand(int _detailInformationId, ProductDetailDTO productDetailToUpdate)
        {
            detailInformationId = _detailInformationId;
            updatedProductDetail = productDetailToUpdate;
        }
    }
}
