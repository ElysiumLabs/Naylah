using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Naylah.Rest.Table.NLinq
{
    public class TableQueryable<T> : IOrderedQueryable<T>
    {
        public TableQueryable(ITableQueryContext queryContext)
        {
            Initialize(new TableQueryProvider(queryContext), null);
        }

        public TableQueryable(IQueryProvider provider)
        {
            Initialize(provider, null);
        }

        internal TableQueryable(IQueryProvider provider, Expression expression)
        {
            Initialize(provider, expression);
        }

        private void Initialize(IQueryProvider provider, Expression expression)
        {
            if (provider == null)
                throw new ArgumentNullException("provider");
            if (expression != null && !typeof(IQueryable<T>).
                   IsAssignableFrom(expression.Type))
                throw new ArgumentException(
                     String.Format("Not assignable from {0}", expression.Type), "expression");

            Provider = provider;
            Expression = expression ?? Expression.Constant(this);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return (Provider.Execute<IEnumerable<T>>(Expression)).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return (Provider.Execute<System.Collections.IEnumerable>(Expression)).GetEnumerator();
        }

        public Type ElementType
        {
            get { return typeof(T); }
        }

        public Expression Expression { get; private set; }
        public IQueryProvider Provider { get; private set; }
    }

    public class TableQueryProvider : IQueryProvider
    {
        private readonly ITableQueryContext queryContext;

        public TableQueryProvider(ITableQueryContext queryContext)
        {
            this.queryContext = queryContext;
        }

        public virtual IQueryable CreateQuery(Expression expression)
        {
            Type elementType = expression.Type.GetElementType();
            try
            {
                return
                   (IQueryable)Activator.CreateInstance(typeof(TableQueryable<>).
                          MakeGenericType(elementType), new object[] { this, expression });
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException;
            }
        }

        public virtual IQueryable<T> CreateQuery<T>(Expression expression)
        {
            return new TableQueryable<T>(this, expression);
        }

        object IQueryProvider.Execute(Expression expression)
        {
            return queryContext.Execute(expression, false);
        }

        T IQueryProvider.Execute<T>(Expression expression)
        {
            return (T)queryContext.Execute(expression,
                       (typeof(T).Name == "IEnumerable`1"));
        }
    }

    public interface ITableQueryContext
    {
        object Execute(Expression expression, bool isEnumerable);
    }
}
