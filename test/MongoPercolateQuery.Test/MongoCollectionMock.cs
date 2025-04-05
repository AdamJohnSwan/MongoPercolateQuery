using MongoDB.Driver;
using Moq;

namespace MongoPercolateQuery.Test;

public class MongoCollectionMock<TDocument>
{
    public Mock<IMongoDatabase> MongoDatabaseMock { get; } = new();
    public Mock<IMongoCollection<TDocument>> CollectionMock { get; } = new();

    private readonly Mock<IAsyncCursor<TDocument>> asyncCursorMock = new();

    public MongoCollectionMock()
    {
        MongoDatabaseMock
            .Setup(x => x.GetCollection<TDocument>(It.IsAny<string>(), It.IsAny<MongoCollectionSettings?>()))
            .Returns(CollectionMock.Object);

        CollectionMock
            .Setup(x => x.FindAsync(
                It.IsAny<FilterDefinition<TDocument>>(),
                It.IsAny<FindOptions<TDocument>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(asyncCursorMock.Object);
        CollectionMock
            .Setup(x => x.FindSync(
                It.IsAny<ExpressionFilterDefinition<TDocument>>(),
                It.IsAny<FindOptions<TDocument>>(),
                It.IsAny<CancellationToken>()))
            .Returns(asyncCursorMock.Object);
    }

    public void SetupFind(List<TDocument> expectedResult)
    {
        asyncCursorMock
            .SetupSequence(a => a.MoveNext(It.IsAny<CancellationToken>()))
            .Returns(true)
            .Returns(false);
        asyncCursorMock
            .SetupSequence(a => a.MoveNextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true)
            .ReturnsAsync(false);
        asyncCursorMock
            .SetupGet(_async => _async.Current)
            .Returns(expectedResult);
    }
}
