using Microsoft.EntityFrameworkCore;
using OCMS.Domain.Interfaces;
using OCMS.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCMS.Infrastructure.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _db;
        protected readonly DbSet<T> _set;

        public BaseRepository(AppDbContext db)
        {
            _db = db;
            _set = db.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default)
            => await _set.FindAsync([id], ct);

        public virtual async Task AddAsync(T entity, CancellationToken ct = default)
            => await _set.AddAsync(entity, ct);

        public virtual void Update(T entity) => _set.Update(entity);

        public virtual void Remove(T entity) => _set.Remove(entity);

        public virtual IQueryable<T> Query() => _set.AsQueryable();
    }
}
