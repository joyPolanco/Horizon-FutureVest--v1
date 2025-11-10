using Application.Dtos;
using Application.Dtos.Country;
using Application.Dtos.MacroeconomicIndicatorDto;
using Application.Dtos.Ranking;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Persistence.Common;
using Persistence.Entities;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class RankingService: BaseRankingService
        
         
    {
        private readonly MacroindicatorService _macroindicatorService;

        public RankingService(CountryService countryService, EstimatedRateConfigurationService rateConfigurationService, MacroindicatorService macroindicatorService) : base(countryService, rateConfigurationService)
        {
            _macroindicatorService = macroindicatorService;
        }



        private  IEnumerable<RankingCountryWithIndicatorsDto> GetCountriesAvaliableforRanking(
            List<RankingCountryWithIndicatorsDto> countriesYearValid,
            List<RankingMacroIndicatorDto> systemMacroIndicators
        )
        {
            var requiredMacroindicatorsIds = new HashSet<int>(
                systemMacroIndicators.Where(m => m.WeightFactor > 0).Select(m => m.Id)
            );

           var countries= countriesYearValid.Where(c =>
            {
                var countryMacroindicatorsIds = new HashSet<int>(c.Indicators.Select(i => i.MacroIndicator.Id));

                return countryMacroindicatorsIds.Count == systemMacroIndicators.Count
                       || requiredMacroindicatorsIds.All(id => countryMacroindicatorsIds.Contains(id));
            });

            return countries;
        }


        public async Task<List<RankingCountryDto>> GetRanking(int Year)


        {

            var countriesYearValid =  await _countryService.GetCountriesForGeneralRankingByYear(Year);

            var SystemMacroindicators = await  _macroindicatorService.GetAllWithIncludeForRanking();
            var countriesForRanking = GetCountriesAvaliableforRanking(countriesYearValid, SystemMacroindicators);
            var MacroIndicatorsExtremes = GetMacroindicatorsExtremeValues( countriesForRanking,false);


            var countriesIndicatorsOperations =await  GetContriesIndicatorsWithOperations(countriesForRanking, MacroIndicatorsExtremes);
            var finalData = await GetFinalRankingWithOperations(countriesIndicatorsOperations);
            

            return   finalData.OrderByDescending(c=>c.Score).ToList();
        }

        protected override async Task<IEnumerable<MacroIndicatorBaseEntity>> GetMacroindicatorsForRanking()
        {
            var list= await _macroindicatorService.GetAllList();
            return list.Select(m => new MacroIndicatorBaseEntity { Id = m.Id, WeightFactor = m.WeightFactor });
        }
    }
    }

