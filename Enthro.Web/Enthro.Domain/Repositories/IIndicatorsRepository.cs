using Enthro.Domain.Dto;
using Enthro.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Enthro.Domain.Repositories
{
    public interface IIndicatorsRepository
    {
        Task<IEnumerable<IndicatorDto>> GetAsync(Int32? month = null, Gender? gender = null, IndicatorType? type = null);
    }
}
