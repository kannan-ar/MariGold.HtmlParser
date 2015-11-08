namespace MariGold.HtmlParser.Tests
{
	using System;
	using NUnit.Framework;
	using MariGold.HtmlParser;
	using System.Linq;
	using System.IO;
	
	[TestFixture]
	public class ExternalStyles
	{
		[Test]
		public void BasicExternalStyleSheet()
		{
			string path = TestUtility.GetFolderPath("Html\\basicexternalstyle.htm");
			string html = string.Empty;

			using (StreamReader sr = new StreamReader(path))
			{
				html = sr.ReadToEnd();
			}

			HtmlParser parser = new HtmlTextParser(html);

			Assert.AreEqual(true, parser.Parse());
			parser.ParseCSS();
			
			Assert.IsNotNull(parser.Current);
            
			HtmlNode node = parser.Current.Children[0];
			
			while (node.Tag != "body")
				node = node.Next;
			
			HtmlNode body = node;
			
			node = node.Children[0];
			
			while (node.Tag != "p")
				node = node.Next;
			
			node.AnalyzeNode("p", "test", "<p>test</p>", body, false, true, 1, 0, 1);
			node.Styles.CheckKeyValuePair(0, "font-size", "50px");
		}
		
		[Test]
		public void BasicExternalStyleSheetWithImportant()
		{
			string path = TestUtility.GetFolderPath("Html\\basicstyleimportant.html");
			string html = string.Empty;

			using (StreamReader sr = new StreamReader(path))
			{
				html = sr.ReadToEnd();
			}

			HtmlParser parser = new HtmlTextParser(html);

			Assert.AreEqual(true, parser.Parse());
			parser.ParseCSS();
			
			Assert.IsNotNull(parser.Current);
            
			HtmlNode node = parser.Current.Children[0];
			
			while (node.Tag != "body")
				node = node.Next;
			
			HtmlNode body = node;
			
			node = node.Children[0];
			
			while (node.Tag != "div")
				node = node.Next;
			
			node.AnalyzeNode("div", "test", "<div class=\"cls\">test</div>", body, false, true, 1, 1, 1);
			node.Attributes.CheckKeyValuePair(0, "class", "cls");
			node.Styles.CheckKeyValuePair(0, "font-size", "20px");
		}
		
		[Test]
		public void BasicExternalStyleSheetWithTwoElements()
		{
			string path = TestUtility.GetFolderPath("Html\\basicstylewithtwoelements.html");
			string html = string.Empty;

			using (StreamReader sr = new StreamReader(path))
			{
				html = sr.ReadToEnd();
			}

			HtmlParser parser = new HtmlTextParser(html);

			Assert.AreEqual(true, parser.Parse());
			parser.ParseCSS();
			
			Assert.IsNotNull(parser.Current);
            
			HtmlNode node = parser.Current.Children[0];
			
			while (node.Tag != "body")
				node = node.Next;
			
			HtmlNode body = node;
			
			node = node.Children[0];
			
			while (node.Tag != "div")
				node = node.Next;
			
			node.AnalyzeNode("div", "test", "<div class=\"cls\">test</div>", body, false, true, 1, 1, 1);
			node.Attributes.CheckKeyValuePair(0, "class", "cls");
			node.Styles.CheckKeyValuePair(0, "font-size", "10px");
		}
	}
}
