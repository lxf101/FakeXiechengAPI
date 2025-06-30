using FakeXiechengAPI.Dtos;
using System.ComponentModel.DataAnnotations;

namespace FakeXiechengAPI.ValidationAttributes
{
    public class TouristRouteTitleMustBeDifferentFromDescriptionAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var touristRouteDto = (TouristRouteForManipulationDto)validationContext.ObjectInstance;
            if (touristRouteDto.Title == touristRouteDto.Description)
            {
               return new ValidationResult("路线名称和描述不能相同", new[] { "TouristRouteForCreationDto" });
            }
            return ValidationResult.Success;
        }
    }
}
