namespace MariGold.HtmlParser
{
    using System;

    /// <summary>
    /// An abstract class to define methods for parse an html document.
    /// </summary>
    public abstract class HtmlParser
    {
        internal abstract void SetAnalyzer(HtmlAnalyzer analyzer);
        internal abstract void SetPosition(int position);

        /// <summary>
        /// Last parsed HtmlNode.
        /// </summary>
        public abstract HtmlNode Current { get; }
        
        /// <summary>
        /// Parse and assign all CSS properties of processed HtmlNode(s) and its children
        /// </summary>
        public abstract void ParseCSS();

        /// <summary>
        /// A method to travel through an html document. Upon each travel, the method will parse next element along with its children.
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
}
