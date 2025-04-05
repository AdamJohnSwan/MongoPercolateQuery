using Mongo2Go;
using MongoDB.Driver;
using MongoPercolateQuery.Extensions;
using MongoPercolateQuery.Models;
using MongoPercolateQuery.Test.TestModels;
using System.Diagnostics;
using Xunit.Abstractions;

namespace MongoPercolateQuery.Test.LiveTests;

public class PerformanceTests
{
    private readonly ITestOutputHelper _output;
    private readonly MongoDbRunner _runner;
    private readonly IMongoCollection<Query> _queries;

    public PerformanceTests(ITestOutputHelper output)
    {
        _output = output;

        _runner = MongoDbRunner.Start();

        MongoClient client = new(_runner.ConnectionString);
        IMongoDatabase database = client.GetDatabase("IntegrationTest");
        _queries = database.GetCollection<Query>("TestCollection");
    }

    [Fact]
    public async Task MatchAgainstManyQueries()
    {
        // Arrange
        Stopwatch timer = new();
        string idToInsert = "75326e60-ab5c-4e67-80c7-fdef856954ba";
        List<Query> queries = new QueryFaker()
            .Generate(10000);
        queries[500].Search = PercolateSearch.Create($"\"{idToInsert}\"");
        _queries.InsertMany(queries);

        Document document = new DocumentFaker()
            .RuleFor(x => x.Content, f => f.Random.Words(200))
            .Generate();
        document.Content = document.Content.Insert(5, idToInsert);
        // Act
        timer.Start();
        List<Query> result = await _queries
            .Find(Builders<Query>.Filter.Percolate(x => x.Search, document.Content))
            .ToListAsync();
        timer.Stop();

        // Log
        _output.WriteLine($"Elapsed time: {timer.ElapsedMilliseconds} ms");
        Assert.Single(result);
    }
}
