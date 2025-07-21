using System.ComponentModel.DataAnnotations;

namespace PhoneDirectoryApi.Models.ViewModels
{
    public class AutoDeleteSettingsViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Enable Auto-Delete")]
        public bool IsEnabled { get; set; }

        [Required]
        [Range(1, 1440)]
        [Display(Name = "Delete Interval (minutes)")]
        public int DeleteIntervalMinutes { get; set; }

        [Required]
        [Range(1, 1000)]
        [Display(Name = "Contacts to Delete Per Batch")]
        public int ContactsToDelete { get; set; }

        [Display(Name = "Delete Only Inactive Contacts")]
        public bool DeleteOnlyInactive { get; set; }
    }
}
