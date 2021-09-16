using System.Collections.Generic;
using System.Threading.Tasks;

namespace Naylah.Data
{
    public class RangedTableDataServiceOperations<TEntity, TModel, TIdentifier>
        where TEntity : class, IEntity<TIdentifier>, IModifiable, ISoftDeletable, IEntityUpdate<TModel>, new()
        where TModel : class, IEntity<TIdentifier>, new()
    {
        private readonly TableDataService<TEntity, TModel, TIdentifier> service;
        private readonly TableDataServiceWrapper<TEntity, TModel, TIdentifier> wrapper;

        public RangedTableDataServiceOperations(TableDataService<TEntity, TModel, TIdentifier> service)
        {
            this.service = service;
            wrapper = service.CreateWrapper();
        }

        public IEnumerable<TEntity> ToEntities(IEnumerable<TModel> models, UpsertType upsertType)
        {
            var es = new List<TEntity>();
            foreach (var model in models)
            {
                var e = wrapper.CreateEntity(model, upsertType);
                wrapper.UpdateEntity(e, model, upsertType);

                if (upsertType == UpsertType.Insert)
                {
                    wrapper.GenerateId(e);
                }

                es.Add(e);
            }

            return es;
        }

        public IEnumerable<TEntity> ToEntities(IEnumerable<TIdentifier> identifiers)
        {
            var es = new List<TEntity>();
            foreach (var identifier in identifiers)
            {
                es.Add(service.CreateEntity(identifier));
            }

            return es;
        }


        public IEnumerable<TModel> ToModels(IEnumerable<TEntity> entities)
        {
            var ms = new List<TModel>();
            foreach (var entity in entities)
            {
                ms.Add(wrapper.ToModel(entity));
            }
            return ms;
        }

        public async Task<IEnumerable<TModel>> AddRangeAsync(IEnumerable<TModel> models)
        {
            var imp = TableDataServiceExtensions.GetImplementation<ICommandRangeRepository<TEntity>>(service.CreateWrapper().Repository);
            var es = ToEntities(models, UpsertType.Insert);
            var entities = await imp.AddAsync(es);

            await wrapper.CommitAsync();

            return ToModels(entities);
        }

        public async Task<IEnumerable<TModel>> EditRangeAsync(IEnumerable<TModel> models)
        {
            var imp = TableDataServiceExtensions.GetImplementation<ICommandRangeRepository<TEntity>>(service.CreateWrapper().Repository);
            var es = ToEntities( models, UpsertType.Update);
            var entities = await imp.EditAsync(es);

            await wrapper.CommitAsync();

            return ToModels(entities);
        }

        public async Task RemoveRangeAsync(IEnumerable<TIdentifier> identifiers)
        {
            var imp = TableDataServiceExtensions.GetImplementation<ICommandRangeRepository<TEntity>>(service.CreateWrapper().Repository);
            var es = ToEntities(identifiers);

            await imp.RemoveAsync(es);
            await wrapper.CommitAsync();
        }

    }


    public static class RangedTableDataServiceOperationsExtensions
    {
        public static async Task<IEnumerable<TModel>> AddRangeAsync<TEntity, TModel, TIdentifier>(this TableDataService<TEntity, TModel, TIdentifier> service, IEnumerable<TModel> models)
            where TEntity : class, IEntity<TIdentifier>, IModifiable, ISoftDeletable, IEntityUpdate<TModel>, new()
            where TModel : class, IEntity<TIdentifier>, new()
        {
            var r = new RangedTableDataServiceOperations<TEntity, TModel, TIdentifier>(service);
            return await r.AddRangeAsync(models);
        }

        public static async Task<IEnumerable<TModel>> EditRangeAsync<TEntity, TModel, TIdentifier>(this TableDataService<TEntity, TModel, TIdentifier> service, IEnumerable<TModel> models)
            where TEntity : class, IEntity<TIdentifier>, IModifiable, ISoftDeletable, IEntityUpdate<TModel>, new()
            where TModel : class, IEntity<TIdentifier>, new()
        {
            var r = new RangedTableDataServiceOperations<TEntity, TModel, TIdentifier>(service);
            return await r.EditRangeAsync(models);
        }

        public static async Task RemoveRangeAsync<TEntity, TModel, TIdentifier>(this TableDataService<TEntity, TModel, TIdentifier> service, IEnumerable<TIdentifier> identifiers)
            where TEntity : class, IEntity<TIdentifier>, IModifiable, ISoftDeletable, IEntityUpdate<TModel>, new()
            where TModel : class, IEntity<TIdentifier>, new()
        {
            var r = new RangedTableDataServiceOperations<TEntity, TModel, TIdentifier>(service);
            await r.RemoveRangeAsync(identifiers);
        }


    }
}
