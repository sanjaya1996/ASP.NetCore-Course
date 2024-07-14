using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace ModelValidationsExample.CustomValidators
{
    public class DateRangeValidatorAttribute:ValidationAttribute
    {
        public string OtherPropertyName { get; set; }
        public DateRangeValidatorAttribute(string otherPropertyName) {
            OtherPropertyName = otherPropertyName;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value == null || OtherPropertyName == null) {
                return null;
            }

            DateTime to_date = Convert.ToDateTime(value);
            PropertyInfo? otherProperty = validationContext.ObjectType.GetProperty(OtherPropertyName);
            if(otherProperty != null)
            {
                DateTime from_date = Convert.ToDateTime(otherProperty.GetValue(validationContext.ObjectInstance));
                if (from_date > to_date)
                {
                    return new ValidationResult(ErrorMessage, new string[] { OtherPropertyName, validationContext.MemberName });
                }

                return ValidationResult.Success;
            }

            return null;
        }
    }
}
