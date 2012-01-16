using System;
using System.Collections.Generic;

namespace PetStore.Domain.Core
{
    /// <summary>
    /// A basic repository interface for entity objects
    /// http://martinfowler.com/eaaCatalog/repository.html
    /// http://domaindrivendesign.org/node/109
    /// 
    /// You should define further specialisation in another interface. 
    /// </summary>
    /// <remarks>
    /// You may also hide members exposed here from your concrete types by using "Explicit Interface Implementation"
    /// </remarks>
    /// <example>
    /// Example use with a database and specialised as a customer repository
    /// <![CDATA[ class CustomerRepository : SqlRepository, ICustomerRepository<Customer> { ... } ]]> 
    /// </example>
    /// <typeparam name="T">The type that the repository returns</typeparam>
    public interface IEntityRepository<T>
    {
        /// <summary>
        /// Gets a single entity from the backing store.
        /// </summary>
        /// <param name="filter">An expression to reduce the list.</param>
        /// <returns>The first occurrence of an object that matches the expression; or null if no objects match.</returns>
        T GetOne(Predicate<T> filter);

        /// <summary>
        /// Gets a reduced object collection based on an expression.
        /// </summary>
        /// <param name="filter">An expression to reduce the list.</param>
        /// <returns>All objects that match the expression.</returns>
        List<T> GetAll(Predicate<T> filter);

        /// <summary>
        /// Gets all instances from the backing store.
        /// </summary>
        /// <returns>The complete list of objects for this repository. Apply caching here</returns>
        /// <remarks>Apply Caching here</remarks>
        List<T> GetAll();

        /// <summary>
        /// Saves an object to the backing store.
        /// </summary>
        /// <remarks>
        /// Performs both insert/add/update type operations.
        /// </remarks>
        /// <param name="instance">The instance to be saved.</param>
        T Save(T instance);

        /// <summary>
        /// Deletes an object from the backing store.
        /// </summary>
        /// <param name="instance">The instance to be deleted.</param>
        void Delete(T instance);
    }
}