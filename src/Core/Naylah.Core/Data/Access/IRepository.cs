namespace Naylah.Data.Access
{
    public interface IRepository<TEntity, TIdentifier> :
        IReadableRepository<TEntity, TIdentifier>,
        IQueryableRepository<TEntity, TIdentifier>
        where TEntity : IEntity
    {
        void Create(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        void Delete(TIdentifier id);

        void Save();
    }
}