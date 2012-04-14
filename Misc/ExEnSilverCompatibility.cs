using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using Microsoft.Xna.Framework.Media;

// Use this on non-Silverlight platforms so that the Hint functions will compile
// (The Conditional attributes will strip out the function calls when compiled)

namespace Microsoft.Xna.Framework
{
	public static class ExEnSilverCompatibility
	{

		#region SpriteBatch

		[Conditional("SILVERLIGHT_COMPATIBILITY")]
		public static void HintDisableCache(this SpriteBatch spriteBatch, bool disable) { }

		[Conditional("SILVERLIGHT_COMPATIBILITY")]
		public static void HintDynamicColor(this SpriteBatch spriteBatch, bool enable) { }

		[Conditional("SILVERLIGHT_COMPATIBILITY")]
		public static void HintDynamicRectangle(this SpriteBatch spriteBatch, bool enable) { }

		#endregion


		#region GraphicsDevice

		[Conditional("SILVERLIGHT_COMPATIBILITY")]
		public static void HintClipEnable(this GraphicsDevice graphicsDevice) { }

		[Conditional("SILVERLIGHT_COMPATIBILITY")]
		public static void HintClipDisable(this GraphicsDevice graphicsDevice) { }

		#endregion


		#region Song

		[Conditional("SILVERLIGHT_COMPATIBILITY")]
		public static void HintBeginPreload(this Song song) { }

		#endregion

	}
}
