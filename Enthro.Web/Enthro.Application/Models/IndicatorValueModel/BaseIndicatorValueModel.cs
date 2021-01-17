using System;
using System.ComponentModel.DataAnnotations;

namespace Enthro.Application.Models
{
    public abstract class BaseIndicatorValueModel
    {
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public Double SDS { get; set; }
    }
}
