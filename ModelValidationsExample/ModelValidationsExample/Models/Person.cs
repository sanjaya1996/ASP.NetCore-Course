using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using ModelValidationsExample.CustomValidators;
using System.ComponentModel.DataAnnotations;

namespace ModelValidationsExample.Models
{
    public class Person: IValidatableObject
    {
        [Required(ErrorMessage = "{0} can't be empty or null")]
        [Display(Name = "Person Name")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "{0} should be between {2} and {1} characters long")]
        [RegularExpression(@"^[a-zA-Z\s.]*$", ErrorMessage = "{0} should contain only alphabets, space and dots")]
        public string? PersonName { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [EmailAddress(ErrorMessage = "{0} is not valid")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        // [ValidateNever]
        public string? Phone { get; set; }
        [Required(ErrorMessage = "{0} can't be blank")]
        public string? Password { get; set; }
        [Required(ErrorMessage = "{0} can't be blank")]
        [Compare("Password", ErrorMessage = "{0} and {1} do not match")]
        [Display(Name = "Re-enter password")]
        public string? ConfirmPassword { get; set; }
        [Range(0, 999.99, ErrorMessage = "{0} should be between ${1} and ${2}")]
        public double? Price { get; set; }
        // [MinimumYearValidator(2005, ErrorMessage = "Date of Birth should not be newer than Jan 01, {0}")]
        [MinimumYearValidator(2000)]
        //[BindNever]
        public DateTime? DateOfBirth { get; set; }
        public DateTime? FromDate { get; set; }
        [DateRangeValidatorAttribute("FromDate", ErrorMessage = "'From Date' should be older than or equal to 'To Date' ")]
        public DateTime? ToDate { get; set; }
        public int? Age { get; set; }
        public List<string?> Tags { get; set; } = new List<string?>();
        public override string ToString()
        {
            return $"Person object - Person name: {PersonName}, Email: {Email}, Phone: {Phone}, Password: {Password}, ConfirmPassword: {ConfirmPassword}, Price: {Price}";
        }

      
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(Age.HasValue == false && DateOfBirth.HasValue == false)
            {
                yield return new ValidationResult("Either Date of Birth or age should be supplied", new[] {nameof(Age)});
            }
        }
    }
}
