namespace MariGold.HtmlParser.Tests
{
	using System;
	using NUnit.Framework;
	using MariGold.HtmlParser;
	using System.Linq;
	
	[TestFixture]
	public class SpecialTags
	{
		[Test]
		public void MetaTag()
		{
			string html = "<html><head><meta charset=\"utf-8\" /></head><body></body></html>";
			
			HtmlParser parser = new HtmlTextParser(html);
			
			Assert.AreEqual(true, parser.Traverse());
			Assert.IsNotNull(parser.Current);
			TestUtility.AreEqual(parser.Current, "html", "<head><meta charset=\"utf-8\" /></head><body></body>", html);
			Assert.IsNull(parser.Current.Parent);
			Assert.AreEqual(true, parser.Current.HasChildren);
			Assert.AreEqual(2, parser.Current.Children.Count());
			Assert.AreEqual(false, parser.Current.SelfClosing);
			Assert.AreEqual(0, parser.Current.Attributes.Count);
			
			Assert.IsNotNull(parser.Current.Children.ElementAt(0));
			TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "head", "<meta charset=\"utf-8\" />", "<head><meta charset=\"utf-8\" /></head>");
			Assert.IsNotNull(parser.Current.Children.ElementAt(0).Parent);
			Assert.AreEqual(parser.Current, parser.Current.Children.ElementAt(0).Parent);
			Assert.AreEqual(true, parser.Current.Children.ElementAt(0).HasChildren);
			Assert.AreEqual(1, parser.Current.Children.ElementAt(0).Children.Count());
			Assert.AreEqual(false, parser.Current.Children.ElementAt(0).SelfClosing);
			Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Attributes.Count);
			
			Assert.IsNotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0));
			TestUtility.AreEqual(parser.Current.Children.ElementAt(0).Children.ElementAt(0), "meta", "<meta charset=\"utf-8\" />", "<meta charset=\"utf-8\" />");
			Assert.IsNotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Parent);
			Assert.AreEqual(parser.Current.Children.ElementAt(0), parser.Current.Children.ElementAt(0).Children.ElementAt(0).Parent);
			Assert.AreEqual(false, parser.Current.Children.ElementAt(0).Children.ElementAt(0).HasChildren);
			Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Children.ElementAt(0).Children.Count());
			Assert.AreEqual(true, parser.Current.Children.ElementAt(0).Children.ElementAt(0).SelfClosing);
			Assert.AreEqual(1, parser.Current.Children.ElementAt(0).Children.ElementAt(0).Attributes.Count);
			TestUtility.CheckKeyValuePair(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Attributes.ElementAt(0), "charset", "utf-8");
			
			Assert.IsNotNull(parser.Current.Children.ElementAt(1));
			TestUtility.AreEqual(parser.Current.Children.ElementAt(1), "body", "", "<body></body>");
			Assert.IsNotNull(parser.Current.Children.ElementAt(1).Parent);
			Assert.AreEqual(parser.Current, parser.Current.Children.ElementAt(1).Parent);
			Assert.AreEqual(false, parser.Current.Children.ElementAt(1).HasChildren);
			Assert.AreEqual(0, parser.Current.Children.ElementAt(1).Children.Count());
			Assert.AreEqual(false, parser.Current.Children.ElementAt(1).SelfClosing);
			Assert.AreEqual(0, parser.Current.Children.ElementAt(1).Attributes.Count);
			
			Assert.AreEqual(false, parser.Traverse());
			Assert.IsNull(parser.Current);
		}
		
		[Test]
		public void LinkSelfAndCloseTag()
		{
			string html = "<head><link href=\"/favicon.ico\" rel=\"shortcut icon\" type=\"image/x-icon\" /><link href=\"/favicon.ico\" rel=\"shortcut icon\" type=\"image/x-icon\"></link></head>";
			
			HtmlParser parser = new HtmlTextParser(html);
			
			Assert.AreEqual(true, parser.Traverse());
			Assert.IsNotNull(parser.Current);
			TestUtility.AreEqual(parser.Current, "head", 
			                     "<link href=\"/favicon.ico\" rel=\"shortcut icon\" type=\"image/x-icon\" /><link href=\"/favicon.ico\" rel=\"shortcut icon\" type=\"image/x-icon\"></link>", html);
			Assert.IsNull(parser.Current.Parent);
			Assert.AreEqual(true, parser.Current.HasChildren);
			Assert.AreEqual(2, parser.Current.Children.Count());
			Assert.AreEqual(false, parser.Current.SelfClosing);
			Assert.AreEqual(0, parser.Current.Attributes.Count);
			
			Assert.IsNotNull(parser.Current.Children.ElementAt(0));
			TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "link", 
			 "<link href=\"/favicon.ico\" rel=\"shortcut icon\" type=\"image/x-icon\" />", "<link href=\"/favicon.ico\" rel=\"shortcut icon\" type=\"image/x-icon\" />");
			Assert.IsNotNull(parser.Current.Children.ElementAt(0).Parent);
			Assert.AreEqual(parser.Current, parser.Current.Children.ElementAt(0).Parent);
			Assert.AreEqual(false, parser.Current.Children.ElementAt(0).HasChildren);
			Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Children.Count());
			Assert.AreEqual(true, parser.Current.Children.ElementAt(0).SelfClosing);
			Assert.AreEqual(3, parser.Current.Children.ElementAt(0).Attributes.Count);
			TestUtility.CheckKeyValuePair(parser.Current.Children.ElementAt(0).Attributes.ElementAt(0), "href", "/favicon.ico");
			TestUtility.CheckKeyValuePair(parser.Current.Children.ElementAt(0).Attributes.ElementAt(1), "rel", "shortcut icon");
			TestUtility.CheckKeyValuePair(parser.Current.Children.ElementAt(0).Attributes.ElementAt(2), "type", "image/x-icon");
			
			Assert.IsNotNull(parser.Current.Children.ElementAt(1));
			TestUtility.AreEqual(parser.Current.Children.ElementAt(1), "link", "<link href=\"/favicon.ico\" rel=\"shortcut icon\" type=\"image/x-icon\">", "<link href=\"/favicon.ico\" rel=\"shortcut icon\" type=\"image/x-icon\">");
			Assert.IsNotNull(parser.Current.Children.ElementAt(1).Parent);
			Assert.AreEqual(parser.Current, parser.Current.Children.ElementAt(1).Parent);
			Assert.AreEqual(false, parser.Current.Children.ElementAt(1).HasChildren);
			Assert.AreEqual(0, parser.Current.Children.ElementAt(1).Children.Count());
			Assert.AreEqual(true, parser.Current.Children.ElementAt(1).SelfClosing);
			Assert.AreEqual(3, parser.Current.Children.ElementAt(1).Attributes.Count);
			TestUtility.CheckKeyValuePair(parser.Current.Children.ElementAt(1).Attributes.ElementAt(0), "href", "/favicon.ico");
			TestUtility.CheckKeyValuePair(parser.Current.Children.ElementAt(1).Attributes.ElementAt(1), "rel", "shortcut icon");
			TestUtility.CheckKeyValuePair(parser.Current.Children.ElementAt(1).Attributes.ElementAt(2), "type", "image/x-icon");
			
			Assert.AreEqual(false, parser.Traverse());
			Assert.IsNull(parser.Current);
		}
		
		[Test]
		public void DocTypeAndHtmlNode()
		{
			string html = "<!DOCTYPE html><html></html>";
			
			HtmlParser parser = new HtmlTextParser(html);
			
			Assert.AreEqual(true, parser.Traverse());
			Assert.IsNotNull(parser.Current);
			TestUtility.AreEqual(parser.Current, "DOCTYPE", "<!DOCTYPE html>", "<!DOCTYPE html>");
			Assert.IsNull(parser.Current.Parent);
			Assert.AreEqual(false, parser.Current.HasChildren);
			Assert.AreEqual(0, parser.Current.Children.Count());
			Assert.AreEqual(true, parser.Current.SelfClosing);
			Assert.AreEqual(1, parser.Current.Attributes.Count);
			TestUtility.CheckKeyValuePair(parser.Current.Attributes.ElementAt(0), "html", "");
			
			Assert.AreEqual(true, parser.Traverse());
			Assert.IsNotNull(parser.Current);
			TestUtility.AreEqual(parser.Current, "html", "", "<html></html>");
			Assert.IsNull(parser.Current.Parent);
			Assert.AreEqual(false, parser.Current.HasChildren);
			Assert.AreEqual(0, parser.Current.Children.Count());
			Assert.AreEqual(false, parser.Current.SelfClosing);
			Assert.AreEqual(0, parser.Current.Attributes.Count);
			
			Assert.AreEqual(false, parser.Traverse());
			Assert.IsNull(parser.Current);
		}

        [Test]
        public void DocTypeAndHtmlNodeWithHeadAndTitle()
        {
            string html = "<!DOCTYPE html><html><head><title></title></head></html>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "DOCTYPE", "<!DOCTYPE html>", "<!DOCTYPE html>");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(false, parser.Current.HasChildren);
            Assert.AreEqual(0, parser.Current.Children.Count());
            Assert.AreEqual(true, parser.Current.SelfClosing);
            Assert.AreEqual(1, parser.Current.Attributes.Count);
            TestUtility.CheckKeyValuePair(parser.Current.Attributes.ElementAt(0), "html", "");

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "html", "<head><title></title></head>", "<html><head><title></title></head></html>");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(true, parser.Current.HasChildren);
            Assert.AreEqual(1, parser.Current.Children.Count());
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(0, parser.Current.Attributes.Count);

            Assert.IsNotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "head", "<title></title>", "<head><title></title></head>");
            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children.ElementAt(0).Parent);
            Assert.AreEqual(true, parser.Current.Children.ElementAt(0).HasChildren);
            Assert.AreEqual(1, parser.Current.Children.ElementAt(0).Children.Count());
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).SelfClosing);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Attributes.Count);

            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0).Children.ElementAt(0), "title", "", "<title></title>");
            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Parent);
            Assert.AreEqual(parser.Current.Children.ElementAt(0), parser.Current.Children.ElementAt(0).Children.ElementAt(0).Parent);
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).Children.ElementAt(0).HasChildren);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Children.ElementAt(0).Children.Count());
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).Children.ElementAt(0).SelfClosing);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Children.ElementAt(0).Attributes.Count);

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void DocTypeFullHeadWithAttrAndTitleLink()
        {
            string html = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"><html xmlns=\"http://www.w3.org/1999/xhtml\"><head><link href=\"http://google.com\" /></head></html>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "DOCTYPE", "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">", "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(false, parser.Current.HasChildren);
            Assert.AreEqual(0, parser.Current.Children.Count());
            Assert.AreEqual(true, parser.Current.SelfClosing);
            Assert.AreEqual(4, parser.Current.Attributes.Count);
            TestUtility.CheckKeyValuePair(parser.Current.Attributes.ElementAt(0), "html", "");
            TestUtility.CheckKeyValuePair(parser.Current.Attributes.ElementAt(1), "PUBLIC", "");
            TestUtility.CheckKeyValuePair(parser.Current.Attributes.ElementAt(2), "-//W3C//DTD XHTML 1.0 Transitional//EN", "");
            TestUtility.CheckKeyValuePair(parser.Current.Attributes.ElementAt(3), "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd", "");

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
            TestUtility.AreEqual(parser.Current, "html", "<head><link href=\"http://google.com\" /></head>", "<html xmlns=\"http://www.w3.org/1999/xhtml\"><head><link href=\"http://google.com\" /></head></html>");
            Assert.IsNull(parser.Current.Parent);
            Assert.AreEqual(true, parser.Current.HasChildren);
            Assert.AreEqual(1, parser.Current.Children.Count());
            Assert.AreEqual(false, parser.Current.SelfClosing);
            Assert.AreEqual(1, parser.Current.Attributes.Count);
            TestUtility.CheckKeyValuePair(parser.Current.Attributes.ElementAt(0), "xmlns", "http://www.w3.org/1999/xhtml");

            Assert.IsNotNull(parser.Current.Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0), "head", "<link href=\"http://google.com\" />", "<head><link href=\"http://google.com\" /></head>");
            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Parent);
            Assert.AreEqual(parser.Current, parser.Current.Children.ElementAt(0).Parent);
            Assert.AreEqual(true, parser.Current.Children.ElementAt(0).HasChildren);
            Assert.AreEqual(1, parser.Current.Children.ElementAt(0).Children.Count());
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).SelfClosing);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Attributes.Count);

            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0));
            TestUtility.AreEqual(parser.Current.Children.ElementAt(0).Children.ElementAt(0), "link", "<link href=\"http://google.com\" />", "<link href=\"http://google.com\" />");
            Assert.IsNotNull(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Parent);
            Assert.AreEqual(parser.Current.Children.ElementAt(0), parser.Current.Children.ElementAt(0).Children.ElementAt(0).Parent);
            Assert.AreEqual(false, parser.Current.Children.ElementAt(0).Children.ElementAt(0).HasChildren);
            Assert.AreEqual(0, parser.Current.Children.ElementAt(0).Children.ElementAt(0).Children.Count());
            Assert.AreEqual(true, parser.Current.Children.ElementAt(0).Children.ElementAt(0).SelfClosing);
            Assert.AreEqual(1, parser.Current.Children.ElementAt(0).Children.ElementAt(0).Attributes.Count);
            TestUtility.CheckKeyValuePair(parser.Current.Children.ElementAt(0).Children.ElementAt(0).Attributes.ElementAt(0),
                "href", "http://google.com");

            Assert.AreEqual(false, parser.Traverse());
            Assert.IsNull(parser.Current);
        }

        [Test]
        public void CData()
        {
            string html = "<![CDATA[test]]>";

            HtmlParser parser = new HtmlTextParser(html);

            Assert.AreEqual(true, parser.Traverse());
            Assert.IsNotNull(parser.Current);
        }
	}
}
