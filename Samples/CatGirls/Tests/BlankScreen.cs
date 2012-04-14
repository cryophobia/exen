using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoTouch.Foundation;

namespace CatGirls.Tests
{
	[Preserve(AllMembers=true)]
	class BlankScreen : Test
	{
		public static void DrawIcon(SpriteBatch sb, Texture2D icon, Vector2 position, int frame)
		{
			sb.Draw(icon, position, Color.Black);
		}


		public override void Draw(SpriteBatch sb)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);
			base.Draw(sb);
		}
	}
}
