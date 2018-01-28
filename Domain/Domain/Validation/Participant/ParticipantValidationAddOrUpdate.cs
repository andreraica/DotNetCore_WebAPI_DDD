using Domain.Specification.Participant;
using Domain.Validation.Interface;

namespace Domain.Validation.User
{
    public sealed class ParticipantValidationAddOrUpdate : Validation<Entities.Participant>, IValidation<Entities.Participant>
    {
        public ParticipantValidationAddOrUpdate(Entities.Participant participant, bool isUpdate = false)
        {
            ParticipantSpecification.ValidIfParticipantExists validIfParticipantExists = new ParticipantSpecification.ValidIfParticipantExists();
            ParticipantSpecification.ValidParticipantYearsOld validParticipantYearsOld = new ParticipantSpecification.ValidParticipantYearsOld();            

            AddRule("validParticipantYearsOld", new Rule<Entities.Participant>(validParticipantYearsOld, "O acesso a todo conteúdo é liberado apenas para maiores de 18 anos."));
            AddRule("ValidIfParticipantExists", new Rule<Entities.Participant>(validIfParticipantExists, string.Format("Participante com o CPF {0} já cadastrado em nosso sistema.", participant != null ? participant.Cpf : string.Empty)));
        }
    }
}
