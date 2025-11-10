using Application.Dtos.Country;
using Application.Services;
using Application.ViewModels.Country;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Horizon_FutureVest__v1.Controllers
{
    public class CountriesController (CountryService countryService) : Controller
    {
        private readonly CountryService _countryService = countryService;
        public async Task <IActionResult> Index()
        {
            var dtos =  await _countryService.GetAllList();
            if (dtos.Count() == 0) return View("Index",new List<CountryViewModel>());
            List<CountryViewModel> viewModels = dtos.Select(dto => new CountryViewModel()
            {
                Id= dto.Id,
                Name = dto.Name,
                IsoCode = dto.IsoCode,
                
            }).ToList();
            return View(viewModels);
        }


        public IActionResult Add()
        {

            ViewBag.Message = TempData["Message"];
            return View("Save", new SaveCountryViewModel() { Id = 0, Name = "", IsoCode = "" });

        }

        [HttpPost]
        public async Task<IActionResult> Add(SaveCountryViewModel vm)
        {

            if (!ModelState.IsValid)
            {
                return View("Save", vm);

            }

            CountryDto dto = new() { Id = 0, IsoCode = vm.IsoCode.ToUpper(), Name = vm.Name };
             var resultDto= await _countryService.AddAsync(dto);
            TempData["Message"] = resultDto.ResultMessage;

            return RedirectToAction("Add");

        }


        public async Task<IActionResult> Edit(int id)
        {


            ViewBag.Message = TempData["Message"];

            CountryDto? dto = await _countryService.GetByIdAsync(id);
            if (dto != null) {
                SaveCountryViewModel vm = new()
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    IsoCode = dto.IsoCode,
                };
                ViewBag.IsEditMode = true;

                return View("Save", vm);
            }
           
            return RedirectToAction("Index");

        }


        [HttpPost]
        public async Task<IActionResult> Edit (SaveCountryViewModel vm)
        {
            ViewBag.IsEditMode = true;

            if (!ModelState.IsValid) {

                return View("Save", vm);


            }

            CountryDto dto = new() { Id = vm.Id, Name = vm.Name, IsoCode = vm.IsoCode };
            var resultDto = await _countryService.UpdateAsync(dto);
            TempData["Message"] = resultDto.ResultMessage;
            return RedirectToAction("Edit",vm);
        }

        public async Task<IActionResult> Delete(int id)
        {


            ViewBag.Message = TempData["Message"];
           

                CountryDto? dto = await _countryService.GetByIdAsync(id);
                if (dto != null)
                {
                    DeleteCountryViewModel vm = new()
                    {
                        Id = dto.Id,
                        Name = dto.Name,
                        IsoCode = dto.IsoCode,
                    };

                    return View("Delete", vm);
                
                  }
            return View("Delete", new DeleteCountryViewModel() { Id=0, IsoCode="", Name=""});

        }


        [HttpPost]
        public async Task<IActionResult> Delete(DeleteCountryViewModel vm)
        {

            var resultDto = await _countryService.DeleteAsync(vm.Id);
            ViewBag.Message = resultDto.ResultMessage; // usar ViewBag en lugar de TempData
            return View("Delete", vm); ;
        }
    }
}
