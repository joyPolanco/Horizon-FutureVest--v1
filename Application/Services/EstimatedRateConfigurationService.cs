using Application.Dtos;
using Application.Dtos.EstimatedConfiguration;
using Persistence.Entities;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class EstimatedRateConfigurationService(GenericRepository<EstimatedRate> repository)
    {
        private readonly GenericRepository<EstimatedRate> _repo = repository;


        public async Task<OperationResultDto> ApplyConfiguration(EstimatedRateConfigurationDto configurationDto)
        {
            try {
                if (configurationDto.MinRate > configurationDto.MaxRate)
                {
                    return new OperationResultDto()
                    {
                        ResultMessage = "La tasa mínima estimada de retorno debe ser mayor a la tasa máxima estimada.",
                        WasSuccessful = false
                    };

                }
                var singleRecord= _repo.GetAllQuery().FirstOrDefault();
                if(singleRecord != null)
                {
                   await _repo.UpdateAsync(singleRecord.Id, 
                        new EstimatedRate  { Id = singleRecord.Id ,  MaxRate=configurationDto.MaxRate, MinRate=configurationDto.MinRate});
                    
                    return new OperationResultDto()
                    {
                        ResultMessage = "Se guardó la configuración de las tasas de retorno correctamente",
                        WasSuccessful = false
                    };

                }
                else
                {
                    return new OperationResultDto()
                    {
                        ResultMessage = "Se produjo un error al aplicar la configuración",
                        WasSuccessful = false
                    };
                }


            }
            catch
            {
                return new OperationResultDto ()
                {
                    ResultMessage = "Se produjo un error al aplicar la configuración",
                    WasSuccessful = false
                };
            }



        }

        public async Task <decimal> GetMinEstimatedRate()
        {
            var config = await _repo.GetByIdAsync(1);
            return config != null ? config.MinRate : 2m;
        }
        public async Task<decimal> GetMaxEstimatedRate()
        {
            var config = await _repo.GetByIdAsync(1);
            return config!=null? config.MaxRate : 15m;
        }



    }
}
