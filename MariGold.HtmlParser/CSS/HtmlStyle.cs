namespace MariGold.HtmlParser
{
	using System;
	using System.Collections.Generic;

	internal sealed class HtmlStyle
	{
		private string name;
		private string value;
		private bool important;
		private int specificity;

		public string Name
		{
			get
			{
				return name;
			}
		}

		public string Value
		{
			get
			{
				return value;
			}
		}

		public bool Important
		{
			get
			{
				return important;
			}

			internal set
			{
				important = value;
			}
		}

        internal int Specificity
		{
			get
			{
				return specificity;
			}

			set
			{
				specificity = value;
			}
		}

        internal HtmlStyle(string name, string value, bool important, int specificity)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name");
			}

			if (string.IsNullOrEmpty(value))
			{
				throw new ArgumentNullException("value");
			}

            if (specificity < 0)
			{
                throw new ArgumentOutOfRangeException("specificity");
			}

			this.name = name;
			this.value = value;
			this.important = important;
            this.specificity = specificity;
		}

		internal void OverWrite(HtmlStyle htmlStyle)
		{
			if (string.Compare(name, htmlStyle.Name, StringComparison.InvariantCultureIgnoreCase) == 0)
			{
				if (!important && htmlStyle.Important)
				{
					value = htmlStyle.Value;
					important = htmlStyle.Important;
					specificity = htmlStyle.Specificity;
				}
                else if (htmlStyle.Specificity >= specificity && (!important || (important && htmlStyle.Important)))
				{
					value = htmlStyle.Value;
				}
			}
		}

		internal void ModifyStyle(string value)
		{
			this.value = value;
		}
		
		internal HtmlStyle Clone()
		{
			return new HtmlStyle(name, value, important, specificity);
		}
		
		static internal bool IsNonStyleElement(string tag)
		{
			tag = tag.ToLower();

			return tag == "style" || tag == "script" || tag == "html";
		}
	}
}
