using Application.Interface.Application;
using Application.Validation;
using Domain.Entities;
using Domain.Interfaces.Services;
using Domain.Validation;
using Infrastructure.CrossCutting.Interfaces;

namespace Application.Application
{
    public class ParticipantApplication : ApplicationBase<Participant> , IParticipantApplication
    {
        private readonly IParticipantService _participantService; 
        private readonly ICacheManager _cacheManager;

        public ParticipantApplication(IParticipantService participantService, 
                                      ICacheManager cacheManager) : base(participantService)
        {
            _participantService = participantService;
            _cacheManager = cacheManager;
        }

        public ValidationAppResult Add(Participant participant)
        {
            ValidationResult validationResult = _participantService.Add(participant);

            if (validationResult.IsValid)
            {
                Commit();
                return DomainToApplicationResult(validationResult);
            }

            Rollback();
            return DomainToApplicationResult(validationResult);
        }
        
        public Participant GetByCpf(string cpf)
        {
            return _participantService.GetByCpf(cpf);
        }
    }
}