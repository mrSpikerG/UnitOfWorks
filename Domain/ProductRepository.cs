using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain {
    internal class ProductRepository : IRepoitory<Product> {
        public void Delete(Product entity) {
            this
        }

        public IEnumerable FindAll() {
            throw new NotImplementedException();
        }

        public Product FindById(params object[] values) {
            throw new NotImplementedException();
        }

        public void Insert(Product entity) {
            throw new NotImplementedException();
        }

        public void Update(Product entity) {
            throw new NotImplementedException();
        }
    }
}
