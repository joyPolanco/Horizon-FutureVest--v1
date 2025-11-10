using Application.Dtos;
using Application.Dtos.MacroeconomicIndicatorDto;
using Application.Dtos.MacroeconomicIndicatorSimulation;
using Application.Dtos.Ranking;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Persistence.Entities;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class SimulationMacroindicatorService(GenericRepository<SimulationMacroIndicator> repoMacroSimulation, MacroindicatorService macroindicatorService)
    {
        private readonly GenericRepository<SimulationMacroIndicator> _repo = repoMacroSimulation;
        private readonly MacroindicatorService  _macroindicatorService = macroindicatorService;

        public async Task<OperationResultDto> AddAsync(SaveSimulationMacroindicatorDto dto)
        {
            try
            {
                var existingMacroeconomic = await _repo.GetAllQuery().Where(m => m.RealMacroIndicatorId == dto.MacroindicatorId).FirstOrDefaultAsync();
                var capacidad = _repo.GetAllQuery().Sum(mci => mci.WeightFactor);

                if (existingMacroeconomic == null)
                {
                
                    if (capacidad + dto.WeightFactor > 1)
                        return new OperationResultDto()
                        {
                            WasSuccessful = false,
                            ResultMessage = $"La suma de los macroindicadores debe ser igual a 1. Reduzca el peso del macroindicador a {1 - capacidad} o menos para que sea un total de 1"
                        };
                    SimulationMacroIndicator? entity = new() { Id = 0, WeightFactor = dto.WeightFactor ,RealMacroIndicatorId=dto.MacroindicatorId};
                   var ent= await _repo.AddAsync(entity);

                    return new OperationResultDto()
                    {
                        WasSuccessful = true,
                        ResultMessage = $"Se registró el macroindicador correctamente"
                    };

                }

                else
                {
                    return new OperationResultDto()
                    {
                        WasSuccessful = false,
                        ResultMessage = $"Ya existe un macroindicador en la simulacion para el macroindicador"
                    };
                }

            }
            catch (Exception )
            {
                return new OperationResultDto()
                {
                    WasSuccessful = false,
                    ResultMessage = $"Ocurrió un error al agregar el macroindicador a la simulación"
                };

            }
        }
        public async Task<OperationResultDto> UpdateAsync(SaveSimulationMacroindicatorDto dto)
        {
            try
            {
                var capacidad = _repo.GetAllQuery().Sum(mci => mci.WeightFactor);
                var existencia = _repo.GetAllQuery().Where(mci => mci.RealMacroIndicatorId==dto.MacroindicatorId && mci.Id!=dto.Id).Count();
                var entidad = await _repo.GetAllQuery().Include(m=>m.RealMacroIndicator).Where(m=> m.Id==dto.Id).FirstOrDefaultAsync();
                if (entidad == null)
                    return new OperationResultDto()
                    {
                        WasSuccessful = false,
                        ResultMessage = $"No se encontró el macroindicador"
                    };

                if (existencia != 0)
                    return new OperationResultDto()
                    {
                        WasSuccessful = false,
                        ResultMessage = $"Ya existe un macroindicador en la simulación registrado con el macroindicador"
                    };
                var capacidadCambioPeso = capacidad - entidad.WeightFactor;

                if (capacidadCambioPeso + dto.WeightFactor > 1)
                    return new OperationResultDto()
                    {
                        WasSuccessful = false,
                        ResultMessage = $"La suma de los macroindicadores debe ser igual a 1. Reduzca el peso del macroindicador para que sea un total de 1"
                    };

                SimulationMacroIndicator? entity = new() { Id = dto.Id,WeightFactor = dto.WeightFactor, RealMacroIndicatorId=dto.MacroindicatorId };
                await _repo.UpdateAsync(entity.Id, entity);
                return new OperationResultDto()
                {
                    WasSuccessful = true,
                    ResultMessage = $"Se modificó el macroindicador de la simulación correctamente"
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


        public async Task<List<SimulationMacroindicatorDto>> GetAllList()
        {
            var entities = await _repo.GetAllQuery().Include(m => m.RealMacroIndicator).Where(m=>m.RealMacroIndicator!=null).Select(m => new SimulationMacroindicatorDto
            {
                Id = m.Id,
                Name = m.RealMacroIndicator!.Name,
                WeightFactor = m.WeightFactor,
                MacroindicatorId=m.RealMacroIndicatorId

            }).ToListAsync();

            return entities;
        }





        public async Task<SimulationMacroindicatorDto?> GetByIdAsync(int id)
        {

            try
            {
                var ent = await _repo.GetAllQuery().Include(m => m.RealMacroIndicator).Where(m => m.Id == id && m.RealMacroIndicator != null).FirstOrDefaultAsync();
                if (ent != null)
                {
                    return new SimulationMacroindicatorDto { Id = ent.Id, WeightFactor = ent.WeightFactor, Name = ent.RealMacroIndicator!.Name, MacroindicatorId=ent.RealMacroIndicatorId};
                }
                else return null;
            }
            catch
            {
                return null;
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

        public async Task<List<RankingSimulationMacroIndicatorDto>> GetAllForSimulationRanking()
        {
            try
            {
                return await _repo.GetAllQuery()
                    .Include(sm=>sm.RealMacroIndicator)
                    .Select(sm=> new RankingSimulationMacroIndicatorDto 
                    { 
                    Id=sm.Id,
                    WeightFactor=sm.WeightFactor
                   
                    })

                    .ToListAsync
                    
                    ();
               
            }
            catch (Exception)
            {
                return [];
            }
        }


      







    }
}
