using Application.Application;
using Application.Interface;
using Application.Interface.Application;
using Application.ViewModels;
using Infrastructure.CrossCutting.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Middleware.Api.Controllers;
using Moq;
using System;
using Xunit;

namespace Tests.Tests.Middleware.API.Controllers
{
    public class ParticipantControllerTests
    {
        private ParticipantController _participantController;
        private Mock<IParticipantApplication> _participantApplication;

        public ParticipantControllerTests()
        {
            _participantApplication = new Mock<IParticipantApplication>();

            _participantController = new ParticipantController(_participantApplication.Object);
        }

        //IActionResult Get(string cpf)
        [Theory]
        [InlineData("54143897644")]
        public void GetCpf_Deve_Retornar_OK(string login)
        {
            _participantApplication.Setup(x => x.GetByCpf(It.IsAny<string>())).Returns(new Domain.Entities.Participant());

            var result = _participantController.Get(login);
            Assert.IsType<OkObjectResult>(result);
        }

        [Theory]
        [InlineData("")]
        public void GetCpf_Deve_Retornar_Erro(string login)
        {
            _participantApplication.Setup(x => x.GetByCpf(It.IsAny<string>())).Returns(new Domain.Entities.Participant());

            //var result = _participantController.Get(login);
            //Assert.IsType<BadRequestObjectResult>(result);
            Assert.ThrowsAny<Exception>(() => _participantController.Get(login));
        }
        
        //[Theory]
        //[InlineData("")]
        //[InlineData("54143897644")]
        //public void Balance_Deve_Retornar_Erro_Sem_Token_REDIS(string login)
        //{
        //    _balanceApplication.Setup(x => x.GetBalance(It.IsAny<string>(), It.IsAny<string>())).Returns(BalanceStub.RetornaBalanceResultErro());

        //    var result = _participantController.GetBalance(login);
        //    Assert.IsType<ObjectResult>(result);
        //}
    }
}
