using BePopJwt.Entity.Entities.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.DataAccess.Repositories.GenericRepositories
{
    public interface IRepository<TEntity>where TEntity : BaseEntity
    {
        Task<TEntity?>GetByIdAsync(int id);
        Task<IList<TEntity>> GetAllAsync();
        Task CreateAsnc(TEntity entity);
        void Delete(TEntity entity);
        void Update(TEntity entity);
    }
}
