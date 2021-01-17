using Enthro.Domain.Enumerations;
using System;
using System.ComponentModel.DataAnnotations;

namespace Enthro.Application.Models
{
    public class AnthropometryModel
    {
        public AnthropometryModel()
        {
            VisitDate = DateTime.Today;
        }

        [Required]
        public DateTime? VisitDate { get; set; }

        [Required]
        public DateTime? BirthDate { get; set; }

        [Required]
        public Gender? Gender { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:F2}")]
        [Range(0.1, Double.MaxValue)]
        public Double? Weight { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:F2}")]
        [Range(0.1, Double.MaxValue)]
        public Double? Height { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}")]
        [Range(0.1, Double.MaxValue)]
        public Double? FatherHeight { get; set; }

        [DisplayFormat(DataFormatString = "{0:F2}")]
        [Range(0.1, Double.MaxValue)]
        public Double? MotherHeight { get; set; }
    }
}
