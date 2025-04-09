using MongoPercolateQuery.Models;
using System.Text;

namespace MongoPercolateQuery;

internal class QueryStringParser(PercolateSearchCreateOptions options)
{
    private readonly PercolateSearchCreateOptions _options = options;
    public (IEnumerable<Term> postiveTerms, IEnumerable<Term> negatedTerms) Parse(string queryText)
    {
        List<string> postiveData = [];
        List<string> negatedData = [];
        StringBuilder phrase = new();
        // A phrase is a group of words surrounded by quotes.
        bool inPhrase = false;
        // A negation is a word or phrase with a hyphen in front of it that denotes 'do not include this item in the search'
        bool inNegation = false;
        int index = 0;

        while (index < queryText.Length)
        {
            QueryToken token = GetNextToken(queryText[index..]);
            index += token.Offset;
            if (token.Type == Type.Text)
            {
                if (inPhrase)
                {
                    phrase.Append(token.Data);
                }
                else if (inNegation)
                {
                    negatedData.Add(token.Data);
                }
                else
                {
                    postiveData.Add(token.Data);
                }
            }
            else if (token.Type == Type.Whitespace)
            {
                if (inPhrase)
                {
                    phrase.Append(token.Data);
                }
                else
                {
                    // `-foo` should negate `foo`, `-"foo bar" should negate the phrase `foo bar`
                    // `-foo bar` should negate `foo` but not `bar`
                    // Not in a phrase and there is a space, meaning this negation is over (if there was one)
                    inNegation = false;
                }
            }
            else if (token.Type == Type.Hyphen)
            {
                if (!inPhrase)
                {
                    inNegation = true;
                }
            }
            else if (token.Type == Type.Quote)
            {
                if (inPhrase)
                {
                    if (inNegation)
                    {
                        negatedData.Add(phrase.ToString());
                    }
                    else
                    {
                        postiveData.Add(phrase.ToString());
                    }
                    phrase.Clear();
                    inPhrase = false;
                }
                else
                {
                    inPhrase = true;
                }
            }
        }

        return (postiveData.Select(GetTerm), negatedData.Select(GetTerm));
    }

    private record QueryToken(Type Type, string Data, int Offset);

    private enum Type
    {
        Whitespace,
        Hyphen,
        Quote,
        Text,
        Invalid
    }

    private static Type GetType(char c)
    {
        return c switch
        {
            '\n' or '\v' or '\f' or '\r' or ' ' => Type.Whitespace,
            '-' => Type.Hyphen,
            '"' => Type.Quote,
            _ => Type.Text,
        };
    }

    private static QueryToken GetNextToken(string text)
    {
        if (text.Length == 0)
        {
            return new(Type.Whitespace, "", 0);
        }
        int start = 0;
        StringBuilder data = new();
        Type type = GetType(text[start]);
        do
        {
            data.Append(text[start]);
            start++;
        }
        while (start < text.Length && GetType(text[start]) == type);

        return new(type, data.ToString(), start);
    }

    private Term GetTerm(string text)
    {
        return new()
        {
            OriginalTerm = text,
            ParsedTerms = [text]
        };
    }
}
