using System.ComponentModel.DataAnnotations;

namespace PracticeCSharp.Domain.ViewModels.User
{
    public class UserViewModel
    {
        [Display(Name = "Id")]
        public long Id { get; set; }

        [Required(ErrorMessage = "Укажите роль")]
        [Display(Name = "Роль")]
        public string Role { get; set; }

        [Display(Name = "Логин")]
        [Required(ErrorMessage = "Укажите логин")]
        [MaxLength(20, ErrorMessage = "Имя должно иметь длину меньше 20 символов")]
        [MinLength(3, ErrorMessage = "Имя должно иметь длину больше 3 символов")]
        public string Name { get; set; }

        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Укажите пароль")]
        [MinLength(4, ErrorMessage = "Пароль должен иметь длину не менее 4 символов")]
        public string Password { get; set; }
    }
}