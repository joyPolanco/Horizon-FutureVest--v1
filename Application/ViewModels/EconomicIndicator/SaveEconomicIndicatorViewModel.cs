using Application.ViewModels.Country;
using Application.ViewModels.MacroeconomicIndicator;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.EconomicIndicator
{
    public class SaveEconomicIndicatorViewModel
    {
        public required int Id { get; set; }


        [Required(ErrorMessage = "El valor es requerido. Se debe usar la notación decimal (EN-ES-DO). Ej 0.00, 0.0")]
        public  decimal? Value { get; set; }


        [Required(ErrorMessage = "El país es requerido")]
        public required int CountryId { get; set; }

        [Range(1000, 9999, ErrorMessage = "El año debe ser un número de 4 dígitos")]

        [Required(ErrorMessage = "El año es requerido")]

        public required int  Year { get; set; }


        [Required(ErrorMessage = "El macroindicador es requerido")]
        public required int MacroeconomicIndicatorId { get; set; }


    }
}

