using Marten;
using SalesService.Domain.Abstractions;
using SalesService.Domain.Aggregates.Orders;
using SalesService.Domain.Aggregates.Products;
using SalesService.Domain.Aggregates.SalesRepresentatives;
using System.Linq.Expressions;

namespace SalesService.Persistence
{
    public class UnitOfWork(
        IDocumentStore documentStore,
        IQuerySession querySession)
    {
        public async Task AddAsync<T>(T entity, CancellationToken cancellationToken = default) where T : Entity
        {
            using var session = await documentStore.LightweightSerializableSessionAsync(cancellationToken);
            session.Store(entity);
            session.Events.StartStream<T>(entity.Id);
            await session.SaveChangesAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<T>> ListAsync<T>(Expression<Func<T, bool>> filter = null, CancellationToken cancellationToken = default) where T : Entity
        {
            if (filter is not null)
            {
                return await querySession.Query<T>().Where(filter).ToListAsync(cancellationToken);
            }

            return await querySession.Query<T>().ToListAsync(cancellationToken);
        }

        public async Task<T> GetByIdAsync<T>(Guid id, CancellationToken cancellationToken = default) where T : Entity
        {
            return await querySession.Query<T>().FirstOrDefaultAsync(item => item.Id.Equals(id), cancellationToken);
        }

        public async Task<IReadOnlyList<T>> GetByIdsAsync<T>(IEnumerable<Guid> ids, CancellationToken cancellationToken = default) where T : Entity
        {
            return await querySession.Query<T>().Where(item => ids.Contains(item.Id)).ToListAsync(cancellationToken);
        }

        public async Task UpdateAsync(Entity entity, IEntityEvent entityEvent, CancellationToken cancellationToken = default)
        {
            using var session = await documentStore.LightweightSerializableSessionAsync(cancellationToken);
            session.Events.Append(entity.Id, entityEvent);
            await session.SaveChangesAsync(cancellationToken);
        }

        public void SeedData()
        {
            using var session = documentStore.LightweightSession();

            if (!session.Query<Product>().Any())
            {
                session.Store(InitialData.Products);
            }

            if (!session.Query<SalesRepresentative>().Any())
            {
                session.Store(InitialData.SalesRepresentatives);
            }

            if (!session.Query<Order>().Any())
            {
                session.Store(InitialData.Orders);
            }

            session.SaveChangesAsync();
        }
    }
}
