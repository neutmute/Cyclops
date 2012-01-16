using System;
using System.Collections.Generic;

namespace PetStore.Domain.Core
{
    /// <summary>
    /// A basic repository interface for value objects (Immutable objects)
    /// http://martinfowler.com/eaaCatalog/repository.html
    /// http://domaindrivendesign.org/node/135
    /// 
    /// You should define further specialisation in another interface. 
    /// </summary>
    /// <remarks>
    /// You may also hide members exposed here from your concrete types by using "Explicit Interface Implementation"
    /// </remarks>
    /// <typeparam name="T">The type that the repository returns</typeparam>
    public interface IValueObjectRepository<T>
    {
        /// <summary>
        /// Get a single entity
        /// </summary>
        /// <param name="filter"></param> 
        /// <returns></returns>
        T GetOne(Predicate<T> filter);

        /// <summary>
        /// Get a reduced object collection based on an expression
        /// </summary>
        /// <param name="filter">An expression to reduce the list</param>
        /// <returns>All objects that match the expression</returns>
        List<T> GetAll(Predicate<T> filter);

        /// <summary>
        /// Get all 
        /// </summary>
        /// <returns>The complete list of objects for this repository</returns>
        List<T> GetAll();
    }
}
