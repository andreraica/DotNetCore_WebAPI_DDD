using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IParticipantRepository : IRepositoryBase<Participant>
    {
        Participant GetByCpf(string cpf);
    }
}
