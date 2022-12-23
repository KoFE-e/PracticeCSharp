using PracticeCSharp.DAL.Interfaces;
using PracticeCSharp.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticeCSharp.DAL.Repositories
{
    public class OrderRepository : IBaseRepository<Order>
    {
        private readonly ApplicationDbContext _db;

        public OrderRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task Create(Order entity)
        {
            await _db.Order.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(Order entity)
        {
            _db.Order.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public IQueryable<Order> GetAll()
        {
            return _db.Order;
        }

        public async Task<Order> Update(Order entity)
        {
            _db.Order.Update(entity);
            await _db.SaveChangesAsync();

            return entity;
        }
    }
}
