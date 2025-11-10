using Application.Dtos;
using Application.Dtos.Country;
using Application.Dtos.Ranking;
using Microsoft.EntityFrameworkCore;
using Persistence.Entities;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CountryService (CountryRepository repository)
    {
        CountryRepository _repo = repository;
        public async Task <OperationResultDto> AddAsync ( CountryDto countryDto)
        {
            try
            {
                var existingCountry = await _repo.SearchByIsoAndNameAsync(countryDto.IsoCode,countryDto.Name);
                if (existingCountry == null)
                {
                    Country? country = new Country { Id = 0, IsoCode = countryDto.IsoCode, Name = countryDto.Name };
                   await  _repo.AddAsync(country);
                    return new OperationResultDto()  { 
                      WasSuccessful = true, 
                      ResultMessage = $"Se registró el país {countryDto.Name}-{countryDto.IsoCode} correctamente" };

                }

                else
                {
                    return new OperationResultDto() {
                        WasSuccessful = false,
                        ResultMessage=$"Ya existe un país registrado con el código ISO {countryDto.IsoCode} o el nombre {countryDto.Name}"};
                }
            } catch 
            {
                return new OperationResultDto() {
                    WasSuccessful = false, 
                    ResultMessage = $"Ocurrió un error interno al procesar los datos" };

            }
        }

        public async Task<OperationResultDto> UpdateAsync(CountryDto countryDto)
        {
            try
            {
                if (countryDto != null)
                {
                    Country? country = new Country { Id = countryDto.Id, IsoCode = countryDto.IsoCode, Name = countryDto.Name };
                    await _repo.UpdateAsync(country.Id,country);

                    return new OperationResultDto() { 
                        WasSuccessful = true, 
                        ResultMessage = $"Se modificó el país {countryDto.Name}-{countryDto.IsoCode} correctamente" };

                }

                else
                {
                    return new OperationResultDto()  {
                      WasSuccessful = false, 
                      ResultMessage = $"Faltaron datos en el request" };
                }
            }
            catch
            {
                return new OperationResultDto() { 
                  WasSuccessful = false,
                  ResultMessage = $"Ocurrió un error interno al procesar los datos" };

            }
        }

        public async Task<OperationResultDto> DeleteAsync(int id)
        {
            try
            {
                   await _repo.DeleteAsync(id);
             
                    return new OperationResultDto()
                    {
                        WasSuccessful = true,
                        ResultMessage = $"Se eliminó el país correctamente"
                    };
              }

            
            catch
            {
                return new OperationResultDto()
                {
                    WasSuccessful = false,
                    ResultMessage = $"Ocurrió un error al eliminar el país"
                };

            }
        }

        public async Task<CountryDto?> GetByIdAsync(int id)
        {

            try
            {
                var country = await _repo.GetByIdAsync(id);
                if (country != null)
                {
                    return new CountryDto { Id = country.Id, IsoCode = country.IsoCode, Name = country.Name };
                }
                else return null;
            }catch
            {
                return null;
            }

        }
        public async Task<List<CountryDto>> GetAllList()
        {
            try
            {
                var countries = await _repo.GetAllList();
                if (countries != null)
                {
                   var dtos= countries.Select(c=>new CountryDto { Id=c.Id,IsoCode=c.IsoCode, Name=c.Name }).ToList();
                    return dtos;

                }
                else return [];
            }
            catch (Exception)
            {
                return [];
            }
        }


        private IQueryable<RankingCountryWithIndicatorsDto> GetRankingQuery(int year, bool WithSimulation)
        {
            return  _repo.GetAllQuery()
                .Include(c=>c.EconomicIndicators)
                .ThenInclude(ind=>ind.MacroeconomicIndicator)
                .ThenInclude(m=>m!.SimulationMacroIndicator)
                .Where(c => c.EconomicIndicators.Any(ind => ind.Year == year))
                .Select(C => new RankingCountryWithIndicatorsDto
                {
                    Id = C.Id,
                    IsoCode = C.IsoCode,
                    Name = C.Name,
                    Indicators = C.EconomicIndicators
                        .Where(ind => ind.Year == year &&
                        ((WithSimulation&& ind.MacroeconomicIndicator!=null&& ind.MacroeconomicIndicator.SimulationMacroIndicator!=null ) || !WithSimulation)) 
                        .Select(ind => new RankingEconomicIndicatorDto
                        {
                            Id = ind.Id,
                            Value = ind.Value,
                            MacroIndicator = new RankingMacroIndicatorDto
                            {
                                Id = ind.MacroeconomicIndicatorId,
                                HigherIsBetter = ind.MacroeconomicIndicator!.HigherIsBetter,
                                Name = ind.MacroeconomicIndicator.Name,
                                WeightFactor = ind.MacroeconomicIndicator.WeightFactor,

                                SimulationMacroindicator = ind.MacroeconomicIndicator.SimulationMacroIndicator == null
                                      ? null
                                    : (WithSimulation
                                        ? new RankingSimulationMacroIndicatorDto
                                        {
                                            Id = ind.MacroeconomicIndicator.SimulationMacroIndicator.Id,
                                            WeightFactor = ind.MacroeconomicIndicator.SimulationMacroIndicator.WeightFactor
                                        }
                                        : null)
                            }
                        }).ToList()
                });
        }


        public async Task<List<RankingCountryWithIndicatorsDto>> GetCountriesForGeneralRankingByYear(int year)
        {
            var query =   GetRankingQuery(year,false);
            return await query.ToListAsync();
        }

        public async Task<List<RankingCountryWithIndicatorsDto>>GetCountriesForSimulationRankingByYear(int year)
        {
            var query =  GetRankingQuery(year, true);
            return await query.ToListAsync();
        }


    }
}
