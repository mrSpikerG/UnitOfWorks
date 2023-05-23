using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models {
    public class AWSPicture {
        public DateTime LastModified { get; set; }
        public string Uri { get; set; }
        public long Size { get; set; }

        public AWSPicture(DateTime lastModified, string uri, long size) {
            LastModified = lastModified;
            Uri = uri;
            Size = size;
        }
    }
}
