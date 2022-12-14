using System.ComponentModel.DataAnnotations;

namespace PracticeCSharp.Domain.ViewModels.Profile
{
    public class ProfileViewModel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Укажите возраст")]
        [Range(0, 100, ErrorMessage = "Диапазон возраста должен быть от 0 до 100")]
        public byte Age { get; set; }

        [Required(ErrorMessage = "Укажите адрес")]
        [MinLength(5, ErrorMessage = "Минимальная длина должна быть равна 5 символов")]
        [MaxLength(200, ErrorMessage = "Максимальная длина должна быть менее 200 символов")]
        public string Address { get; set; }

        public string UserName { get; set; }

        public string NewPassword { get; set; }
    }
}