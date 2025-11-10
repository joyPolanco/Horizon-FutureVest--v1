using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.SaveMacroeconomicIndicatorViewModel
{
    public class SaveMacroindicatorViewModel
    {
        public required int Id { get; set; }

        [Required(ErrorMessage = "Se debe ingresar el nombre del macroindicador")]
        [MinLength(3, ErrorMessage = "El nombre del macroindicador debe tener una longitud mínima de 3 caracteres")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Se debe ingresar el peso del macroindicador")]
        [Range(0, 1, ErrorMessage = "El valor debe estar entre 0-1, debe ser menor o igual que 1 (valor<=1). Se debe usar la notación decimal (EN-ES-DO). Ej 0.00, 0.0")]
        public decimal? WeightFactor { get; set; }

        [Required(ErrorMessage = "Se debe especificar la naturaleza del macroindicador")]
        public bool HigherIsBetter { get; set; }
    }
}
