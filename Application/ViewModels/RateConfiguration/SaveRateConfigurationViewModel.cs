using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.RateConfiguration
{
    public class SaveRateConfigurationViewModel :IValidatableObject
    {
        [Required(ErrorMessage ="La tasa mínima estimada es requerida")]
        public  decimal MinRate { get; set; }

        [Required(ErrorMessage = "La tasa máxima estimada es requerida")]

        public  decimal MaxRate { get; set; }

        public IEnumerable<ValidationResult> Validate (ValidationContext validationContext)
        {
            if (MinRate >= MaxRate)
            {
                yield return new ValidationResult(
                    "El valor mínimo debe ser menor que el valor máximo.",
                    new[] { nameof(MinRate), nameof(MaxRate) }
                );
            }
        }
    }
    }

