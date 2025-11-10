using Application.Dtos.MacroeconomicIndicatorDto;
using Application.Dtos.MacroeconomicIndicatorSimulation;
using Application.Services;
using Application.ViewModels.MacroeconomicIndicator;
using Application.ViewModels.Ranking;
using Application.ViewModels.RankingSimulation;
using Application.ViewModels.RankingSimulation.MacroindicatorSimulation;
using Application.ViewModels.SaveMacroeconomicIndicatorViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Horizon_FutureVest__v1.Controllers
{
    public class SimulationMacroindicatorsController(
        SimulationMacroindicatorService service, 
        EconomicIndicatorService indicatorService,
        MacroindicatorService macroindicatorService
        ) : Controller
    {
        private readonly EconomicIndicatorService _indicatorService = indicatorService;
        private readonly SimulationMacroindicatorService _service = service;
        private readonly MacroindicatorService _macroindicatorService = macroindicatorService;
        public async Task<IActionResult> Index()
        {
            var Years = await _indicatorService.GetValidYears();
            var macroindicators = await _macroindicatorService.GetAllList();
            SimulatorPageFilterViewModel vm = new()
            {
                CountryList = new List<RankingCountryViewModel>(),
                IndicatorsYear = Years,
                SelectedYear = Years.Max(),
                Macroindicators = macroindicators.Select(m => new SimulationMacroindicatorViewModel
                {
                    Id = m.Id,
                    Name = m.Name,
                    WeightFactor = m.WeightFactor
                }).ToList()



            };


            return View(vm);
        }

   

        public async Task<IActionResult> Add()
        {
            if (TempData.ContainsKey("Message"))
            {
                ViewBag.Message = TempData["Message"];
                ViewBag.WasSuccesful = TempData["WasSuccesful"];
            }
            var macroindicators = await _macroindicatorService.GetAllList();

            return View("Save", new SaveSimulationMacroindicatorViewModel
            {
                Id = 0,
               
                MacroindicatorId = 1,
                Macroindicators = macroindicators.Select(mi => new MacroeconomicIndicatorViewModel
                {
                    HigherIsBetter = mi.HigherIsBetter,
                    Id = mi.Id,
                    Name = mi.Name,
                    WeightFactor = mi.WeightFactor
                }).ToList()

            });
        }

        [HttpPost]
        public async Task<IActionResult> Add(SaveSimulationMacroindicatorViewModel vm)
        {
            var macroindicators = await _macroindicatorService.GetAllList();
            vm.Macroindicators = macroindicators.Select(mi => new MacroeconomicIndicatorViewModel
            {
                HigherIsBetter = mi.HigherIsBetter,
                Id = mi.Id,
                Name = mi.Name,
                WeightFactor = mi.WeightFactor
            }).ToList();

            if (!ModelState.IsValid)
            {
                return View("Save", vm);
            }

            var dto = new SaveSimulationMacroindicatorDto
            {
                Id = 0,
                WeightFactor = vm.WeightFactor, 
                MacroindicatorId= vm.MacroindicatorId
                
            };

            var resultDto = await _service.AddAsync(dto);

            if (!resultDto.WasSuccessful)
            {
                ViewBag.Message = resultDto.ResultMessage;
                ViewBag.WasSuccesful = false;
                return View("Save", vm);
            }

            TempData["Message"] = resultDto.ResultMessage;
            TempData["WasSuccesful"] = true;

            return RedirectToAction(nameof(Add));
        }


        public async Task<IActionResult> Edit(int id)
        {


            if (TempData.ContainsKey("Message"))
            {
                ViewBag.Message = TempData["Message"];
                ViewBag.WasSuccesful = TempData["WasSuccesful"];
            }

            SimulationMacroindicatorDto? dto = await _service.GetByIdAsync(id);
            if (dto != null)
            {
                EditSimulationMacroindicatorViewModel vm = new()
                {
                    Id = dto.Id,
                    WeightFactor = dto.WeightFactor
                    ,
                    MacroindicatorId = dto.MacroindicatorId
                };

                return View("Edit", vm);
            }

            return RedirectToAction("Index");

        }


        [HttpPost]
        public async Task<IActionResult> Edit(EditSimulationMacroindicatorViewModel vm)
        {

            if (!ModelState.IsValid)
            {

                return View("Edit", vm);


            }

            SaveSimulationMacroindicatorDto dto = new()
            {
                Id = vm.Id,
                WeightFactor = vm.WeightFactor,
                MacroindicatorId=vm.MacroindicatorId,
            };
            var resultDto = await _service.UpdateAsync(dto);
            if (!resultDto.WasSuccessful)
            {
                ViewBag.Message = resultDto.ResultMessage;
                ViewBag.WasSuccesful = false;
                return View("Save", vm);
            }

            TempData["Message"] = resultDto.ResultMessage;
            TempData["WasSuccesful"] = true;


            return RedirectToAction("Edit");
        }

        public async Task<IActionResult> Delete(int id)
        {


            ViewBag.Message = TempData["Message"];


            SimulationMacroindicatorDto? dto = await _service.GetByIdAsync(id);
            if (dto != null)
            {
                DeleteSimulationMacroindicatorViewModel vm = new()
                {
                    Id = dto.Id,
                    Name = dto.Name?? string.Empty,

                };

                return View("Delete", vm);

            }
            return View("Delete", new DeleteSimulationMacroindicatorViewModel() { Id = 0, Name = "", });

        }


        [HttpPost]
        public async Task<IActionResult> Delete(DeleteSimulationMacroindicatorViewModel vm)
        {

            var resultDto = await _service.DeleteAsync(vm.Id);
            ViewBag.Message = resultDto.ResultMessage;

            return View("Delete", vm); ;
        }


}
}
