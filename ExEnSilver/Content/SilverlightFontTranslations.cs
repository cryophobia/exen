using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace Microsoft.Xna.Framework.Content
{
	public static class SilverlightFontTranslations
	{
		internal static Dictionary<string, SpriteFont> list = new Dictionary<string, SpriteFont>();

		public static void Add(string assetName, SpriteFont font)
		{
			list[assetName] = font;
		}
	}
}
