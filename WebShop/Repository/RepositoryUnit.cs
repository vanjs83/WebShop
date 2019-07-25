using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShop.Data;
using WebShop.Models;

namespace WebShop.Repository
{
    public class RepositoryUnit
    {
        private readonly DbShop _context;
        public RepositoryUnit( DbShop context)
        {
            _context = context;
        }


        public IEnumerable<MeasuringUnit> GetAllMesaurnigUnit()
        {

            return _context.MeasuringUnit.ToList();
        }

    }
}
