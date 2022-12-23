using System.ComponentModel.DataAnnotations;

namespace PracticeCSharp.Domain.ViewModels.Account
{
    public class ChangePasswordViewModel
    {
        public string UserName { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        [MinLength(4, ErrorMessage = "Длина пароля должна быть более 3 символов")]
        public string NewPassword { get; set; }
    }
}