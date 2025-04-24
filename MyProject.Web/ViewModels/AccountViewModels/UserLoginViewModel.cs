using System.ComponentModel.DataAnnotations;
namespace MyProject.Web.ViewModels.AccountViewModels
{
    public class UserLoginViewModel
    {
        [Required, EmailAddress] public string Email { get; set; }
        [Required, DataType(DataType.Password)] public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}