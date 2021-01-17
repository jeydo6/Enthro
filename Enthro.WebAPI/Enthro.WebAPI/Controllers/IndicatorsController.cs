using Enthro.Application.Dto;
using Enthro.Domain.Enumerations;
using Enthro.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Enthro.Controllers
{
    [Authorize]
    [ApiController]
    [Route("indicators")]
    public class IndicatorsController : ControllerBase
    {
        private readonly IIndicatorsRepository _indicators;
        private readonly ILogger<IndicatorsController> _logger;

        public IndicatorsController(
            IIndicatorsRepository indicators,
            ILogger<IndicatorsController> logger
        )
        {
            _indicators = indicators;
            _logger = logger;
        }

        [HttpGet]
        [Produces(typeof(IndicatorDto[]))]
        public async Task<IActionResult> GetAsync(Int32? month, Gender? gender, IndicatorType? type)
        {
            var entities = await _indicators.GetAsync(month, gender, type);

            return Ok(
                entities
                    .Select(i => new IndicatorDto
                    {
                        ID = i.ID,
                        Month = i.Month,
                        Type = i.Type,
                        Gender = i.Gender,
                        L = i.L,
                        M = i.M,
                        S = i.S,
                        SD3N = i.SD3N,
                        SD2N = i.SD2N,
                        SD1N = i.SD1N,
                        SD0 = i.SD0,
                        SD1P = i.SD1P,
                        SD2P = i.SD2P,
                        SD3P = i.SD3P
                    })
                    .ToList()
            );
        }
    }
}
