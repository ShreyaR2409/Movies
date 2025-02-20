﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public  string FirstName { get; set; }
        public  string LastName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public  string Email { get; set; }
        public  string Password { get; set; }
        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public Role? Role { get; set; }
        public string ApiKey { get; set; }
    }
}
