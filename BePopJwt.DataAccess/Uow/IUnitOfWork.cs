using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.DataAccess.Uow
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
    }
}
