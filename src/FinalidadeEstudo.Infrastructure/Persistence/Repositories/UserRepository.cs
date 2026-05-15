using FinalidadeEstudo.Domain.Entities;
using FinalidadeEstudo.Domain.Interfaces.Repositories;
using FinalidadeEstudo.Domain.ValueObject;
using Microsoft.EntityFrameworkCore;

namespace FinalidadeEstudo.Infrastructure.Persistence.Repositories;

internal sealed class UserRepository(AppDbContext context) :
    RepositoryBase<User>(context), IUserRepository
{
    public async Task<bool> ExistByCpfCnpjAsync(string cpfCnpj, CancellationToken ct = default)
    {
        var VOCpfCnpj = CpfCnpj.Create(cpfCnpj);

        return await Entity.AnyAsync(
            x => x.CpfCnpj.Equals(VOCpfCnpj),
            ct);
    }
}
