﻿using DataAccessEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces {
    public interface ICategoryRepository : IGenericRepoitory<Category> {

       
        public Category GetMostPopular();
    }
}
