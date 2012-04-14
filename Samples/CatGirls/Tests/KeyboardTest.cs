using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoTouch.Foundation;

namespace CatGirls.Tests
{
	[Preserve(AllMembers=true)]
	class KeyboardTest : Test
	{
		public override void Draw(SpriteBatch sb)
		{
			base.Draw(sb);

			KeyboardState ks = Keyboard.GetState();
			Keys[] keys = ks.GetPressedKeys();

			string keyText = string.Join("\n", keys.Select(k => k.ToString()).ToArray());

			// Hey let's test the other functions as well...
			Color c = Color.Wheat;
			for(int i = 0; i < 256; ++i)
			{
				Keys k = (Keys)i;
				if(keys.Contains(k))
				{
					if(!(ks.IsKeyDown(k) && !ks.IsKeyUp(k) && ks[k] == KeyState.Down))
						c = Color.Red;
				}
				else
				{
					if(!(!ks.IsKeyDown(k) && ks.IsKeyUp(k) && ks[k] == KeyState.Up))
						c = Color.Red;
				}
			}

			GraphicsDevice.Clear(c);
			sb.Begin();
			sb.DrawString(UIFont, keyText, Vector2.Zero, Color.Black);
			sb.End();
		}
	}
}
