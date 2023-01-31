using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessEF.Interfaces {
    internal interface IUnitOfWorks {
        public ICategoryRepository Category { get; set; }
        public IProductRepository Product {get; set;}
    }
}
