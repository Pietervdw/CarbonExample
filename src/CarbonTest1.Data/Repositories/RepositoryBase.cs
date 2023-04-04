using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NLog;
using CarbonTest1.Data.Interfaces;
using CarbonTest1.Infrastructure.Identity;
using CarbonTest1.Models;
using CarbonTest1.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CarbonTest1.Data.Repositories
{
	public abstract class RepositoryBase<T> : IDisposable, IRepositoryBase<T> where T : EntityBase
	{
		private readonly EosDbContext _context;
		private readonly Logger _logger;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly string _userId;

		public RepositoryBase(EosDbContext context, IHttpContextAccessor httpContextAccessor)
		{
			_context = context;
			_httpContextAccessor = httpContextAccessor;
			_logger = LogManager.GetLogger(this.GetType().ToString());

			if (httpContextAccessor != null && _httpContextAccessor.HttpContext != null)
				_userId = _httpContextAccessor.HttpContext.User.UserId();

		}

		public async Task<EntityResponse<T>> AddAsync(T entity, bool detach = false)
		{
			var response = new EntityResponse<T>();
			try
			{
				_context.Entry(entity).State = EntityState.Added;
				await _context.SaveChangesAsync();

				if (detach)
					_context.Entry(entity).State = EntityState.Detached;

				response.Entity = entity;
				return response;
			}
			catch (Exception ex)
			{
				var errorMessage = ex.Message;
				if (ex.InnerException != null)
					errorMessage = ex.InnerException.Message;

				_logger.Error(ex, errorMessage);				
				response.AddError(errorMessage);
			}
			return response;
		}

		public async Task AddAsync(params T[] entities)
		{
			try
			{
				foreach (T item in entities)
				{
					_context.Entry(item).State = EntityState.Added;
				}
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				_logger.Error(ex, ex.Message);
			}
		}

		public async Task AddAsync(List<T> entities)
		{
			try
			{
				foreach (T item in entities)
				{
					_context.Entry(item).State = EntityState.Added;
				}
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				_logger.Error(ex, ex.Message);
			}
		}	

		public T Get(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] navigationProperties)
		{
			T item = null;

			IQueryable<T> dbQuery = _context.Set<T>();

			//Apply eager loading
			foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
				dbQuery = dbQuery.Include<T, object>(navigationProperty);

			item = dbQuery
				.AsNoTracking() //Don't track changes for the item
				.FirstOrDefault(where);

			return item;
		}

		public async Task<T> GetAsync(Expression<Func<T, bool>> where, bool noTracking = true, params Expression<Func<T, object>>[] navigationProperties)
		{	
			IQueryable<T> dbQuery = _context.Set<T>();

			foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
				dbQuery = dbQuery.Include<T, object>(navigationProperty);

			if (!noTracking)
				return await dbQuery.FirstOrDefaultAsync(where);

			return await dbQuery.AsNoTracking().FirstOrDefaultAsync(where);
		}

		public async Task<PagedList<T>> GetAllAsync(int pageNumber, int pageSize, Expression<Func<T, bool>> where = null)
		{
			int currentPage = pageNumber;
			int skip = pageNumber == 1 ? 0 : pageNumber * pageSize;

			IQueryable<T> dbQuery = _context.Set<T>();
			int totalRows = await dbQuery.CountAsync();
			int filteredTotalRows = totalRows;
			if (where != null)
			{
				dbQuery = dbQuery.Where(where);
				filteredTotalRows = await dbQuery.CountAsync();
			}

			if (filteredTotalRows > pageSize && pageNumber != 1)
				skip = pageSize;

			dbQuery = dbQuery.Skip(skip).Take(pageSize);

			var dataList = new PagedList<T>(dbQuery.ToList(), totalRows, currentPage, pageSize, filteredTotalRows);
			return dataList;
		}

		public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> where = null)
		{
			IQueryable<T> dbQuery = _context.Set<T>();
			if (where != null)
				dbQuery = dbQuery.Where(where);

			return await dbQuery.ToListAsync();
		}

		public async Task UpdateAsync(T entity, bool detach = false)
		{
			try
			{
				_context.Entry(entity).State = EntityState.Modified;
				await _context.SaveChangesAsync();

				if (detach)
					_context.Entry(entity).State = EntityState.Detached;
			}
			catch (Exception ex)
			{
				_logger.Error(ex, ex.Message);
			}
		}

		public void Update(params T[] entities)
		{
			try
			{				
				foreach (T item in entities)
				{		
					_context.Entry(item).State = EntityState.Modified;
				}
				_context.SaveChanges();
			}
			catch (Exception ex)
			{
				_logger.Error(ex, ex.Message);
			}
		}

		public void Remove(T entity)
		{
			try
			{
				_context.Entry(entity).State = EntityState.Deleted;
				_context.SaveChanges();
			}
			catch (Exception ex)
			{
				_logger.Error(ex, ex.Message);
			}
		}

		public void Remove(params T[] entities)
		{
			foreach (T item in entities)
			{
				_context.Entry(item).State = EntityState.Deleted;
			}
			_context.SaveChanges();
		}

		public void RemoveRange(Expression<Func<T, bool>> where)
		{
			IQueryable<T> dbQuery = _context.Set<T>();
			var range = dbQuery.Where(where);
			_context.Set<T>().RemoveRange(range);
			_context.SaveChanges();
		}

		public async Task<int> CountAsync()
		{
			IQueryable<T> dbQuery = _context.Set<T>();
			var count = await dbQuery.CountAsync();
			return count;
		}

		public async Task<int> CountAsync(Expression<Func<T, bool>> where)
		{
			IQueryable<T> dbQuery = _context.Set<T>();
			var count = await dbQuery.CountAsync(where);
			return count;
		}

		public async Task<bool> AnyAsync()
		{
			IQueryable<T> dbQuery = _context.Set<T>();
			return await dbQuery.AnyAsync();
		}

		public async Task<bool> AnyAsync(Expression<Func<T, bool>> where)
		{
			IQueryable<T> dbQuery = _context.Set<T>();
			return await dbQuery.AnyAsync(where);
		}

		public void Dispose()
		{
			_context.Dispose();
		}

		
	}
}
