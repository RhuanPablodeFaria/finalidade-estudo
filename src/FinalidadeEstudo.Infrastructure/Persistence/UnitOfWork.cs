using FinalidadeEstudo.Domain.Interfaces;
using FinalidadeEstudo.Domain.Interfaces.Repositories;
using FinalidadeEstudo.Infrastructure.Persistence.Repositories;

namespace FinalidadeEstudo.Infrastructure.Persistence;

public sealed class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    #region Repositories

    public IUserRepository? _users;

    #endregion



    #region Repositories initialization 

    public IUserRepository Users => _users ??= new UserRepository(context);

    #endregion

    public Task<int> CommitAsync(CancellationToken ct = default) =>
        context.SaveChangesAsync(ct);
}
