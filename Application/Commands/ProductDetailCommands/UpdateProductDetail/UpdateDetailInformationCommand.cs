using MediatR;
using Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.Product;

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
