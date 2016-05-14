namespace MariGold.HtmlParser
{
    using System;
    using System.Net;

    internal sealed class WebManager
    {
        private readonly string uriSchema;
        private readonly string baseUrl;

        internal WebManager(string uriSchema,string baseUrl)
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
                url = string.Concat(baseUrl, url);
            }

            return url;
        }

        internal string ExtractStylesFromLink(string url)
        {
            string styles = string.Empty;

            url = CleanUrl(url);

            if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                using (WebClient client = new WebClient())
                {
                    client.Encoding = System.Text.Encoding.UTF8;
                    styles = client.DownloadString(url);
                }
            }

            return styles;
        }
    }
}
