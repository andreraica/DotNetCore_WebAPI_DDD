using Application.Interface.Application;
using Application.Validation;
using Application.ViewModels;
using Domain.Entities;
using Infrastructure.CrossCutting.ExtensionMethods;
using Infrastructure.CrossCutting.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Net;

namespace Middleware.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class ParticipantController : ApiBaseController
    {
        private readonly IParticipantApplication _participantApplication;

        public ParticipantController(IParticipantApplication participantApplication)
        {
            _participantApplication = participantApplication;
        }

        [Route("{cpf}"), HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Middleware.Api.Models.ResultDataSuccess<ParticipantViewModel>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        public IActionResult Get(string cpf)
        {
            if (String.IsNullOrEmpty(cpf))
                throw new Exception("Não foi possível obter o participante. Cpf vazio", new Exception("ParticipantController - public IActionResult Get(string cpf) - cpf vazio"));

            var participant = _participantApplication.GetByCpf(cpf);

            if (participant == null)
            {
                participant = new Participant()
                {
                    Cpf = cpf
                };
            }

            return Ok(participant);
        }

        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(string))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        public IActionResult Post([FromBody]ParticipantViewModel model)
        {
            if (ModelState.IsValid)
            {
                Participant participant = AutoMapperExtensionMethods<Participant>.Map<ParticipantViewModel>(model);
                participant.Active = true;
                participant.DateModified = DateTime.UtcNow.ToBrazillianDate();

                ValidationAppResult validationAppResult = _participantApplication.Add(participant);

                if (validationAppResult.IsValid)
                    return Ok("Dados cadastrado com sucesso.");

                return BadRequest(validationAppResult.Erros);
            }

            return BadRequest("Erro: Não foi possivel gravar o registro.");
        }

    }
}
