using System.ComponentModel.DataAnnotations;

namespace user_service.Models
{
    public class UserFormViewModel
    {
        [Required]
        public string DisplayName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Avatar { get; set; }

        public bool IsEnabled { get; set; }
    }
}
