using Enthro.Domain.Dto;
using Enthro.Domain.Enumerations;
using Enthro.Domain.Repositories;
using Enthro.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Enthro.Application.Repositories
{
    public class IndicatorsRepository : IIndicatorsRepository
    {
        private readonly IIndicatorsService _indicatorsService;

        private List<IndicatorDto> _store;

        public IndicatorsRepository(
            IIndicatorsService indicatorsService
        )
        {
            _indicatorsService = indicatorsService;
        }

        public async Task<IEnumerable<IndicatorDto>> GetAsync(Int32? month = null, Gender? gender = null, IndicatorType? type = null)
        {
            if (_store == null)
            {
                _store = (await _indicatorsService
                    .GetAsync())
                    .ToList();
            }

            IQueryable<IndicatorDto> indicators = _store.AsQueryable();

            if (month.HasValue)
            {
                indicators = indicators
                    .Where(i => i.Month == month.Value);
            }
            if (gender.HasValue)
            {
                indicators = indicators
                    .Where(i => i.Gender == gender.Value);
            }
            if (type.HasValue)
            {
                indicators = indicators
                    .Where(i => i.Type == type.Value);
            }

            return await Task.FromResult(
                indicators.ToList()
            );
        }
    }
}