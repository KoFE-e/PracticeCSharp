using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace PracticeCSharp.Domain.ViewModels.Car
{
    public class CarViewModel
    {
        
        public long Id { get; set; }

        [Display(Name = "Название")]
        [Required(ErrorMessage = "Введите имя")]
        [MinLength(2, ErrorMessage = "Минимальная длина должна быть более одного символа")]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        [MinLength(10, ErrorMessage = "Минимальная длина должна быть больше 10 символов")]
        public string Description { get; set; }

        [Display(Name = "Модель")]
        [Required(ErrorMessage = "Укажите модель")]
        [MinLength(2, ErrorMessage = "Минимальная длина должна быть больше двух символов")]
        public string Model { get; set; }

        [Display(Name = "Скорость")]
        [Range(1, int.MaxValue, ErrorMessage = "Скорость должна быть больше 0")]
        [Required(ErrorMessage = "Укажите скорость")]
        public double Speed { get; set; }

        [Display(Name = "Стоимость")]
        [Required(ErrorMessage = "Укажите стоимость")]
        [Range(1, int.MaxValue, ErrorMessage = "Цена должна быть больше 0")]
        public double Price { get; set; }

        [Display(Name = "Дата выпуска")]
        [Required(ErrorMessage = "Укажите дату выпуска")]
        public string DateCreate { get; set; }

        [Display(Name = "Тип автомобиля")]
        [Required(ErrorMessage = "Выберите тип")]
        public string TypeCar { get; set; }

        [Display(Name = "Фото")]
        public IFormFile Avatar { get; set; }

        public string Image { get; set; }
    }
}
