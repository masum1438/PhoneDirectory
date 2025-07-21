using PhoneDirectoryApi.Models.Domain;

namespace PhoneDirectoryApi.Services
{
    public interface IAutoDeleteService
    {
        //Task<AutoDeleteSettings> GetSettingsAsync();
        ////Task UpdateSettingsAsync(AutoDeleteSettings settings);
        //Task<AutoDeleteSettings> UpdateSettingsAsync(AutoDeleteSettings settings);
        Task<AutoDeleteSettings> GetSettingsAsync();
        Task<AutoDeleteSettings> UpdateSettingsAsync(AutoDeleteSettings settings);
        Task ToggleAutoDeleteAsync(bool isEnabled);

    }
}
