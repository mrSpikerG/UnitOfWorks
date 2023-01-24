using Azure.Core;
using Domain;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessEF {
    internal class ProductRepository : IRepoitory<Product> {

        private ProductLibContext Context;

        public ProductRepository(ProductLibContext context) {
            this.Context = context;
        }
        public void Delete(Product entity) {
            this.Context.Products.Remove(entity);
            this.Context.SaveChangesAsync();
        }

        public IEnumerable FindAll() {
            return this.Context.Products;
        }

        public Product FindById(params object[] id) {
            return this.Context.Products.First(x => x.Id == (int)id[0]);
        }

        public void Insert(Product entity) {
            this.Context.Products.Add(entity);
            this.Context.SaveChangesAsync();
        }

        public void Update(Product entity) {
            Product temp = this.Context.Products.First(x => x.Id == entity.Id);
            temp.Name = entity.Name;
            temp.Cost = entity.Cost;
            temp.Type = entity.Type;
            this.Context.SaveChangesAsync();
        }
    }
}
