using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IVerificationService<T> where T : class
    {
        Task<bool> VerifyAsync(int id, CancellationToken cancellationToken);
    }
}
