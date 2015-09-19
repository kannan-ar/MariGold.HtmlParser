namespace MariGold.HtmlParser
{
    using System;

    public abstract class HtmlParser
    {
        internal abstract void SetAnalyzer(HtmlAnalyzer analyzer);
        internal abstract void SetPosition(int position);

        public abstract HtmlNode Current { get; }
        public abstract void ParseCSS();

        public abstract bool Traverse();
        public abstract bool FindFirst(string tag);
    }
}
