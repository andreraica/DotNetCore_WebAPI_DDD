using Application.Validation;
using Domain.Entities;

namespace Application.Interface.Application
{
    public interface IParticipantApplication : IApplicationBase<Participant>
    {
        Participant GetByCpf(string cpf);
        ValidationAppResult Add(Participant user);
    }
}