namespace Naylah
{
    public interface IEntity
    {
    }

    public interface IEntity<TIdentifier> : IEntity, IModifiable
    {
        TIdentifier Id { get; set; }
    }
}