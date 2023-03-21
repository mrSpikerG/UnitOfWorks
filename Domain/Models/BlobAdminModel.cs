using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models {
    public class BlobAdminModel {
        public string URI { get; set; }
        public string Name { get; set; }
        public DateTimeOffset? CreationTime { get; set; }
    }
}
