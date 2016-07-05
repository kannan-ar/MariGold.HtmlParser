﻿namespace MariGold.HtmlParser
{
    using System;

    internal abstract class CSSProperty
    {
        internal const string fontFamily = "font-family";
        internal const string font = "font";
        internal const string fontSize = "font-size";
        internal const string fontWeight = "font-weight";
        internal const string fontStyle = "font-style";

        internal const string color = "color";
        internal const string textDecoration = "text-decoration";
        internal const string backgroundColor = "background-color";
        internal const string background = "background";

        internal abstract bool AppendStyle(HtmlStyle parentStyle, HtmlNode child);
    }
}
