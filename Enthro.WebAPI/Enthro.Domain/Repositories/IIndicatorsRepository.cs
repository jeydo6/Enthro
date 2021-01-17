using Enthro.Domain.Entities;
using Enthro.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Enthro.Domain.Repositories
{
    public interface IIndicatorsRepository
    {
        Task AddAsync(Indicator item);

        Task AddAsync(IEnumerable<Indicator> items);

        Task<IEnumerable<Indicator>> GetAsync(Int32? month, Gender? gender, IndicatorType? type);

        Task RemoveAsync(Indicator item);

        Task RemoveAsync(IEnumerable<Indicator> items);
    }
}
