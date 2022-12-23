﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticeCSharp.Domain.ViewModels.Order
{
    public class OrderViewModel
    {
        public long Id { get; set; }

        public string CarName { get; set; }

        public string Model { get; set; }

        public double Speed { get; set; }

        public string TypeCar { get; set; }

        public string Address { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public string DateCreate { get; set; }

        public string Avatar { get; set; }

    }
}
