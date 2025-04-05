using Bogus;
using MongoDB.Bson;
using MongoPercolateQuery.Models;

namespace MongoPercolateQuery.Test.TestModels;

internal class Query
{
    public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
    public required PercolateSearch Search { get; set; }
}

internal class QueryFaker : Faker<Query>
{
    public QueryFaker()
    {
        RuleFor(x => x.Id, f => ObjectId.GenerateNewId());
        RuleFor(x => x.Search, f => new());
    }
}
