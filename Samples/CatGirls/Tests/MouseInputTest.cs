using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoTouch.Foundation;
using Microsoft.Xna.Framework.Input;

namespace CatGirls.Tests
{
	[Preserve(AllMembers=true)]
	class MouseInputTest : Test
	{
		public static void DrawIcon(SpriteBatch sb, Texture2D icon, Vector2 position, int frame)
		{
			sb.Draw(icon, position, Color.White);
		}

		public override void BeginRun()
		{
			base.BeginRun();
			Game.IsMouseVisible = false;
		}

		public override void EndRun()
		{
			base.EndRun();
			Game.IsMouseVisible = true;
		}


		Texture2D cursor;

		public override void LoadContent()
		{
			base.LoadContent();
			cursor = Content.Load<Texture2D>("Cursor");
		}

		bool down;
		Vector2 position;

		public override void Update(float seconds)
		{
			base.Update(seconds);

			MouseState state = Mouse.GetState();
			position = new Vector2(state.X, state.Y);
			down = state.LeftButton == ButtonState.Pressed;
		}

		public override void Draw(SpriteBatch sb)
		{
			GraphicsDevice.Clear(Color.Black);
			base.Draw(sb);

			sb.Begin();
			sb.Draw(CatGirl, position, null, down ? Color.Red : Color.White, 0,
					CatGirl.Center().Floor(), 1f, SpriteEffects.None, 0f);
			sb.Draw(cursor, position, Color.White);
			sb.DrawString(UIFont, position.ToString(), position + new Vector2(5), Color.Yellow);
			sb.End();
		}
	}
}
