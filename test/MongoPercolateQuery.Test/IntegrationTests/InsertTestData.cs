using MongoDB.Driver;
using MongoPercolateQuery.Test.TestModels;

namespace MongoPercolateQuery.Test.IntegrationTests;
public class InsertTestData
{
#pragma warning disable xUnit1004 // Test methods should not be skipped
    [Fact(Skip = "Only for inserting test data")]
#pragma warning restore xUnit1004 // Test methods should not be skipped
    public void Insert()
    {
        IMongoDatabase database = new MongoClient("mongodb://localhost:27017")
            .GetDatabase("IntegrationTest");

        IMongoCollection<Query> collection = database.GetCollection<Query>("QueryCollection");
        collection.InsertMany(new QueryFaker()
            .Generate(1000));
    }

#pragma warning disable xUnit1004 // Test methods should not be skipped
    [Fact(Skip = "Only for deleting test data")]
#pragma warning restore xUnit1004 // Test methods should not be skipped
    public void Delete()
    {
        IMongoDatabase database = new MongoClient("mongodb://localhost:27017")
            .GetDatabase("IntegrationTest");

        IMongoCollection<Query> queries = database.GetCollection<Query>("QueryCollection");
        IMongoCollection<Document> documents = database.GetCollection<Document>("DocumentCollection");

        queries.DeleteMany(Builders<Query>.Filter.Empty);
        documents.DeleteMany(Builders<Document>.Filter.Empty);
    }
}
