﻿using System.ComponentModel.DataAnnotations;

namespace AuthentcationServiceForTradingMarket.Model
{
    public class RegisterModel
    {
        [Required, StringLength(50)]
        public string FirstName { get; set; }

        [Required, StringLength(50)]
        public string LastName { get; set; }
        [Required, StringLength(100)]
        public string Username { get; set; }

        [Required, StringLength(128)]
        public string Email { get; set; }

        [Required, MinLength(8)]
        public string password { get; set; }

        [Required, Compare("password")]
        public string ConfirmPassword { get; set; }
    }
}