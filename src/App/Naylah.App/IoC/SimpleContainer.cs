//using Naylah.DI.Abstractions;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace Naylah.App.IoC
//{
//    /// <summary>
//    /// Simple dependency container implementation
//    /// </summary>
//	public class SimpleContainer : IDependencyContainer
//    {
//        /// <summary>
//        /// The _resolver
//        /// </summary>
//        private readonly IDependencyResolver resolver;

// /// <summary> /// The _services /// </summary> private readonly Dictionary<Type, List<object>> services;

// /// <summary> /// The _registered services /// </summary> private readonly Dictionary<Type,
// List<Func<IDependencyResolver, object>>> registeredServices;

// /// <summary> /// Initializes a new instance of the <see cref="SimpleContainer"/> class. ///
// </summary> public SimpleContainer() { this.resolver = new SimpleResolver(this.ResolveAll);
// this.services = new Dictionary<Type, List<object>>(); this.registeredServices = new
// Dictionary<Type, List<Func<IDependencyResolver, object>>>(); }

// #region IDependencyContainer Members

// ///
// <summary>
// /// Gets the resolver from the container ///
// </summary>
// ///
// <returns>An instance of <see cref="IResolver"/></returns>
// public IDependencyResolver GetResolver() { return this.resolver; }

// /// <summary> /// Registers an instance of T to be stored in the container. /// </summary> ///
// <typeparam name="T">Type of instance</typeparam> /// <param name="instance">Instance of type
// T.</param> /// <returns>An instance of <see cref="SimpleContainer"/></returns> public
// IDependencyContainer Register<T>(T instance) where T : class { var type = typeof(T); List<object> list;

// if (!this.services.TryGetValue(type, out list)) { list = new List<object>();
// this.services.Add(type, list); }

// list.Add(instance); return this; }

// /// <summary> /// Registers a type to instantiate for type T. /// </summary> /// <typeparam
// name="T">Type of instance</typeparam> /// <typeparam name="TImpl">Type to register for
// instantiation.</typeparam> /// <returns>An instance of <see cref="SimpleContainer"/></returns>
// public IDependencyContainer Register<T, TImpl>() where T : class where TImpl : class, T { return
// this.Register<T>(t => Activator.CreateInstance<TImpl>() as T); }

// /// <summary> /// Registers a type to instantiate for type T as singleton. /// </summary> ///
// <typeparam name="T">Type of instance</typeparam> /// <typeparam name="TImpl">Type to register for
// instantiation.</typeparam> /// <returns>An instance of <see
// cref="IDependencyContainer"/></returns> public IDependencyContainer RegisterSingle<T, TImpl>()
// where T : class where TImpl : class, T { var type = typeof(T); List<object> list;

// if (!this.services.TryGetValue(type, out list)) { list = new List<object>();
// this.services.Add(type, list); }

// var instance = Activator.CreateInstance<TImpl>() as TImpl;

// list.Add(instance); return this; }

// /// <summary> /// Tries to register a type /// </summary> /// <typeparam name="T">Type of
// instance</typeparam> /// <param name="type">Type of implementation</param> /// <returns>An
// instance of <see cref="SimpleContainer"/></returns> public IDependencyContainer Register<T>(Type
// type) where T : class { return this.Register(typeof(T), type); }

// /// <summary> /// Tries to register a type /// </summary> /// <param name="type">Type to
// register.</param> /// <param name="impl">Type that implements registered type.</param> ///
// <returns>An instance of <see cref="SimpleContainer"/></returns> public IDependencyContainer
// Register(Type type, Type impl) { List<Func<IDependencyResolver, object>> list; if
// (!this.registeredServices.TryGetValue(type, out list)) { list = new List<Func<IDependencyResolver,
// object>>(); this.registeredServices.Add(type, list); }

// list.Add(t => Activator.CreateInstance(impl));

// return this; }

// /// <summary> /// Registers a function which returns an instance of type T. /// </summary> ///
// <typeparam name="T">Type of instance.</typeparam> /// <param name="func">Function which returns an
// instance of T.</param> /// <returns>An instance of <see cref="SimpleContainer"/></returns> public
// IDependencyContainer Register<T>(Func<IDependencyResolver, T> func) where T : class { var type =
// typeof(T); List<Func<IDependencyResolver, object>> list; if
// (!this.registeredServices.TryGetValue(type, out list)) { list = new List<Func<IDependencyResolver,
// object>>(); this.registeredServices.Add(type, list); }

// list.Add(func);

// return this; }

// /// <summary> /// Resolves all. /// </summary> /// <param name="type">The type.</param> ///
// <returns>IEnumerable&lt;System.Object&gt;.</returns> private IEnumerable<object> ResolveAll(Type
// type) { List<object> list; if (this.services.TryGetValue(type, out list)) { foreach (var service
// in list) { yield return service; } }

// List<Func<IDependencyResolver, object>> getter; if (this.registeredServices.TryGetValue(type, out
// getter)) { foreach (var serviceFunc in getter) { yield return serviceFunc(this.resolver); } } }

// #endregion IDependencyContainer Members

// /// <summary> /// Class Resolver. /// </summary> private class SimpleResolver :
// IDependencyResolver { /// <summary> /// The _resolve object delegate /// </summary> private
// readonly Func<Type, IEnumerable<object>> resolveObjectDelegate;

// /// <summary> /// Initializes a new instance of the <see cref="SimpleResolver"/> class. ///
// </summary> /// <param name="resolveObjectDelegate">The resolve object delegate.</param> internal
// SimpleResolver(Func<Type, IEnumerable<object>> resolveObjectDelegate) { this.resolveObjectDelegate
// = resolveObjectDelegate; }

// #region IResolver Members

// /// <summary> /// Resolve a dependency. /// </summary> /// <typeparam name="T">Type of instance to
// get.</typeparam> /// <returns>An instance of {T} if successful, otherwise null.</returns> public T
// GetService<T>() { return this.GetServices<T>().FirstOrDefault(); }

// ///
// <summary>
// /// Resolve a dependency by type. ///
// </summary>
// ///
// <param name="type">Type of object.</param>
// ///
// <returns>An instance to type if found as <see cref="object"/>, otherwise null.</returns>
// public object GetService(Type type) { return this.GetServices(type).FirstOrDefault(); }

// /// <summary> /// Resolve a dependency. /// </summary> /// <typeparam name="T">Type of instance to
// get.</typeparam> /// <returns>All instances of {T} if successful, otherwise null.</returns> public
// IEnumerable<T> GetServices<T>() { return this.resolveObjectDelegate(typeof(T)).Cast<T>(); }

// /// <summary> /// Resolve a dependency by type. /// </summary> /// <param name="type">Type of
// object.</param> /// <returns>All instances of type if found as <see cref="object"/>, otherwise
// null.</returns> public IEnumerable<object> GetServices(Type type) { return
// this.resolveObjectDelegate(type); }

// ///
// <summary>
// /// Determines whether the specified type is registered. ///
// </summary>
// ///
// <param name="type">The type.</param>
// ///
// <returns><c>true</c> if the specified type is registered; otherwise, <c>false</c>.</returns>
// //public bool IsRegistered(Type type) //{ // return this.Resolve(type) != null; //}

// ///// <summary> ///// Determines whether this instance is registered. ///// </summary> /////
// <typeparam name="T"></typeparam> ///// <returns><c>true</c> if this instance is registered;
// otherwise, <c>false</c>.</returns> //public bool IsRegistered<T>() where T : class //{ // return
// this.Resolve<T>() != null; //}

//            #endregion IResolver Members
//        }
//    }
//}