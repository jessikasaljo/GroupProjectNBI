﻿using Application.Helpers;
using AutoMapper;
using Domain.Models;
using Domain.RepositoryInterface;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.ProductDetailCommands.UpdateProductDetail
{
    public class UpdateDetailInformationCommandHandler : IRequestHandler<UpdateDetailInformationCommand, OperationResult<string>>
    {
        private readonly IGenericRepository<DetailInformation> detailDatabase;
        private readonly ILogger logger;
        private readonly IMapper mapper;

        public UpdateDetailInformationCommandHandler(IGenericRepository<DetailInformation> _detailDatabase, ILogger<UpdateDetailInformationCommand> _logger, IMapper _mapper)
        {
            detailDatabase = _detailDatabase;
            logger = _logger;
            mapper = _mapper;
        }

        public async Task<OperationResult<string>> Handle(UpdateDetailInformationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingDetail = await detailDatabase.GetFirstOrDefaultAsync(a => a.Id == request.detailInformationId, cancellationToken);
                if (existingDetail == null)
                {
                    return OperationResult<string>.FailureResult("Detail information not found", logger);
                }

                existingDetail.Title = request.updatedProductDetail.Title;
                existingDetail.Text = request.updatedProductDetail.Text;
                existingDetail.ProductId = request.updatedProductDetail.ProductId;
                await detailDatabase.UpdateAsync(existingDetail, cancellationToken);
                return OperationResult<string>.SuccessResult("Detail information updated successfully", logger);
            }
            catch (Exception exception)
            {
                return OperationResult<string>.FailureResult($"Error occurred while updating detail information: {exception.Message}", logger);
            }
        }
    }
}
