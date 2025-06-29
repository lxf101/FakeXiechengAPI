﻿using FakeXiechengAPI.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace FakeXiechengAPI.Dtos
{
    [TouristRouteTitleMustBeDifferentFromDescriptionAttribute]
    public class TouristRouteForCreationDto //: IValidatableObject
    {
        [Required(ErrorMessage = "title 不能为空")]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        [MaxLength(1500)]
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime? DepartureTime { get; set; }
        public string Features { get; set; }
        public string Fees { get; set; }
        public string Notes { get; set; }
        public string? Rating { get; set; }
        public string? TravelDays { get; set; }
        public string? TripType { get; set; }
        public string? DepartureCity { get; set; }
        public ICollection<TouristRoutePictureForCreationDto> TouristRoutePictures { get; set; } = new List<TouristRoutePictureForCreationDto>();

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if(Title == Description)
        //    {
        //        yield return new ValidationResult("路线名称和描述不能相同", new[] { "TouristRouteForCreationDto" });
        //    }
        //}
    }
}
