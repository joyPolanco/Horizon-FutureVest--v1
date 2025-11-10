using Application.Services;
using Application.ViewModels.Ranking;
using Application.ViewModels.RankingSimulation;
using Application.ViewModels.RankingSimulation.MacroindicatorSimulation;
using Microsoft.AspNetCore.Mvc;

namespace Horizon_FutureVest__v1.Controllers
{
    public class RankingSimulatorController(
       SimulationMacroindicatorService service,
        EconomicIndicatorService indicatorService,
        MacroindicatorService macroindicatorService,
        SimulationRankingService rankingService
        ) : Controller
    {
        private readonly EconomicIndicatorService _indicatorService = indicatorService;
        private readonly SimulationMacroindicatorService _service = service;
        private readonly MacroindicatorService _macroindicatorService = macroindicatorService;
        public async Task<IActionResult> Index()
        {
            var Years = await _indicatorService.GetValidYears();
            var macroindicators = await _service.GetAllList();
            SimulatorPageFilterViewModel vm = new()
            {
                CountryList = new List<RankingCountryViewModel>(),
                IndicatorsYear = Years,
                SelectedYear = Years.Any() ? Years.Max() : 0,
                Macroindicators = macroindicators != null ? macroindicators.Select(m => new SimulationMacroindicatorViewModel
                {
                    Id = m.Id,
                    Name = m.Name,
                    WeightFactor = m.WeightFactor
                }).ToList()

                : new List<SimulationMacroindicatorViewModel>()

            };


            return View(vm);
        }




        [HttpPost]
        public async Task<IActionResult> Filter(SimulatorPageFilterViewModel filterViewModel)
        {
            var resultValidateSum = await rankingService.ValidateMacroIndicatorsWeight();
            var Years = await _indicatorService.GetValidYears();
            var macroindicators = await _service.GetAllList();
            filterViewModel.IndicatorsYear = Years;
            filterViewModel.Macroindicators = macroindicators.Select(m => new SimulationMacroindicatorViewModel
            {
                Id = m.Id,
                Name =m.Name,
                WeightFactor = m.WeightFactor
            }).ToList();

            if (resultValidateSum.WasSuccessful)
            {
                var CountriesDtos = await rankingService.GetRanking(filterViewModel.SelectedYear);
                filterViewModel.CountryList = CountriesDtos.Select(c => new RankingCountryViewModel
                {
                    CountryIsoCode = c.CountryIsoCode,
                    CountryName = c.CountryName,
                    ReturnInvestmentRate = c.ReturnInvestmentRate.ToString("0.00").TrimEnd('0').TrimEnd('.') + "%",
                    Score = c.Score.ToString("F2")
                }).ToList();

                if (!filterViewModel.CountryList.Any())
                {
                    ViewBag.NoCountriesMessage = "No hay países que cumplan con los requisitos.";
                }
                ViewBag.SumMacroIsValid = null;
            }
            else
            {
                filterViewModel.CountryList = new List<RankingCountryViewModel>();
                ViewBag.SumMacroIsValid = false;
            }

            return View("Index", filterViewModel);
        }


    }
}

