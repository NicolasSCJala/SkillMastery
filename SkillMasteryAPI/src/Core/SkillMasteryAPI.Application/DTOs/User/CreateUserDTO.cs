﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillMasteryAPI.Application.DTOs.User
{
    public class CreateUserDTO
    {
        public  string FirstName { get; set; } = string.Empty;
        public  string LastName { get; set; } = string.Empty;
        public   string Email { get; set; } = string.Empty;
    }
}
