using Application.Dtos.Country;
using Application.Dtos.EconomicIndicator;
using Application.Dtos.MacroeconomicIndicatorDto;
using Application.Services;
using Application.ViewModels.Country;
using Application.ViewModels.EconomicIndicator;
using Application.ViewModels.MacroeconomicIndicator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Horizon_FutureVest__v1.Controllers
{
    public class IndicatorsController(EconomicIndicatorService economicIndicatorService, CountryService countryService, MacroindicatorService macroindicatorService) : Controller
    {
        private readonly EconomicIndicatorService _indicatorService = economicIndicatorService;
        private readonly CountryService _countryService = countryService;
        private readonly MacroindicatorService _macroindicatorService = macroindicatorService;

        [HttpGet]

        public async Task<IActionResult> Index()
        {
            var countriesDtos = await _countryService.GetAllList();
            var indicatorsDtos = await _indicatorService.GetAllByYearAndCountry(null, null);
            var IndicatorViewModels = indicatorsDtos.Select(
                    ei => new EconomicIndicatorViewModel
                    {
                        CountryId = ei.CountryId,
                        CountryName = ei.CountryName,
                        Id = ei.Id,
                        MacroeconomicIndicatorName = ei.MacroeconomicIndicatorName,
                        MacroeconomicIndicatorId = ei.MacroeconomicIndicatorId,
                        Value = ei.Value,
                        Year = ei.Year,
                    }).ToList();

            var vm = new EconomicIndicatorFilterViewModel
            {
                Countries = countriesDtos.Select(c => new CountryViewModel { Id = c.Id, IsoCode = c.IsoCode, Name = c.Name }).ToList(),
                Years = Enumerable.Range(2000, DateTime.Now.Year - 1999).ToList(),
                CountryIdCurrentSelection = null,
                YearCurrentSelection = null,
                EconomicIndicators = IndicatorViewModels
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Filter(EconomicIndicatorFilterViewModel filterViewModel)
        {
            var indicators = await _indicatorService
                .GetAllByYearAndCountry(filterViewModel.CountryIdCurrentSelection, filterViewModel.YearCurrentSelection);

            filterViewModel.EconomicIndicators = indicators.Select(ind =>
                new EconomicIndicatorViewModel
                {
                    CountryId = ind.CountryId,
                    CountryName = ind.CountryName,
                    Id = ind.Id,
                    MacroeconomicIndicatorId = ind.MacroeconomicIndicatorId,
                    MacroeconomicIndicatorName = ind.MacroeconomicIndicatorName,
                    Value = ind.Value,
                    Year = ind.Year
                }).ToList();

            await LoadFilterListsAsync(filterViewModel);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_EconomicIndicatorList", filterViewModel.EconomicIndicators);
            }

            return View("Index", filterViewModel);
        }

        private async Task LoadFilterListsAsync(EconomicIndicatorFilterViewModel vm)
        {
            var countriesDtos = await _countryService.GetAllList();

            vm.Countries = countriesDtos
                .Select(c => new CountryViewModel { Id = c.Id, Name = c.Name ,IsoCode=c.IsoCode})
                .ToList();

            vm.Years = Enumerable.Range(2000, DateTime.Now.Year - 1999).ToList();
        }


    

        public async Task<IActionResult> Add()
        {
            if (TempData.ContainsKey("Message"))
            {
                ViewBag.Message = TempData["Message"];
                ViewBag.WasSuccesful = TempData["WasSuccesful"];
            }
           
            await loadSelectOptions();

            return View("Save", new SaveEconomicIndicatorViewModel()
            {
                Id = 0,
                CountryId = 1,
                MacroeconomicIndicatorId = 1,
                Year = DateTime.Now.Year,
                Value =null
            });
        }


    

        [HttpPost]
        public async Task<IActionResult> Add(SaveEconomicIndicatorViewModel vm)
        {
            if (!ModelState.IsValid)
            {
               await loadSelectOptions(vm.MacroeconomicIndicatorId, vm.CountryId, vm.Year);
            }

            var dto = new EconomicIndicatorSaveDto
            {
                Id = 0,
                CountryId = vm.CountryId,
                MacroeconomicIndicatorId = vm.MacroeconomicIndicatorId,
                Value = vm.Value ?? 0,
                Year = vm.Year
            };

            var resultDto = await _indicatorService.AddAsync(dto);

            if (!resultDto.WasSuccessful)
            {
                ViewBag.Message = resultDto.ResultMessage;
                ViewBag.WasSuccesful = false;
                await loadSelectOptions(vm.MacroeconomicIndicatorId, vm.CountryId, vm.Year);
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

            var dto = await _indicatorService.GetByIdAsync(id);
            if (dto == null) return RedirectToAction("Index");

            var vm = new SaveEconomicIndicatorViewModel
            {
                Id = dto.Id,
                CountryId = dto.CountryId,
                MacroeconomicIndicatorId = dto.MacroeconomicIndicatorId,
                Value = dto.Value,
                Year = dto.Year
            };

            ViewBag.IsEditMode = true;

            await loadSelectOptions(dto.MacroeconomicIndicatorId, dto.CountryId, dto.Year);

            return View("Save", vm);
        }

        private async Task loadSelectOptions(int selectedMacro = 1, int selectedCountry = 1, int? selectedYear = null)
        {
            var countriesDtos = await _countryService.GetAllList();
            var macroIndicatorsDtos = await _macroindicatorService.GetAllList();

            ViewBag.MacroIndicators = new SelectList(macroIndicatorsDtos, "Id", "Name", selectedMacro);
            ViewBag.Countries = new SelectList(countriesDtos, "Id", "Name", selectedCountry);        }



        [HttpPost]
        public async Task<IActionResult> Edit(SaveEconomicIndicatorViewModel vm)
        {
            ViewBag.IsEditMode = true;

            if (!ModelState.IsValid)
            {
            await loadSelectOptions(vm.MacroeconomicIndicatorId,vm.CountryId,vm.Year);

                return View("Save", vm);
            }

            EconomicIndicatorSaveDto dto = new()
            {
                Id = vm.Id, 
                CountryId = vm.CountryId,
                MacroeconomicIndicatorId = vm.MacroeconomicIndicatorId,
                Value = vm.Value ?? 0,
                Year = vm.Year
            };

            var resultDto = await _indicatorService.UpdateAsync(dto);
            if (!resultDto.WasSuccessful)
            {
                ViewBag.Message = resultDto.ResultMessage;
                ViewBag.WasSuccesful = false;
                await loadSelectOptions(vm.MacroeconomicIndicatorId, vm.CountryId, vm.Year);
                return View("Save", vm);
            }

            TempData["Message"] = resultDto.ResultMessage;
            TempData["WasSuccesful"] = true;


            return RedirectToAction("Edit", new { id = vm.Id });
        }



        public async Task<IActionResult> Delete(int id)
        {


            ViewBag.Message = TempData["Message"];


            EconomicIndicatorDto? dto = await _indicatorService.GetByIdAsync(id);
            if (dto != null)
            {
                DeleteEconomicIndicatorViewModel vm = new()
                {
                    Id = dto.Id,
                    CountryName = dto.CountryName,
                    MacroIndicatorName=dto.MacroeconomicIndicatorName,
                    Value = dto.Value,
                    Year = dto.Year
                    

                };

                return View("Delete", vm);

            }
            return View("Delete", new DeleteMacroeconomicViewModel() { Id = 0, Name = "", });

        }


        [HttpPost]
        public async Task<IActionResult> Delete(DeleteEconomicIndicatorViewModel vm)
        {

            var resultDto = await _indicatorService.DeleteAsync(vm.Id);
            ViewBag.Message = resultDto.ResultMessage;

            return View("Delete", vm); ;
        }
    }
}

