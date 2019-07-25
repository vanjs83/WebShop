using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShop.Data;
using WebShop.Models;

namespace WebShop.Common
{
    public class ModelPaging
    {
        private readonly DbShop _context;

        public ModelPaging(DbShop context)
        {
            _context = context;
            Data =  _context.ProductCategory.Include(x => x.Product).Include(x => x.Category).ToList();
            Count = Data.Count;
        }

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int Count { get; set; }
        public int PageSize { get; set; } = 1;

        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));

        public List<ProductCategory> Data { get; set; } = new List<ProductCategory>();
        public bool ShowPrevious => CurrentPage > 1;
        public bool ShowNext => CurrentPage < TotalPages;
        public bool ShowFirst => CurrentPage != 1;
        public bool ShowLast => CurrentPage != TotalPages;


        public async Task OnGetAsync()
        {
            Data = await _context.ProductCategory.Include(x => x.Product).Include(x => x.Category).ToListAsync();
            Count = Data.Count;
        }



    }
}
