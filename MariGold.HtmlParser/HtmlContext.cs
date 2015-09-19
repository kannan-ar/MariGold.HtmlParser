namespace MariGold.HtmlParser
{
    using System;

    internal sealed class HtmlContext
    {
        private readonly string html;

        internal HtmlContext(string html)
        {
            if (string.IsNullOrEmpty(html))
            {
                throw new ArgumentNullException("html");
            }

            this.html = html;
        }

        internal string Html
        {
            get
            {
                return html;
            }
        }
    }
}
