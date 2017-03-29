## MariGold.HtmlParser
MariGold.HtmlParser is a utility to parse the HTML documents into a collection of IHtmlNode type instances. You can either traverse through the document by parsing every root elements one by one or parse the entire document at once. Once an HTML element parsed, it will recursively parse all the child elements.

### Installing via NuGet

In Package Manager Console, enter the following command:
```
Install-Package MariGold.HtmlParser
```

### Usage
MariGold.HtmlParser can be used to parse both HTML and CSS of an HTML document.

#### Traverse through html elements
In the following example, the first loop iteration will parse the first div and the following div in the second and final iteration.
```csharp
using MariGold.HtmlParser;

HtmlParser parser = new HtmlTextParser("<div>first</div><div>second</div>");

while (parser.Traverse())
{
	IHtmlNode node = parser.Current;
}
```

#### Parse the HTML document
To parse the entire document at once, use Parse method. It will parse all the HTML elements in the given document and Current property will point to the first root element
```csharp
if (parser.Parse())
{
	IHtmlNode node = parser.Current;
}
```

#### Travel through IHtmlNode collection
Use Next and Previous properties to travel through the IHtmlNode collection. Use the Children property to access the descendant elements.
```csharp
IHtmlNode node = parser.Current;
            
while (node != null)
{
	node = node.Next;
}
```

#### Parse CSS styles
By default parsing HTML will not parse the CSS styles. The ParseStyles method will parse any external or inline styles in the document. The parsed styles can be accessed using the Styles and InheritedStyles properties of IHtmlNode.
```csharp
HtmlParser parser = new HtmlTextParser(@"<html>
		<head>
			<style type='text/css'>
                            .cls
                            {
                                font-size:10px;
                            }
                        </style>
		</head>
		<body>
			<div class='cls' style='font-family:Arial'>sample</div>
		</body>
	</html>");

if (parser.Parse())
{
	parser.ParseStyles();
}
```
To resolve any protocol free or relative url of external style sheets, use the UriSchema and BaseURL properties.
```csharp
parser.UriSchema = Uri.UriSchemeHttp;
parser.BaseURL = "http://site.com";
```