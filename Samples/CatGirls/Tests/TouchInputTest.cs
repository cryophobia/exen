using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoTouch.Foundation;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace CatGirls.Tests
{
	[Preserve(AllMembers=true)]
	class TouchInputTest : Test
	{
		Texture2D cursor;

		TouchPanelCapabilities caps;
		TouchCollection state;

		MouseState mouseState;
		Vector2 mousePosition;

		public override void LoadContent()
		{
			base.LoadContent();
			cursor = Content.Load<Texture2D>("Cursor");

			caps = TouchPanel.GetCapabilities();
		}


		public override void Update(float seconds)
		{
			base.Update(seconds);

			state = TouchPanel.GetState();
			mouseState = Mouse.GetState();
			mousePosition = new Vector2(mouseState.X, mouseState.Y);
		}

		public override void Draw(SpriteBatch sb)
		{
			GraphicsDevice.Clear(Color.Black);
			base.Draw(sb);

			sb.Begin();

			sb.Draw(CatGirl, mousePosition, null, Color.White * 0.3f, 0,
					CatGirl.Center().Floor(), 1f, SpriteEffects.None, 0f);

			Vector2 textPosition = Vector2.Zero;
			sb.DrawString(UIFont, "Connected = " + caps.IsConnected, textPosition, caps.IsConnected ? Color.White : Color.Red);
			textPosition.Y += UIFont.LineSpacing;
			sb.DrawString(UIFont, "MaximumTouchCount = " + caps.MaximumTouchCount, textPosition, Color.White);
			textPosition.Y += UIFont.LineSpacing;
			sb.DrawString(UIFont, "DisplayWidth = " + TouchPanel.DisplayWidth, textPosition, Color.White);
			textPosition.Y += UIFont.LineSpacing;
			sb.DrawString(UIFont, "DisplayHeight = " + TouchPanel.DisplayHeight, textPosition, Color.White);
			textPosition.Y += UIFont.LineSpacing;
			sb.DrawString(UIFont, "DisplayOrientation = " + TouchPanel.DisplayOrientation, textPosition, Color.White);

			foreach(TouchLocation touch in state)
			{
				Color c = Color.Gray;
				switch(touch.State)
				{
					case TouchLocationState.Invalid: c = Color.Brown; break;
					case TouchLocationState.Moved: c = Color.Green; break;
					case TouchLocationState.Pressed: c = Color.White; break;
					case TouchLocationState.Released: c = Color.Red; break;
				}

				sb.Draw(cursor, touch.Position, c);

				string id = touch.Id.ToString();
				sb.DrawString(UIFont, id, touch.Position + new Vector2(-10, -40), Color.Black);
				sb.DrawString(UIFont, id, touch.Position + new Vector2(-12, -42), Color.Black);
				sb.DrawString(UIFont, id, touch.Position + new Vector2(-11, -41), c);
			}

			sb.End();
		}
	}
}
