namespace MariGold.HtmlParser.Tests
{
    using MariGold.HtmlParser;
    using System.Linq;
    using Xunit;

    public class SpecialTags
    {
        [Fact]
        public void MetaTag()
        {
            string html = "<html><head><meta charset=\"utf-8\" /></head><body></body></html>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "html", "<head><meta charset=\"utf-8\" /></head><body></body>", html);
            Assert.Null(parser.Current.Parent);
            Assert.True(parser.Current.HasChildren);
            Assert.Equal(2, parser.Current.Children.Count());
            Assert.False(parser.Current.SelfClosing);
            Assert.Empty(parser.Current.Attributes);

            Assert.NotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "head", "<meta charset=\"utf-8\" />", "<head><meta charset=\"utf-8\" /></head>");
            Assert.NotNull(parser.Current.Children.ElementAt(0).Parent);
            Assert.Equal(parser.Current, parser.Current.Children.ElementAt(0).Parent);
            Assert.True(parser.Current.Children.ElementAt(0).HasChildren);
            Assert.Single(parser.Current.Children.ElementAt(0).Children);
            Assert.False(parser.Current.Children.ElementAt(0).SelfClosing);
            Assert.Empty(parser.Current.Children.ElementAt(0).Attributes);

            Assert.NotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0).Children.ElementAt(0), "meta", "<meta charset=\"utf-8\" />", "<meta charset=\"utf-8\" />");
            Assert.NotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Parent);
            Assert.Equal(parser.Current.Children.ElementAt(0), parser.Current.Children.ElementAt(0).Children.ElementAt(0).Parent);
            Assert.False(parser.Current.Children.ElementAt(0).Children.ElementAt(0).HasChildren);
            Assert.Empty(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Children);
            Assert.True(parser.Current.Children.ElementAt(0).Children.ElementAt(0).SelfClosing);
            Assert.Single(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Attributes);
            TestUtility.CheckKeyValuePair(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Attributes.ElementAt(0), "charset", "utf-8");

            Assert.NotNull(parser.Current.Children.ElementAt(1));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(1), "body", "", "<body></body>");
            Assert.NotNull(parser.Current.Children.ElementAt(1).Parent);
            Assert.Equal(parser.Current, parser.Current.Children.ElementAt(1).Parent);
            Assert.False(parser.Current.Children.ElementAt(1).HasChildren);
            Assert.Empty(parser.Current.Children.ElementAt(1).Children);
            Assert.False(parser.Current.Children.ElementAt(1).SelfClosing);
            Assert.Empty(parser.Current.Children.ElementAt(1).Attributes);

            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }

        [Fact]
        public void LinkSelfAndCloseTag()
        {
            string html = "<head><link href=\"/favicon.ico\" rel=\"shortcut icon\" type=\"image/x-icon\" /><link href=\"/favicon.ico\" rel=\"shortcut icon\" type=\"image/x-icon\"></link></head>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "head",
                                 "<link href=\"/favicon.ico\" rel=\"shortcut icon\" type=\"image/x-icon\" /><link href=\"/favicon.ico\" rel=\"shortcut icon\" type=\"image/x-icon\"></link>", html);
            Assert.Null(parser.Current.Parent);
            Assert.True(parser.Current.HasChildren);
            Assert.Equal(2, parser.Current.Children.Count());
            Assert.False(parser.Current.SelfClosing);
            Assert.Empty(parser.Current.Attributes);

            Assert.NotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "link",
             "<link href=\"/favicon.ico\" rel=\"shortcut icon\" type=\"image/x-icon\" />", "<link href=\"/favicon.ico\" rel=\"shortcut icon\" type=\"image/x-icon\" />");
            Assert.NotNull(parser.Current.Children.ElementAt(0).Parent);
            Assert.Equal(parser.Current, parser.Current.Children.ElementAt(0).Parent);
            Assert.False(parser.Current.Children.ElementAt(0).HasChildren);
            Assert.Empty(parser.Current.Children.ElementAt(0).Children);
            Assert.True(parser.Current.Children.ElementAt(0).SelfClosing);
            Assert.Equal(3, parser.Current.Children.ElementAt(0).Attributes.Count);
            TestUtility.CheckKeyValuePair(parser.Current.Children.ElementAt(0).Attributes.ElementAt(0), "href", "/favicon.ico");
            TestUtility.CheckKeyValuePair(parser.Current.Children.ElementAt(0).Attributes.ElementAt(1), "rel", "shortcut icon");
            TestUtility.CheckKeyValuePair(parser.Current.Children.ElementAt(0).Attributes.ElementAt(2), "type", "image/x-icon");

            Assert.NotNull(parser.Current.Children.ElementAt(1));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(1), "link", "<link href=\"/favicon.ico\" rel=\"shortcut icon\" type=\"image/x-icon\">", "<link href=\"/favicon.ico\" rel=\"shortcut icon\" type=\"image/x-icon\">");
            Assert.NotNull(parser.Current.Children.ElementAt(1).Parent);
            Assert.Equal(parser.Current, parser.Current.Children.ElementAt(1).Parent);
            Assert.False(parser.Current.Children.ElementAt(1).HasChildren);
            Assert.Empty(parser.Current.Children.ElementAt(1).Children);
            Assert.True(parser.Current.Children.ElementAt(1).SelfClosing);
            Assert.Equal(3, parser.Current.Children.ElementAt(1).Attributes.Count);
            TestUtility.CheckKeyValuePair(parser.Current.Children.ElementAt(1).Attributes.ElementAt(0), "href", "/favicon.ico");
            TestUtility.CheckKeyValuePair(parser.Current.Children.ElementAt(1).Attributes.ElementAt(1), "rel", "shortcut icon");
            TestUtility.CheckKeyValuePair(parser.Current.Children.ElementAt(1).Attributes.ElementAt(2), "type", "image/x-icon");

            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }

        [Fact]
        public void DocTypeAndHtmlNode()
        {
            string html = "<!DOCTYPE html><html></html>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "DOCTYPE", "<!DOCTYPE html>", "<!DOCTYPE html>");
            Assert.Null(parser.Current.Parent);
            Assert.False(parser.Current.HasChildren);
            Assert.Empty(parser.Current.Children);
            Assert.True(parser.Current.SelfClosing);
            Assert.Single(parser.Current.Attributes);
            TestUtility.CheckKeyValuePair(parser.Current.Attributes.ElementAt(0), "html", "");

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "html", "", "<html></html>");
            Assert.Null(parser.Current.Parent);
            Assert.False(parser.Current.HasChildren);
            Assert.Empty(parser.Current.Children);
            Assert.False(parser.Current.SelfClosing);
            Assert.Empty(parser.Current.Attributes);

            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }

        [Fact]
        public void DocTypeAndHtmlNodeWithHeadAndTitle()
        {
            string html = "<!DOCTYPE html><html><head><title></title></head></html>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "DOCTYPE", "<!DOCTYPE html>", "<!DOCTYPE html>");
            Assert.Null(parser.Current.Parent);
            Assert.False(parser.Current.HasChildren);
            Assert.Empty(parser.Current.Children);
            Assert.True(parser.Current.SelfClosing);
            Assert.Single(parser.Current.Attributes);
            TestUtility.CheckKeyValuePair(parser.Current.Attributes.ElementAt(0), "html", "");

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "html", "<head><title></title></head>", "<html><head><title></title></head></html>");
            Assert.Null(parser.Current.Parent);
            Assert.True(parser.Current.HasChildren);
            Assert.Single(parser.Current.Children);
            Assert.False(parser.Current.SelfClosing);
            Assert.Empty(parser.Current.Attributes);

            Assert.NotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "head", "<title></title>", "<head><title></title></head>");
            Assert.NotNull(parser.Current.Children.ElementAt(0).Parent);
            Assert.Equal(parser.Current, parser.Current.Children.ElementAt(0).Parent);
            Assert.True(parser.Current.Children.ElementAt(0).HasChildren);
            Assert.Single(parser.Current.Children.ElementAt(0).Children);
            Assert.False(parser.Current.Children.ElementAt(0).SelfClosing);
            Assert.Empty(parser.Current.Children.ElementAt(0).Attributes);

            Assert.NotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0).Children.ElementAt(0), "title", "", "<title></title>");
            Assert.NotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Parent);
            Assert.Equal(parser.Current.Children.ElementAt(0), parser.Current.Children.ElementAt(0).Children.ElementAt(0).Parent);
            Assert.False(parser.Current.Children.ElementAt(0).Children.ElementAt(0).HasChildren);
            Assert.Empty(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Children);
            Assert.False(parser.Current.Children.ElementAt(0).Children.ElementAt(0).SelfClosing);
            Assert.Empty(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Attributes);

            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }

        [Fact]
        public void DocTypeFullHeadWithAttrAndTitleLink()
        {
            string html = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"><html xmlns=\"http://www.w3.org/1999/xhtml\"><head><link href=\"http://google.com\" /></head></html>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "DOCTYPE", "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">", "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
            Assert.Null(parser.Current.Parent);
            Assert.False(parser.Current.HasChildren);
            Assert.Empty(parser.Current.Children);
            Assert.True(parser.Current.SelfClosing);
            Assert.Equal(4, parser.Current.Attributes.Count);
            TestUtility.CheckKeyValuePair(parser.Current.Attributes.ElementAt(0), "html", "");
            TestUtility.CheckKeyValuePair(parser.Current.Attributes.ElementAt(1), "PUBLIC", "");
            TestUtility.CheckKeyValuePair(parser.Current.Attributes.ElementAt(2), "-//W3C//DTD XHTML 1.0 Transitional//EN", "");
            TestUtility.CheckKeyValuePair(parser.Current.Attributes.ElementAt(3), "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd", "");

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "html", "<head><link href=\"http://google.com\" /></head>", "<html xmlns=\"http://www.w3.org/1999/xhtml\"><head><link href=\"http://google.com\" /></head></html>");
            Assert.Null(parser.Current.Parent);
            Assert.True(parser.Current.HasChildren);
            Assert.Single(parser.Current.Children);
            Assert.False(parser.Current.SelfClosing);
            Assert.Single(parser.Current.Attributes);
            TestUtility.CheckKeyValuePair(parser.Current.Attributes.ElementAt(0), "xmlns", "http://www.w3.org/1999/xhtml");

            Assert.NotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "head", "<link href=\"http://google.com\" />", "<head><link href=\"http://google.com\" /></head>");
            Assert.NotNull(parser.Current.Children.ElementAt(0).Parent);
            Assert.Equal(parser.Current, parser.Current.Children.ElementAt(0).Parent);
            Assert.True(parser.Current.Children.ElementAt(0).HasChildren);
            Assert.Single(parser.Current.Children.ElementAt(0).Children);
            Assert.False(parser.Current.Children.ElementAt(0).SelfClosing);
            Assert.Empty(parser.Current.Children.ElementAt(0).Attributes);

            Assert.NotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0).Children.ElementAt(0), "link", "<link href=\"http://google.com\" />", "<link href=\"http://google.com\" />");
            Assert.NotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Parent);
            Assert.Equal(parser.Current.Children.ElementAt(0), parser.Current.Children.ElementAt(0).Children.ElementAt(0).Parent);
            Assert.False(parser.Current.Children.ElementAt(0).Children.ElementAt(0).HasChildren);
            Assert.Empty(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Children);
            Assert.True(parser.Current.Children.ElementAt(0).Children.ElementAt(0).SelfClosing);
            Assert.Single(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Attributes);
            TestUtility.CheckKeyValuePair(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Attributes.ElementAt(0),
                "href", "http://google.com");

            Assert.False(parser.Traverse());
            Assert.Null(parser.Current);
        }

        [Fact]
        public void CData()
        {
            string html = "<![CDATA[test]]>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.True(parser.Traverse());
            Assert.NotNull(parser.Current);
        }
    }
}
