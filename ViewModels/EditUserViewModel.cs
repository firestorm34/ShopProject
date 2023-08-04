using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopProject.ViewModels
{
    public class EditUserViewModel
    {
        public int Id { get; set; }
        [DataType("E-mail")]
        public string Email { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        [DataType("Password")]
        public string Password { get; set; }
        [DataType("Password")]
        public string NewPassword { get; set; }
        public bool IsPasswordChanged { get; set; } = false;

    }
}
