using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoTouch.Foundation;

namespace CatGirls.Tests
{
	[Preserve(AllMembers=true)]
	class ScreenSize : Test
	{
		int displayMode = 0;

		public override void Tap(Point point)
		{
			base.Tap(point);
			displayMode++;
			if(displayMode > 2)
				displayMode = 0;
		}

		public override Rectangle BackButton()
		{
			Rectangle r = base.BackButton();
			r.X -= 40; r.Y += 40;
			return r;
		}

		Texture2D corner;

		public override void LoadContent()
		{
			base.LoadContent();
			corner = Content.Load<Texture2D>("Corner");
		}

		public override void Draw(SpriteBatch sb)
		{
			GraphicsDevice.Clear(Color.Gray);
			base.Draw(sb);

			Viewport vp = GraphicsDevice.Viewport;

			Point tl = new Point(vp.X, vp.Y);
			Point tr = new Point(vp.X+vp.Width, vp.Y);
			Point bl = new Point(vp.X, vp.Y+vp.Height);
			Point br = new Point(vp.X+vp.Width, vp.Y+vp.Height);

			bool coords = displayMode != 0;
			string tlString = coords ? tl.ToString() : "Top Left";
			Vector2 tlOrigin = Vector2.Zero;
			string trString = coords ? tr.ToString() : "Top Right";
			Vector2 trOrigin = new Vector2(UIFont.MeasureString(trString).X.Floor(), 0);
			string blString = coords ? bl.ToString() : "Bottom Left";
			Vector2 blOrigin = new Vector2(0, UIFont.MeasureString(blString).Y.Floor());
			string brString = coords ? br.ToString() : "Bottom Right";
			Vector2 brOrigin = UIFont.MeasureString(brString).Floor();

			sb.Begin();

			Color cornerColor = displayMode == 2 ? Color.White : Color.White * 0.5f;
			sb.Draw(corner, tl.AsVector2(), null, cornerColor, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
			sb.Draw(corner, tr.AsVector2(), null, cornerColor, 0, new Vector2(corner.Width, 0), 1f, SpriteEffects.FlipHorizontally, 0);
			sb.Draw(corner, bl.AsVector2(), null, cornerColor, 0, new Vector2(0, corner.Height), 1f, SpriteEffects.FlipVertically, 0);
			sb.Draw(corner, br.AsVector2(), null, cornerColor, 0, corner.Size(), 1f, SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically, 0);

			if(displayMode != 2)
			{
				Color textColor = Color.Yellow;
				sb.DrawString(UIFont, tlString, tl.AsVector2(), textColor, 0, tlOrigin, 1, SpriteEffects.None, 0);
				sb.DrawString(UIFont, trString, tr.AsVector2(), textColor, 0, trOrigin, 1, SpriteEffects.None, 0);
				sb.DrawString(UIFont, blString, bl.AsVector2(), textColor, 0, blOrigin, 1, SpriteEffects.None, 0);
				sb.DrawString(UIFont, brString, br.AsVector2(), textColor, 0, brOrigin, 1, SpriteEffects.None, 0);
				
				sb.DrawString(UIFont, GraphicsDevice.PresentationParameters.DisplayOrientation.ToString(),
						vp.Bounds.Center.AsVector2(), Color.Yellow);
			}
			sb.End();
		}

	}
}
