namespace Application.Interfaces
{
    public interface IVerificationService<T> where T : class
    {
        Task<bool> VerifyAsync(int id, CancellationToken cancellationToken);
    }
}
