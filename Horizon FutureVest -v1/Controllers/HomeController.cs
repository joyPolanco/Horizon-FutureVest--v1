using Application.Services;
using Application.ViewModels.EconomicIndicator;
using Application.ViewModels.Ranking;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Drawing;

namespace Horizon_FutureVest__v1.Controllers
{
    public class HomeController(RankingService rankingService, EconomicIndicatorService indicatorService) : Controller
    {
        private readonly RankingService rankingService = rankingService;
        private readonly EconomicIndicatorService _indicatorService = indicatorService;



        public async Task<IActionResult> Index()
        {
            var Years = await _indicatorService.GetValidYears();

            RankingPageFilterViewModel vm = new()
            {
                Countries = new List<RankingCountryViewModel>(),
                CurrentYearSelection = Years.Any() ? Years.Max() : 0,
                Years = Years,

            };


            return View(vm);
        }



        [HttpPost]
        public async Task<IActionResult> Filter(RankingPageFilterViewModel filterViewModel)
        {
            var resultValidateSum = await rankingService.ValidateMacroIndicatorsWeight();
            var Years = await _indicatorService.GetValidYears();

            filterViewModel.Years = Years;

            if (resultValidateSum.WasSuccessful)
            {
                var CountriesDtos = await rankingService.GetRanking(filterViewModel.CurrentYearSelection);
                filterViewModel.Countries = CountriesDtos.Select(c => new RankingCountryViewModel
                {
                    CountryIsoCode = c.CountryIsoCode,
                    CountryName = c.CountryName,
                    ReturnInvestmentRate = c.ReturnInvestmentRate.ToString("0.00").TrimEnd('0').TrimEnd('.') + "%",
                    Score = c.Score.ToString("F2") 
                }).ToList();

                if (!filterViewModel.Countries.Any())
                {
                    ViewBag.NoCountriesMessage = "No hay países que cumplan con los requisitos.";
                }
                ViewBag.SumMacroIsValid = null;
            }
            else
            {
                filterViewModel.Countries = new List<RankingCountryViewModel>();
                ViewBag.SumMacroIsValid = false;
            }

                return View("Index", filterViewModel);
            }


        }
}










