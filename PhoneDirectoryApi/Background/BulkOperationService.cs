
using PhoneDirectoryApi.FileHandlers;
using PhoneDirectoryApi.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PhoneDirectoryApi.Background
{
    public class BulkOperationService : BackgroundService
    {
        private readonly ILogger<BulkOperationService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public BulkOperationService(
            ILogger<BulkOperationService> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("BulkOperationService is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var contactService = scope.ServiceProvider.GetRequiredService<IContactService>();

                try
                {
                    _logger.LogInformation("Running background contact disable operation at {Time}", DateTime.UtcNow);

                    // Fetch all active contacts
                    var allContacts = await contactService.GetAllContactsAsync();

                    // Take the top 10 active contacts to disable
                    var contactsToDisable = allContacts
                        .Where(c => c.Status == true)
                        .Take(10)
                        .Select(c => c.Id)
                        .ToList();

                    if (contactsToDisable.Any())
                    {
                        await contactService.BulkDisableContactsAsync(contactsToDisable);
                        _logger.LogInformation("Successfully disabled {Count} contacts.", contactsToDisable.Count);
                    }
                    else
                    {
                        _logger.LogInformation("No active contacts found to disable.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while processing background task.");
                }

                // Wait 1 minute before next execution
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }

            _logger.LogInformation("BulkOperationService is stopping.");
        }
    }

    // Background/ScheduledContactCleanupService.cs
    public class ScheduledContactCleanupService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<ScheduledContactCleanupService> _logger;

        public ScheduledContactCleanupService(
            IServiceScopeFactory scopeFactory,
            ILogger<ScheduledContactCleanupService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Contact cleanup service started");

            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();
                var autoDeleteService = scope.ServiceProvider.GetRequiredService<IAutoDeleteService>();
                var contactService = scope.ServiceProvider.GetRequiredService<IContactService>();

                try
                {
                    var settings = await autoDeleteService.GetSettingsAsync();

                    if (settings.IsEnabled)
                    {
                        _logger.LogDebug("Running auto-delete cycle");

                        var ids = await contactService.GetNextDeletableContactIdsAsync(settings.ContactsToDelete,
                            settings.DeleteOnlyInactive);

                        if (ids.Any())
                        {
                            await contactService.BulkDeleteContactsAsync(ids);
                            _logger.LogInformation("Deleted {Count} contacts", ids.Count);

                        }
                        else
                        {
                            _logger.LogDebug("No contacts available for deletion");
                        }

                        await Task.Delay(
                            TimeSpan.FromMinutes(settings.DeleteIntervalMinutes),
                            stoppingToken);
                    }
                    else
                    {
                        _logger.LogDebug("Auto-delete is disabled, checking again in 1 minute");
                        await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in contact cleanup service");
                    await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
                }
            }

            _logger.LogInformation("Contact cleanup service stopped");
        }
    }
}
