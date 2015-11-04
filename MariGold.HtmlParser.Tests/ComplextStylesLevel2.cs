namespace MariGold.HtmlParser.Tests
{
	using System;
	using NUnit.Framework;
	using MariGold.HtmlParser;
	using System.Linq;
	
	[TestFixture]
	public partial class ComplexStyles
	{
		[Test]
		public void AttributeImmediateChildrenClassIdentity()
		{
			string html = @"<style>
								[attr] > .cls #pt
								{
									background-color:red;
								}
							</style>
							<div attr><div class='cls'><p>one1</p><p id='pt'>one2</p></div><div>two</div></div>";
			
			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			HtmlNode node = parser.Current;
			
			while (node.Tag != "div")
			{
				node = node.Next;
			}
			
			TestUtility.AnalyzeNode(node, "div", "<div class='cls'><p>one1</p><p id='pt'>one2</p></div><div>two</div>", 
			                        "<div attr><div class='cls'><p>one1</p><p id='pt'>one2</p></div><div>two</div></div>", null, false, true, 2, 1, 0);
			TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "attr", "");
			
			TestUtility.AnalyzeNode(node.Children[0], "div", "<p>one1</p><p id='pt'>one2</p>", "<div class='cls'><p>one1</p><p id='pt'>one2</p></div>", node, false, true, 2, 1, 0);
			TestUtility.CheckKeyValuePair(node.Children[0].Attributes.ElementAt(0), "class", "cls");
			
			TestUtility.AnalyzeNode(node.Children[0].Children[0], "p", "one1", "<p>one1</p>", node.Children[0], false, true, 1, 0, 0);
			
			TestUtility.AnalyzeNode(node.Children[0].Children[1], "p", "one2", "<p id='pt'>one2</p>", node.Children[0], false, true, 1, 1, 1);
			TestUtility.CheckKeyValuePair(node.Children[0].Children[1].Attributes.ElementAt(0), "id", "pt");
			TestUtility.CheckKeyValuePair(node.Children[0].Children[1].Styles.ElementAt(0), "background-color", "red");
			
			TestUtility.AnalyzeNode(node.Children[1], "div", "two", "<div>two</div>", node, false, true, 1, 0, 0);
			
		}
		
		[Test]
		public void AttributeImmediateChildrenClassIdentityNthChild()
		{
			string html = @"<style>
								[attr] > .cls #pt:nth-child(2)
								{
									background-color:red;
								}
							</style>
							<div attr><div class='cls'><p>one1</p><p id='pt'>one2</p></div><div>two</div></div>";
			
			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			HtmlNode node = parser.Current;
			
			while (node.Tag != "div")
			{
				node = node.Next;
			}
			
			TestUtility.AnalyzeNode(node, "div", "<div class='cls'><p>one1</p><p id='pt'>one2</p></div><div>two</div>", 
			                        "<div attr><div class='cls'><p>one1</p><p id='pt'>one2</p></div><div>two</div></div>", null, false, true, 2, 1, 0);
			TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "attr", "");
			
			TestUtility.AnalyzeNode(node.Children[0], "div", "<p>one1</p><p id='pt'>one2</p>", "<div class='cls'><p>one1</p><p id='pt'>one2</p></div>", node, false, true, 2, 1, 0);
			TestUtility.CheckKeyValuePair(node.Children[0].Attributes.ElementAt(0), "class", "cls");
			
			TestUtility.AnalyzeNode(node.Children[0].Children[0], "p", "one1", "<p>one1</p>", node.Children[0], false, true, 1, 0, 0);
			
			TestUtility.AnalyzeNode(node.Children[0].Children[1], "p", "one2", "<p id='pt'>one2</p>", node.Children[0], false, true, 1, 1, 1);
			TestUtility.CheckKeyValuePair(node.Children[0].Children[1].Attributes.ElementAt(0), "id", "pt");
			TestUtility.CheckKeyValuePair(node.Children[0].Children[1].Styles.ElementAt(0), "background-color", "red");
			
			TestUtility.AnalyzeNode(node.Children[1], "div", "two", "<div>two</div>", node, false, true, 1, 0, 0);
			
		}
		
		[Test]
		public void AttributeImmediateChildrenClassIdentitySpanFirstChild()
		{
			string html = @"<style>
								[attr] > .cls #pt span:first-child
								{
									background-color:red;
								}
							</style>
							<div attr><div class='cls'><p>one1</p><p id='pt'><span>1</span><span>2</span></p></div><div>two</div></div>";
			
			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			HtmlNode node = parser.Current;
			
			while (node.Tag != "div")
			{
				node = node.Next;
			}
			
			TestUtility.AnalyzeNode(node, "div", "<div class='cls'><p>one1</p><p id='pt'><span>1</span><span>2</span></p></div><div>two</div>", 
			                        "<div attr><div class='cls'><p>one1</p><p id='pt'><span>1</span><span>2</span></p></div><div>two</div></div>", null, false, true, 2, 1, 0);
			TestUtility.CheckKeyValuePair(node.Attributes.ElementAt(0), "attr", "");
			
			TestUtility.AnalyzeNode(node.Children[0], "div", "<p>one1</p><p id='pt'><span>1</span><span>2</span></p>", "<div class='cls'><p>one1</p><p id='pt'><span>1</span><span>2</span></p></div>", 
			                        node, false, true, 2, 1, 0);
			TestUtility.CheckKeyValuePair(node.Children[0].Attributes.ElementAt(0), "class", "cls");
			
			TestUtility.AnalyzeNode(node.Children[0].Children[0], "p", "one1", "<p>one1</p>", node.Children[0], false, true, 1, 0, 0);
			
			TestUtility.AnalyzeNode(node.Children[0].Children[1], "p", "<span>1</span><span>2</span>", "<p id='pt'><span>1</span><span>2</span></p>", node.Children[0], false, true, 2, 1, 0);
			TestUtility.CheckKeyValuePair(node.Children[0].Children[1].Attributes.ElementAt(0), "id", "pt");
			
			TestUtility.AnalyzeNode(node.Children[0].Children[1].Children[0], "span", "1", "<span>1</span>", node.Children[0].Children[1], false, true, 1, 0, 1);
			TestUtility.CheckKeyValuePair(node.Children[0].Children[1].Children[0].Styles.ElementAt(0), "background-color", "red");
			
			TestUtility.AnalyzeNode(node.Children[0].Children[1].Children[1], "span", "2", "<span>2</span>", node.Children[0].Children[1], false, true, 1, 0, 0);
			
			TestUtility.AnalyzeNode(node.Children[1], "div", "two", "<div>two</div>", node, false, true, 1, 0, 0);
			
		}
		
		public void FirstChildIdentity()
		{
			string html = @"<style>
								:first-child #dv > p
								{
									background-color:red;
								}
							</style>
							<div><div><div id='dv'><p>1</p><span>2</span></div></div><div>3</div></div>";
			
			HtmlParser parser = new HtmlTextParser(html);

			Assert.IsTrue(parser.Parse());
			parser.ParseCSS();

			Assert.IsNotNull(parser.Current);
			
			HtmlNode node = parser.Current;
			/*
			TestUtility.AnalyzeNode(node,"div","<div><div id='dv'><p>1</p><span>2</span></div></div><div>3</div>",
			                        "<div><div><div id='dv'><p>1</p><span>2</span></div></div><div>3</div></div>",null,false,true,
			*/
		}
	}
}
