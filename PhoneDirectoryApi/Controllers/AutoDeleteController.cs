using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneDirectoryApi.Models.Domain;
using PhoneDirectoryApi.Models.ViewModels;
using PhoneDirectoryApi.Services;

namespace PhoneDirectoryApi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AutoDeleteController : Controller
    {
        private readonly IAutoDeleteService _autoDeleteService;
        private readonly ILogger<AutoDeleteController> _logger;

        public AutoDeleteController(IAutoDeleteService autoDeleteService, ILogger<AutoDeleteController> logger)
        {
            _autoDeleteService = autoDeleteService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var settings = await _autoDeleteService.GetSettingsAsync();

            var viewModel = new AutoDeleteSettingsViewModel
            {
                Id = settings.Id,
                IsEnabled = settings.IsEnabled,
                DeleteIntervalMinutes = settings.DeleteIntervalMinutes,
                ContactsToDelete = settings.ContactsToDelete,
                DeleteOnlyInactive = settings.DeleteOnlyInactive
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(AutoDeleteSettingsViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var updatedSettings = new AutoDeleteSettings
            {
                Id = model.Id,
                IsEnabled = model.IsEnabled,
                DeleteIntervalMinutes = model.DeleteIntervalMinutes,
                ContactsToDelete = model.ContactsToDelete,
                DeleteOnlyInactive = model.DeleteOnlyInactive
            };

            await _autoDeleteService.UpdateSettingsAsync(updatedSettings);

            TempData["Success"] = "Settings updated successfully!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ToggleAutoDelete(bool isEnabled)
        {
            await _autoDeleteService.ToggleAutoDeleteAsync(isEnabled);
            TempData["Success"] = $"Auto-delete {(isEnabled ? "enabled" : "disabled")} successfully!";
            return RedirectToAction("Index");
        }
    }
}
