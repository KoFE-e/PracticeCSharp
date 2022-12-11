using System.Linq;
using System.Threading.Tasks;
using PracticeCSharp.DAL.Interfaces;
using PracticeCSharp.DAL;
using PracticeCSharp.Domain.Entity;

namespace Automarket.DAL.Repositories
{
    public class ProfileRepository : IBaseRepository<Profile>
    {
        private readonly ApplicationDbContext _dbContext;

        public ProfileRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Create(Profile entity)
        {
            await _dbContext.Profile.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public IQueryable<Profile> GetAll()
        {
            return _dbContext.Profile;
        }

        public async Task Delete(Profile entity)
        {
            _dbContext.Profile.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Profile> Update(Profile entity)
        {
            _dbContext.Profile.Update(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }
    }
}