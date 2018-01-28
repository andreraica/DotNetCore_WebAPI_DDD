using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Application.ViewModels
{
    public class ParticipantViewModel : BaseViewModel
    {
        [Required(ErrorMessage = "O campo nome é obrigatório")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo Cpf é obrigatório")]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "O campo sexo é obrigatório")]
        public int Gender { get; set; }

        [Required(ErrorMessage = "O campo data de nascimento é obrigatório")]
        public DateTime BirthDate { get; set; }
    }
}
