namespace MariGold.HtmlParser
{
	using System;
	using System.Collections.Generic;
	
	internal class CSSInheritance
	{
		private List<string> tags;
		
		internal CSSInheritance()
		{
			tags = new List<string>()
			{
				"font-family",
				"font-size",
				"color"
			};
		}
		
		private bool CanInherit(string tag)
		{
			return tags.Contains(tag);
		}
		
		private void AppendStyles(HtmlNode node, List<HtmlStyle> styles)
		{
			foreach (HtmlStyle style in styles)
			{
				bool found = false;
				
				for (int i = 0; node.HtmlStyles.Count > i; i++)
				{
					HtmlStyle htmlStyle = node.HtmlStyles[i];
					
					if (string.Compare(style.Name, htmlStyle.Name, true) == 0)
					{
						found = true;
						break;
					}
				}
				
				if (!found)
				{
					node.HtmlStyles.Add(style);
				}
			}
		}
		
		private void CopyStyles(HtmlNode node, List<HtmlStyle> styles)
		{
			foreach (HtmlStyle style in node.HtmlStyles)
			{
				if (CanInherit(style.Name))
				{
					bool found = false;
					
					for (int i = 0; styles.Count > i; i++)
					{
						if (string.Compare(style.Name, styles[i].Name, true) == 0)
						{
							found = true;
							styles[i].ModifyStyle(style.Value);
							break;
						}
					}
					
					if (!found)
					{
						styles.Add(style);
					}
				}
			}
		}
		
		private string FindParentStyle(HtmlNode node, string styleName)
		{
			string value = string.Empty;
			bool found = false;
			
			foreach (HtmlStyle style in node.HtmlStyles)
			{
				if (string.Compare(styleName, style.Name, true) == 0)
				{
					found = true;
					value = style.Value;
				}
			}
			
			if (!found && node.Parent != null)
			{
				value = FindParentStyle(node.Parent, styleName);
			}
			
			return value;
		}
		
		private void InheritFromParent(HtmlNode node)
		{
			foreach (HtmlStyle style in node.HtmlStyles)
			{
				if (string.Compare(style.Value, "inherit", true) == 0 && node.Parent != null)
				{
					string value = FindParentStyle(node.Parent, style.Name);
					
					if (value != string.Empty)
					{
						style.ModifyStyle(value);
					}
				}
			}
		}
		
		private void ApplyToChildren(HtmlNode node, List<HtmlStyle> styles)
		{
			if (node == null)
			{
				return;
			}
			
			AppendStyles(node, styles);
			
			InheritFromParent(node);
			
			CopyStyles(node, styles);
			
			foreach (HtmlNode child in node.Children)
			{
				ApplyToChildren(child, styles);
			}
		}
		
		internal void Apply(HtmlNode node)
		{
			List<HtmlStyle> styles = new List<HtmlStyle>();
			
			ApplyToChildren(node, styles);
		}
	}
}
