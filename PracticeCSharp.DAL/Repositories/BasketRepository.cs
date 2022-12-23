using PracticeCSharp.DAL.Interfaces;
using PracticeCSharp.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticeCSharp.DAL.Repositories
{
    public class BasketRepository : IBaseRepository<Basket>
    {

        private readonly ApplicationDbContext _db;

        public BasketRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task Create(Basket entity)
        {
            await _db.Basket.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(Basket entity)
        {
            _db.Basket.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public IQueryable<Basket> GetAll()
        {
            return _db.Basket;
        }

        public Task<Basket> Update(Basket entity)
        {
            throw new NotImplementedException();
        }
    }
}
