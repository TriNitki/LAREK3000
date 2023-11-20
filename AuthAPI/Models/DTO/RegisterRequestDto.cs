using System.ComponentModel.DataAnnotations;

namespace AuthAPI.Models.DTO
{
    public class RegisterRequestDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public string? Address { get; set; }

        public string[] Roles { get; set; }
    }
}
