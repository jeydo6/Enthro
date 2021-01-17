using Enthro.Domain.Enumerations;
using System;
using System.ComponentModel.DataAnnotations;

namespace Enthro.Application.Models
{
    public class THIndicatorValueModel : BaseIndicatorValueModel
    {
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public Double Height { get; set; }

        public Gender Gender { get; set; }
    }
}
