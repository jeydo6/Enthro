using Enthro.Domain.Enumerations;
using System;
using System.ComponentModel.DataAnnotations;

namespace Enthro.Application.Models
{
    public class WFAIndicatorValueModel : BaseIndicatorValueModel
    {
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public Double Weight { get; set; }

        public Int32 Month { get; set; }

        public Gender Gender { get; set; }
    }
}
