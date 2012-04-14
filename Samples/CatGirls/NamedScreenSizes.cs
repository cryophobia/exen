using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace CatGirls
{
	static class NamedScreenSizes
	{
		public static Point Get(string name)
		{
			name = name != null ? name.ToLower(System.Globalization.CultureInfo.InvariantCulture) : null;
			switch(name)
			{
				case "720p":
					return new Point(1280, 720);
				case "iphone-portrait":
					return new Point(320, 480);
				case "iphone":
					return new Point(480, 320);
				case "ipad-portrait":
					return new Point(768, 1024);
				case "ipad":
					return new Point(1024, 768);
				default:
				case "web":
					return new Point(480, 480); // Square for some reason...
			}
		}
	}
}
