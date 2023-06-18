namespace MariGold.HtmlParser;

using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

internal sealed class WebManager
{
    private readonly string uriSchema;
    private readonly string baseUrl;

    internal WebManager(string uriSchema, string baseUrl)
    {
        this.uriSchema = uriSchema;
        this.baseUrl = baseUrl;
    }

    private string CleanUrl(string url)
    {
        if (url.StartsWith("//") && !string.IsNullOrEmpty(uriSchema))
        {
            url = string.Concat(uriSchema, ":" + url);
        }

        if (Uri.IsWellFormedUriString(url, UriKind.Relative) && !string.IsNullOrEmpty(baseUrl))
        {
            url = string.Concat(baseUrl,
                (!baseUrl.EndsWith("/") && !url.StartsWith("/") ? "/" : string.Empty), url);
        }

        return url;
    }

    internal async Task<string> ExtractStylesFromLinkAsync(string url)
    {
        string styles = string.Empty;

        url = CleanUrl(url);

        if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
        {
            using HttpClient client = new();
            styles = await client.GetStringAsync(url).ConfigureAwait(false);
        }

        return styles;
    }
}
