using Microsoft.EntityFrameworkCore;
using PhoneDirectoryApi.Data;
using PhoneDirectoryApi.Models.Domain;

namespace PhoneDirectoryApi.Services
{
    // Services/AutoDeleteService.cs
    // AutoDeleteService.cs
    public class AutoDeleteService : IAutoDeleteService
    {
        private readonly PhoneDirectoryContext _context;
        private readonly ILogger<AutoDeleteService> _logger;

        public AutoDeleteService(
            PhoneDirectoryContext context,
            ILogger<AutoDeleteService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<AutoDeleteSettings> GetSettingsAsync()
        {
            var settings = await _context.AutoDeleteSettings.FirstOrDefaultAsync();
            if (settings == null)
            {
                settings = new AutoDeleteSettings
                {
                    IsEnabled = false,
                    DeleteIntervalMinutes = 5,
                    ContactsToDelete = 10,
                    DeleteOnlyInactive = false
                };
                _context.AutoDeleteSettings.Add(settings);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Created default auto-delete settings");
            }
            return settings;
        }

        public async Task<AutoDeleteSettings> UpdateSettingsAsync(AutoDeleteSettings settings)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var existingSettings = await GetSettingsAsync();
                existingSettings.IsEnabled = settings.IsEnabled;
                existingSettings.DeleteIntervalMinutes = settings.DeleteIntervalMinutes;
                existingSettings.ContactsToDelete = settings.ContactsToDelete;
                existingSettings.DeleteOnlyInactive = settings.DeleteOnlyInactive;

                _context.AutoDeleteSettings.Update(existingSettings);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("Updated auto-delete settings");
                return existingSettings;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Failed to update auto-delete settings");
                throw;
            }
        }

        public async Task ToggleAutoDeleteAsync(bool isEnabled)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var settings = await GetSettingsAsync();
                settings.IsEnabled = isEnabled;
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("Auto-delete toggled to {Status}", isEnabled ? "Enabled" : "Disabled");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Failed to toggle auto-delete");
                throw;
            }
        }
    }
}
