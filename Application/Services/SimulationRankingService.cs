using Application.Dtos;
using Application.Dtos.Ranking;
using Microsoft.EntityFrameworkCore;
using Persistence.Common;
using Persistence.Entities;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class SimulationRankingService :BaseRankingService
    {

        private readonly SimulationMacroindicatorService _simulationMacroindicatorService;
        public SimulationRankingService(
            CountryService countryService, 
            EstimatedRateConfigurationService rateConfigurationService, 
            MacroindicatorService macroindicatorService,
             SimulationMacroindicatorService simulationMacroindicatorService
            ) 
            : base(countryService, rateConfigurationService)
        {
            _simulationMacroindicatorService = simulationMacroindicatorService;
        }
        

       
        private IEnumerable<RankingCountryWithIndicatorsDto> GetCountriesAvaliableforRanking(
     List<RankingCountryWithIndicatorsDto> countriesYearValid,
    List<RankingSimulationMacroIndicatorDto> systemSimulationIndicators
)
        {
            var requiredMacroindicatorsIds = new HashSet<int>(systemSimulationIndicators
                .Where(s => s.WeightFactor > 0)
                .Select(s => s.Id));

            var countries = countriesYearValid.Where(c =>
            {
                var countrySimulationMacroindicatorsIds = new HashSet<int>(c.Indicators.Select(i => i.MacroIndicator.SimulationMacroindicator.Id));

                return countrySimulationMacroindicatorsIds.Count == systemSimulationIndicators.Count()
                 || requiredMacroindicatorsIds.All(id => countrySimulationMacroindicatorsIds.Contains(id));
            });

            return countries;
        }

        public async Task<List<RankingCountryDto>> GetRanking(int Year)


        {

            var countriesYearValid = await _countryService.GetCountriesForSimulationRankingByYear(Year);
         
            var SystemSimulationMacroindicators = await  _simulationMacroindicatorService.GetAllForSimulationRanking();

            var countriesForRanking = GetCountriesAvaliableforRanking(countriesYearValid, SystemSimulationMacroindicators);
            var MacroIndicatorsExtremes = GetMacroindicatorsExtremeValues(countriesForRanking, true);


            var countriesIndicatorsOperations = await GetContriesIndicatorsWithOperations(countriesForRanking, MacroIndicatorsExtremes);
            var finalData = await GetFinalRankingWithOperations(countriesIndicatorsOperations);


            return finalData.OrderByDescending(c => c.Score).ToList();
        }

        protected override async Task<IEnumerable<MacroIndicatorBaseEntity>> GetMacroindicatorsForRanking()
        {
            var list = await _simulationMacroindicatorService.GetAllList();
            return list.Select(m => new MacroIndicatorBaseEntity { Id = m.Id, WeightFactor = m.WeightFactor });
        }
    }


}

    
