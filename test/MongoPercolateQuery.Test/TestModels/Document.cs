using Bogus;
using MongoDB.Bson;

namespace MongoPercolateQuery.Test.TestModels;

internal class Document
{
    public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
    public required string Content { get; set; }
}

internal class DocumentFaker : Faker<Document>
{
    public DocumentFaker()
    {
        RuleFor(x => x.Id, f => ObjectId.GenerateNewId());
        RuleFor(x => x.Content, f => f.Lorem.Paragraph());
    }
}