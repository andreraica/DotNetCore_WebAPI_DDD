
using Domain.Entities;

namespace Tests.Infrastructure.Stubs
{
    public static class ParticipantStub
    {
        public static Participant Participante()
        {
            return new Participant()
            {
                Cpf = "12345678912"
            };
        }
    }
}