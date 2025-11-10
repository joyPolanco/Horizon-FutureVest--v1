using Application.Dtos.Country;
using Application.Dtos.MacroeconomicIndicatorDto;
using Application.Services;
using Application.ViewModels.Country;
using Application.ViewModels.MacroeconomicIndicator;
using Application.ViewModels.SaveMacroeconomicIndicatorViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Horizon_FutureVest__v1.Controllers
{
    public class MacroindicatorsController(MacroindicatorService service) : Controller
    {
        private readonly MacroindicatorService _service = service;
        public async Task<IActionResult> Index()
        {
            var dtos = await _service.GetAllList();
            if (dtos.Count() == 0) return View("Index", new List<MacroeconomicIndicatorViewModel>());
            List<MacroeconomicIndicatorViewModel> viewModels = dtos.Select(dto => new MacroeconomicIndicatorViewModel()
            {
                Id = dto.Id,
                Name = dto.Name,
                HigherIsBetter=dto.HigherIsBetter,
                WeightFactor=dto.WeightFactor

            }).ToList();
            return View(viewModels);
        }


        public IActionResult Add()
        {
            if (TempData.ContainsKey("Message"))
            {
                ViewBag.Message = TempData["Message"];
                ViewBag.WasSuccesful = TempData["WasSuccesful"];
            }

            return View("Save", new SaveMacroindicatorViewModel
            {
                Id = 0,
                Name = "",
                HigherIsBetter = true,
                WeightFactor = null
            });
        }

        [HttpPost]
        public async Task<IActionResult> Add(SaveMacroindicatorViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("Save", vm);
            }

            var dto = new MacroeconomicIndicatorDto
            {
                Id = 0,
                HigherIsBetter = vm.HigherIsBetter,
                Name = vm.Name,
                WeightFactor = vm.WeightFactor ?? 0
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

            MacroeconomicIndicatorDto? dto = await _service.GetByIdAsync(id);
            if (dto != null)
            {
                SaveMacroindicatorViewModel vm = new()
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    HigherIsBetter= dto.HigherIsBetter,
                    WeightFactor= dto.WeightFactor
                };
                ViewBag.IsEditMode = true;

                return View("Save", vm);
            }

            return RedirectToAction("Index");

        }


        [HttpPost]
        public async Task<IActionResult> Edit(SaveMacroindicatorViewModel vm)
        {
            ViewBag.IsEditMode = true;


            if (!ModelState.IsValid)
            {
                return View("Save", vm);


            }

            MacroeconomicIndicatorDto dto = new()
            {
                Id = vm.Id,
                Name = vm.Name,
                HigherIsBetter = vm.HigherIsBetter,
                WeightFactor = vm.WeightFactor ?? 0
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

     
            return RedirectToAction("Edit", vm);
        }

        public async Task<IActionResult> Delete(int id)
        {


            ViewBag.Message = TempData["Message"];


            MacroeconomicIndicatorDto? dto = await _service.GetByIdAsync(id);
            if (dto != null)
            {
                DeleteMacroeconomicViewModel vm = new()
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    
                };

                return View("Delete", vm);

            }
            return View("Delete", new DeleteMacroeconomicViewModel (){Id = 0, Name="", });

        }


        [HttpPost]
        public async Task<IActionResult> Delete(DeleteMacroeconomicViewModel vm)
        {

            var resultDto = await _service.DeleteAsync(vm.Id);
            ViewBag.Message = resultDto.ResultMessage;
  
            return View("Delete", vm); ;
        }
    }
}

