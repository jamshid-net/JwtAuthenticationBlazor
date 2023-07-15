using System.ComponentModel.DataAnnotations;

namespace JwtAuthorizeTest.Shared.DTOs;
public class RegisterDto
{
    [Required(ErrorMessage ="User name is required!")]
    public string UserName { get; set; }

    [Required,DataType(DataType.Password)]
    public string Password { get; set; }
    [Required]
    [Compare(nameof(Password),ErrorMessage = "The password and confirmation password do not match")]
    public string ConfirmPassword { get;set; }

}
