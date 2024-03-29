﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.EntityFrameworkCore;
using Naylah.Data;
using Naylah.Data.Access;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Naylah.Data.Providers.CosmosDB;

namespace Naylah.ConsoleAspNetCore.Customizations
{
    //public class StringTableDataServiceV2<TEntity>
    //    : TableDataService2<TEntity, string>
    //    where TEntity : class, IEntity<string>, IModifiable, new()
    //{
    //    public StringTableDataServiceV2(
    //        IRepository<TEntity, string> repository,
    //        IUnitOfWork unitOfWork) : base(repository, unitOfWork)
    //    {
    //    }

    //    public StringTableDataServiceV2(
    //        IRepository<TEntity, string> repository,
    //        IUnitOfWork unitOfWork,
    //        Domain.Abstractions.IHandler<Notification> notificationsHandler) : base(repository, unitOfWork, notificationsHandler)
    //    {
    //    }

    //    protected override Task<TEntity> FindByIdAsync(string identifier)
    //    {
    //        return FindByAsync(x => x.Id == identifier);
    //    }

    //    protected override Task GenerateId(TEntity entity)
    //    {
    //        entity.GenerateId();
    //        return Task.FromResult(1);
    //    }
    //}

    public class CosmosStringAppTableDataService<TEntity, TModel> : StringAppTableDataService<TEntity, TModel>
       where TEntity : class, IEntity<string>, IModifiable, ISoftDeletable, IEntityUpdate<TModel>, new()
       where TModel : class, IEntity<string>, new()
    {
        public CosmosStringAppTableDataService(IMapper mapper, IUnitOfWork _unitOfWork, IRepository<TEntity> repository) : base(mapper, _unitOfWork, repository)
        {
        }

        protected override async Task<TEntity> FindByAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var list = await GetEntities().Where(predicate).ToFeedIterator().ToCosmosListAsync();
            return  list.FirstOrDefault();
        }
    }

    public class StringAppTableDataService<TEntity, TModel> : StringTableDataService<TEntity, TModel>
        where TEntity : class, IEntity<string>, IModifiable, ISoftDeletable, IEntityUpdate<TModel>, new()
        where TModel : class, IEntity<string>, new()
    {
        public StringAppTableDataService(IMapper mapper, IUnitOfWork _unitOfWork, IRepository<TEntity> repository) : base(repository, _unitOfWork)
        {
            Mapper = mapper;
        }

        public IMapper Mapper { get; }

        protected override IQueryable<TModel> Project(IQueryable<TEntity> entities)
        {
            return entities.AsNoTracking().ProjectTo<TModel>(Mapper.ConfigurationProvider);
        }
    }
}
