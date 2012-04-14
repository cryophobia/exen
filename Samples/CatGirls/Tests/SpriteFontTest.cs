using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTouch.Foundation;

namespace CatGirls.Tests
{
	[Preserve(AllMembers=true)]
	class SpriteFontTest : Test
	{
		string text = "AaBbCcXxYyZz 1234567890";

		SpriteFont fancyFont;
		SpriteFont rubbishXnaFont;

		public override void LoadContent()
		{
			fancyFont = Content.Load<SpriteFont>("FancyFont");
			// XNA 4.0 fonts are DXT3 encoded which makes them uggggly, much prefer to use Nuclex's processor
			rubbishXnaFont = Content.Load<SpriteFont>("RubbishFont");
		}


		public override void Draw(SpriteBatch sb)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);
			base.Draw(sb);

			sb.Begin();
			Vector2 position = Vector2.Zero;

			sb.DrawString(UIFont, text, position, Color.White, 0, Vector2.Zero, 2f, SpriteEffects.None, 0f);
			position.Y += UIFont.MeasureString(text).Y * 2;
			sb.DrawString(rubbishXnaFont, text, position, Color.White, 0, Vector2.Zero, 2f, SpriteEffects.None, 0f);
			position.Y += rubbishXnaFont.MeasureString(text).Y * 2;

			sb.DrawString(UIFont, text, position, Color.White);
			position.Y += UIFont.MeasureString(text).Y;
			sb.DrawString(UIFont, text, position, Color.Red);
			position.Y += UIFont.MeasureString(text).Y;

			sb.DrawString(rubbishXnaFont, text, position, Color.White);
			position.Y += rubbishXnaFont.MeasureString(text).Y;
			sb.DrawString(rubbishXnaFont, text, position, Color.Red);
			position.Y += rubbishXnaFont.MeasureString(text).Y;

			sb.DrawString(fancyFont, text, position, Color.White);
			position.Y += fancyFont.MeasureString(text).Y;
			sb.DrawString(fancyFont, text, position, Color.Red);
			position.Y += fancyFont.MeasureString(text).Y;

			sb.End();
		}
	}
}
