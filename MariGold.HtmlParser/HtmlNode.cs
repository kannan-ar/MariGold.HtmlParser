namespace MariGold.HtmlParser
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Stores all the information of an HTML element including child elements, attributes and CSS styles
	/// </summary>
	public sealed class HtmlNode
	{
		private readonly string tag;
		private readonly HtmlNode parent;
		private readonly HtmlContext context;
		private readonly List<HtmlStyle> htmlStyles;
		private readonly bool isText;
		
		private int htmlStart;
		private int textStart;
		private int textEnd;
		private int htmlEnd;
		private bool selfClosing;
		private List<HtmlNode> children;
		private HtmlNode previous;
		private HtmlNode next;
		private Dictionary<string, string> attributes;
		private Dictionary<string, string> styles;

		internal HtmlNode(string tag, int htmlStart, int textStart, HtmlContext context, HtmlNode parent)
		{
			if (string.IsNullOrEmpty(tag))
			{
				throw new ArgumentNullException("tag");
			}

			if (context == null)
			{
				throw new ArgumentNullException("context");
			}

			this.tag = tag;
			this.htmlStart = htmlStart;
			this.textStart = textStart;
			this.textEnd = -1;
			this.htmlEnd = -1;
			this.context = context;
			this.parent = parent;
			htmlStyles = new List<HtmlStyle>();
			isText = tag == HtmlTag.TEXT;

			if (parent != null)
			{
				parent.Children.Add(this);
			}
		}

		internal HtmlNode(string tag, int htmlStart, int textStart, int textEnd, int htmlEnd,
			HtmlContext context, HtmlNode parent)
			: this(tag, htmlStart, textStart, context, parent)
		{
			SetBoundary(textEnd, htmlEnd);
		}

		internal bool IsOpened
		{
			get
			{
				return textEnd == -1 && htmlEnd == -1;
			}
		}

		internal List<HtmlStyle> HtmlStyles
		{
			get
			{
				return htmlStyles;
			}
		}
        
		internal void SetBoundary(int textEnd, int htmlEnd)
		{
			if (textEnd != -1 && textEnd < textStart)
			{
				throw new ArgumentOutOfRangeException("textEnd");
			}

			if (htmlEnd != -1 && htmlEnd <= htmlStart)
			{
				throw new ArgumentOutOfRangeException("htmlEnd");
			}

			this.textEnd = textEnd;
			this.htmlEnd = htmlEnd;
		}

		internal void Finilize(int position)
		{
			if (textEnd == -1)
			{
				textEnd = position;
			}

			if (htmlEnd == -1)
			{
				htmlEnd = position;
			}
		}

		internal void AddStyles(IEnumerable<HtmlStyle> styles)
		{
			htmlStyles.AddRange(styles);
		}

		internal void CopyHtmlStyles(List<HtmlStyle> newStyles, SelectorWeight weight)
		{
			foreach (HtmlStyle newStyle in newStyles)
			{
				bool found = false;

				newStyle.Weight = weight;

				foreach (HtmlStyle style in htmlStyles)
				{
					if (string.Compare(newStyle.Name, style.Name, true) == 0)
					{
						found = true;
						style.OverWrite(newStyle);
					}
				}

				if (!found)
				{
					htmlStyles.Add(newStyle);
				}
			}
		}

		/// <summary>
		/// Html tag of the node.
		/// </summary>
		public string Tag
		{
			get
			{
				return tag;
			}
		}

		/// <summary>
		/// Inner Html string of the node
		/// </summary>
		public string InnerHtml
		{
			get
			{
				return context.Html.Substring(textStart, textEnd - textStart);
			}
		}

		/// <summary>
		/// Html string of the node.
		/// </summary>
		public string Html
		{
			get
			{
				return context.Html.Substring(htmlStart, htmlEnd - htmlStart);
			}
		}

		/// <summary>
		/// Parent node.
		/// </summary>
		public HtmlNode Parent
		{
			get
			{
				return parent;
			}
		}

		/// <summary>
		/// List of child nodes
		/// </summary>
		public List<HtmlNode> Children
		{
			get
			{
				if (children == null)
				{
					children = new List<HtmlNode>();
				}

				return children;
			}
		}

		/// <summary>
		/// Previous Node
		/// </summary>
		public HtmlNode Previous
		{
			get
			{
				return previous;
			}

			internal set
			{
				previous = value;
			}
		}

		/// <summary>
		/// Next Node
		/// </summary>
		public HtmlNode Next
		{
			get
			{
				return next;
			}

			internal set
			{
				next = value;
			}
		}

		/// <summary>
		/// Returns true if the node has any child nodes.
		/// </summary>
		public bool HasChildren
		{
			get
			{
				return children != null && children.Count > 0;
			}
		}

		/// <summary>
		/// Returns true if the html element is self closing
		/// </summary>
		public bool SelfClosing
		{
			get
			{
				return selfClosing;
			}

			internal set
			{
				selfClosing = value;
			}
		}

		public bool IsText
		{
			get
			{
				return isText;
			}
		}
		
		/// <summary>
		/// Dictionary of html attributes
		/// </summary>
		public Dictionary<string, string> Attributes
		{
			get
			{
				if (attributes == null)
				{
					attributes = new Dictionary<string, string>();
				}

				return attributes;
			}
		}

		/// <summary>
		/// Dictionary of CSS styles
		/// </summary>
		public Dictionary<string, string> Styles
		{
			get
			{
				if (styles == null)
				{
					styles = new Dictionary<string, string>();

					foreach (HtmlStyle style in htmlStyles)
					{
						styles.Add(style.Name, style.Value);
					}
				}

				return styles;
			}
		}
	}
}
