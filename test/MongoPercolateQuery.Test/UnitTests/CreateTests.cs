using MongoPercolateQuery.Models;

namespace MongoPercolateQuery.Test.UnitTests;

public class CreateTests
{
    [Fact]
    public void CreatePercolateSearchWithVariousStrings()
    {
        // Arrange
        Dictionary<string, PercolateSearch> answers = new()
        {
            { "\"exact match\"", new()
                {
                    PositiveTerms =
                    [
                        new("exact match", ["exact match"])
                    ]
                }
            },
            { "-\"exclude this\"", new()
                {
                    NegatedTerms =
                    [
                        new("exclude this", ["exclude this"])
                    ]
                }
            },
            { "common mispeling", new()
                {
                    PositiveTerms =
                    [
                        new("common", ["common"]),
                        new("mispeling", ["mispeling"])
                    ]
                }
            },
            { "likely dearly accidentally", new()
                {
                    PositiveTerms =
                    [
                        new("likely", ["likely"]),
                        new("dearly", ["dearly"]),
                        new("accidentally", ["accidentally"])
                    ]
                }
            },
            { "fish -whale", new()
                {
                    PositiveTerms =
                    [
                        new("fish", ["fish"])
                    ],
                    NegatedTerms =
                    [
                        new("whale", ["whale"])
                    ]
                }
            },
            { "include -exclude", new()
                {
                    PositiveTerms =
                    [
                        new("include", ["include"])
                    ],
                    NegatedTerms =
                    [
                        new("exclude", ["exclude"])
                    ]
                }
            },
            { "\"confiscate vehicle\" -impound", new()
                {
                    PositiveTerms =
                    [
                        new("confiscate vehicle", ["confiscate vehicle"])
                    ],
                    NegatedTerms =
                    [
                        new("impound", ["impound"])
                    ]
                }
            },
            { "musical \"theater degree\"", new()
                {
                    PositiveTerms =
                    [
                        new("musical", ["musical"]),
                        new("theater degree", ["theater degree"])
                    ]
                }
            },
            { "\"mispeled\" mispeled", new()
                {
                    PositiveTerms =
                    [
                        new("mispeled", ["mispeled"]),
                        new("mispeled", ["mispeled"])
                    ]
                }
            },
        };

        // Act
        Dictionary<string, PercolateSearch> attempts = answers.ToDictionary(a => a.Key, a => PercolateSearch.Create(a.Key));
        // Assert
        Assert.Equal(answers.Count, attempts.Count);
        foreach (string key in answers.Keys)
        {
            PercolateSearch answer = answers[key];
            PercolateSearch attempt = attempts[key];

            Assert.Equal(answer.PositiveTerms.Count(), attempt.PositiveTerms.Count());
            Assert.Equal(answer.NegatedTerms.Count(), attempt.NegatedTerms.Count());

            for (int i = 0; i < answer.PositiveTerms.Count(); i++)
            {
                Assert.True(answer.PositiveTerms.ElementAt(i).Equals(attempt.PositiveTerms.ElementAt(i)));
            }
            for (int i = 0; i < answer.NegatedTerms.Count(); i++)
            {
                Assert.True(answer.NegatedTerms.ElementAt(i).Equals(attempt.NegatedTerms.ElementAt(i)));
            }
        }
    }
}
