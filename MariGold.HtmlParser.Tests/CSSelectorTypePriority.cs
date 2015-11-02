namespace MariGold.HtmlParser.Tests
{
	using System;
	using NUnit.Framework;
	using MariGold.HtmlParser;
	using System.Linq;
	
	[TestFixture]
	public class CSSelectorTypePriority
	{
		[Test]
		public void InlineIdentity()
		{
			string html = @"<style>
                                .cls
                                {
                                	color:#fff;
                                }
                            </style>
                            <div class='cls' style='color:#000'></div>";
			
			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			HtmlNode node = parser.Current;
			
			while (node.Tag != "div")
			{
				node = node.Next;
			}
			
			TestUtility.AnalyzeNode(node, "div", "", "<div class='cls' style='color:#000'></div>", null, false, false, 0, 2, 1);
			TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "class", "cls");
			TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(1), "style", "color:#000");
			TestUtility.CheckKeyValuePair(node.Styles.ElementAt(0), "color", "#000");
		}
		
		[Test]
		public void ClassAttribute()
		{
			string html = @"<style>
                                .cls
                                {
                                	color:red;
                                }
                                
                                [attr]
                                {
                                	color:blue;
                                }
                            </style>
                            <div class='cls' attr></div>";
			
			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			HtmlNode node = parser.Current;
			
			while (node.Tag != "div")
			{
				node = node.Next;
			}
			
			TestUtility.AnalyzeNode(node, "div", "", "<div class='cls' attr></div>", null, false, false, 0, 2, 1);
			TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "class", "cls");
			TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(1), "attr", "");
			TestUtility.CheckKeyValuePair(node.Styles.ElementAt(0), "color", "blue");
		}
		
		[Test]
		public void AttributeClass()
		{
			string html = @"<style>
			
                                [attr]
                                {
                                	color:blue;
                                }
                                
                                .cls
                                {
                                	color:red;
                                }
                                
                            </style>
                            <div class='cls' attr></div>";
			
			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			HtmlNode node = parser.Current;
			
			while (node.Tag != "div")
			{
				node = node.Next;
			}
			
			TestUtility.AnalyzeNode(node, "div", "", "<div class='cls' attr></div>", null, false, false, 0, 2, 1);
			TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "class", "cls");
			TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(1), "attr", "");
			TestUtility.CheckKeyValuePair(node.Styles.ElementAt(0), "color", "red");
		}
		
		[Test]
		public void ElementClass()
		{
			string html = @"<style>
			
								div
                                {
                                	color:red;
                                }
                                
                                .cls
                                {
                                	color:blue;
                                }
                                
                            </style>
                            <div class='cls'></div>";
			
			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			HtmlNode node = parser.Current;
			
			while (node.Tag != "div")
			{
				node = node.Next;
			}
			
			TestUtility.AnalyzeNode(node, "div", "", "<div class='cls'></div>", null, false, false, 0, 1, 1);
			TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "class", "cls");
			TestUtility.CheckKeyValuePair(node.Styles.ElementAt(0), "color", "blue");
		}
		
		[Test]
		public void ClassElement()
		{
			string html = @"<style>
			
                                .cls
                                {
                                	color:blue;
                                }
                                
                                div
                                {
                                	color:red;
                                }
                                
                            </style>
                            <div class='cls'></div>";
			
			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			HtmlNode node = parser.Current;
			
			while (node.Tag != "div")
			{
				node = node.Next;
			}
			
			TestUtility.AnalyzeNode(node, "div", "", "<div class='cls'></div>", null, false, false, 0, 1, 1);
			TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "class", "cls");
			TestUtility.CheckKeyValuePair(node.Styles.ElementAt(0), "color", "blue");
		}
		
		[Test]
		public void ClassIdentity()
		{
			string html = @"<style>
			
                                .cls
                                {
                                	color:blue;
                                }
                                
                                #dv
                                {
                                	color:red;
                                }
                                
                            </style>
                            <div id='dv' class='cls'></div>";
			
			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			HtmlNode node = parser.Current;
			
			while (node.Tag != "div")
			{
				node = node.Next;
			}
			
			TestUtility.AnalyzeNode(node, "div", "", "<div id='dv' class='cls'></div>", null, false, false, 0, 2, 1);
			TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "id", "dv");		
			TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(1), "class", "cls");
			TestUtility.CheckKeyValuePair(node.Styles.ElementAt(0), "color", "red");
		}
		
		[Test]
		public void IdentityClass()
		{
			string html = @"<style>
			
                                #dv
                                {
                                	color:red;
                                }
                                
                                .cls
                                {
                                	color:blue;
                                }
                                
                            </style>
                            <div id='dv' class='cls'></div>";
			
			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			HtmlNode node = parser.Current;
			
			while (node.Tag != "div")
			{
				node = node.Next;
			}
			
			TestUtility.AnalyzeNode(node, "div", "", "<div id='dv' class='cls'></div>", null, false, false, 0, 2, 1);
			TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "id", "dv");		
			TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(1), "class", "cls");
			TestUtility.CheckKeyValuePair(node.Styles.ElementAt(0), "color", "red");
		}
		
		[Test]
		public void GlobalClass()
		{
			string html = @"<style>
			
                                *
                                {
                                	color:red;
                                }
                                
                                .cls
                                {
                                	color:blue;
                                }
                                
                            </style>
                            <div class='cls'></div>";
			
			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			HtmlNode node = parser.Current;
			
			while (node.Tag != "div")
			{
				node = node.Next;
			}
			
			TestUtility.AnalyzeNode(node, "div", "", "<div class='cls'></div>", null, false, false, 0, 1, 1);
			TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "class", "cls");
			TestUtility.CheckKeyValuePair(node.Styles.ElementAt(0), "color", "blue");
		}
		
		[Test]
		public void ClassImportantInline()
		{
			string html = @"<style>
			
                                .cls
                                {
                                	color:blue !important;
                                }
                                
                            </style>
                            <div class='cls' style='color:red'></div>";
			
			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			HtmlNode node = parser.Current;
			
			while (node.Tag != "div")
			{
				node = node.Next;
			}
			
			TestUtility.AnalyzeNode(node, "div", "", "<div class='cls' style='color:red'></div>", null, false, false, 0, 2, 1);
			TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "class", "cls");
			TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(1), "style", "color:red");
			TestUtility.CheckKeyValuePair(node.Styles.ElementAt(0), "color", "blue");
		}
	}
}
