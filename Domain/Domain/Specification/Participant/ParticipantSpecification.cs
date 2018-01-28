using System;
using Domain.Specification.Interface;

namespace Domain.Specification.Participant
{
    public class ParticipantSpecification
    {
        public class ValidIfParticipantExists : ISpecification<Entities.Participant>
        {
            private readonly Entities.Participant _participant;
            private readonly bool _isUpdate;

            public ValidIfParticipantExists(Entities.Participant participant = null, bool isUpdate = false)
            {
                _participant = participant;
                _isUpdate = isUpdate;
            }

            public bool IsSatisfiedBy(Entities.Participant participant)
            {
                if (_isUpdate)
                {
                    if (participant != null && _participant != null)
                    {
                        if (!string.IsNullOrEmpty(participant.Cpf) && !string.IsNullOrEmpty(_participant.Cpf))
                            if (participant.Cpf == _participant.Cpf)
                                return true;
                    }
                }

                return _participant == null;
            }
        }

        public class ValidParticipantYearsOld : ISpecification<Entities.Participant>
        {
            public bool IsSatisfiedBy(Entities.Participant participant)
            {
                if (participant.BirthDate.AddYears(18) < DateTime.Now  )
                    return true;

                return false;
            }
        }
        
    }
}
