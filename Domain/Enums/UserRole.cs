﻿using System.ComponentModel.DataAnnotations;

namespace Domain.Enums
{
    public enum UserRole
    {
        [Display(Name ="ادمین")]
        Admin,

        [Display(Name ="خریدار")]
        Customer
    }
}
