using System.ComponentModel.DataAnnotations;

namespace DeliveryAPI.Models.DTO.AuthDTO
{
    public class LoginRequestDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
