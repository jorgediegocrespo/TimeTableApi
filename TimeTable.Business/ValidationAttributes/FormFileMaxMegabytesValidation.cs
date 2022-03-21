using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace TimeTable.Business.ValidationAttributes
{
    public class FormFileMaxMegabytesValidation : ValidationAttribute
    {
        private readonly int maxMegabytesValidation;

        public FormFileMaxMegabytesValidation(int maxMegabytesValidation)
        {
            this.maxMegabytesValidation = maxMegabytesValidation;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            if (((IFormFile)value).Length > maxMegabytesValidation * 1024 * 1024) //Length is in bytes. 
                return new ValidationResult($"The max length of the form file is {maxMegabytesValidation} MB");

            return ValidationResult.Success;
        }
    }
}
