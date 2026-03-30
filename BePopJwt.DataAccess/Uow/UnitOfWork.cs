using BePopJwt.DataAccess.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.DataAccess.Uow
{
    public class UnitOfWork(AppDbContext _context) : IUnitOfWork
    {
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
