﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractEntities.DTOs
{
    public class UserRegistrationDTO
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [PasswordPropertyText]
        public string Password { get; set; }
        [Required]
        [PasswordPropertyText]
        public string PasswordCheck { get; set; }
    }
}
