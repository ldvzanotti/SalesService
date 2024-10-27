namespace SalesService.Domain.Abstractions
{
    public abstract class Entity
    {
        public Guid Id { get; init; }
        public DateTime CreationDate { get; init; }

        protected Entity()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }
    }
}
