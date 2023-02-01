using DataAccessEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces {
    public interface ICategoryConnectionRepository : IGenericRepoitory<CategoryConnection> {

        int GetCategoryByProduct(int id);
    }
}
