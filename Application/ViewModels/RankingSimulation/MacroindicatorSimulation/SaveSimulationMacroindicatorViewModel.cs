using Application.Dtos.MacroeconomicIndicatorDto;
using Application.ViewModels.MacroeconomicIndicator;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.RankingSimulation.MacroindicatorSimulation
{
    public class SaveSimulationMacroindicatorViewModel
    {
        public required int Id { get; set; }

        [Required(ErrorMessage = "Se debe ingresar el peso del macroindicador")]
        public  decimal WeightFactor { get; set; }

        [Required(ErrorMessage = "Se debe seleccionar uno de los macroindicadores existentes")]
        public required int MacroindicatorId { get; set; }
        public  List<MacroeconomicIndicatorViewModel>? Macroindicators { get; set; }
    }
}
