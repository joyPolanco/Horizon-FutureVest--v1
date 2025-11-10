using Application.Dtos;
using Application.Dtos.EconomicIndicator;
using Application.Dtos.MacroeconomicIndicatorDto;
using Application.ViewModels.EconomicIndicator;
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
   
    public class EconomicIndicatorService(EconomicIndicatorRepository repository)
    {
        EconomicIndicatorRepository _repo = repository;
        public async Task<OperationResultDto> AddAsync(EconomicIndicatorSaveDto dto)
        {
            try


            {

                var entityExisting = _repo.GetAllQuery()
                 .Where(ei => ei.CountryId == dto.CountryId
                           && ei.Year == dto.Year
                           && ei.MacroeconomicIndicatorId == dto.MacroeconomicIndicatorId)
                 .Include(ei => ei.Country)
                  .Include(ei => ei.MacroeconomicIndicator)
                  .FirstOrDefault();



                if (entityExisting != null)
                {
                    if (entityExisting.Country != null && entityExisting.MacroeconomicIndicator != null)
                    {
                        return new OperationResultDto
                        {
                            WasSuccessful = false,
                            ResultMessage = $"Ya un hay registro para {entityExisting.Country.Name} - en el año {dto.Year} para el macroindicador {entityExisting.MacroeconomicIndicator.Name}"
                        };
                    }
                    return new OperationResultDto
                    {
                        WasSuccessful = false,
                        ResultMessage = $"Ya hay un registro con los datos proporcionados"
                    };
                }


                else
                {
                    EconomicIndicator economicIndicator = new()
                    {
                        Id = 0,
                        MacroeconomicIndicatorId = dto.MacroeconomicIndicatorId,
                        CountryId = dto.CountryId,
                        Year = dto.Year,
                        Value = dto.Value,

                    };
                    EconomicIndicator? ent = await _repo.AddAsync(economicIndicator);


                    return new OperationResultDto()
                    {
                        WasSuccessful = true,
                        ResultMessage = $"Se registró correctamente el indicador económico"
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new OperationResultDto()
                {
                    WasSuccessful = false,
                    ResultMessage = $"Ocurrió un error interno al procesar los datos"
                };

            }
        }
        public async Task<OperationResultDto> UpdateAsync(EconomicIndicatorSaveDto dto)
        {
            try


            {

                   await _repo.UpdateAsync(dto.Id,
                       new EconomicIndicator() { 
                           CountryId=dto.CountryId,
                           Id=dto.Id,MacroeconomicIndicatorId=dto.MacroeconomicIndicatorId,
                           Value=dto.Value,Year=dto.Year,});


                    return new OperationResultDto()
                    {
                        WasSuccessful = true,
                        ResultMessage = $"Se modificó correctamente el indicador económico"
                    };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new OperationResultDto()
                {
                    WasSuccessful = false,
                    ResultMessage = $"Ocurrió un error interno al procesar los datos"
                };

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
                    ResultMessage = $"Se eliminó el indicador económico correctamente"
                };
            }


            catch
            {
                return new OperationResultDto()
                {
                    WasSuccessful = false,
                    ResultMessage = $"Ocurrió un error al eliminar el macroindicador"
                };

            }
        }

        public async Task<EconomicIndicatorDto?> GetByIdAsync(int id)
        {

            try
            {
                var ent = await _repo.GetByIdWithInclude(id);
                if (ent != null)
                {
                    return new EconomicIndicatorDto {
                        CountryId=ent.CountryId,
                        Value=ent.Value,
                        Id=ent.Id,
                        MacroeconomicIndicatorId=ent.MacroeconomicIndicatorId,
                        Year=ent.Year ,
                        CountryName = ent.Country != null ? ent.Country.Name : "",
                        MacroeconomicIndicatorName = ent.MacroeconomicIndicator != null ? ent.MacroeconomicIndicator.Name : ""


                    };
                }
                else return null;
            }
            catch
            {
                return null;
            }

        }
        public async Task<List<EconomicIndicatorDto>> GetAllList()
        {
            try
            {
                var economicIndicators = await _repo.GetAllList();
                if (economicIndicators != null)
                {
                    var dtos = economicIndicators.Select(ent => new EconomicIndicatorDto()
                   { CountryId = ent.Id, 
                        Value = ent.Value,
                        Id = ent.Id, MacroeconomicIndicatorId = ent.Id, 
                        Year = ent.Year ,
                        CountryName = ent.Country != null ? ent.Country.Name : "",
                        MacroeconomicIndicatorName = ent.MacroeconomicIndicator != null ? ent.MacroeconomicIndicator.Name : ""


                    }).ToList();
                    return dtos;

                }
                else return [];
            }
            catch (Exception)
            {
                return [];
            }
        }

        private IQueryable<EconomicIndicator>? GetAllByYearAndCountryQuery(int ?CountryId,int ?Year)
        {
            try
            {


                if (CountryId == null && Year == null)
                {
                   return _repo.GetAllQuery().Include(ei=>ei.Country);

                }
                if (CountryId ==null && Year != null)
                {
                    return _repo.GetAllQuery().Where(n => n.Year == Year);

                }
                if(CountryId!=null && Year == null)
                {
                    return _repo.GetAllQuery().Where(n => n.CountryId == CountryId);

                }
                return _repo.GetAllQuery().Where(n => n.Year == Year && n.CountryId==CountryId);


            }
            catch (Exception)
            {
               return null;
            }
        }


        public async Task< List<EconomicIndicatorDto>>GetAllByYearAndCountry(int? CountryId, int? Year)
        {
            var query = GetAllByYearAndCountryQuery(CountryId,Year);      

            if (query != null)
            {
                return await query.Include(ei=>ei.Country).Include(ei=>ei.MacroeconomicIndicator).Select(ei=> new EconomicIndicatorDto() 
                { 
                    CountryId=ei.CountryId,
                    MacroeconomicIndicatorId=ei.MacroeconomicIndicatorId,
                    Id=ei.Id,
                    Value=ei.Value,
                    Year=ei.Year,
                    CountryName = ei.Country != null ? ei.Country.Name : "",
                    MacroeconomicIndicatorName = ei.MacroeconomicIndicator != null ? ei.MacroeconomicIndicator.Name : ""
                }).ToListAsync();
            }
            else
            {
                return [];
            }
        
        }



        public async Task<List<int>> GetValidYears()
        {
            return await _repo.GetAllQuery().Select(r => r.Year).ToListAsync();
        }

    }


}
