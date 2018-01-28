using Domain.Entities;
using Domain.Validation;

namespace Domain.Interfaces.Services
{
    public interface IParticipantService : IServiceBase<Participant>
    {
        Participant GetByCpf(string cpf);
        new ValidationResult Add(Participant user);
    }
}
