using System;
using Domain.Entities;
using Application.ViewModels;
using Infrastructure.CrossCutting.Helper;

namespace Middleware.Api.Models
{
    public static class ParticipantToParticipantViewModel
    {
        public static Participant ReturnParticipant(ParticipantViewModel model,Participant participant){
            participant.BirthDate = model.BirthDate;
            participant.Cpf = model.Cpf;
            participant.DateModified = DateTime.UtcNow.ToBrazillianDate();
            participant.Gender = model.Gender;
            participant.Name = model.Name;

            return participant;
        }
    }
}