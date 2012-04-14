using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoTouch.Foundation;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CatGirls.Tests
{
	// This test will not work on Silverlight, as it has no support for setting the Viewport

	[Preserve(AllMembers=true)]
	class ViewportTest : Test
	{
		void DrawTestingSprites(SpriteBatch sb, Color background)
		{
			sb.Begin();
			Vector2 viewportSize = new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
			sb.Draw(WhiteBox, Vector2.Zero, null, background, 0f, Vector2.Zero, viewportSize, SpriteEffects.None, 0f);
			sb.Draw(CatGirl, viewportSize/2f, null, Color.Lime, 0f, CatGirl.Center(), 1f, SpriteEffects.None, 0f);
			sb.Draw(CatGirlIcon, Vector2.Zero, null, Color.Red, 0f, CatGirlIcon.Center(), 1f, SpriteEffects.None, 0f);
			sb.Draw(CatGirlIcon, viewportSize, null, Color.Blue, MathHelper.Pi, CatGirlIcon.Center(), 1f, SpriteEffects.None, 0f);
			sb.Draw(CatGirlIcon, new Vector2(viewportSize.X, 0), null, Color.Yellow, 0, CatGirlIcon.Center(), 1f, SpriteEffects.None, 0f);
			sb.Draw(CatGirlIcon, new Vector2(0, viewportSize.Y), null, Color.Magenta, 0, CatGirlIcon.Center(), 1f, SpriteEffects.None, 0f);
			sb.End();
		}

		public override void Draw(SpriteBatch sb)
		{
			GraphicsDevice.Clear(Color.White);

			// TODO: add support for PresentationParameters to ExEn
			//int w = GraphicsDevice.PresentationParameters.BackBufferWidth;
			//int h = GraphicsDevice.PresentationParameters.BackBufferHeight;

			Viewport original = GraphicsDevice.Viewport;
			int w = original.Width;
			int h = original.Height;

			GraphicsDevice.Viewport = new Viewport(0, 0, w/2, h/2);
			// Test XNA 4.0 clear-whole-surface behaviour:
			GraphicsDevice.Clear(Color.DarkGray);
			DrawTestingSprites(sb, Color.DarkRed);

			GraphicsDevice.Viewport = new Viewport(w/4, h/4, w/2, h/2);
			DrawTestingSprites(sb, Color.DarkGreen);

			GraphicsDevice.Viewport = new Viewport(3*(w/4), 3*(h/4), w/4, h/4);
			DrawTestingSprites(sb, Color.DarkBlue);

			GraphicsDevice.Viewport = new Viewport(5*(w/8), 3*(h/8), w/4, h/4);
			DrawTestingSprites(sb, Color.DarkCyan);

			GraphicsDevice.Viewport = original;
			base.Draw(sb);
		}
	}
}
