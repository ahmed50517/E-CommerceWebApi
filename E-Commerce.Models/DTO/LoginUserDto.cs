using System.ComponentModel.DataAnnotations;

namespace E_Commerce.DTO
{
    public class LoginUserDto
    {
        
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
      
    }
}
