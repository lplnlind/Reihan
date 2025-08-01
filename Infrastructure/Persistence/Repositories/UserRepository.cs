﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context) { }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _dbSet.Where(w => w.UserName == username).FirstOrDefaultAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email.Value == email);
        }

        public async Task<IEnumerable<User>> GetByIdsAsync(List<int> ids)
            => await _dbSet.Where(w => ids.Contains(w.Id)).ToListAsync();

        public async Task<bool> IsUserActiveAsync(int userId)
        {
            var user = await _dbSet.FirstOrDefaultAsync(u => u.Id == userId);
            return user?.IsActive ?? false;
        }
    }
}
