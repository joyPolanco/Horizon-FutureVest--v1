using Application.Dtos;
using Application.Dtos.Country;
using Application.Dtos.MacroeconomicIndicatorDto;
using Application.Dtos.MacroeconomicIndicatorSimulation;
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
    public class MacroindicatorService(MacroeconomicIndicatorRepository repository)
    {
        MacroeconomicIndicatorRepository _repo = repository;
        public async Task<OperationResultDto> AddAsync(MacroeconomicIndicatorDto dto)
        {
            try
            {
                var existingMacroeconomic = await _repo.SearchByNameAsync( dto.Name);
                var capacidad = _repo.GetAllQuery().Sum(mci => mci.WeightFactor);

                if (existingMacroeconomic == null)
                {
                    if(capacidad == 1 && dto.WeightFactor!=0) 
                        return new OperationResultDto()
                        {
                            WasSuccessful = false,
                            ResultMessage = $"La suma de los macroindicadores ya es igual a 1. Modifique el peso de uno de los macroindicadores o elimine uno"
                        };
                    if(capacidad + dto.WeightFactor>1)
                        return new OperationResultDto()
                        {
                            WasSuccessful = false,
                            ResultMessage = $"La suma de los macroindicadores debe ser igual a 1. Reduzca el peso del macroindicador a {1-capacidad} o menos para que sea un total de 1"
                        };
                    MacroeconomicIndicator? entity = new() { Id = 0, HigherIsBetter = dto.HigherIsBetter, Name = dto.Name, WeightFactor = dto.WeightFactor };
                    await _repo.AddAsync(entity);
                    return new OperationResultDto()
                    {
                        WasSuccessful = true,
                        ResultMessage = $"Se registró el macroindicador ( {dto.Name}) - Peso({dto.WeightFactor}) correctamente"
                    };

                }

                else
                {
                    return new OperationResultDto()
                    {
                        WasSuccessful = false,
                        ResultMessage = $"Ya existe un macroindicador registrado con el nombre {dto.Name}"
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
        public async Task SetSimulationMacroindicator(int id, SimulationMacroindicatorDto dto)
        {
            var entidad= await _repo.GetByIdAsync(id);
            if (entidad != null)
            {
                entidad.SimulationMacroIndicator = new SimulationMacroIndicator { Id = id, RealMacroIndicatorId = dto.MacroindicatorId, WeightFactor = dto.WeightFactor };
                await _repo.UpdateAsync(entidad.Id, entidad);
            }

        }
        public async Task<OperationResultDto> UpdateAsync(MacroeconomicIndicatorDto dto)
        {
            try
            {
                var capacidad = _repo.GetAllQuery().Sum(mci => mci.WeightFactor);
                var existencia = _repo.GetAllQuery().Where(mci => mci.Name == dto.Name && mci.Id != dto.Id).Count();
                var entidad = await _repo.GetByIdAsync(dto.Id);
                if (entidad == null) 
                    return new OperationResultDto()
                {
                    WasSuccessful = false,
                    ResultMessage = $"No se encontró el macroindicador"
                };

                 if(existencia!=0)
                    return new OperationResultDto()
                    {
                        WasSuccessful = false,
                        ResultMessage = $"El nombre ingresado se encuentra reservado para otro macroindicador"
                    };
                var capacidadCambioPeso = capacidad - entidad.WeightFactor;

                if (capacidadCambioPeso + dto.WeightFactor > 1)
                    return new OperationResultDto()
                    {
                        WasSuccessful = false,
                        ResultMessage = $"La suma de los macroindicadores debe ser igual a 1. Reduzca el peso del macroindicador para que sea un total de 1"
                    };

                MacroeconomicIndicator? entity = new() { Id = dto.Id, HigherIsBetter = dto.HigherIsBetter, Name = dto.Name, WeightFactor = dto.WeightFactor };
                await _repo.UpdateAsync(entity.Id,entity);
                return new OperationResultDto()
                {
                    WasSuccessful = true,
                    ResultMessage = $"Se modificó el macroindicador ( {dto.Name}) - Peso({dto.WeightFactor}) correctamente"
                };

                            
            }
            catch
            {
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
                    ResultMessage = $"Se eliminó el macroindicador correctamente"
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

        public async Task<MacroeconomicIndicatorDto?> GetByIdAsync(int id)
        {

            try
            {
                var ent = await _repo.GetByIdAsync(id);
                if (ent != null)
                {
                    return new MacroeconomicIndicatorDto { Id = ent.Id,HigherIsBetter=ent.HigherIsBetter,WeightFactor=ent.WeightFactor, Name=ent.Name};
                }
                else return null;
            }
            catch
            {
                return null;
            }

        }
        public async Task<List<MacroeconomicIndicatorDto>> GetAllList()
        {
            try
            {
                var macroeconomicIndicators = await _repo.GetAllList();
                if (macroeconomicIndicators != null)
                {
                    var dtos = macroeconomicIndicators.Select(ent => new MacroeconomicIndicatorDto()
                    { Id = ent.Id, HigherIsBetter = ent.HigherIsBetter, WeightFactor = ent.WeightFactor, Name = ent.Name }).ToList();
                    return dtos;

                }
                else return [];
            }
            catch (Exception)
            {
                return [];
            }
        }
  


        public decimal? GetMacroindicatorSum() {
            return _repo.GetAllQuery().Sum(m => m.WeightFactor);
        }


        public Task<List<RankingMacroIndicatorDto>> GetAllWithIncludeForRanking()
        {
            return _repo.GetAllQuery()
                    .Include(m => m.EconomicIndicators)
                    .Include(m => m.SimulationMacroIndicator)
                    .ThenInclude(z => z.RealMacroIndicator)
                    .Select(rm => new RankingMacroIndicatorDto
                    {
                        Id = rm.Id,
                        HigherIsBetter = rm.HigherIsBetter,
                        Name = rm.Name,
                        WeightFactor = rm.WeightFactor,
                        SimulationMacroindicator = rm.SimulationMacroIndicator == null ? null : new RankingSimulationMacroIndicatorDto
                        {
                            Id = rm.SimulationMacroIndicator.Id,
                            WeightFactor = rm.SimulationMacroIndicator.WeightFactor,
                            MacroIndicator = null
                        }
                    }).ToListAsync();
                


        }
        
        
     






    }

}
