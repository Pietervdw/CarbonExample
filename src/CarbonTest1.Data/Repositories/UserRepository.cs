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
using System.Text;
using System.Threading.Tasks;

namespace CarbonTest1.Data.Repositories
{
	public class UserRepository : IUserRepository, IRepositoryBase<ApplicationUser>
	{
		private readonly CarbonDbContext _context;
		private readonly Logger _logger;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly string _userId;

		public UserRepository(CarbonDbContext context, IHttpContextAccessor httpContextAccessor)
		{
			_context = context;
			_httpContextAccessor = httpContextAccessor;
			_logger = LogManager.GetLogger(this.GetType().ToString());

			if (httpContextAccessor != null && _httpContextAccessor.HttpContext != null)
				_userId = _httpContextAccessor.HttpContext.User.UserId();
		}

		public async Task<ApplicationUser> GetByRefreshToken(string refreshToken)
		{
			var user = await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
			return user;
		}

		public async Task<EntityResponse<ApplicationUser>> AddAsync(ApplicationUser entity, bool detach = false)
		{
			var response = new EntityResponse<ApplicationUser>();
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

		public async Task AddAsync(params ApplicationUser[] entities)
		{
			try
			{
				foreach (ApplicationUser item in entities)
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

		public async Task AddAsync(List<ApplicationUser> entities)
		{
			try
			{
				foreach (ApplicationUser item in entities)
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

		public async Task<bool> AnyAsync()
		{
			return await _context.Users.AnyAsync();
		}

		public async Task<bool> AnyAsync(Expression<Func<ApplicationUser, bool>> where)
		{
			return await _context.Users.AnyAsync(where);
		}

		public async Task<int> CountAsync()
		{
			return await _context.Users.CountAsync();
		}

		public async Task<int> CountAsync(Expression<Func<ApplicationUser, bool>> where)
		{
			return await _context.Users.CountAsync(where);
		}

		public ApplicationUser Get(Expression<Func<ApplicationUser, bool>> where, params Expression<Func<ApplicationUser, object>>[] navigationProperties)
		{
			ApplicationUser item = null;

			IQueryable<ApplicationUser> dbQuery = _context.Set<ApplicationUser>();

			//Apply eager loading
			foreach (Expression<Func<ApplicationUser, object>> navigationProperty in navigationProperties)
				dbQuery = dbQuery.Include<ApplicationUser, object>(navigationProperty);

			item = dbQuery
				.AsNoTracking() //Don't track changes for the item
				.FirstOrDefault(where);

			return item;
		}

		public async Task<PagedList<ApplicationUser>> GetAllAsync(int pageNumber, int pageSize, Expression<Func<ApplicationUser, bool>> where = null)
		{
			int currentPage = pageNumber;
			int skip = pageNumber == 1 ? 0 : pageNumber * pageSize;

			IQueryable<ApplicationUser> dbQuery = _context.Set<ApplicationUser>();
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

			var dataList = new PagedList<ApplicationUser>(dbQuery.ToList(), totalRows, currentPage, pageSize, filteredTotalRows);
			return dataList;
		}

		public async Task<ApplicationUser> GetAsync(Expression<Func<ApplicationUser, bool>> where, bool noTracking = true, params Expression<Func<ApplicationUser, object>>[] navigationProperties)
		{
			IQueryable<ApplicationUser> dbQuery = _context.Set<ApplicationUser>();

			foreach (Expression<Func<ApplicationUser, object>> navigationProperty in navigationProperties)
				dbQuery = dbQuery.Include<ApplicationUser, object>(navigationProperty);

			if (!noTracking)
				return await dbQuery.FirstOrDefaultAsync(where);

			return await dbQuery.AsNoTracking().FirstOrDefaultAsync(where);
		}

		public async Task<List<ApplicationUser>> GetAllAsync(Expression<Func<ApplicationUser, bool>> where = null)
		{
			IQueryable<ApplicationUser> dbQuery = _context.Set<ApplicationUser>();

			if (where != null)
				return dbQuery.Where(where).ToList();

			return dbQuery.ToList();
		}


		public void Remove(ApplicationUser entity)
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

		public void Remove(params ApplicationUser[] entities)
		{
			foreach (ApplicationUser item in entities)
			{
				_context.Entry(item).State = EntityState.Deleted;
			}
			_context.SaveChanges();
		}

		public void RemoveRange(Expression<Func<ApplicationUser, bool>> where)
		{
			IQueryable<ApplicationUser> dbQuery = _context.Set<ApplicationUser>();
			var range = dbQuery.Where(where);
			_context.Set<ApplicationUser>().RemoveRange(range);
			_context.SaveChanges();
		}

		public void Update(params ApplicationUser[] entities)
		{
			try
			{
				foreach (ApplicationUser item in entities)
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

		public async Task UpdateAsync(ApplicationUser entity, bool detach = false)
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


	}
}
