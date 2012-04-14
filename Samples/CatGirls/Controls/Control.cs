using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CatGirls.Controls
{
	abstract class Control
	{
		// Dead-simple control system. No concept of focus, etc.

		public virtual void Tap(Point point) { }
		public virtual void MouseMove(Point point) { }
		public virtual void MouseUp(Point point) { }
		public virtual void MouseDown(Point point) { }

		public virtual void Draw(SpriteBatch sb) { }
	}
}
