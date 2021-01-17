using Enthro.Application.Models;
using Enthro.Domain.Dto;
using Enthro.Domain.Enumerations;
using Enthro.Domain.Repositories;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Enthro.Application.Queries
{
    public class GetAnthropometryValueHandler : IRequestHandler<GetAnthropometryValueQuery, AnthropometryValueModel>
    {
        private readonly IIndicatorsRepository _indicators;

        private static readonly Int32 _secondsInMonth = 2629746;

        public GetAnthropometryValueHandler(
            IIndicatorsRepository indicators
        )
        {
            _indicators = indicators;
        }

        public async Task<AnthropometryValueModel> Handle(GetAnthropometryValueQuery request, CancellationToken cancellationToken)
        {
            Int32 month = (Int32)Math.Round((request.Model.VisitDate.Value - request.Model.BirthDate.Value).TotalSeconds / _secondsInMonth);

            return new AnthropometryValueModel
            {
                HeightForAge = await GetHFAIndicatorValue(month, request.Model.Gender.Value, request.Model.Height.Value),
                BMIForAge = await GetBMIFAIndicatorValue(month, request.Model.Gender.Value, request.Model.Weight.Value, request.Model.Height.Value),
                TargetHeight = await GetTHIndicatorValue(request.Model.Gender.Value, request.Model.FatherHeight, request.Model.MotherHeight)
            };
        }

        private Double GetSDS(Double value, Double l, Double m, Double s)
        {
            return (Math.Pow(value / m, l) - 1) / (l * s);
        }

        private async Task<HFAIndicatorValueModel> GetHFAIndicatorValue(Int32 month, Gender gender, Double height)
        {
            IndicatorDto indicator = (await _indicators.GetAsync(month, gender, IndicatorType.HeightForAge))
                    .FirstOrDefault();

            if (indicator != null)
            {
                Double heightCorrection = month < 24 ? 7 : 0;

                return new HFAIndicatorValueModel
                {
                    Month = month,
                    Gender = gender,
                    Height = height,
                    SDS = Math.Round(GetSDS(height + heightCorrection, indicator.L, indicator.M, indicator.S), 2)
                };
            }

            return null;
        }

        private async Task<BMIFAIndicatorValueModel> GetBMIFAIndicatorValue(Int32 month, Gender gender, Double weight, Double height)
        {
            IndicatorDto indicator = (await _indicators.GetAsync(month, gender, IndicatorType.BMIForAge))
                    .FirstOrDefault();

            Double bmi = weight / Math.Pow(height / 100, 2);

            if (indicator != null)
            {
                return new BMIFAIndicatorValueModel
                {
                    Month = month,
                    Gender = gender,
                    BMI = bmi,
                    SDS = Math.Round(GetSDS(bmi, indicator.L, indicator.M, indicator.S), 2)
                };
            }

            return null;
        }

        private async Task<THIndicatorValueModel> GetTHIndicatorValue(Gender gender, Double? fatherHeight, Double? motherHeight)
        {
            if (fatherHeight.HasValue && motherHeight.HasValue)
            {
                Int32 month = 19 * 12;
                IndicatorDto indicator = (await _indicators.GetAsync(month, gender, IndicatorType.HeightForAge))
                        .FirstOrDefault();

                if (indicator != null)
                {
                    Double heightCorrection = gender switch
                    {
                        Gender.Male => 13,
                        Gender.Female => -13,
                        _ => 0,
                    };

                    Double height = (fatherHeight.Value + motherHeight.Value + heightCorrection) / 2;

                    return new THIndicatorValueModel
                    {
                        Gender = gender,
                        Height = height,
                        SDS = Math.Round(GetSDS(height, indicator.L, indicator.M, indicator.S), 2)
                    };
                }
            }

            return null;
        }
    }
}