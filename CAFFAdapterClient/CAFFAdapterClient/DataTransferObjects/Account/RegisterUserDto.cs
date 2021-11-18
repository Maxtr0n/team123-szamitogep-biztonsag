using System.ComponentModel.DataAnnotations;

namespace CAFFAdapterClient.DataTransferObjects.Account
{
    public class RegisterUserDto
    {
        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Lastname { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
