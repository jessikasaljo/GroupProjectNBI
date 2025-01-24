using Application.Interfaces;
using Domain.RepositoryInterface;

namespace Application.Helpers
{
    public class VerificationService<T> : IVerificationService<T> where T : class
    {
        private readonly IGenericRepository<T> repository;
        public VerificationService(IGenericRepository<T> _repository)
        {
            repository = _repository;
        }
        public async Task<bool> VerifyAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                T? item = await repository.GetByIdAsync(id, cancellationToken);
                return item != null;
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException($"{exception}");
            }
        }
    }
}
