using System.Threading.Tasks;

namespace MariGold.HtmlParser;

/// <summary>
/// An abstract base class for parse html documents.
/// </summary>
public abstract class HtmlParser
{
    protected string uriSchema;
    protected string baseUrl;

    internal abstract void SetAnalyzer(HtmlAnalyzer analyzer);
    internal abstract void SetPosition(int position);

    public string UriSchema
    {
        get
        {
            return uriSchema;
        }

        set
        {
            uriSchema = value;
        }
    }

    public string BaseURL
    {
        get
        {
            return baseUrl;
        }

        set
        {
            baseUrl = value;
        }
    }

    /// <summary>
    /// Last parsed HtmlNode.
    /// </summary>
    public abstract IHtmlNode Current { get; }

    /// <summary>
    /// Parse and assign CSS properties of all processed HtmlNode(s) and its children
    /// </summary>
    public abstract Task ParseStylesAsync();

    /// <summary>
    /// Travel through html document elements. Upon each travel, the method will parse next element along with its children.
    /// </summary>
    /// <returns>Returns true if successfully parsed and html element</returns>
    public abstract bool Traverse();

    /// <summary>
    /// Finds and loads the current HtmlNode using the first html element with the given html tag
    /// </summary>
    /// <param name="tag">The html tag to be find</param>
    /// <returns>Returns true if successfully finds an html element using the given tag</returns>
    public abstract bool FindFirst(string tag);

    /// <summary>
    /// Parse all the html elements in the given html document.
    /// </summary>
    /// <returns>Returns true if successfully parsed all the elements in the html document</returns>
    public abstract bool Parse();
}
