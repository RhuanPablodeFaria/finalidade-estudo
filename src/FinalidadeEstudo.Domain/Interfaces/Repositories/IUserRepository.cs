using FinalidadeEstudo.Domain.Entities;

namespace FinalidadeEstudo.Domain.Interfaces.Repositories;

public interface IUserRepository : IRepositoryBase<User>
{
    Task<bool> ExistByCpfCnpjAsync(string cpfCnpj, CancellationToken cancellationToken);
}
