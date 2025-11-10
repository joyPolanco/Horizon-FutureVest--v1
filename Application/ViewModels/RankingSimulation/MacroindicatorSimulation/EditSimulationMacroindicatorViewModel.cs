using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.RankingSimulation.MacroindicatorSimulation
{
    public class EditSimulationMacroindicatorViewModel
    {
        public  required int Id { get; set; }
        [Required(ErrorMessage = "Se debe ingresar el peso del macroindicador para la simulación")]

        public  decimal WeightFactor { get; set; }

        public required int MacroindicatorId { get; set; }
    }
}
