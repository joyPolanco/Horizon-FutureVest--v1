using Application.Dtos.EstimatedConfiguration;
using Application.Services;
using Application.ViewModels.RateConfiguration;
using Microsoft.AspNetCore.Mvc;

namespace Horizon_FutureVest__v1.Controllers
{
    public class RatesConfigurationController (EstimatedRateConfigurationService rateConfigurationService) : Controller
    {
        private readonly EstimatedRateConfigurationService _rateConfigurationService=rateConfigurationService;
        public async Task<IActionResult> Index()
        {

          
                return View(new SaveRateConfigurationViewModel() {
                    MaxRate = await _rateConfigurationService.GetMaxEstimatedRate(),
                    MinRate = await _rateConfigurationService.GetMinEstimatedRate()});
            
        }

        [HttpPost]
        public async Task<IActionResult> Update(SaveRateConfigurationViewModel model)
           
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }
            var resultDto = await _rateConfigurationService.ApplyConfiguration(
                new EstimatedRateConfigurationDto { MaxRate = model.MaxRate, MinRate = model.MinRate }

                );
            ViewBag.Message = resultDto.ResultMessage;

            if (resultDto.WasSuccessful)
            {
                return RedirectToAction("Index");

            }
            return View("Index", model);
        }
        }
    }
