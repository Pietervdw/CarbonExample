using CarbonTest1.Models;
using CarbonTest1.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CarbonTest1.Data.Interfaces
{
	public interface IRepositoryBase<T>
	{
		Task<EntityResponse<T>> AddAsync(T entity, bool detach = false);
		Task AddAsync(params T[] entities);
		Task AddAsync(List<T> entities);

		Task<T> GetAsync(Expression<Func<T, bool>> where, bool noTracking = true, params Expression<Func<T, object>>[] navigationProperties);
		T Get(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties);

		Task<PagedList<T>> GetAllAsync(int pageNumber, int pageSize, Expression<Func<T, bool>> where = null);
		Task<List<T>> GetAllAsync(Expression<Func<T, bool>> where = null);

		Task UpdateAsync(T entity, bool detach = false);
		void Update(params T[] entities);

		void Remove(T entity);
		void Remove(params T[] entities);
		void RemoveRange(Expression<Func<T, bool>> where);

		Task<int> CountAsync();
		Task<int> CountAsync(Expression<Func<T, bool>> where);

		Task<bool> AnyAsync();
		Task<bool> AnyAsync(Expression<Func<T, bool>> where);
	}
}
