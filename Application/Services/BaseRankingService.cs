using Application.Dtos;
using Application.Dtos.Ranking;
using Application.Dtos.Ranking.RankingOperations;
using Persistence.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public abstract class BaseRankingService 
    {
        protected readonly CountryService _countryService;
        protected readonly EstimatedRateConfigurationService _rateConfigurationService;

        protected BaseRankingService(
            CountryService countryService,
            EstimatedRateConfigurationService rateConfigurationService
            )

        {
            _countryService = countryService;
            _rateConfigurationService = rateConfigurationService;
        }

     
        protected decimal NormalizeValue(decimal value, decimal min, decimal max, bool highIsBetter)
        {
            if (max == min) return 0.5m;
            return highIsBetter
                ? (value - min) / (max - min) : (max - value) / (max - min);
        }


        protected abstract  Task<IEnumerable<MacroIndicatorBaseEntity>> GetMacroindicatorsForRanking();
        public async Task<OperationResultDto> ValidateMacroIndicatorsWeight()
        {
            var entities =await  GetMacroindicatorsForRanking();
            var sum = entities.Sum(m => m.WeightFactor);
            if (sum < 1)
            {
                return new OperationResultDto()
                {
                    ResultMessage = "Se deben ajustar los pesos de los macroindicadores regstrado hasta que la suma de lo mismo sea igual a 1",
                    WasSuccessful = false

                };
            }
            else
            {
                return new OperationResultDto()
                {
                    ResultMessage = "Los macroindicadores suman 1",
                    WasSuccessful = true

                };
            }
        }


        protected List<RankingMacroIndicatorExtremeDto> GetMacroindicatorsExtremeValues(IEnumerable<RankingCountryWithIndicatorsDto >countriesForRanking,bool ForSimulation)
        {
            if(!ForSimulation)
            return countriesForRanking
            .SelectMany(c => c.Indicators)
            .GroupBy(e => e.MacroIndicator.Id)
            .Select(g => new RankingMacroIndicatorExtremeDto
            {
                MacroIndicadorId = g.Key,
                MaxValue = g.Max(ind => ind.Value),
                MinValue = g.Min(ind => ind.Value),
                Weight = g.First().MacroIndicator.WeightFactor ,
                HighIsBetter = g.First().MacroIndicator.HigherIsBetter,
            })
            .ToList();
            else
                return countriesForRanking
                .SelectMany(c => c.Indicators).Where(c=>c.MacroIndicator.SimulationMacroindicator!=null&& c.MacroIndicator!=null)
                .GroupBy(e => e.MacroIndicator.Id)
                .Select(g => new RankingMacroIndicatorExtremeDto
                {
                    MacroIndicadorId = g.Key,
                    MaxValue = g.Max(ind => ind.Value),
                    MinValue = g.Min(ind => ind.Value),
                    Weight =  g.First().MacroIndicator.SimulationMacroindicator!.WeightFactor,
                    HighIsBetter = g.First().MacroIndicator.HigherIsBetter,
                })
                .ToList();
        }


        protected Task<IList<CountryWithIndicatorsScoreDto>> GetContriesIndicatorsWithOperations(
    IEnumerable<RankingCountryWithIndicatorsDto> countriesForRanking,
    List<RankingMacroIndicatorExtremeDto> MacroIndicatorsExtremes)
        {
            var result = countriesForRanking.Select(c => new CountryWithIndicatorsScoreDto
            {
                CountryId = c.Id,
                IsoCode = c.IsoCode,
                CountryName = c.Name,
                Indicators = c.Indicators.Select(i =>
                {
                    var macroIndicadorData = MacroIndicatorsExtremes
                        .First(m => m.MacroIndicadorId == i.MacroIndicator.Id);

                    decimal normalizationValue;

                    if (macroIndicadorData.HighIsBetter)
                    {
                        normalizationValue = macroIndicadorData.MaxValue == macroIndicadorData.MinValue
                            ? 0.5m
                            : (i.Value - macroIndicadorData.MinValue) /
                              (macroIndicadorData.MaxValue - macroIndicadorData.MinValue);
                    }
                    else
                    {
                        normalizationValue = macroIndicadorData.MaxValue == macroIndicadorData.MinValue
                            ? 0.5m
                            : (macroIndicadorData.MaxValue - i.Value) /
                              (macroIndicadorData.MaxValue - macroIndicadorData.MinValue);
                    }

                    return new CountryIndicatorScoreDto
                    {
                        IndicatorId = i.Id,
                        Value = i.Value,
                        SubScore = normalizationValue * macroIndicadorData.Weight
                    };
                }).ToList()
            }).ToList();

            return Task.FromResult<IList<CountryWithIndicatorsScoreDto>>(result);
        }

        protected async Task<List<RankingCountryDto>> GetFinalRankingWithOperations(IList<CountryWithIndicatorsScoreDto>  countriesIndicatorsOperations)
{
            var minRate = await _rateConfigurationService.GetMinEstimatedRate();
            var maxRate = await _rateConfigurationService.GetMaxEstimatedRate();
            var CountriesScoring = countriesIndicatorsOperations.Select(c => new
            {
                Score = c.Indicators.Sum(i => i.SubScore),
                CountryIsoCode = c.IsoCode,
                Name =c.CountryName

            });
            return CountriesScoring.Select(c => new RankingCountryDto
            {
                Score = c.Score,
                ReturnInvestmentRate = (decimal)(minRate + (maxRate - minRate) * c.Score),
                CountryIsoCode = c.CountryIsoCode,
                CountryName = c.Name

            }).OrderByDescending(c=>c.Score).ToList();

        }
        }

    }

