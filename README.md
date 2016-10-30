##MariGold.HtmlParser
MariGold.HtmlParser is a utility to parse the HTML documents into a collection of IHtmlNode type instances. You can either traverse through the document by parsing every root elements one by one or parse the entire document at once. Once an HTML element parsed, it will recursively parse all the child elements.

###Installing via NuGet

In Package Manager Console, enter the following command:
```
Install-Package MariGold.HtmlParser
```

####Traverse through html elements
In the following example, the first loop iteration will parse the first div and the following div in the second and final iteration.
```csharp
using MariGold.HtmlParser;

HtmlParser parser = new HtmlTextParser("<div>first</div><div>second</div>");

while (parser.Traverse())
{
	IHtmlNode node = parser.Current;
}
```

####Parse the HTML document
To parse the entire document at once, use Parse method. It will parse all the HTML elements in the given document and Current property will point to the first root element
```csharp
if (parser.Parse())
{
	IHtmlNode node = parser.Current;
}
```

####Travel through IHtmlNode collection
Use Next and Previous properties to travel through the IHtmlNode collection. Use Children property to access the descendant elements.
```csharp
IHtmlNode node = parser.Current;
            
while (node != null)
{
	node = node.Next;
}
```

####Parse CSS styles
By default parsing HTML will not parse the CSS styles. ParseStyles will parse any external and inline styles in the document. It will also process styles of every element in the document.
```csharp
HtmlParser parser = new HtmlTextParser(@"<html>
		<head>
			<style type='text\css'>
                            .cls
                            {
                                font-size:10px;
                            }
                        </style>
		</head>
		<body>
			<div class='cls' style='font-family:Arial'>sample</div>
		<body>
	</html>");

if (parser.Parse())
{
	parser.ParseStyles();
	IHtmlNode node = parser.Current;
}
```
To resolve any protocol free or relative url of external style sheets, use the UriSchema and BaseURL properties.
```csharp
parser.UriSchema = Uri.UriSchemeHttp;
parser.BaseURL = "http://site.com";
```