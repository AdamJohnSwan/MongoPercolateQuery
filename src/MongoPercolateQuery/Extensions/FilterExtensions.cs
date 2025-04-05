using MongoDB.Driver;
using MongoPercolateQuery.Models;
using System.Linq.Expressions;

namespace MongoPercolateQuery.Extensions;

public static class FilterExtensions
{
    public static FilterDefinition<TDocument> Percolate<TDocument>(
        this FilterDefinitionBuilder<TDocument> builder,
        Expression<Func<TDocument, PercolateSearch>> field,
        string document)
    {
        throw new NotImplementedException();
    }
}
