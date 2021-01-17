using Enthro.Domain.Entities;
using Enthro.Domain.Enumerations;
using Enthro.Domain.Repositories;
using Enthro.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Enthro.Persistence.Repositories
{
    public class IndicatorsRepository : IIndicatorsRepository
    {
        private readonly EnthroDbContext _dbContext;

        public IndicatorsRepository(
            EnthroDbContext dbContext
        )
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Indicator item)
        {
            if (item != null)
            {
                await _dbContext.Indicators.AddAsync(item);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task AddAsync(IEnumerable<Indicator> items)
        {
            if (items != null)
            {
                await _dbContext.Indicators.AddRangeAsync(items);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Indicator>> GetAsync(Int32? month, Gender? gender, IndicatorType? type)
        {
            IQueryable<Indicator> indicators = _dbContext.Indicators;

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

            return await indicators
                .ToListAsync();
        }

        public async Task RemoveAsync(Indicator item)
        {
            if (item != null)
            {
                Indicator entity = await _dbContext.Indicators
                    .FirstOrDefaultAsync(i =>
                        i.Month == item.Month
                        && i.Gender == item.Gender
                        && i.Type == item.Type
                    );

                _dbContext.Indicators.Remove(entity);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task RemoveAsync(IEnumerable<Indicator> items)
        {
            if (items != null)
            {
                IEnumerable<Indicator> entities = (await _dbContext.Indicators
                    .ToListAsync())
                    .Join(
                        items,
                        o => new { o.Month, o.Gender, o.Type },
                        n => new { n.Month, n.Gender, n.Type },
                        (o, n) => o
                    );

                _dbContext.Indicators.RemoveRange(entities);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
