namespace Naylah.Data.Access
{
    public interface IRepository<TEntity> : IReadOnlyRepository<TEntity> where TEntity : IEntity
    {
        void Create(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        void Delete(object id);

        void Save();
    }
}