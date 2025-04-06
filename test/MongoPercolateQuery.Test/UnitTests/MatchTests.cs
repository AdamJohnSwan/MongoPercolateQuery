using MongoDB.Bson;
using MongoDB.Driver;
using MongoPercolateQuery.Extensions;
using MongoPercolateQuery.Test.TestModels;

namespace MongoPercolateQuery.Test.UnitTests;

public class MatchTests
{
    [Fact]
    public async Task MatchDocumentsToQuery()
    {
        // Arrange
        int matchAmount = 2;
        string idToInsert = "75326e60-ab5c-4e67-80c7-fdef856954ba";
        Document document = new DocumentFaker()
            .RuleFor(x => x.Content, f => f.Random.Words(200))
            .Generate();
        document.Content = document.Content.Insert(5, idToInsert);

        List<Query> queries = new QueryFaker()
            .Generate(100);

        int[] selectedIndexes = [matchAmount];
        List<Query> queriesShouldMatch = [];
        Random random = new();
        for (int i = 0; i < matchAmount; i++)
        {
            Query query;
            do
            {
                query = queries.ElementAt(random.Next(queries.Count));
            } while (queriesShouldMatch.Contains(query));
            queriesShouldMatch.Add(query);
        }
        IEnumerable<ObjectId> queriesShouldMatchIds = queriesShouldMatch
            .Select(x => x.Id);

        MongoCollectionMock<Query> collectionMock = new();
        FilterDefinition<Query> filter = Builders<Query>.Filter.Percolate(x => x.Search, document.Content);

        // Act
        List<Query> result = await collectionMock.CollectionMock.Object
            .Find(filter)
            .ToListAsync();

        // Assert
        Assert.Equal(matchAmount, result.Count);
        Assert.All(result, query =>
        {
            Assert.True(query.Id != ObjectId.Empty && queriesShouldMatchIds.Contains(query.Id));
        });
    }
}
