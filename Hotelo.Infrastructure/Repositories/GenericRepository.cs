using System.Linq.Expressions;
using Hotelo.Core.Interfaces.Repositories;
using Hotelo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hotelo.Infrastructure.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly HoteloDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(HoteloDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);
    public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();
    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> p) => await _dbSet.Where(p).ToListAsync();
    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> p) => await _dbSet.AnyAsync(p);
    public async Task<int> CountAsync(Expression<Func<T, bool>>? p = null) => p == null ? await _dbSet.CountAsync() : await _dbSet.CountAsync(p);

    public async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }
}