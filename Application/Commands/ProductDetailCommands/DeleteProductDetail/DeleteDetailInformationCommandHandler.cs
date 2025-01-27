using Application.Helpers;
using Domain.Models;
using Domain.RepositoryInterface;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.ProductDetailCommands.DeleteProductDetail
{
    public class DeleteDetailInformationCommandHandler : IRequestHandler<DeleteDetailInformationCommand, OperationResult<string>>
    {
        private readonly IGenericRepository<DetailInformation> detailDatabase;
        private readonly ILogger logger;
        public DeleteDetailInformationCommandHandler(IGenericRepository<DetailInformation> _detailDatabase, ILogger<DeleteDetailInformationCommandHandler> _logger)
        {
            detailDatabase = _detailDatabase;
            logger = _logger;
        }

        public async Task<OperationResult<string>> Handle(DeleteDetailInformationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingDetail = await detailDatabase.GetFirstOrDefaultAsync(a => a.Id == request.detailInformationId, cancellationToken);
                if (existingDetail == null)
                {
                    return OperationResult<string>.FailureResult("Detail information not found", logger);
                }

                await detailDatabase.DeleteAsync(request.detailInformationId, cancellationToken);
                return OperationResult<string>.SuccessResult("Detail information deleted successfully", logger);
            }
            catch (Exception exception)
            {
                return OperationResult<string>.FailureResult($"Error occurred while deleting detail information: {exception.Message}", logger);
            }
        }
    }
}
