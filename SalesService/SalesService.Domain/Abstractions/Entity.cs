namespace SalesService.Domain.Abstractions
{
    public interface IEntity
    {
        public Guid Id { get; init; }
        public DateTime CreationDate { get; init; }
    }
}
