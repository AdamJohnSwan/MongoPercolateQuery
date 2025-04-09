namespace MongoPercolateQuery.Models;

public class PercolateSearch
{
    public IEnumerable<Term> PositiveTerms { get; set; } = [];
    public IEnumerable<Term> NegatedTerms { get; set; } = [];
    public static PercolateSearch Create(string query, PercolateSearchCreateOptions? options = null)
    {
        options ??= new();
        QueryStringParser parser = new(options);
        (IEnumerable<Term> positiveTerms, IEnumerable<Term> negativeTerms) = parser.Parse(query);
        return new()
        {
            PositiveTerms = positiveTerms,
            NegatedTerms = negativeTerms
        };
    }
}

public class Term : IEquatable<Term>
{
    public Term()
    {
    }
    public Term(string originalTerm, IEnumerable<string> parsedTerms)
    {
        OriginalTerm = originalTerm;
        ParsedTerms = parsedTerms;
    }

    public string OriginalTerm { get; set; } = "";
    /// <summary>
    /// Contains term that have been parsed based on <see cref="PercolateSearchCreateOptions"/>
    /// </summary>
    /// <remarks>
    /// TODO: Parse the terms based on the options. Right now this just contains the original term.
    /// </remarks>
    public IEnumerable<string> ParsedTerms { get; set; } = [];

    public bool Equals(Term? other)
    {
        return other != null
            && OriginalTerm == other.OriginalTerm
            && Enumerable.SequenceEqual(ParsedTerms, other.ParsedTerms);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Term);
    }

    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }
}
