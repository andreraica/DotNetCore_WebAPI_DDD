using System;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Validation;
using Domain.Validation.User;
using Domain.Interfaces.Repositories.Dapper;

namespace Domain.Service.Services
{
    public class ParticipantService : ServiceBase<Participant> , IParticipantService
    {
        private readonly IParticipantRepository _participantRepository;
        private readonly IParticipantDapper _participantDapper;

        public ParticipantService(IParticipantRepository participantRepository, IParticipantDapper participantDapper)
            : base(participantRepository)
        {
            _participantRepository = participantRepository;
            _participantDapper = participantDapper;
        }

        public Participant GetByCpf(string cpf)
        {
            return _participantDapper.GetByCpf(cpf);
        }

        public new ValidationResult Add(Participant participant)
        {
            try
            {
                ValidationResult validationResult = new ValidationResult();

                if (participant != null)
                {
                    if (participant.IsValid(new ParticipantValidationAddOrUpdate(participant,false)))
                    {
                        _participantDapper.AddOrUpdate(participant);
                        return validationResult;
                    }

                    validationResult = participant.ResultValidation;
                    return validationResult;
                }

                validationResult.AddError("Ocorreu um erro, dados do usuário inválidos.");
                return validationResult;
            }
            catch (Exception exception)
            {
                ValidationResult validationResult = new ValidationResult();
                validationResult.AddError(string.Format("Ocorreu um erro, Detalhes do erro: {0}",exception.Message));

                return validationResult;
            }
        }

        public override Participant GetById(Guid id)
        {
            return _participantDapper.GetById(id);            
        }

        public override IEnumerable<Participant> GetAll()
        {
            return _participantDapper.GetAll();            
        }

        public override IEnumerable<Participant> GetAllActive()
        {
            return _participantDapper.GetAllActive();
        }
      public override void Update(Participant entity)
        {
            _participantDapper.Update(entity);
        }

        public override void AddOrUpdate(Participant entity)
        {
            _participantDapper.AddOrUpdate(entity);
        }

        public override void Remove(Participant entity)
        {
            _participantDapper.Remove(entity);
        }

    }
}
