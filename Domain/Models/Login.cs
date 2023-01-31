using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Models {
    public class Login {

        [Required(ErrorMessage = "User name is  Required! ")]
        public string? UserName { get; set; }

        [JsonIgnore]
        [Required(ErrorMessage = "User password is  Required! ")]
        public string? Password { get; set; }
    }
}
