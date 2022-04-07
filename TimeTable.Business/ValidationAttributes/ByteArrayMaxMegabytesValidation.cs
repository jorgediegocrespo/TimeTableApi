using System.ComponentModel.DataAnnotations;

namespace TimeTable.Business.ValidationAttributes
{
    public class ByteArrayMaxMegabytesValidation : ValidationAttribute
    {
        private readonly int maxKilobytesValidation;

        public ByteArrayMaxMegabytesValidation(int maxKilobytesValidation)
        {
            this.maxKilobytesValidation = maxKilobytesValidation;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            if (((byte[])value).Length > maxKilobytesValidation * 1024) //Length is in bytes. 
                return new ValidationResult($"The max length of the byte array is {maxKilobytesValidation} KB");

            return ValidationResult.Success;
        }
    }
}
