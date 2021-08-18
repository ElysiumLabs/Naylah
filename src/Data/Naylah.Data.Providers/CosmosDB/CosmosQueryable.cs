using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Naylah.Data.Providers.CosmosDB
{
    //public static class CosmosDbExtensions
    //{
    //    public static CosmosAsyncQueryable<T> ToCosmosAsyncQuery<T>(this IQueryable<T> queryable)
    //    {
    //        return new CosmosAsyncQueryable<T>(((CosmosAsyncQueryable<T>)queryable).container, queryable.Expression);
    //    }

    //    public static CosmosAsyncQueryable<T> ToCosmosAsyncQuery<T>(this Container cosmosContainer)
    //    {
    //        return new CosmosAsyncQueryable<T>(cosmosContainer, Enumerable.Empty<T>());
    //    }
    //}


    //public class CosmosAsyncQueryProvider<TEntity> : IQueryProvider
    //{
    //    internal readonly Container cosmosContainer;

    //    internal CosmosAsyncQueryProvider(Container cosmosContainer)
    //    {
    //        this.cosmosContainer = cosmosContainer;
    //    }

    //    public IQueryable CreateQuery(Expression expression)
    //    {
    //        return new CosmosAsyncQueryable<TEntity>(cosmosContainer.GetItemLinqQueryable<TEntity>(false));
    //    }

    //    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
    //    {
    //        return new CosmosAsyncQueryable<TElement>(cosmosContainer.GetItemLinqQueryable<TElement>(false));
    //    }

    //    public object Execute(Expression expression)
    //    {
    //        throw new NotImplementedException();
    //        return _inner.Execute(expression);
    //    }

    //    public TResult Execute<TResult>(Expression expression)
    //    {
    //        throw new NotImplementedException();
    //        return _inner.Execute(expression);
    //        return _inner.Execute<TResult>(expression);
    //    }

    //    public Task<object> ExecuteAsync(Expression expression, CancellationToken cancellationToken)
    //    {
    //        return Task.FromResult(Execute(expression));
    //    }

    //    public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
    //    {
    //        return Task.FromResult(Execute<TResult>(expression));
    //    }
    //}

    //public class CosmosAsyncQueryable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>
    //{
    //    internal readonly Container container;

    //    public CosmosAsyncQueryable(Container container, IEnumerable<T> enumerable) : base(enumerable)
    //    {
    //        this.container = container;
    //    }

    //    public CosmosAsyncQueryable(Container container, Expression expression) : base(expression)
    //    {
    //        this.container = container;
    //    }

    //    public async IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    //    {
    //        var cosmosQuery = container.GetItemLinqQueryable<T>(false).Provider.CreateQuery<T>(this.AsQueryable().Expression);

    //        var iterator = cosmosQuery.ToFeedIterator();

    //        while (iterator.HasMoreResults)
    //        {
    //            foreach (var item in await iterator.ReadNextAsync(cancellationToken))
    //            {
    //                yield return item;
    //            }
    //        }
    //    }

    //    public IEnumerator<T> GetEnumerator()
    //    {
    //        throw new NotImplementedException();
    //    }

    //}

    //public class CosmosAsyncEnumerator<T> : IAsyncEnumerator<T>
    //{
    //    private readonly IEnumerator<T> _inner;

    //    public CosmosAsyncEnumerator(IEnumerator<T> inner)
    //    {
    //        _inner = inner;
    //    }

    //    public void Dispose()
    //    {
    //        _inner.Dispose();
    //    }

    //    public ValueTask<bool> MoveNextAsync()
    //    {
    //        return new ValueTask<bool>(_inner.MoveNext());
    //    }

    //    public ValueTask DisposeAsync()
    //    {
    //        _inner.Dispose();
    //        return new ValueTask();
    //    }

    //    public T Current
    //    {
    //        get { return _inner.Current; }
    //    }

    //}
}
