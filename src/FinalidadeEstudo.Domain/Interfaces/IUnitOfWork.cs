using FinalidadeEstudo.Domain.Interfaces.Repositories;

namespace FinalidadeEstudo.Domain.Interfaces;

public interface IUnitOfWork
{
    #region IRepositories

    IUserRepository Users { get; }

    #endregion


    Task<int> CommitAsync(CancellationToken cancellationToken);

}
