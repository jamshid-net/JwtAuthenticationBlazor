using System.ComponentModel.DataAnnotations;

namespace JwtAuthorizeTest.Shared.DTOs;
public class LoginDto
{
    [Required]
    public string UserName { get; set; }
    [Required,DataType(DataType.Password)]
    public string Password { get; set; }
}
