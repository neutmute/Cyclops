namespace Sprocker.Core.Mapping
{
    public interface IMapContext<TResult>
    {
        /// <summary>
        /// Maps the current property to a column with the given name.
        /// </summary>
        /// <param name="columnName">The name of the column the current property should be mapped to.</param>
        /// <returns>The fluent interface that can be used further specify mappings.</returns>
        IMapContext<TResult> ToColumn(string columnName);
    }
}
