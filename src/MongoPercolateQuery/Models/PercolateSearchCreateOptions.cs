namespace MongoPercolateQuery.Models;
public class PercolateSearchCreateOptions
{
    /// <summary>
    /// Whether to correct misspellings when parsing the string term
    /// </summary>
    /// <example>
    /// Parses mispelling to misspelling
    /// </example>
    public bool CorrectMisspellings { get; set; } = true;
    /// <summary>
    /// Whether to include the different roots a word can have
    /// </summary>
    /// <example>
    /// Parses lively to live
    /// </example>
    public bool ParseWordRoots { get; set; } = true;
}
