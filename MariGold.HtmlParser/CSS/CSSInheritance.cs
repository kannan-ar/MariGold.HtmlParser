namespace MariGold.HtmlParser
{
	using System;
	using System.Collections.Generic;
	
	internal sealed class CSSInheritance
	{
		private List<string> tags;
		
		internal CSSInheritance()
		{
			tags = new List<string>() {
				"font-family",
				"font-size",
				"color",
				"font-weight",
				"text-decoration",
				"font-style"
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
					
					if (string.Compare(style.Name, htmlStyle.Name, StringComparison.InvariantCultureIgnoreCase) == 0)
					{
						found = true;
						break;
					}
				}
				
				if (!found)
				{
					node.HtmlStyles.Add(style.Clone());
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
						if (string.Compare(style.Name, styles[i].Name, StringComparison.InvariantCultureIgnoreCase) == 0)
						{
							found = true;
							styles[i].ModifyStyle(style.Value);
							break;
						}
					}
					
					if (!found)
					{
						styles.Add(style.Clone());
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
				if (string.Compare(styleName, style.Name, StringComparison.InvariantCultureIgnoreCase) == 0)
				{
					found = true;
					value = style.Value;
				}
			}
			
			if (!found && node.Parent != null)
			{
				value = FindParentStyle(node.GetParent(), styleName);
			}
			
			return value;
		}
		
		private void InheritFromParent(HtmlNode node)
		{
			foreach (HtmlStyle style in node.HtmlStyles)
			{
				if (string.Compare(style.Value, "inherit", StringComparison.InvariantCultureIgnoreCase) == 0 && node.Parent != null)
				{
					string value = FindParentStyle(node.GetParent(), style.Name);
					
					if (value != string.Empty)
					{
						style.ModifyStyle(value);
					}
				}
			}
		}
		
		private List<HtmlStyle> CloneStyles(List<HtmlStyle> styles)
		{
			List<HtmlStyle> newStyles = new List<HtmlStyle>();
			
			foreach (HtmlStyle style in styles)
			{
				newStyles.Add(style.Clone());
			}
			
			return newStyles;
		}
		
		private void ApplyToChildren(HtmlNode node, List<HtmlStyle> styles)
		{
			if (node == null)
			{
				return;
			}
			
			AppendStyles(node, styles);
			
			InheritFromParent(node);
			
			List<HtmlStyle> newStyles = CloneStyles(styles);
			
			CopyStyles(node, newStyles);
			
			foreach (HtmlNode child in node.GetChildren())
			{
				ApplyToChildren(child, newStyles);
			}
			
			if (node.Parent == null && node.Next != null)
			{
				ApplyToChildren(node.GetNext(), styles);
			}
		}
		
		internal void Apply(HtmlNode node)
		{
			List<HtmlStyle> styles = new List<HtmlStyle>();
			
			ApplyToChildren(node, styles);
		}
	}
}
