using Enthro.Domain.Dto;
using Enthro.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Enthro.Domain.Services
{
    public interface IIndicatorsService
    {
        Task<IEnumerable<IndicatorDto>> GetAsync(Int32? month = null, Gender? gender = null, IndicatorType? type = null);
    }
}