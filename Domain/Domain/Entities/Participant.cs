using Domain.Validation.Interface;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Participant : BaseEntity
    {        
        public string Name {get; set;}
        public string Cpf { get; set; }     
        public int Gender { get; set; }
        public DateTime BirthDate { get; set; }
        
        [NotMapped]
        public Validation.ValidationResult ResultValidation { get; private set; }

        public bool IsValid(IValidation<Participant> validation)
        {
            ResultValidation = validation.Validate(this);
            return ResultValidation.IsValid;
        }
    }
}