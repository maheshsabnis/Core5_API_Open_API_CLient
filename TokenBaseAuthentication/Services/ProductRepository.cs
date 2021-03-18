using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TokenBaseAuthentication.Models;

namespace TokenBaseAuthentication.Services
{
    public class ProductRepository : IRepository<Product, int>
    {
        MyAppDbContext ctx;
        public ProductRepository(MyAppDbContext ctx)
        {
            this.ctx = ctx;
        }
        public Product Create(Product entity)
        {
            ctx.Products.Add(entity);
            ctx.SaveChanges();
            return entity;
        }

        public IEnumerable<Product> Get()
        {
            return ctx.Products.ToList();
        }

        public Product Get(int id)
        {
            return ctx.Products.Find(id);
        }
    }
}
