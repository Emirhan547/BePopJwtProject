using BePopJwt.Entity.Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;

namespace BePopJwt.DataAccess.Interceptors
{
    public class AuditInterceptor : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
        {
            var context = eventData.Context;

            if (context == null)
                return base.SavingChangesAsync(eventData, result, cancellationToken);

            foreach (var entry in context.ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property(x => x.CreatedDate).CurrentValue = DateTime.UtcNow;
                    entry.Property(x => x.UpdatedDate).IsModified = false;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property(x => x.UpdatedDate).CurrentValue = DateTime.UtcNow;
                    entry.Property(x => x.CreatedDate).IsModified = false;
                }
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}