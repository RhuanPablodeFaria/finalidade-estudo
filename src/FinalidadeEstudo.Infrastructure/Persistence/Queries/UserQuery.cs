using FinalidadeEstudo.Domain.Entities;
using FinalidadeEstudo.Domain.Interfaces.Queries;

namespace FinalidadeEstudo.Infrastructure.Persistence.Queries;

public sealed class UserQuery(AppDbContext context) : QueryBase<User>(context), IUserQuery
{
}
