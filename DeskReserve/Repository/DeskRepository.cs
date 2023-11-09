﻿using DeskReserve.Data.DBContext;
using DeskReserve.Data.DBContext.Entity;
using Microsoft.EntityFrameworkCore;
using DeskReserve.Exception;

namespace DeskReserve.Repository
{
	public class DeskRepository : IDeskRepository
	{
		private readonly ApplicationDbContext _context;

		public DeskRepository(ApplicationDbContext context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public async Task<IEnumerable<Desk>> GetAll()
		{
			return await _context.Desks.ToListAsync() ?? throw new EntityNotFoundException();
		}

		public async Task<Desk> GetById(Guid id)
		{
			return await _context.Desks.FindAsync(id) ?? throw new EntityNotFoundException();
		}

		public async Task<bool> Update(Desk desk)
		{
			var existingDesk = await _context.Desks.FindAsync(desk.DeskId) ?? throw new EntityNotFoundException();

			_context.Entry(desk).State = EntityState.Modified;

			return await _context.SaveChangesAsync() > 0;
		}


		public async Task<bool> Create(Desk desk)
		{
			_context.Desks.AddAsync(desk);

			return await _context.SaveChangesAsync() > 0;
		}

		public async Task<bool> Delete(Guid id)
		{
			var desk = await _context.Desks.FindAsync(id) ?? throw new EntityNotFoundException();

			_context.Desks.Remove(desk);

			return await _context.SaveChangesAsync() > 0;
		}
	}
}
