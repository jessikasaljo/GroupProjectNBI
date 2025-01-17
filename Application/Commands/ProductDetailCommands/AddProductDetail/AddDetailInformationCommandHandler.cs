using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Commands.UserCommands.AddUser;
using Application.Helpers;
using Domain.Models;
using Domain.RepositoryInterface;
using Microsoft.Extensions.Logging;
using Application.Interfaces;

namespace Application.Commands.ProductDetailCommands.AddProductDetail
{
    public class AddDetailInformationCommandHandler : IRequestHandler<AddDetailInformationCommand, OperationResult<string>>
    {
        private readonly IGenericRepository<DetailInformation> informationDatabase;
        private readonly IVerificationService<ProductDetail> detailService;
        private readonly ILogger<AddDetailInformationCommand> logger;
        public AddDetailInformationCommandHandler(IGenericRepository<DetailInformation> _informationDatabase, IVerificationService<ProductDetail> _detailService, ILogger<AddDetailInformationCommand> _logger)
        {
            informationDatabase = _informationDatabase;
            detailService = _detailService;
            logger = _logger;
        }
        public async Task<OperationResult<string>> Handle(AddDetailInformationCommand request, CancellationToken cancellationToken)
        {
            var newProductDetail = new DetailInformation
            {
                ProductId = request.newProductDetail.ProductId,
                Title = request.newProductDetail.Title,
                Text = request.newProductDetail.Text
            };

            bool productDetailExists = false;
            try
            {
                productDetailExists = await detailService.VerifyAsync(newProductDetail.ProductId, cancellationToken);
            }
            catch (Exception exception)
            {
                return OperationResult<string>.FailureResult($"Error occured {exception}", logger);
            }
            if (!productDetailExists)
            {
                return OperationResult<string>.FailureResult("Product does not exist", logger);
            }

            try
            {
                await informationDatabase.AddAsync(newProductDetail, cancellationToken);
                return OperationResult<string>.SuccessResult("Product detail added successfully", logger);
            }
            catch (Exception exception)
            {
                return OperationResult<string>.FailureResult($"Error occurred while adding product detail: {exception.Message}", logger);
            }
        }
    }
}
