using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using MonoTouch.Foundation;

namespace CatGirls.Tests
{
	[Preserve(AllMembers=true)]
	class CatGirlSprite : Test
	{
		public static void DrawIcon(SpriteBatch sb, Texture2D icon, Vector2 position, int frame)
		{
			sb.Draw(icon, position, Color.White);
		}


		string text = "Cat Girl";
		Vector2 textOrigin;

		public override void LoadContent()
		{
			textOrigin = (UIFont.MeasureString(text) / 2).Floor();
		}

		public override void Draw(SpriteBatch sb)
		{
			GraphicsDevice.Clear(Color.White);
			base.Draw(sb);

			Vector2 catGirlPosition = GraphicsDevice.Viewport.Bounds.Center.AsVector2();
			Vector2 textPosition = catGirlPosition + new Vector2(0, (CatGirl.Width/2) + textOrigin.Y);

			sb.Begin();
			sb.Draw(CatGirl, catGirlPosition, null, Color.White, 0, CatGirl.Center().Floor(), 1f, SpriteEffects.None, 0f);
			sb.DrawString(UIFont, text, textPosition, Color.Black, 0, textOrigin, 1f, SpriteEffects.None, 0f);
			sb.End();
		}
	}
}
