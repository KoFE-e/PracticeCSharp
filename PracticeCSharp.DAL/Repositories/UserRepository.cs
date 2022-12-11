using System.Linq;
using System.Threading.Tasks;
using PracticeCSharp.DAL.Interfaces;
using PracticeCSharp.DAL;
using PracticeCSharp.Domain.Entity;

namespace PracticeCSharp.DAL.Repositories
{
    public class UserRepository : IBaseRepository<User>
    {
        private readonly ApplicationDbContext _db;

        public UserRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public IQueryable<User> GetAll()
        {
            return _db.User;
        }

        public async Task Delete(User entity)
        {
            _db.User.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task Create(User entity)
        {
            await _db.User.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<User> Update(User entity)
        {
            _db.User.Update(entity);
            await _db.SaveChangesAsync();

            return entity;
        }
    }
}