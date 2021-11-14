using System.ComponentModel.DataAnnotations;

namespace CAFFAdapterClient.DataTransferObjects.Account
{
    public class LoginUserDto
    {
        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
