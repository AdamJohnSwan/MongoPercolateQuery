using MongoPercolateQuery.Models;

namespace MongoPercolateQuery.Test.UnitTests;

public class CreateTests
{
    [Fact]
    public void CreatePercolateSearchWithVariousStrings()
    {
        // Arrange
        List<string> queries =
        [
            "\"exact match\"",
            "-\"exclude this\"",
            "common mispeling",
            "likely dearly acidentally",
            "fish -whale",
            "mispeled -exclude",
            "\"confiscate vehicle\" -impound",
            "musical \"theater degree\"",
            "\"mispeled\" mispeled",
            "acceptible socialize"
        ];

        // Act
        List<PercolateSearch> percolateSearches = [.. queries.Select(PercolateSearch.Create)];

        // Assert
        Assert.Equal(queries.Count, percolateSearches.Count);
        Assert.All(percolateSearches, q =>
        {
            Assert.NotNull(q);
            Assert.Equal(typeof(PercolateSearch), q.GetType());
        });
    }
}
