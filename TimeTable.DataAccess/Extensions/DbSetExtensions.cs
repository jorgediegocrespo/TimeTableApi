using Microsoft.EntityFrameworkCore;
using TimeTable.DataAccess.Contracts.Entities.Base;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;

namespace TimeTable.DataAccess.Extensions
{
    public static class DbSetExtensions
    {
        public static async Task<EntityEntry<T>> RemoveConcurrently<T>(this DbSet<T> dbSet, int id, byte[] rowVersion) where T : BaseEntity
        {
            var toRemove = await dbSet.FirstOrDefaultAsync(x => x.Id == id);
            if (toRemove?.RowVersion.SequenceEqual(rowVersion) != true)
                throw new DbUpdateConcurrencyException();

            return dbSet.Remove(toRemove);
        }
    }
}
