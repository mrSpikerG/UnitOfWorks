using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models {
    public class UserInfo {
        public UserInfo(string name, string mail, IList<string> roles) {
            this.Name = name;
            this.Mail = mail;
            this.Roles = roles;
        }

        public string Name { get; set; }
        public string Mail { get; set; }
        public IList<string> Roles { get; set; }
    }
}
